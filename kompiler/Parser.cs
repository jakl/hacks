using System;
using System.Collections;	// ArrayLis
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Parser is a singleton class that receives a list of tokens 
/// and parses them according to Modula-2 grammar. It has an emitter
/// that creates appropriate MASM 6.11 assembler code.
/// </summary>
namespace kompiler {
  public class Parser {
    // Store associations between identifiers and values
    private Symbols m_sym = Symbols.GetSymbols();

    // Ask the emitter to create assembly code as needed
    private Emitter m_emitter = Emitter.GetEmitter();

    //single reference to the parser
    private static Parser parser;

    //Source code as a list of tokens
    List<Token> m_tokens;

    //Current token
    private int m_index;

    // To prevent access by more than one thread. This is the specific lock 
    //    belonging to the Class object.
    private static Object parserlock = typeof(Parser);

    // Instead of a constructor, we offer a static method to return the only
    //    instance.
    private Parser() { } // private constructor so no one else can create one.

    static public Parser GetParser() {
      lock (parserlock) {
        // if this is the first request, initialize the one instance
        if (parser == null)
          parser = new Parser();

        // return a reference to the only instance
        return parser;
      }
    }


    /// <summary>
    /// Match is used to validate that the current token is expected.
    /// PRE:  The current token has been loaded.
    /// POST: The current token is verified and the next one loaded. If errors are encountered,
    ///    an exception is thrown.
    /// </summary>
    Token Match(Token.TOKENTYPE tok) {
      // Have we loaded a token from the tokenizer?
      if (m_tokens[m_index] == null)
        throw new Exception("Parser - Match: c_tokCur is null.");

      // Is the current token the one we expected?
      if (m_tokens[m_index].m_tokType == tok) {

        // Is this the normal end of the source code file?
        if (tok == Token.TOKENTYPE.EOF)
          return null;

        // Otherwise load the next token from the tokenizer.
        m_index++;

        //return the matched token
        return m_tokens[m_index - 1];

      } else { // We have the wrong token; bail out gracefully
        string strMsg = string.Format("Expected {0}; found {1} ('{2}')at source line {3}",
            tok.ToString(), m_tokens[m_index].m_tokType.ToString(),
            m_tokens[m_index].m_strName, m_tokens[m_index].m_iLineNum);

        throw new Exception("Parser - Match: " + strMsg);
      }
    } // Match

    /// <summary>
    /// Return the TOKENTYPE of the begining of a declaration,
    ///  or throw an exception if it doesn't exist
    /// </summary>
    /// <returns></returns>
    Attribute.ID_CAT MatchDeclStart() {
      Token.TOKENTYPE ret = 0;
      try {
        ret = Match(Token.TOKENTYPE.CONST).m_tokType;
      } catch (Exception e) { }
      try {
        ret = Match(Token.TOKENTYPE.TYPE).m_tokType;
      } catch (Exception e) { }
      try {
        ret = Match(Token.TOKENTYPE.VAR).m_tokType;
      } catch (Exception e) { }
      switch (ret) {
        case Token.TOKENTYPE.CONST: return Attribute.ID_CAT.CONST;
        case Token.TOKENTYPE.TYPE: return Attribute.ID_CAT.TYPE;
        case Token.TOKENTYPE.VAR: return Attribute.ID_CAT.VAR;
        default: throw (new Exception(string.Format(
            "Expected Const, Type, or Var to begin a declaration; found {0} ('{1}')at source line {2}",
            m_tokens[m_index].m_tokType.ToString(), m_tokens[m_index].m_strName, m_tokens[m_index].m_iLineNum
          )));
      }
    }

