using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace kompiler {
  /// <summary>
  /// A token is a category of lexemes.
  /// Some tokens are keywords like "MODULE". Some tokens are symbols; the token of type
  ///    PLUS is the symbol "+". Some tokens are a large set; some tokens of type
  ///    INTEGER are "73" and "255".
  /// The enumerations below include all the tokens that we need to recognize.
  /// 
  /// Note that the Modula-2 User's Manual offers both # and <> to mean NOT EQUAL 
  ///    (pp. 96, 97, and 110). Our code does not use #. We only use <>.
  /// 
  /// Many errors are reported by TOKENTYPE which is just a number (like 34 for T_ID).
  /// Therefore, I added enumeration numbers for tokens in the left column as a
  ///    convenience during debugging.
  ///    
  /// Author: Tom Fuller
  /// Date:   January 6, 2007
  /// </summary>
  public class Token {
    public enum TOKENTYPE {
      /* 0*/
      ERROR = 0, AND, ARRAY, BEGIN,
      /* 4*/
      CARDINAL, CONST, DIV, DO,
      /* 8*/
      ELSE, END, EXIT, FOR, IF,
      /*13*/
      //MODULE & MOD were switched for my convience
      INTEGER, LOOP, MODULE, MOD,
      /*17*/
      NOT, OF, OR, PROCEDURE,
      /*21*/
      REAL, THEN, TYPE, VAR,
      /*25*/
      WHILE, WRCARD, WRINT, WRLN,
      /*29*/
      WRREAL, RDCARD, RDINT, RDREAL, WRSTR,

      /*34*/
      ID, CARD_NUM, REAL_NUM, INT_NUM,

      /*38*/
      STRING,

      /*39*/
      ASSIGN, COLON, COMMA, DOT,
      /*43*/
      DOT_DOT, EQUAL, LEFT_BRACK, LEFT_PAREN,
      /*47*/
      MINUS, MULT, NOT_EQ, PLUS,
      /*51*/
      RIGHT_BRACK, RIGHT_PAREN, SEMI_COLON, SLASH,
      /*55*/
      GRTR_THAN, GRTR_THAN_EQ, LESS_THAN, LESS_THAN_EQ,

      LEFT_BRACE, RIGHT_BRACE, ANDAND, OROR,

   
      COMMENT, EOF
    };

    /*********************************************************************************
     There are 34 reserved keywords we need to recognize in Modula-2 source code files.
        Note that enumerated types 0 - 33 above correspond precisely to these keywords. 
        Some number types are not used except by very ambitious students: CARDINAL, REAL.
        Some statements are also reserved for the fearless: WHILE, DO, FOR.
    *********************************************************************************/

    public static int c_iKeywordCount = 34;

    /*********************************************************************************
    Every token that requires a regex to match it, is in this Dictionary of token types to regexes
    *********************************************************************************/
    public static readonly Dictionary<TOKENTYPE, Regex> regexes = new Dictionary<TOKENTYPE, Regex>(){
      { TOKENTYPE.ID , new Regex(@"^[a-zA-Z_][_a-zA-Z0-9]*") },{ TOKENTYPE.REAL_NUM , new Regex(@"^[+-]?\d+\.\d*([eE][+-]?\d*)?") },
      { TOKENTYPE.INT_NUM , new Regex(@"^\d+") },{ TOKENTYPE.COMMENT , new Regex(@"^\(\*.*?\*\)", RegexOptions.Singleline) },
      //This string regex will not perform as expected for: "This is a string where a single quote really does end it'
      //I was having trouble with back references like \1 in the following regex: ^(["']).*?[^\1]\1[^\1]
      { TOKENTYPE.STRING , new Regex("^[\"'].*?[^\"'][\"'][^\"']") }
    };

    /*********************************************************************************
    Every token that requires a single character to match it, is in this Dictionary of token types to chars
    *********************************************************************************/

    public static readonly Dictionary<TOKENTYPE, char> singlechars = new Dictionary<TOKENTYPE, char>(){
      {TOKENTYPE.COLON, ':' }, {TOKENTYPE.COMMA, ',' }, {TOKENTYPE.DOT, '.' }, {TOKENTYPE.EQUAL, '='},
      {TOKENTYPE.LEFT_BRACK, '['}, {TOKENTYPE.LEFT_PAREN, '('}, {TOKENTYPE.MINUS, '-'}, {TOKENTYPE.MULT, '*'},
      {TOKENTYPE.PLUS, '+'}, {TOKENTYPE.RIGHT_BRACK, ']'}, {TOKENTYPE.RIGHT_PAREN, ')'},
      {TOKENTYPE.SEMI_COLON, ';'}, {TOKENTYPE.SLASH, '/'}, {TOKENTYPE.LESS_THAN, '<'}, {TOKENTYPE.GRTR_THAN, '>'},
      {TOKENTYPE.LEFT_BRACE, '{'}, {TOKENTYPE.RIGHT_BRACE, '}'}
    };

    /*********************************************************************************
    Every token that requires multiple characters to match it, is in this Dictionary of token types to chars
    *********************************************************************************/

    public static readonly Dictionary<TOKENTYPE, string> multichars = new Dictionary<TOKENTYPE, string>(){
      {TOKENTYPE.ASSIGN, ":="}, {TOKENTYPE.DOT_DOT, ".."}, { TOKENTYPE.NOT_EQ,"<>"}, {TOKENTYPE.LESS_THAN_EQ,"<="}, 
      {TOKENTYPE.GRTR_THAN_EQ,">="}, {TOKENTYPE.ANDAND, "&&"}, {TOKENTYPE.OROR, "||"}
    };

    /*********************************************************************************
      The CToken class itself stores information about a single token.
     *********************************************************************************/
    public TOKENTYPE m_tokType;    /* what token (i.e. class of lexemes)          */
    public string m_strName;    /* lexeme name: reserved word, identifier      */
    public int m_iLineNum;   /* the (source file)line containing the token  */

    // default constructor, not used
    public Token() {
      m_tokType = Token.TOKENTYPE.ERROR;
      m_strName = "blank token";
      m_iLineNum = 0;
    }

    // lexing error, gives lexeme and line number
    public Token(string inName, int inLine) {
      m_tokType = Token.TOKENTYPE.ERROR;
      m_strName = inName;
      m_iLineNum = inLine;
    }

    // normal constructor with tokentype, lexeme and line number
    public Token(Token.TOKENTYPE tok, string inName, int inLine) {
      m_tokType = tok;
      m_strName = inName;
      m_iLineNum = inLine;
    }

    public override string ToString() {
      return string.Format("{3,3}:Token {0,-2}: {1,-15} {2,18}", (int)m_tokType,
          m_tokType.ToString(), m_strName, m_iLineNum);
    }
  } // class Token
}