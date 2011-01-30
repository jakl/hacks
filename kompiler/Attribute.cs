using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kompiler {
  class Attribute {

    //The category of an identifier/variable
    public enum ID_CAT {
      CONST, TYPE, VAR, PROC
    }

    //The type of a variable/identifier
    public enum VAR_TYPE {
      INTEGER, REAL, STRING, CHAR, CARDINAL
    }

    public ID_CAT m_id_cat;
    public VAR_TYPE m_var_type;
    public object m_value;

    //dump the above 3 instanced data members to a string
    public override string ToString() {
      string ret = m_id_cat.ToString() + " " + m_var_type.ToString() + " ";
      if (m_value is int)
        ret += (int)m_value;
      else if (m_value is string)
        ret += (string)m_value;
      return ret;
    }


    public Attribute(ID_CAT cat, VAR_TYPE type, object value) {
      m_id_cat = cat;
      m_var_type = type;
      m_value = value;
    }
  }
}