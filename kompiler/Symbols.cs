using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace kompiler {
  class Symbols {
    //A stack of variable tables, where the variable name is a hash key to its attributes
    Stack<Dictionary<string, Attribute>> m_scopes = new Stack<Dictionary<string,Attribute>>();

    //A stack of variables that will be added during the next comit()
    Dictionary<string, object> m_incomplete_vars = new Dictionary<string, object>();

    Attribute.ID_CAT m_cat;//track the current category when defining multiple variables

    //consider this exerpt from All.mod :
    //VAR k, m : INTEGER ;  //k and m are both of category VAR
    //k and m will be put on the incomplete vars stack
    //k and m will be commited to the latest scope on the scopes stack when
    //   commit(INTEGER) is called with a variable type as the parameter (in this case INTEGER)


    // The single object instance for this class.
    private static Symbols c_symbols;

    // To prevent access by more than one thread. This is the specific lock 
    //    belonging to the Symbols Class object.
    private static Object c_symbolsLock = typeof(Symbols);

    // Instead of a constructor, we offer a static method to return the only
    //    instance.
    private Symbols() { // private constructor so no one else can create one.
      m_scopes.Push(new Dictionary<string, Attribute>());//default global scope
    }

    static public Symbols GetSymbols() {
      lock (c_symbolsLock) {
        // if this is the first request, initialize the one instance
        if (c_symbols == null) {
          // create the single object instance
          c_symbols = new Symbols();
        }

        // return a reference to the only instance
        return c_symbols;
      }
    }

    /// <summary>
    /// Get the memory usage in the current scope
    /// </summary>
    public int Mem {
      get {
        int i = 0;
        foreach (KeyValuePair<string, Attribute> var in m_scopes.Peek())
          switch (var.Value.m_var_type) {
            case Attribute.VAR_TYPE.INTEGER:
              i += 2;
              break;
          }
        return i;
      }
    }

    /// <summary>
    /// Create a new scope
    /// </summary>
    public void nest() {
      m_scopes.Push(new Dictionary<string, Attribute>());
    }

    /// <summary>
    /// Destroy the latest scope
    /// </summary>
    public void unnest() {
      m_scopes.Pop();
    }

    /// <summary>
    /// Get the Attribute for a variable, by name, in the nearest scope possible
    /// If no variable is named such, return null
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Attribute get(string name) {
      foreach(Dictionary<string, Attribute> scope in m_scopes)
        if (scope.ContainsKey(name))
          return scope[name];
      return null;
    }

    /// <summary>
    /// Dump all the variables in all the scopes into a human readable string for debugging
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      string dump = "";
      int i = m_scopes.Count;
      foreach (Dictionary<string, Attribute> scope in m_scopes) {
        dump += "\nScope " + --i;
        foreach (KeyValuePair<string, Attribute> var in scope)
          dump += "\n" + var.Key + ": " + var.Value.ToString();
      }
      return dump;
    }

    /// <summary>
    /// Cache a variable's name, until the type is known, at which point call setType(VAR_TYPE)
    /// The most recent call to setCat(ID_CAT) will be used for the variable's category
    /// Value will default to null
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cat"></param>
    public void add(string name) {
      m_incomplete_vars.Add(name, null);
    }

    /// <summary>
    /// Cache a variable's name and value, until the type is known, at which point call setType(VAR_TYPE)
    /// The most recent call to setCat(ID_CAT) will be used for the variable's category
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void add(string name, object value) {
      m_incomplete_vars.Add(name, value);
    }

    /// <summary>
    /// When a type is declared after any number of variables have been added,
    /// set all cached variables to that type and add to the current scope
    /// Clear the cached variables afterwards
    /// </summary>
    /// <param name="type"></param>
    public void commit(Attribute.VAR_TYPE type) {
      foreach (KeyValuePair<string, object> var in m_incomplete_vars)
        //default all variable values to null
        m_scopes.Peek()[var.Key]= new Attribute(m_cat, type, var.Value);
      m_incomplete_vars.Clear();
    }

    /// <summary>
    /// Set the category for the following variables that will be added
    /// </summary>
    /// <param name="cat"></param>
    public void beginCategory(Attribute.ID_CAT cat){
      m_cat = cat;
    }

    /// <summary>
    /// Set a variable's value, using the variable, by name, in the closest scope
    /// If no such named variable exists, nothing is changed
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void setVal(string name, object value) {
      foreach (Dictionary<string, Attribute> scope in m_scopes)
        if (scope.ContainsKey(name)) {
          scope[name].m_value = value;
          break;
        }
    }
  }
}
