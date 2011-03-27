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

    int m_IfCount = 1;
    int m_LoopCount = 1;

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

    public string SymbolDump {
      get { return m_sym.ToString(); }
    }

    /// <summary>
    /// Closes Emitter
    /// </summary>
    public void Close(){
      m_emitter.Close();
      m_IfCount = 1;
      m_LoopCount = 1;
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
    /// This is the main method of this class. It receives a tokenizer and a FileDefaultManager 
    ///    and creates a parse tree.
    /// PRE:  The token list has been loaded.
    /// POST: The parse tree is complete. The assembler file has been created
    ///    and assembled. The executable is complete. If errors are encountered,
    ///    they have been displayed and parsing/assembling suspended.
    /// Returns the core assembly code for easy viewing
    /// Will change the working directory to inside a local project folder
    /// Please change the directory up one level after this call
    /// </summary>
    public string Parse(List<Token> tokens, string projectName) {
      m_tokens = tokens;
      m_index = 0;
      m_emitter.init(projectName);
      m_sym.init();

      // Parse a single module
      Module();

      // Exit the main scope. Pass the total memory usage to the emitter.
      //c_emitter.MainProcPostamble(c_sym.ExitProcScope());

      // The parse is complete. Write all assembler files; then assemble them.
      //reset the working directory back to the origional
      return m_emitter.WriteAFiles(projectName, m_sym.Mem);
    } // Parse

    /// <summary>
    /// This is the outermost parse unit. A module is the fundamental unit
    ///    of Modula-2 code.
    /// PRE:  The tokenizer, symbol table, and emitter are available.
    /// POST: The parse tree is complete. We are ready to assemble. Return the main procedure name
    /// </summary>
    void Module() {
      //(MODULE) test04 ; //examples in parentheses
      Match(Token.TOKENTYPE.MODULE);

      // The next token should be an id -- the module name.
      // We retain it to check the id given at the module close.
      string strModule = Match(Token.TOKENTYPE.ID).m_strName;//MODULE (test04) ;
      Match(Token.TOKENTYPE.SEMI_COLON);//MODULE test04 (;)

      //Compiler EBNF handout marks the below as a block

      //(VAR i : INTEGER ;)
      DclList(false); // parse all declarations

      //Parse procedures
      while (m_tokens[m_index].m_tokType == Token.TOKENTYPE.PROCEDURE)
        PROCEDURE();

      // The statement begins with BEGIN.
      Match(Token.TOKENTYPE.BEGIN);

      StmtList(-1); // parse a sequence of statements

      // We are at the end of the "main" M2 procedure.
      Match(Token.TOKENTYPE.END);

      //Close the current procedure, knowing how much memory was required
      m_emitter.ProcPostamble(m_sym.Mem);

      // The next token should be the module name given at first.
      if (Match(Token.TOKENTYPE.ID).m_strName != strModule)
        // announce the error
        throw new Exception("Module name not repeated at close of module.");

      // Otherwise we have the right token.
      Match(Token.TOKENTYPE.DOT);
      Match(Token.TOKENTYPE.EOF); // end of input file
    } // Module

    /// <summary>
    /// DclList (M2 Users Manual, p. 99)
    /// Matches a var, const, or type, and calls the proper functions that
    /// generate appropriate code.
    /// </summary>
    /// <param name="isParams">True if this is a Declaration List inside
    /// a procedure header's parenthesis surrounding an expected parameter list.
    /// False if this is a general Declaration List</param>
    void DclList(bool isParams) {
      //Pieces of comment examples is parentheses are what will be matched next

      if (isParams)
        VAR(isParams);
      else

      while (true) {
        //(VAR) i : INTEGER ;
        //or
        //(CONST) low = 3;
        if (Token.TOKENTYPE.VAR == m_tokens[m_index].m_tokType)
          VAR(isParams);
        else if (Token.TOKENTYPE.CONST == m_tokens[m_index].m_tokType)
          CONST(isParams);
        else if (Token.TOKENTYPE.TYPE == m_tokens[m_index].m_tokType)
          TYPE(isParams);
        else break;
        //add another case for TYPE or PROCEDURE later, as needed
      }
    }

    /// <summary>
    /// Parse the declaration of a constant (M2 Users Manual)
    /// </summary>
    void CONST(bool isParam) {
      Match(Token.TOKENTYPE.CONST);

      //	CONST (low = 3; hi = 7;)
      while (true) {
        string const_name;
        try {
          const_name = Match(Token.TOKENTYPE.ID).m_strName;
        } catch { break; }
        Match(Token.TOKENTYPE.EQUAL);
        int const_value = int.Parse(Match(Token.TOKENTYPE.INT_NUM).m_strName);
        m_sym.add(const_name, const_value);
        m_sym.commit();
        if(!isParam)
          Match(Token.TOKENTYPE.SEMI_COLON);
      }
    } // CONST ()

    /// <summary>
    /// PRE:  We have encountered the keyword PROCEDURE.
    /// POST: The procedure has been parsed and emitted as necessary. 
    ///    All relevant local variables and parameters have been loaded into
    ///    the Symbol Table.
    /// </summary>
    void PROCEDURE() {

      //YOU NEED TO FIGURE OUT WHAT AN ATTRIBUTE NEEDS TO HOLD IN ORDER TO
      //GENERATE PROPER PROCEDURE PROTOTYPES IN ASSEMBLY, AND THE ACTUAL
      // PROCEDURE AND CALL

      Match(Token.TOKENTYPE.PROCEDURE);
      string procName = Match(Token.TOKENTYPE.ID).m_strName;
      Match(Token.TOKENTYPE.LEFT_PAREN);
      m_sym.nest();
      m_emitter.NewProcedure(procName);
      DclList(true);
      Match(Token.TOKENTYPE.RIGHT_PAREN);
      Match(Token.TOKENTYPE.SEMI_COLON);
      DclList(false);

      // The Procedure's operations/statements begin with a BEGIN
      Match(Token.TOKENTYPE.BEGIN);

      StmtList(-1); // parse a sequence of statements

      // We are at the end of the procedure.
      Match(Token.TOKENTYPE.END);

      //Close the current procedure, knowing how much memory was required
      m_emitter.ProcPostamble(m_sym.Mem);

      //No need to keep track of non-existant variables, so clear the current scope
      m_sym.unnest();

      // The next token should be the procedure name given at first.
      if (Match(Token.TOKENTYPE.ID).m_strName != procName)
        // announce the error if it doesn't match
        throw new Exception("Procedure name not repeated at END of Procedure: " + procName
          + "\r\nNear line: " + m_tokens[m_index].m_iLineNum);

      // Otherwise we have the right token.
      Match(Token.TOKENTYPE.SEMI_COLON);

    } // Procedure()

    /// <summary>
    /// TYPE  Parse the declaration of a TYPE (M2 Users Manual).
    /// 
    /// This is currently used only to parse the desclaration of ARRAYs.
    ///    
    /// PRE:  The current token is TYPE.
    /// POST: All TYPEs are added to the symbol table.
    /// </summary>
    void TYPE(bool isParam) {
      ///TYPE	prListType = ARRAY [11 .. 30] of INTEGER ;
      Match(Token.TOKENTYPE.TYPE);
      string name = Match(Token.TOKENTYPE.ID).m_strName;
      Match(Token.TOKENTYPE.EQUAL);
      Match(Token.TOKENTYPE.ARRAY);
      Match(Token.TOKENTYPE.LEFT_BRACK);
      int startIndex = int.Parse(Match(Token.TOKENTYPE.INT_NUM).m_strName);
      Match(Token.TOKENTYPE.DOT_DOT);
      int endIndex = int.Parse(Match(Token.TOKENTYPE.INT_NUM).m_strName);
      Match(Token.TOKENTYPE.RIGHT_BRACK);
      Match(Token.TOKENTYPE.OF);
      Match(Token.TOKENTYPE.INTEGER);//for now, only support integers

      if(!isParam)
        Match(Token.TOKENTYPE.SEMI_COLON);

      m_sym.addType(name, startIndex, endIndex);
    } // Type()

    /// <summary>
    /// VAR  Parse the declaration of a variable (M2 Users Manual).
    /// 
    /// The behavior of this powerful function is altered by the bool
    ///    isParam; this is true while parsing the parameter list within
    ///    parentheses following the PROCEDURE name. Note that we may run into several
    ///    VAR's while parsing a DclList (VAR i:INTEGER: VAR j:INTEGER) or 
    ///    while parsing a procedure parameter list.
    ///    
    /// PRE:  false -> parsing a VAR in a DclList (default, and only option, for now)
    ///       true  -> parsing a proc. parameter list within parens.
    ///       Assume false for now. A boolean parameter will be added later for a proc. parameter list when needed.
    /// POST: All variables are added to the symbol table.
    /// </summary>
    void VAR(bool isParam) {
      if (!isParam)
        Match(Token.TOKENTYPE.VAR);

      while (true) {//match multiple types of variables, semicolon delimited

        while (true) {//match multiple variables of the same time, comma delimited
          try {
            m_sym.add(Match(Token.TOKENTYPE.ID).m_strName);
            Match(Token.TOKENTYPE.COMMA);
          } catch { break; }
        }

          Match(Token.TOKENTYPE.COLON);

        //only need to support integer variables for now. Add REAL or STRING to this later, when needed.
        if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.INTEGER) {
          Match(Token.TOKENTYPE.INTEGER);
          m_sym.commit();
        } else {
          try { m_sym.commitTypedVar((AttrType)m_sym.get(Match(Token.TOKENTYPE.ID).m_strName)); } 
          catch { throw new Exception("Invalid type near line " + m_tokens[m_index].m_iLineNum); }
        }

        if(!isParam)
          Match(Token.TOKENTYPE.SEMI_COLON);

        //continue matching variables until none are left
        if (m_tokens[m_index].m_tokType != Token.TOKENTYPE.ID)
          break;
      }
    } // VAR

    /// <summary>
    /// Emit appropriate code for each statement until an end or else is found.
    /// </summary>
    /// <param name="loopID">
    /// If an EXIT is found, generate appropriate code
    /// to leave the loop identified by this number.
    /// Make this negative when not in a loop to catch rogue EXIT statements
    /// </param>
    void StmtList(int loopID) {
      /*SAMPLE CODE FROM TEST04
        i := 3 ;
        WRINT ( i ) ;
        WRLN ;
      */
      bool noValidStatements = true;
      while (m_tokens[m_index].m_tokType != Token.TOKENTYPE.END && m_tokens[m_index].m_tokType != Token.TOKENTYPE.ELSE) {
        if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.ID) {
          if (m_tokens[m_index + 1].m_tokType == Token.TOKENTYPE.LEFT_PAREN)
            PROC_CALL(Match(Token.TOKENTYPE.ID).m_strName);
          else
            Assignment(Match(Token.TOKENTYPE.ID).m_strName);
          noValidStatements = false;
        }
        else if(m_tokens[m_index].m_tokType == Token.TOKENTYPE.WRINT){
          Match(Token.TOKENTYPE.WRINT);
          WRINT();
          noValidStatements = false;
        }
        else if(m_tokens[m_index].m_tokType == Token.TOKENTYPE.WRSTR){
          Match(Token.TOKENTYPE.WRSTR);
          WRSTR();
          noValidStatements = false;
        }
        else if(m_tokens[m_index].m_tokType == Token.TOKENTYPE.WRLN){
          Match(Token.TOKENTYPE.WRLN);
          WRLN();
          noValidStatements = false;
        }
        else if(m_tokens[m_index].m_tokType == Token.TOKENTYPE.IF){
          Match(Token.TOKENTYPE.IF);
          IfThenElse(loopID);
          noValidStatements = false;
        }
        else if(m_tokens[m_index].m_tokType == Token.TOKENTYPE.LOOP){
          Match(Token.TOKENTYPE.LOOP);
          LOOP();
          noValidStatements = false;
        }
        else if(m_tokens[m_index].m_tokType == Token.TOKENTYPE.EXIT){
          Match(Token.TOKENTYPE.EXIT);
          Match(Token.TOKENTYPE.SEMI_COLON);
          if(loopID < 0)
            throw new Exception("Not a valid EXIT since there is no current loop to leave at line: "
              + m_tokens[m_index].m_iLineNum);
          m_emitter.ExitLoop(loopID);
          noValidStatements = false;
        }
        if (noValidStatements == true)
          throw new Exception("No valid statement found while parsing on line: "
            + m_tokens[m_index].m_iLineNum
            + " near this token: "
            + m_tokens[m_index].m_strName);
        noValidStatements = true;
      }
      //Assignment();
    } // StmtList

    void PROC_CALL(string name) {
      List<object> args = new List<object>();
      Match(Token.TOKENTYPE.LEFT_PAREN);
      while (true) {
        if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.ID)
          args.Add(Match(Token.TOKENTYPE.ID).m_strName);
        else if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.INT_NUM)
          args.Add(Match(Token.TOKENTYPE.INT_NUM).m_strName);
        else if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.COMMA)
          Match(Token.TOKENTYPE.COMMA);
        else
          break;
      }
      Match(Token.TOKENTYPE.RIGHT_PAREN);
      m_emitter.ProcCall(name);
      Match(Token.TOKENTYPE.SEMI_COLON);
    }

    /// <summary>
    /// Perform an assignment.
    /// PRE:  We are looking at an ID token for a variable 
    ///    (simple or array) as the beginning of an assignment statement.
    ///    Attr refers to its entry in the symbol table.
    /// POST: Code is emitted to complete the assignment.
    /// </summary>
    /// <param name="name">variable name</param>
    void Assignment(string name) {
      //i := 3 ;
      //or
      //i[3] := 3;
      if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.LEFT_BRACK) {//Array variable
        Match(Token.TOKENTYPE.LEFT_BRACK);
        Expr();
        Match(Token.TOKENTYPE.RIGHT_BRACK);

        //[bp + offset + (index-startindex)*sizeoftype]
        m_emitter.FindArrayOffset(m_sym.getOffset(name), ((AttrVar)m_sym.get(name)).m_type.m_startIndex);

        Match(Token.TOKENTYPE.ASSIGN);
        Expr();
        m_emitter.SetInt();

      } else {//Non-array variable
        Match(Token.TOKENTYPE.ASSIGN);
        Expr();
        m_emitter.SetInt(m_sym.getOffset(name));
      }
      Match(Token.TOKENTYPE.SEMI_COLON);
    } // Assignment

    /// <summary>
    /// Write an integer literal or one stored in a variable in the symbol table
    /// </summary>
    void WRINT() {
      //WRINT ( 42 ) ;

      Expr();
      m_emitter.WRINT();
      Match(Token.TOKENTYPE.SEMI_COLON);
    }

    /// <summary>
    /// Writes a new line character
    /// </summary>
    void WRLN() {
      //WRLN ;

      m_emitter.WRLN();
      Match(Token.TOKENTYPE.SEMI_COLON);
    }

    /// <summary>
    /// Writes a string literal
    /// </summary>
    void WRSTR() {
      //WRSTR ( "Hello world!" ) ;

      if(m_tokens[m_index].m_tokType == Token.TOKENTYPE.LEFT_PAREN)
        Match(Token.TOKENTYPE.LEFT_PAREN);
      m_emitter.WRSTR(Match(Token.TOKENTYPE.STRING).m_strName);
      if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.RIGHT_PAREN)
        Match(Token.TOKENTYPE.RIGHT_PAREN);
      Match(Token.TOKENTYPE.SEMI_COLON);
    }

    /// <summary>
    /// PRE:  We found an "IF".
    /// POST: We emit all the code for this compound statement.
    /// </summary>
    /// <param name="loopID">If we are inside a loop, track the ID number to be able to process an EXIT statement</param>
    void IfThenElse(int loopID) {
      /*Test05.mod Example Excerpt
       * IF k > l THEN
            WRINT ( k ) ;
            WRLN ;
        ELSE
            WRINT ( l ) ;
            WRLN ;
        END ;*/
      //Left operand in a comparison can be an entire expression, which is immediately evaluated
      Expr();
      //Evaluate the right operand, and setup a jump statement to jump to a false labled block of code if the compare failed
      int comparisonID = BeginCompare();
      Match(Token.TOKENTYPE.THEN);
      //Output code to run, if the compare didn't jump to the false label
      StmtList(loopID);
      //Jump to the end of this block of code, past the false label
      m_emitter.JumpEndOfCompare(comparisonID);
      //Create the false label
      m_emitter.FalseCompareBlock(comparisonID);

      try {//Else block might not exist, so only fill in code uner the false label if it does
        Match(Token.TOKENTYPE.ELSE);
        //Output code to run in the case of a failed comparison
        StmtList(loopID);
      } catch { };

      Match(Token.TOKENTYPE.END);
      //Mark the end of this block with an end label
      m_emitter.EndOfCompare(comparisonID);
      Match(Token.TOKENTYPE.SEMI_COLON);
    }

    /// <summary>
    /// Find the next comparison operator, match it, evaluate the following expresion,
    /// emit code to jump to a true or false code block
    /// Return a unique number to identify this comparison block
    /// </summary>
    int BeginCompare() {
      switch (m_tokens[m_index].m_tokType) {
        case Token.TOKENTYPE.LESS_THAN:
          Match(Token.TOKENTYPE.LESS_THAN);
          Expr();
          m_emitter.LessThanOperation(m_IfCount);
          break;
        case Token.TOKENTYPE.GRTR_THAN:
          Match(Token.TOKENTYPE.GRTR_THAN);
          Expr();
          m_emitter.GreaterThanOperation(m_IfCount);
          break;
        case Token.TOKENTYPE.LESS_THAN_EQ:
          Match(Token.TOKENTYPE.LESS_THAN_EQ);
          Expr();
          m_emitter.LessThanEqOperation(m_IfCount);
          break;
        case Token.TOKENTYPE.GRTR_THAN_EQ:
          Match(Token.TOKENTYPE.GRTR_THAN_EQ);
          Expr();
          m_emitter.GreaterThanEqOperation(m_IfCount);
          break;
        case Token.TOKENTYPE.EQUAL:
          Match(Token.TOKENTYPE.EQUAL);
          Expr();
          m_emitter.EqualOperation(m_IfCount);
          break;
        case Token.TOKENTYPE.NOT_EQ:
          Match(Token.TOKENTYPE.NOT_EQ);
          Expr();
          m_emitter.NotEqualOperation(m_IfCount);
          break;
      }
      return m_IfCount++;
    }

    /// <summary>
    /// PRE:  We parsed "LOOP".
    /// POST: We emit all the code for this compound statement.
    /// </summary>
    void LOOP() {
      int loopID = m_LoopCount++;
      m_emitter.BeginLoop(loopID);
      StmtList(loopID);
      Match(Token.TOKENTYPE.END);
      Match(Token.TOKENTYPE.SEMI_COLON);
      m_emitter.EndLoop(loopID);
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
    //

    /// <summary>
    /// An expression is evaluated and the solution is pushed on the stack
    /// </summary>
    void Expr() {
      //  Expr           -> SimpleExpr RelOp SimpleExpr  |  SimpleExpr
      SimpleExpr();
    } // Expr ()

    void SimpleExpr() {
      //  SimpleExpr     -> Term RestSimpleExpr
      Term();
      RestSimpleExpr();
    } // SimpleExpr

    void RestSimpleExpr() {
      //  RestSimpleExpr -> + Term RestSimpleExpr  |  - Term RestSimpleExpr  |  OR Term RestSimpleExpr  |  epsilon
      switch (m_tokens[m_index].m_tokType) {
        case Token.TOKENTYPE.PLUS:
          Match(Token.TOKENTYPE.PLUS);
          Term();
          m_emitter.AddOperation();
          break;
        case Token.TOKENTYPE.MINUS:
          Match(Token.TOKENTYPE.MINUS);
          Term();
          m_emitter.MinusOperation();
          break;
        case Token.TOKENTYPE.OR:
          Match(Token.TOKENTYPE.OR);
          Term();
          m_emitter.BitwiseOrOperation();
          break;
        default://epsilon
          return;
      }
      RestSimpleExpr();
    } // RestSimpleExpr

    void Term() {
      //Term           -> Factor RestTerm
      Factor();
      RestTerm();
    }//Term

    void RestTerm() {
      //  RestTerm       -> *   Factor RestTerm  |  /    Factor RestTerm  |  
      // 				   MOD Factor RestTerm  |  AND  Factor RestTerm  |  epsilon
      switch (m_tokens[m_index].m_tokType) {
        case Token.TOKENTYPE.MULT:
          Match(Token.TOKENTYPE.MULT);
          Factor();
          m_emitter.MultOperation();
          break;
        case Token.TOKENTYPE.DIV:
          Match(Token.TOKENTYPE.DIV);
          Factor();
          m_emitter.DivOperation();
          break;
        case Token.TOKENTYPE.MOD:
          Match(Token.TOKENTYPE.MOD);
          Factor();
          m_emitter.ModOperation();
          break;
        case Token.TOKENTYPE.AND:
          Match(Token.TOKENTYPE.AND);
          Factor();
          m_emitter.AndOperation();
          break;
        default://epsilon
          return;
      }
      RestTerm();
    }//RestTerm

    void Factor() {
      //  Factor         -> ( Expr )  |  Value   |  NOT Factor (M2 User's Manual pp. 109f.)
      switch (m_tokens[m_index].m_tokType) {
        case Token.TOKENTYPE.LEFT_PAREN:
          Match(Token.TOKENTYPE.LEFT_PAREN);
          Expr();
          Match(Token.TOKENTYPE.RIGHT_PAREN);
          break;
        case Token.TOKENTYPE.ID:
        case Token.TOKENTYPE.INT_NUM:
          Value();
          break;
        case Token.TOKENTYPE.NOT:
          Factor();
          m_emitter.NotOperation();
          break;
        default:
          return;
      }
    }//Factor

    /// <summary>
    /// A value is an integer, variable, or constant
    /// </summary>
    void Value() {
      Token t = m_tokens[m_index];
      

      if (t.m_tokType == Token.TOKENTYPE.INT_NUM) {
        //If a integer literal is found, push it onto the stack
        Match(Token.TOKENTYPE.INT_NUM);
        m_emitter.PushInt(int.Parse(t.m_strName));

      } else if (t.m_tokType == Token.TOKENTYPE.ID) {
        string name = t.m_strName;

        Match(Token.TOKENTYPE.ID);
        if (m_sym.get(name) is AttrConst)
          //If a constant is found, push its value onto the stack
          m_emitter.PushInt((int)((AttrConst)m_sym.get(name)).m_value);
        else
          //If a variable is found, push the value at its offset in the stack onto the stack

          if (m_tokens[m_index].m_tokType == Token.TOKENTYPE.LEFT_BRACK) {//found array reference
            Match(Token.TOKENTYPE.LEFT_BRACK);

            Expr();
            m_emitter.PushArrayVar(m_sym.getOffset(name), ((AttrVar)m_sym.get(name)).StartIndex);

            Match(Token.TOKENTYPE.RIGHT_BRACK);

          } else {//found a standard variable
            m_emitter.PushVar(m_sym.getOffset(name));
          }
      }
    } // Value ()    
  }
}