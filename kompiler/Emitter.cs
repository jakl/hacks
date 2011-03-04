using System.Collections;   // ArrayList
using System.IO;
using System;

/// <summary>
/// Emitter is a singleton class that creates assembler files. It works with the
/// FileDefaultManager to keep track of what assembler code to write to what file.
/// A number of assembler files are used during compilations:
///    string.inc
///    PV999999Main_thf.inc
///    PV000001towers.inc
///    etc.
/// </summary>
namespace kompiler {
  public class Emitter {
    //single reference to the emitter
    private static Emitter emitter;

    // To prevent access by more than one thread. This is the specific lock 
    //    belonging to the Class object.
    private static Object emitterlock = typeof(Emitter);

    // Instead of a constructor, we offer a static method to return the only
    //    instance.
    private Emitter() { } // private constructor so no one else can create one.

    static public Emitter GetEmitter() {
      lock (emitterlock) {
        // if this is the first request, initialize the one instance
        if (emitter == null)
          emitter = new Emitter();

        // return a reference to the only instance
        return emitter;
      }
    }

    int m_strCount = 1;

    //Handles all file operations
    FileWriter fw;

    /// <summary>
    /// Setup the emitter for a new compilation project
    /// </summary>
    /// <param name="projectName"></param>
    public void init(string projectName) {
      fw = new FileWriter(projectName);

      //reset the string counter
      m_strCount = 1;
    }

    /// <summary>
    /// Closes file manager
    /// </summary>
    public void Close() {
      fw.CloseFiles();
      fw.ResetWorkingDirectory();
    }

    /// <summary>
    /// Move the int literal into a register to ensure it is 4 bytes wide, then push it onto the stack
    /// </summary>
    /// <param name="value"></param>
    public void PushInt(int value) {
      fw.Add("mov eax, " + value);
      fw.Add("push eax");
    }

    /// <summary>
    /// Push a variable from the depths of the stack onto the top of the stack
    /// </summary>
    /// <param name="offset"></param>
    public void PushVar(int offset) {
      fw.Add("push [ebp+" + offset + "]");
    }

    /// <summary>
    /// PRE:  The desired output on the top of the run-time stack.
    /// POST: The integer is displayed.
    /// </summary>
    public void WRINT() {
      fw.Add("pop eax");
      fw.Add("invoke dwtoa, eax, addr int_buffer");
      fw.Add("invoke StdOut, addr int_buffer");
    }

    /// <summary>
    /// Write a new line
    /// </summary>
    public void WRLN() {
      fw.Add("invoke StdOut, addr nwln_string");
    }

    /// <summary>
    /// Write a string literal to the proper files to make it appear on the screen
    /// </summary>
    /// <param name="strIn"></param>
    public void WRSTR(string strIn) {
      string strName = "str" + m_strCount;
      fw.DeclString(strName + " DB '" + strIn + "', 0");
      fw.Add("invoke StdOut, addr " + strName);
      m_strCount++;
    }

    /// <summary>
    /// Pop an int off the stack and assign an int var within the depths of the stack to that value
    /// </summary>
    /// <param name="offset"></param>
    public void SetInt(int offset) {
      fw.Add("pop eax");
      fw.Add("mov [ebp+" + offset + "], eax");
    }


    //This function is not yet used since multiple procedures aren't implemented
    //The main procedure is created by default by the constructors
    /// <summary>
    /// PRE:  The name of the procedure is passed. We have already called 
    ///    EnterNewProcScope which tracks the current scope number. Note that this
    ///    array of procedure strings must remain parallel to the array of procedures
    ///    maintained by SymbolTable.
    /// POST: The preamble is emitted. This includes creating the assembly string
    ///    and increasing the procedure index.
    /// </summary>
    //public void ProcPreamble(string strProcName) {
    //  m_curProc = strProcName;
    //}

