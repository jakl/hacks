using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kompiler {
  class AttrConst : Attribute {
    public int m_value;

    public AttrConst(int value) {
      m_value = value;
    }

    public override string ToString() {
      return "Const " + m_value;
    }
  }
}
