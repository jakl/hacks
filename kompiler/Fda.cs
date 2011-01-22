using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kompiler {
  /// <summary>
  /// A dead class that was going to be my lexer.
  /// I started work on this finite state automata table but it was too tedious and time consuming.
  /// It is here now to help me remember a critical concept I learned:
  ///   Dictionary literals.
  /// </summary>
  class Fda {
    public enum state {
      START, WORD, END_WORD, DIGIT, END_DIGIT,
      //    a-zA-Z           0-9
      OPEN_PAREN, CLOSE_PAREN, COLON, SEMICOLON, ASSIGN,
      //   (          )          :         ;        :=
      OPEN_COMMENT, CLOSE_COMMENT, QUOTE, QUOTEQUOTE, END_QUOTE,
      //  (*             *)
      EQUAL, LT, GT, LE, GE, NE,
      // =    <   >  <=  >=  <>
      MULTI, PLUS, MINUS, DOT, DOTDOT,
      // *     +     -      .    ..
      SPACE, OPEN_BRACKET, CLOSE_BRACKET
      //        [                ]
    };

    Dictionary<state, Dictionary<char, state>> table =
        new Dictionary<state, Dictionary<char, state>>()
    {
      { state.START, new Dictionary <char, state> () {
        {'w',state.WORD},{'d',state.DIGIT},{'s',state.SPACE},
        {'(',state.OPEN_PAREN},{')',state.CLOSE_PAREN},{':',state.COLON},
        {';',state.SEMICOLON},{'=',state.EQUAL},{':',state.COLON},
      }}
    };
  }
}