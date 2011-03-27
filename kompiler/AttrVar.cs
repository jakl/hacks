using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kompiler {
  class AttrVar : Attribute{
    public int m_offset;
    public AttrType m_type;

    public int StartIndex {
      get {
        if (m_type == null) return 0;
        return m_type.m_startIndex;
      }
    }

    public int EndIndex {
      get {
        if (m_type == null) return 0;
        return m_type.m_endIndex;
      }
    }

    /// <summary>
    /// Define a scalar
    /// </summary>
    /// <param name="offset"></param>
    public AttrVar(int offset) {
      m_offset = offset;
      m_type = null;//Single integers have the default null type
    }

    /// <summary>
    /// Define an array
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    public AttrVar(int offset, AttrType type) {
      m_offset = offset;
      m_type = type;
    }

    public override string ToString() {
      if (m_type == null)
        return "Scalar at offset " + m_offset;
      return "Array indexed from " + m_type.m_startIndex + " to " + m_type.m_endIndex
        + " at offset " + m_offset;
    }
  }
}
