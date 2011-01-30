using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kompiler {
  /// <summary>
  /// This class abstracts the inner mechanics of the process of compilation, to provide a handle for the UI
  /// </summary>
  class Facade {
    Lexer m_lexer = Lexer.GetLexer();
    Parser m_parser = Parser.GetParser();
    private string m_errors;
    public bool m_comments;
    private string m_tokenDump;
    private string m_assemblyDump;

    public string AssemblyDump {
      get { return m_assemblyDump; }
    }

    public string TokenDump {
      get { return m_tokenDump; }
    }

    /// <summary>
    /// An error message that will say whether or not an error occured during lexing
    /// </summary>
    public string Errors {
      get { return m_errors; }
      //set { errors = value; }
    }

    //single reference to the facade
    private static Facade facade;

    // To prevent access by more than one thread. This is the specific lock 
    //    belonging to the Tokenizer Class object.
    private static Object facadelock = typeof(Facade);

    // Instead of a constructor, we offer a static method to return the only
    //    instance.
    private Facade() { } // private constructor so no one else can create one.

    static public Facade GetFacade() {
      lock (facadelock) {
        // if this is the first request, initialize the one instance
        if (facade == null)
          facade = new Facade();

        // return a reference to the only instance
        return facade;
      }
    }

    /// <summary>
    /// Returns all the tokens, formatted in one large string
    /// </summary>
    /// <param name="source">source code to parse as a string</param>
    /// <returns>formatted tokens</returns>
    public bool lex(string source) {
      bool successLexing = m_lexer.Lex(source);
      m_errors = m_lexer.Errors;
      m_tokenDump = "Cnt Line Type                                 Lexeme\r\n";
      int count = 0;
      foreach (Token t in m_lexer.Tokens) {
        if (t.m_tokType != Token.TOKENTYPE.COMMENT)
          m_tokenDump += count++ + "  " + t.ToString() + "\r\n";
        else if(m_comments)
          m_tokenDump += count++ + "  " + t.ToString() + "\r\n";
      }
      return successLexing;
    }

    /// <summary>
    /// Lexes then parses source code
    /// Creates a project folder for the source code based on the projectName parameter
    /// </summary>
    /// <param name="source">source code</param>
    /// <param name="projectName"></param>
    /// <returns>True if successful, false otherwise, with errors in this.Errors</returns>
    public bool parse(string source, string projectName) {
      if (!lex(source))
        return false;
      try {
        m_assemblyDump = m_parser.Parse(m_lexer.Tokens, projectName);
      } catch (Exception e) {
        m_errors += '\n'+e.Message;
        return false;
      }
      return true;
    }
  }
}
