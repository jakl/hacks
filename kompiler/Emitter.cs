using System;
using System.Collections;   // ArrayList
using System.IO;

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

    public int m_iMainMemoryUse;
    private string m_masm_bin_path = @"..\..\..\masm\";
    private int m_strCount = 1;
    private string m_curProc;
    private string m_savedDir;

    /// <summary>
    /// PRE:  The desired output on the top of the run-time stack.
    /// POST: The integer is displayed.
    /// </summary>
    public void WRINT() {
      using (StreamWriter sw = new StreamWriter(m_curProc + ".inc", true))
        sw.Write("PutInt " + "[SP]\r\n");
    }

    public void WRINT(int val) {
      using (StreamWriter sw = new StreamWriter(m_curProc + ".inc", true))
        sw.Write("PutInt " + val + "\r\n");
    }

    public void WRLN() {
      using (StreamWriter sw = new StreamWriter(m_curProc + ".inc", true))
        sw.Write("nwln\r\n");
    }

    public void WRSTR(string strIn) {
      using (StreamWriter sw = new StreamWriter("strings.inc", true))
        sw.Write("str" + m_strCount + " DB '" + strIn + "', 0\r\n");
      using (StreamWriter sw = new StreamWriter(m_curProc + ".inc", true))
        sw.Write("PutStr " + "str" + m_strCount + "\r\n");
    }


    // #########################################################################################
    // A R I T H M E T I C               A R I T H M E T I C               A R I T H M E T I C      
    // #########################################################################################

    /// <summary>
    /// PRE:  The second operand is on the top of the stack. The first is next on the stack.
    /// POST: The sum is on the top of the stack.
    /// </summary>
    public void AddOperation() {
    }


    // #########################################################################################
    // FILE HANDLER METHODS   FILE HANDLER METHODS   FILE HANDLER METHODS   FILE HANDLER METHODS   
    // #########################################################################################

    /// <summary>
    /// PRE:  The parse is complete.
    ///    c_iMainMemoryUse stores the amount of memory needed by the main procedure.
    /// POST: A string is created and the assembler "shell" is written to it.
    /// </summary>
    string MainAFile(string codeFileName, int mainMemUse, string mainProc)
    {
      string strHead = "COMMENT |\r\nTITLE Kompiler output: " + codeFileName + "\r\n| Created: ";

      // create a time stamp
      DateTime dt = DateTime.Now;
      strHead += dt.ToString("F") + "\r\n";
      strHead += ".MODEL SMALL\r\n" // use the small (16-bit) memory model
          + ".486\r\n"          // this allows 32-bit arithmetic (EAX, EDX registers, etc.)
          + ".STACK 1000H\r\n"  // plenty of stack space: 4096 bytes
          + ".DATA\r\n"         // begin DATA section

          //definition of a char for "Press the any key to continue:" (John Broere 2002)
          + "end_ch  DB  ?          ; John Broere 2002 idea to 'pause' at end.\r\r\n\n"
          + "str0    DB  'Press a key...',0"

          // The following file must be created later in the same directory
          // to contain string constants of the form:
          + ";===== string constants inserted here: ======\r\n"
          + "INCLUDE strings.inc\r\r\n\n"

          + ".CODE\r\n"
          + "INCLUDE io.mac\r\n"
          + "main PROC\r\n"
          + ".STARTUP\r\n"
          + "push    EBP            ; save EBP since we use it\r\n"
          + "sub     SP, " + mainMemUse
          + "         ; Room for main proc local vars\r\n"
          + "mov     BP,SP          ; set the stack pointer as the base pointer\r\n"
          + "call " + mainProc + "\r\n"

          // adds a "pause" to the end of the program - thanks to John Broere 2002 !

          // note that John added str0 to the string collection,
          // and he added a character (end_ch) in the data segment above.
          + "nwln\r\n"
          + "PutStr  str0\r\n"
          + "GetCh   end_ch\r\n"

          // end the program
          + "pop     EBP            ; restore EBP\r\n"
          + ".EXIT\r\n"
          + "main    ENDP           ; end of assembly outermost function\r\r\n\n"
          + "; The following procedures must be included.\r\n"
          + "INCLUDE proclist.inc   ; lines like 'INCLUDE V000000main.inc'\r\n"
          + "END\r\n";
      return strHead;
    }

    /// <summary>
    /// PRE:  The name of the procedure is passed. We have already called 
    ///    EnterNewProcScope which tracks the current scope number. Note that this
    ///    array of procedure strings must remain parallel to the array of procedures
    ///    maintained by SymbolTable.
    /// POST: The preamble is emitted. This includes creating the assembly string
    ///    and increasing the procedure index.
    ///    
    /// Note the special version for the main procedure
    /// </summary>
    public void ProcPreamble(string strProcName) {
      using (StreamWriter sw = new StreamWriter("proclist.inc", true))
        sw.Write("INCLUDE " + strProcName + ".inc\r\n");
      using (StreamWriter sw = new StreamWriter(strProcName + ".inc"))
        sw.Write(strProcName + " PROC\r\n");
      m_curProc = strProcName;
    }

    public void init(string projectName) {
      //Create the project directory which the emitter will use
      m_savedDir = Directory.GetCurrentDirectory();
      string projectRootDir = Directory.GetCurrentDirectory() + @"\" + projectName;
      Directory.CreateDirectory(projectRootDir);
      Directory.SetCurrentDirectory(projectRootDir);
      using (StreamWriter sw = new StreamWriter("proclist.inc"))
        sw.Write("");
      using (StreamWriter sw = new StreamWriter("strings.inc"))
        sw.Write("");
    }

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
    public void ProcPostamble(string strProcName, int iMemUse) {
      using (StreamWriter sw = new StreamWriter(strProcName + ".inc", true))
        sw.Write("ret " + iMemUse + "\r\n"
          + strProcName + " ENDP");
    }


    /// <summary>
    /// PRE:  The assembler files have all been "written" to strings.
    /// POST: The files are written to the disk.
    /// Returns the core assembly code for easy viewing
    /// </summary>
    public string WriteAFiles(string codeFileName, string mainProc) {
      // write the outermost "shell" assembler file
      using (StreamWriter sw = new StreamWriter(codeFileName + ".asm"))
      //make the naive assumption that 200 is enough memory for the program
        sw.Write(MainAFile(codeFileName + ".asm", 200, mainProc));

      BuildAndInvokeCmdFile(codeFileName);

      string coreAssemblyCode = new StreamReader(mainProc + ".inc").ReadToEnd();

      //Restore the working directory as it was before emitting took place
      Directory.SetCurrentDirectory(m_savedDir);

      return coreAssemblyCode;
    }

    /// <summary>
    /// PRE:  The parse is complete.
    /// POST: The command file is created for remaining steps of the assembly process
    ///    (compilation and linking to create an execcutable).
    ///    This command file is then run to complete the compilation.
    /// </summary>
    void BuildAndInvokeCmdFile(string name) {
      string strMakeFile =
          "REM ===== Kompiler: auto-created command file ======\r\n"

          // copy files needed for the compiling and linking (respectively)
          + "copy " + m_masm_bin_path + "io.mac .\r\n"
          + "copy " + m_masm_bin_path + "ml.exe .\r\n"

          + "copy " + m_masm_bin_path + "io.obj .\r\n"
          + "copy " + m_masm_bin_path + "link16.exe .\r\n"
          
          // assemble to create the object file
          + "ml /c " + name + ".asm\r\n"

          // link the files to create the executable
          + "link16 "
          + name + ".obj io.obj, "
          + name + ".exe, "
          + name + ".map, , , \r\r\n\n" // Yes, the three commas are necessary!

          // add a pause, so we can see the results of the assembly and linking
          // Thanks to John Broere 2002 !
        //  + "@ PAUSE\r\r\n\n"
        //  + name + ".exe\r\r\n\n" //my 64bit Windows 7 can't run 16 bit exe files; otherwise this should work
          + "@ PAUSE\r\r\n\n";

      // Write the command string to the proper file.
      using (StreamWriter sw = new StreamWriter(name + ".cmd"))
        sw.Write(strMakeFile);

      // Invoke the file just created. This uses the static method in our SystemCommand class.
      //    If an error occurs it will throw the appropriate exception.
      SystemCommand.SysCommand(name + ".cmd");
    }
  }
}