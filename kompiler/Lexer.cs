using System;               // Exception 
using System.Collections;   // ArrayList, HashTable
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace kompiler {
  /// <summary>
  /// Lexer returns tokens (lexemes).
  /// This is implemented as a singleton pattern (Design Patterns in C sharp)
  /// </summary>
  public class Lexer {
    // The Modula-2 source
    string m_source;
    int m_index;//track where the next token should be scanned for in the source
    int m_lineNumber;
    string m_errors;

    public string Errors {
      get { return m_errors; }
    }

    List<Token> m_tokens = new List<Token>();

    public List<Token> Tokens {
      get { return m_tokens; }
    }

    // The hastable of Modula-2 keywords
    Dictionary<string, Token.TOKENTYPE> m_keywords;

    // The single object instance for this class.
    private static Lexer c_tokenizer;

    // To prevent access by more than one thread. This is the specific lock 
    //    belonging to the Tokenizer Class object.
    private static Object c_tokenizerLock = typeof(Lexer);

    // Instead of a constructor, we offer a static method to return the only
    //    instance.
    private Lexer() { } // private constructor so no one else can create one.

    static public Lexer GetLexer() {
      lock (c_tokenizerLock) {
        // if this is the first request, initialize the one instance
        if (c_tokenizer == null) {
          // create the single object instance
          c_tokenizer = new Lexer();

          // Load the keywords
          c_tokenizer.LoadKeywords();
        }

        // return a reference to the only instance
        return c_tokenizer;
      }
    }

    /// <summary>
    /// Load M2 keywords.
    /// PRE:  An array of keywords is available in Token.
    /// POST: The hashtable is loaded and the correct token type is
    ///    associated with each entry.
    /// </summary>
    private void LoadKeywords() {
      m_keywords = new Dictionary<string, Token.TOKENTYPE>();

      // load the keyword hashtable
      for (int i = 0; i < Token.c_iKeywordCount; i++)
        // For each entry, we add the string as the key,
        //    and i cast as a TOKENTYPE as the value.
        m_keywords.Add(((Token.TOKENTYPE)i).ToString(), (Token.TOKENTYPE)i);
    }

    /// <summary>
    /// Tokenizes the string arg into a linked list of tokens, available through the Token attribute of this class, after the function has returned.
    /// 
    /// </summary>
    /// <param name="source">string to lex/tokenize</param>
    /// <returns>error message</returns>
    public bool Lex(string source) {
      m_source = source;
      m_index = 0;
      m_lineNumber = 1;

      m_tokens = new List<Token>();

      while (m_index < m_source.Length) {

        if (Char.IsWhiteSpace(m_source[m_index])) {
          //track which linenumber of the source is currently being lexed, and increment past whitespace
          if (m_source[m_index++] == '\n') m_lineNumber++;
          continue;
        }

        if (LexKeyWords()) continue;
        if (LexRegexes()) continue;
        if (Lex2Chars()) continue;
        if (Lex1Char()) continue;
        m_errors = "Quit lexing at line " + m_lineNumber + " due to the first word in the following:\r\n" + m_source.Substring(m_index);
        return false;
      }
      m_tokens.Add(new Token(Token.TOKENTYPE.EOF, "EOF", ++m_lineNumber));
      //This was the old way of calculating lineNumber: m_source.Substring(0, m_index).Split('\n').Length;

      m_errors = "No errors in lexing";
      return true;
    } //Lex

    private bool LexKeyWords() {
      foreach (string keyword in m_keywords.Keys)
        //If there is room enough left for the keyword to possibly exist before the end of the string
        //and if the keyword does exist
        if (m_source.Length - m_index >= keyword.Length && m_source.Substring(m_index, keyword.Length) == keyword) {
          //position the index after the word
          m_index += keyword.Length;

          m_tokens.Add(new Token(m_keywords[keyword], keyword, m_lineNumber));
          return true;
        }
      return false;
    }

    private bool LexRegexes() {
      Match match;

      //kvp stands for key value pair
      foreach (KeyValuePair<Token.TOKENTYPE, Regex> kvp in Token.regexes) {
        //test each regex against the string
        match = kvp.Value.Match(m_source.Substring(m_index));
        if (match.Success) {
          //save the matched text in value
          string value = match.ToString();

          //increment the index past the end of the match
          m_index += value.Length;
          
          //(*Strings have a regex that ends in [^\"'][\"'][^\"'], meaning it will grab one char too many*)
          if (Token.TOKENTYPE.STRING == kvp.Key) {
            value = value.Substring(1, value.Length - 3);//delete the last char, don't include ""
            m_index--;//decrement the index to reparse the last char
          }

          m_tokens.Add(new Token(kvp.Key, value, m_lineNumber));
          m_lineNumber += value.Split('\n').Length - 1;//find all the newlines in the match
          return true;
        }
      }
      return false;
    }

    private bool Lex2Chars() {
      if (m_source.Length - m_index < 2)
        return false;//cannot find two chars if less than 2 exist

      //kvp stands for key value pair
      foreach (KeyValuePair<Token.TOKENTYPE, string> kvp in Token.multichars)
        if (m_source.Substring(m_index, 2) == kvp.Value) {
          m_index+=2;
          m_tokens.Add(new Token(kvp.Key, kvp.Value.ToString(), m_lineNumber));
          return true;
        }
      return false;
    }

    private bool Lex1Char() {
      //kvp stands for key value pair
      foreach (KeyValuePair<Token.TOKENTYPE, char> kvp in Token.singlechars)
        if (m_source[m_index] == kvp.Value) {
          m_index++;
          m_tokens.Add(new Token(kvp.Key, kvp.Value.ToString(), m_lineNumber));
          return true;
        }
      return false;
    }

    /// <summary>
    /// Check the first token to see if it is MODULE. If it isn't, add the proper begin and end statements.
    /// This is a convenience function to save time when writing short modula 2 programs
    /// </summary>
    public void addModuleWhenNonExistant() {
      int firstNonComment;
      for (firstNonComment = 0; m_tokens[firstNonComment].m_tokType == Token.TOKENTYPE.COMMENT; firstNonComment++)
        ;
      if (m_tokens[firstNonComment].m_tokType != Token.TOKENTYPE.MODULE) {
        m_tokens.RemoveAt(m_tokens.Count-1);//remove EOF

        m_tokens.Insert(firstNonComment, new Token(Token.TOKENTYPE.MODULE, "module", -1));
        m_tokens.Insert(firstNonComment+1, new Token(Token.TOKENTYPE.ID, "unknown", -1));
        m_tokens.Insert(firstNonComment+2, new Token(Token.TOKENTYPE.SEMI_COLON, "semicolon", -1));
        m_tokens.Add(new Token(Token.TOKENTYPE.END, "end", int.MaxValue));
        m_tokens.Add(new Token(Token.TOKENTYPE.ID, "unknown", int.MaxValue));
        m_tokens.Add(new Token(Token.TOKENTYPE.DOT, "dot", int.MaxValue));

        m_tokens.Add(new Token(Token.TOKENTYPE.EOF, "eof", int.MaxValue));//add EOF back in
      }
    }
  } // class Lexer
}