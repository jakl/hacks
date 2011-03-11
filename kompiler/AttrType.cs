using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kompiler {
  class AttrType : Attribute {
    public int m_startIndex, m_endIndex;

    public AttrType(int startIndex, int endIndex) {
      m_startIndex = startIndex;
      m_endIndex = endIndex;
    }

    public override string ToString() {
      return "Array Type indexed from " + m_startIndex + " to " + m_endIndex;
    }
  }
}
