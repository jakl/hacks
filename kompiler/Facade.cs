using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kompiler {
  /// <summary>
  /// This class abstracts the inner mechanics of the process of compilation, to provide a handle for the UI
  /// </summary>
  class Facade {
    Lexer lexer = Lexer.GetTokenizer();
    private string errors;
    public bool comments;

    /// <summary>
    /// An error message that will say whether or not an error occured during lexing
    /// </summary>
    public string Errors {
      get { return errors; }
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
    public string getTokens(string source) {
      errors = lexer.Lex(source);
      string ret = "Cnt Line Type                                 Lexeme\r\n";
      int count = 0;
      foreach (Token t in lexer.Tokens) {
        if (t.m_tokType != Token.TOKENTYPE.COMMENT)
          ret += count++ + "  " + t.ToString() + "\r\n";
        else if(comments)
          ret += count++ + "  " + t.ToString() + "\r\n";
      }
      return ret;
    }

  }
}