    /// <summary>
    /// Return the TOKENTYPE of the end of a declaration,
    ///   or throw an exception if it doesn't exist
    /// </summary>
    /// <returns></returns>
    Attribute.VAR_TYPE MatchDeclEnd() {
      Token.TOKENTYPE matchType = 0;
      try {
        matchType = Match(Token.TOKENTYPE.CARDINAL).m_tokType;
      } catch (Exception e) { }
      try {
        matchType = Match(Token.TOKENTYPE.INTEGER).m_tokType;
      } catch (Exception e) { }
      try {
        matchType = Match(Token.TOKENTYPE.REAL).m_tokType;
      } catch (Exception e) { }
      switch (matchType) {
        case Token.TOKENTYPE.CARDINAL: return Attribute.VAR_TYPE.CARDINAL;
        case Token.TOKENTYPE.INTEGER: return Attribute.VAR_TYPE.INTEGER;
        case Token.TOKENTYPE.REAL: return Attribute.VAR_TYPE.REAL;
        default: throw (new Exception(string.Format(
            "Expected CARDINAL, INTEGER, or REAL to end a declaration; found {0} ('{1}')at source line {2}",
            m_tokens[m_index].m_tokType.ToString(), m_tokens[m_index].m_strName, m_tokens[m_index].m_iLineNum
          )));
      }
    }


    /// <summary>
    /// This is the main method of this class. It receives a tokenizer and a FileDefaultManager 
    ///    and creates a parse tree.
    /// PRE:  The token list has been loaded.
    /// POST: The parse tree is complete. The assembler file has been created
    ///    and assembled. The executable is complete. If errors are encountered,
    ///    they have been displayed and parsing/assembling suspended.
    /// Returns the core assembly code for easy viewing
    /// </summary>
    public string Parse(List<Token> tokens, string projectName) {
      m_tokens = tokens;
      m_index = 0;
      m_emitter.init(projectName);

      // Parse a single module
      string mainProc = Module();

      // Exit the main scope. Pass the total memory usage to the emitter.
      //c_emitter.MainProcPostamble(c_sym.ExitProcScope());

      // The parse is complete. Write all assembler files; then assemble them.
      return m_emitter.WriteAFiles(projectName, mainProc);
    } // Parse

    /// <summary>
    /// This is the outermost parse unit. A module is the fundamental unit
    ///    of Modula-2 code.
    /// PRE:  The tokenizer, symbol table, and emitter are available.
    /// POST: The parse tree is complete. We are ready to assemble. Return the main procedure name
    /// </summary>
    string Module() {
      //(MODULE) test04 ; //examples in parentheses
      Match(Token.TOKENTYPE.MODULE);

      // The next token should be an id -- the module name.
      // We retain it to check the id given at the module close.
      string strModule = Match(Token.TOKENTYPE.ID).m_strName;//MODULE (test04) ;
      Match(Token.TOKENTYPE.SEMI_COLON);//MODULE test04 (;)

      m_emitter.ProcPreamble(strModule);//Main procedure will be named after the module

      //Compiler EBNF handout marks the below as a block

      //(VAR i : INTEGER ;)
      DclList(); // parse all declarations

      // We have finished parsing declarations. Calculate the memory needed for this scope
      //    and adjust memory offsets for main variables.
      //c_sym.AdjustProcMemoryOffset();

      // The statement begins with BEGIN.
      Match(Token.TOKENTYPE.BEGIN);

      StmtList(); // parse a sequence of statements

      // We are at the end of the "main" M2 procedure.
      Match(Token.TOKENTYPE.END);

      //Close the main procedure, knowing how much memory was required
      m_emitter.ProcPostamble(strModule, m_sym.Mem);

      // The next token should be the module name given at first.
      if (Match(Token.TOKENTYPE.ID).m_strName != strModule)
        // announce the error
        throw new Exception("Module name not repeated at close of module.");

      // Otherwise we have the right token.
      Match(Token.TOKENTYPE.DOT);
      Match(Token.TOKENTYPE.EOF); // end of input file
      return strModule;
    } // Module