    /// <summary>
    /// PRE:  The name of the procedure and 
    ///    the amount of memory needed in the stack is passed.
    ///    SymbolTable.ExitProcScope() has been called to re-establish 
    ///    the correct new scope.
    /// POST: The postamble is emitted to the current string.
    ///    The procedure index is returned to the correct value
    ///    (by querying SymbolTable).
    ///    
    /// Note the special version for the main procedure.
    /// </summary>
    public void ProcPostamble(int memUse) {
      fw.EndProcedure(memUse);
    }


    // #########################################################################################
    // A R I T H M E T I C               A R I T H M E T I C               A R I T H M E T I C      
    // #########################################################################################
    //Optimize this section later to modify the top of the stack directly rather than using two registers

    /// <summary>
    /// PRE:  The second operand is on the top of the stack. The first is next on the stack.
    /// POST: The sum is on the top of the stack.
    /// </summary>
    public void AddOperation() {
        fw.Add("pop eax\r\n"
                + "pop ebx\r\n"
                + "add eax, ebx\r\n"
                + "push eax");
    }

    /// <summary>
    /// PRE:  The second operand is on the top of the stack. The first is next on the stack.
    /// POST: The difference is on the top of the stack.
    /// </summary>
    public void MinusOperation() {
        fw.Add("pop ebx\r\n"
                + "pop eax\r\n"
                + "sub eax, ebx\r\n"
                + "push eax");
    }

    /// <summary>
    /// PRE:  The second operand is on the top of the stack. The first is next on the stack.
    /// POST: The answer is on the top of the stack.
    /// </summary>
    public void MultOperation() {
        /*fw.Add("pop eax\r\n"
                + "pop ebx\r\n"
                + "imul bx\r\n"//assume the numbers are 16 bits, and fit in the space of just bx & ax
                + "sal edx, 16\r\n"//dx holds the high bits, so move them to the high bits of edx
                + "add eax, edx\r\n"//add the high bits of edx with the low bits of ax
                + "push eax");*/
      //according to http://www.cs.virginia.edu/~evans/cs216/guides/x86.html, this simpler multiply can be used
      fw.Add("pop eax\r\n"
                + "pop ebx\r\n"
                + "imul eax, ebx\r\n"//assume the numbers are 16 bits, and fit in the space of just bx & ax
                + "push eax");
    }

    /// <summary>
    /// PRE:  The second operand is on the top of the stack. The first is next on the stack.
    /// POST: The answer is on the top of the stack.
    /// </summary>
    public void DivOperation() {
        /*fw.Add("pop ebx\r\n"
                + "pop eax\r\n"
                + "idiv bx\r\n"
                + "push eax");*/
        fw.Add("mov edx, 0\r\n"
                  +"pop ebx\r\n"
                  + "pop eax\r\n"
                  + "idiv ebx\r\n"
                  + "push eax");
    }

    /// <summary>
    /// PRE:  The second operand is on the top of the stack. The first is next on the stack.
    /// POST: The answer is on the top of the stack.
    /// </summary>
    public void ModOperation() {
      fw.Add("mov edx, 0\r\n"
                +"pop ebx\r\n"
                + "pop eax\r\n"
                + "idiv ebx\r\n"
                + "push edx");
    }

    /// <summary>
    /// PRE:  The second operand is on the top of the stack. The first is next on the stack.
    /// POST: The difference is on the top of the stack.
    /// </summary>
    public void BitwiseOrOperation() {
      fw.Add("pop eax\r\n"
                + "pop ebx\r\n"
                + "or eax, ebx\r\n"
                + "push eax");
    }

    /// <summary>
    /// PRE:  The second operand is on the top of the stack. The first is next on the stack.
    /// POST: The difference is on the top of the stack.
    /// </summary>
    public void AndOperation() {
      fw.Add("pop eax\r\n"
                + "pop ebx\r\n"
                + "and eax, ebx\r\n"
                + "push eax");
    }

    /// <summary>
    /// PRE:  The operand is on the top of the stack.
    /// POST: The answer is on the top of the stack.
    /// </summary>
    public void NotOperation() {
        fw.Add("pop eax\r\n"
                + "not eax\r\n"
                + "push eax");
    }

