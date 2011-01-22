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

    LinkedList<Token> m_tokens = new LinkedList<Token>();

    public LinkedList<Token> Tokens {
      get { return m_tokens; }
      //set { m_tokens = value; }
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

    static public Lexer GetTokenizer() {
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
    public string Lex(string source) {
      m_source = source;
      m_index = 0;
      m_lineNumber = 1;

      m_tokens = new LinkedList<Token>();

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
        return "Quit lexing at line " + m_lineNumber + " due to the first word in the following:\r\n" + m_source.Substring(m_index);
      }
      m_tokens.AddLast(new Token(Token.TOKENTYPE.EOF, "EOF", ++m_lineNumber));
      //This was the old way of calculating lineNumber: m_source.Substring(0, m_index).Split('\n').Length;

      return "No errors in lexing";
    } //Lex

    private bool LexKeyWords() {
      foreach (string keyword in m_keywords.Keys)
        //If there is room enough left for the keyword to possibly exist before the end of the string
        //and if the keyword does exist
        if (m_source.Length - m_index >= keyword.Length && m_source.Substring(m_index, keyword.Length) == keyword) {
          //position the index after the word
          m_index += keyword.Length;

          m_tokens.AddLast(new Token(m_keywords[keyword], keyword, m_lineNumber));
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
            value = value.Substring(0, value.Length - 1);//delete the last char
            m_index--;//decrement the index to reparse the last char
          }

          m_tokens.AddLast(new Token(kvp.Key, value, m_lineNumber));
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
          m_tokens.AddLast(new Token(kvp.Key, kvp.Value.ToString(), m_lineNumber));
          return true;
        }
      return false;
    }

    private bool Lex1Char() {
      //kvp stands for key value pair
      foreach (KeyValuePair<Token.TOKENTYPE, char> kvp in Token.singlechars)
        if (m_source[m_index] == kvp.Value) {
          m_index++;
          m_tokens.AddLast(new Token(kvp.Key, kvp.Value.ToString(), m_lineNumber));
          return true;
        }
      return false;
    }
  } // class Lexer
}