    /// <summary>
    /// DclList (M2 Users Manual, p. 99)
    /// This is currently in a bad state of affairs because it doesn't call:
    ///   CONST, PROCEDURE, TYPE, or VAR
    /// And does everything on its own ... incorrectly if anything beside tests 1 - 4
    ///   was compiled
    ///   
    /// On the bright side it will match and handle
    ///   VAR i : INTEGER ;
    /// </summary>
    void DclList() {
      //Pieces of comment examples is parentheses are what will be matched next

      while (true) {
        try {
          //(VAR) i : INTEGER ;
          m_sym.beginCategory(MatchDeclStart());
        } catch (Exception e) { break; }//Ended without finding anything


        //Automatically throw and die on any exceptions in the middle of a declaration

        //VAR (i) : INTEGER ;
        m_sym.add(Match(Token.TOKENTYPE.ID).m_strName);

        Match(Token.TOKENTYPE.COLON);

        //VAR i : (INTEGER) ;
        m_sym.commit(MatchDeclEnd());

        Match(Token.TOKENTYPE.SEMI_COLON);
      }
    }

    /// <summary>
    /// Parse the declaration of a constant (M2 Users Manual)
    /// </summary>
    void CONST() {
    } // CONST ()

    /// <summary>
    /// PRE:  We have encountered the keyword PROCEDURE.
    /// POST: The procedure has been parsed and emitted as necessary. 
    ///    All relevant local variables and parameters have been loaded into
    ///    the Symbol Table.
    /// </summary>
    void PROCEDURE() {
    } // Procedure()

    /// <summary>
    /// TYPE  Parse the declaration of a TYPE (M2 Users Manual).
    /// 
    /// This is currently used only to parse the desclaration of ARRAYs.
    ///    
    /// PRE:  The current token is TYPE.
    /// POST: All TYPEs are added to the symbol table.
    /// </summary>
    void TYPE() {
    } // Type()

    /// <summary>
    /// VAR  Parse the declaration of a variable (M2 Users Manual).
    /// 
    /// The behavior of this powerful function is altered by the bool
    ///    bParsingParameters; this is true while parsing the parameter list within
    ///    parentheses following the PROCEDURE name. Note that we may run into several
    ///    VAR's while parsing a DclList (VAR i:INTEGER: VAR j:INTEGER) or 
    ///    while parsing a procedure parameter list.
    ///    
    /// PRE:  false -> parsing a VAR in a DclList
    ///       true  -> parsing a proc. parameter list within parens.
    /// POST: All variables are added to the symbol table.
    /// </summary>
    void VAR(bool bParsingParameters) {
    } // VAR