    /// <summary>
    /// Compare the integer next to first on the stack, with the first integer on the stack.
    /// Integers are 4 bytes
    /// </summary>
    private void CompareOperation() {
      fw.Add("pop ebx\r\n"
                + "pop eax\r\n"
                + "cmp eax, ebx");
    }

    /// <summary>
    /// PRE: The second operand is on the top of the stack and the first is next
    /// POST: Jump to the appropriate false label, if first is not less than second
    /// True code is assumed to not require a jump
    /// </summary>
    public void LessThanOperation(int id) {
      CompareOperation();
      fw.Add("jge false" + id);
    }

    /// <summary>
    /// PRE: The second operand is on the top of the stack and the first is next
    /// POST: Jump to the appropriate false label, if first is not less than/equal to second
    /// True code is assumed to not require a jump
    /// </summary>
    public void LessThanEqOperation(int id) {
      CompareOperation();
      fw.Add("jg false" + id);
    }

    /// <summary>
    /// PRE: The second operand is on the top of the stack and the first is next
    /// POST: Jump to the appropriate false label, if first is not greater than second
    /// True code is assumed to not require a jump
    /// </summary>
    public void GreaterThanOperation(int id) {
      CompareOperation();
      fw.Add("jle false" + id);
    }

    /// <summary>
    /// PRE: The second operand is on the top of the stack and the first is next
    /// POST: Jump to the appropriate false label, if first is not greater than/equal to second
    /// True code is assumed to not require a jump
    /// </summary>
    public void GreaterThanEqOperation(int id) {
      CompareOperation();
      fw.Add("jl false" + id);
    }

    /// <summary>
    /// PRE: The second operand is on the top of the stack and the first is next
    /// POST: Jump to the appropriate false label, if first is not equal to second
    /// True code is assumed to not require a jump
    /// </summary>
    public void EqualOperation(int id) {
      CompareOperation();
      fw.Add("jne false" + id);
    }

    /// <summary>
    /// PRE: The second operand is on the top of the stack and the first is next
    /// POST: Jump to the appropriate false label, if first is equal to second
    /// True code is assumed to not require a jump
    /// </summary>
    public void NotEqualOperation(int id) {
      CompareOperation();
      fw.Add("je false" + id);
    }

    /// <summary>
    /// Finish a comparison block by jumping to the end, skipping the false lable usually
    /// </summary>
    public void JumpEndOfCompare(int id) {
      fw.Add("jmp endif" + id);
    }

    /// <summary>
    /// Begin code to run after a comparison evaluated to false
    /// </summary>
    public void FalseCompareBlock(int id) {
      fw.Add("false" + id + ":");
    }

    /// <summary>
    /// End of code block relating to a comparison
    /// </summary>
    public void EndOfCompare(int id) {
      fw.Add("endif" + id + ":");
    }

    public void BeginLoop(int id) {
      fw.Add("loop" + id + ":");
    }

    public void ExitLoop(int id) {
      fw.Add("jmp endloop" + id);
    }

    public void EndLoop(int id) {
      fw.Add("jmp loop" + id);
      fw.Add("endloop" + id + ":");
    }

    // #########################################################################################
    // FILE HANDLER METHODS   FILE HANDLER METHODS   FILE HANDLER METHODS   FILE HANDLER METHODS   
    // #########################################################################################
    /// <summary>
    /// PRE:  The assembly code is complete, and the amount of memory that the main procedure uses is known
    /// POST: All files are written to the disk, closed, and compiled.
    /// Returns the main procedure assembly code for easy viewing
    /// </summary>
    /// <param name="codeFileName"></param>
    /// <param name="mainMemory"></param>
    /// <returns></returns>
    public string WriteAFiles(string codeFileName, int mainMemory) {
      fw.WriteMainFile(mainMemory);
      fw.CloseFiles();
      fw.Make();
      return fw.MainProcedureCode;
    }
  }
}