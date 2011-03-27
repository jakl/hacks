using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kompiler {
  class AttrProc : Attribute {

    //parameters tracked for ref or value passing?

    public override string ToString() {
      return "Procedure";
    }
  }
}
