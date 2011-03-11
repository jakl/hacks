using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kompiler {
  class Attribute {
    public const int INTEGER_SIZE = 4;

    /// <summary>
    /// Get the stack memory used by this item. 
    /// Only AttrVar items consume memory. 
    /// Only AttrVar items with a specific type consume anything other than 4 bytes. 
    /// </summary>
    public int Mem {
      get {
        if (!(this is AttrVar))//Not on the stack, thus consuming zero memory
          return 0;
        AttrType type = ((AttrVar)this).m_type;
        if (type == null)
          return INTEGER_SIZE;
        return (type.m_endIndex - type.m_startIndex + 1) * INTEGER_SIZE;
      }
    }
  }
}