    /// <summary>
    /// Check to see what kind of statement is input.
    /// </summary>
    void StmtList() {
      /*SAMPLE CODE FROM TEST04
        i := 3 ;
        WRINT ( i ) ;
        WRLN ;
      */

      while (m_tokens[m_index].m_tokType != Token.TOKENTYPE.END) {
        if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.ID)
          Assignment();
        if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.WRINT)
          WRINT();
        if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.WRSTR)
          WRSTR();
        if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.WRLN)
          WRLN();
      }
      //Assignment();
    } // StmtList

    /// <summary>
    /// Perform an assignment.
    /// PRE:  We are looking at an ID token for a variable 
    ///    (simple or array) as the beginning of an assignment statement.
    ///    Attr refers to its entry in the symbol table.
    /// POST: Code is emitted to complete the assignment.
    /// </summary>
    void Assignment() {
      //i := 3 ;

      string name = Match(Token.TOKENTYPE.ID).m_strName;
      Match(Token.TOKENTYPE.ASSIGN);
      int value = int.Parse(Match(Token.TOKENTYPE.INT_NUM).m_strName);
      m_sym.setVal(name, value);
      Match(Token.TOKENTYPE.SEMI_COLON);
    } // Assignment

    /// <summary>
    /// Write an integer literal or one stored in a variable in the symbol table
    /// </summary>
    void WRINT() {
      //WRINT ( 42 ) ;

      Match(Token.TOKENTYPE.WRINT);
      Match(Token.TOKENTYPE.LEFT_PAREN);

      //Integer literal found
      if(m_tokens[m_index].m_tokType == Token.TOKENTYPE.INT_NUM)
        //Parse the string version to an integer and write it
        m_emitter.WRINT(int.Parse(Match(Token.TOKENTYPE.INT_NUM).m_strName));

      //Integer variable found
      else if(m_tokens[m_index].m_tokType == Token.TOKENTYPE.ID)
        //Take the value of the variable from the symbol table and write it
        //THIS IS A KNOWN HACK AND VARIABLES VALUES *SHOULD* BE ON THE STACK
        //...at least test04 compiles with this hack and I can sleep for church tomorrow
        m_emitter.WRINT((int)m_sym.get(Match(Token.TOKENTYPE.ID).m_strName).m_value);

      Match(Token.TOKENTYPE.RIGHT_PAREN);
      Match(Token.TOKENTYPE.SEMI_COLON);
    }

    /// <summary>
    /// Writes a new line character
    /// </summary>
    void WRLN() {
      //WRLN ;

      Match(Token.TOKENTYPE.WRLN);
      m_emitter.WRLN();
      Match(Token.TOKENTYPE.SEMI_COLON);
    }

    /// <summary>
    /// Writes a string literal
    /// </summary>
    void WRSTR() {
      //WRSTR ( "Hello world!" ) ;

      Match(Token.TOKENTYPE.WRSTR);
      Match(Token.TOKENTYPE.LEFT_PAREN);
      m_emitter.WRSTR(Match(Token.TOKENTYPE.STRING).m_strName);
      Match(Token.TOKENTYPE.RIGHT_PAREN);
      Match(Token.TOKENTYPE.SEMI_COLON);
    }

    //********WARNING********WARNING********WARNING****
    //WARNING: NOTHING BELOW IS USED YET OR IMPLEMENTED
    //********WARNING********WARNING********WARNING****

    /// <summary>
    /// PRE:  We parsed "IF".
    /// POST: We emit all the code for this compound statement.
    /// </summary>
    void IfThenElse() {
    }

    /// <summary>
    /// PRE:  We parsed "LOOP".
    /// POST: We emit all the code for this compound statement.
    /// </summary>
    void LOOP() {
    }


    /// <summary>
    /// The next several functions constitute expressions (Mod 2 User's Manual Chapter 6, p. 109)
    /// 
    ///  For binary operations, the two operands are pushed onto the stack (second on top of stack)
    /// 	and the appropriate operation (AddOperation(), etc.) 
    /// 	knows where to find the two operands.and leaves the result on top of the stack
    /// 
    ///  This grammar is based partly on the Dragon Book
    /// 	(Aho, Sethi, Ullman) pp. 30ff, 48ff, 63ff) and partly on Parsons (pp. 88f.)
    /// 
    ///  Expr           -> SimpleExpr RelOp SimpleExpr  |  SimpleExpr
    /// 
    ///  SimpleExpr     -> Term RestSimpleExpr
    /// 
    ///  RestSimpleExpr -> + Term RestSimpleExpr  |  - Term RestSimpleExpr  |  OR Term RestSimpleExpr  |  epsilon
    /// 
    ///  Term           -> Factor RestTerm
    /// 
    ///  RestTerm       -> *   Factor RestTerm  |  /    Factor RestTerm  |  
    /// 				   MOD Factor RestTerm  |  AND  Factor RestTerm  |  epsilon
    /// 
    ///  Factor         -> ( Expr )  |  Value   |  NOT Factor (M2 User's Manual pp. 109f.)
    /// </summary>

    void Expr() {
    } // Expr ()

    void SimpleExpr() {
    } // SimpleExpr

    void RestSimpleExpr() {
    } // RestSimpleExpr

    // a few more methods . . .

    void Value() {
    } // Value ()    
  }
}