using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kompiler {
  class Scope {
    //Track variables, constants, types, and procedures
    public Dictionary<string, Attribute> m_attrs = new Dictionary<string,Attribute>();

    //Keep track of where variables should be placed next on the stack as they are declared, so they don't overlap
    public int m_offset = 4;

    public void set(string name, Attribute attr) {
      m_attrs[name] = attr;
    }

    public Attribute get(string name) {
      return m_attrs[name];
    }

    public bool contains(string name) {
      return m_attrs.ContainsKey(name);
    }
  }
}
