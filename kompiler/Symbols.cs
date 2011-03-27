using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace kompiler {
  class Symbols {
    //A stack of variable tables, where the variable name is a hash key to its attributes
    Stack<Scope> m_scopes = new Stack<Scope>();

    //A list of variables that will be added during the next comit()
    List<string> m_Vars = new List<string>();
    Dictionary<string, int> m_Consts = new Dictionary<string, int>();

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
      m_scopes.Push(new Scope());//default global scope
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
    /// Initialize symbol system for fresh use
    /// </summary>
    public void init() {
      m_scopes = new Stack<Scope>();
      m_scopes.Push(new Scope());
      m_Vars = new List<string>();
      m_Consts = new Dictionary<string, int>();
    }

    /// <summary>
    /// Get the memory usage in the current scope
    /// </summary>
    public int Mem {
      get {
        return m_scopes.Peek().m_offset-4;
      }
    }

    /// <summary>
    /// Create a new scope
    /// </summary>
    public void nest() {
      m_scopes.Push(new Scope());
    }

    /// <summary>
    /// Destroy the latest scope
    /// </summary>
    public void unnest() {
      m_scopes.Pop();
    }

    /// <summary>
    /// Add an array type to the current scope, to be used as a possible type of future variables.
    /// Note: Not that great of an idea. This needs to be subclassed and organized so variables can have more dynamic types
    /// </summary>
    /// <param name="name"></param>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    /// <param name="type"></param>
    public void addType(string name, int startIndex, int endIndex) {
      //Attribute type is always integer for now
      m_scopes.Peek().set(name, new AttrType(startIndex, endIndex));
    }

    /// <summary>
    /// Get a non-var attribute, by name, in the nearest scope possible
    /// If no variable is named such, return null
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Attribute get(string name) {
      foreach (Scope scope in m_scopes)
        if (scope.contains(name))
          return scope.get(name);
      return null;
    }

    /// <summary>
    /// Get the Attribute for a variable, by name, in the nearest scope possible. 
    /// If no variable is named such, return null. 
    /// It may be nested, in which case the nestedOffset will have the proper offset to add to the 
    /// normal variable's own offset within its own scope
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int getOffset(string name) {
      int nestedOffset = 0;
      foreach (Scope scope in m_scopes) {
        if (scope.contains(name))
          return ((AttrVar)scope.get(name)).m_offset + nestedOffset;
        nestedOffset += scope.m_offset;
      }
      return nestedOffset;
    }

    /// <summary>
    /// Dump all the variables in all the scopes into a human readable string for debugging
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      string dump = "";
      int i = m_scopes.Count;
      foreach (Scope scope in m_scopes) {
        dump += "Scope " + --i + " using " + (scope.m_offset-4) + " bytes of memory for variables\r\n";
        foreach (KeyValuePair<string, Attribute> var in scope.m_attrs)
          dump += var.Key + ": \t\t\t" + var.Value.ToString() + "\r\n";
      }
      return dump;
    }

    /// <summary>
    /// Cache a variable's name, until the type is known, at which point call commit
    /// </summary>
    /// <param name="name"></param>
    public void add(string name) {
      m_Vars.Add(name);
    }

    /// <summary>
    /// Cache a constant until a commit call to commit all cached constants
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void add(string name, int value) {
      m_Consts.Add(name, value);
    }

    /// <summary>
    /// When a type is declared after any number of variables have been added,
    /// set all cached variables to that type and add them to the current scope. 
    /// The type is always an array. 
    /// Clear the cached variables afterwards
    /// </summary>
    /// <param name="type"></param>
    public void commitTypedVar(AttrType type) {
      Scope curScope = m_scopes.Peek();

      //Add variables by name mapped to their attribute to the top hashmap of a stack of hashes
      foreach (String var in m_Vars) {
        curScope.set(var, new AttrVar(curScope.m_offset, type));

        //make room on the stack for this array
        curScope.m_offset += Attribute.INTEGER_SIZE * (type.m_endIndex - type.m_startIndex + 1);
      }
      m_Vars.Clear();//clear the cached variables
    }

    /// <summary>
    /// When finished declaring variable or constant integers, add them to the current scope
    /// Clear the cached variables afterwards
    /// </summary>
    /// <param name="type"></param>
    public void commit() {
      Scope curScope = m_scopes.Peek();

      //Add variables by name mapped to their attribute to the top hashmap of a stack of hashes
      foreach (KeyValuePair<string, int> constant in m_Consts)
        //Add the constant
        curScope.set(constant.Key, new AttrConst(constant.Value));
      foreach (String var in m_Vars)
        //Add the var and increment the stack by 4
        curScope.set(var, new AttrVar(curScope.m_offset += Attribute.INTEGER_SIZE));

      //clear any cached variables or constants
      m_Vars.Clear();
      m_Consts.Clear();
    }

    /// <summary>
    /// Set a variable's value, using the variable, by name, in the closest scope
    /// If no such named variable exists, nothing is changed
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void setVal(string name, int value) {
      foreach (Scope scope in m_scopes)
        if (scope.contains(name)) {
          try { ((AttrConst)scope.get(name)).m_value = value; } catch { }
          break;
        }
    }
  }
}
