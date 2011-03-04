using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kompiler {
  class Attribute {

    public const int INTEGER_SIZE = 4;

    //The category of an identifier/variable
    public enum CATEGORY {
      //leaving unused categories out of the picture until needed
      CONST, VAR//, PROC, TYPE 
    }

    //The type of a variable/identifier
    public enum TYPE {
      //leaving unused types out of the picture until needed
      INTEGER//, REAL, STRING, CHAR, CARDINAL
    }

    public CATEGORY m_category;
    public TYPE m_type;
    public object m_value;
    public int m_offset;
    public int Mem {
      get {
        switch (m_type) {
          case TYPE.INTEGER: 
            //constants take up no memory on the stack; ignore them
            if(m_category != CATEGORY.CONST)
              return INTEGER_SIZE;
            break;
        }
        return 0;
      }
    }

    //dump the instanced data members to a string
    public override string ToString() {
      string ret = m_category.ToString() + " " + m_type.ToString() + " ";
      if (m_value is int)
        ret += (int)m_value;
      else if (m_value is string)
        ret += (string)m_value;
      return ret;
    }


    public Attribute(CATEGORY cat, TYPE type, int offset, object value) {
      m_category = cat;
      m_type = type;
      m_offset = offset;
      m_value = value;
    }
  }
}