using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace kompiler {
  class FileWriter {

    const string MASM_DIRECTORY = "masm";

    public const string MAIN_PROC_NAME = "mainProcedure";

    public const string CODE_FILE_EXTENSION = ".asm";

    public const string STRINGS_FILE_NAME = "strings";
    StreamWriter m_stringIncludes;
    public const string PROCEDURES_FILE_NAME = "procedures";
    StreamWriter m_procedureIncludes;
    public const string PROTOTYPES_FILE_NAME = "prototypes";
    StreamWriter m_prototypeIncludes;

    string m_projectName;

    string m_curProc;

    //Each procedure will exist in its own include file, tracked by this collection of StreamWriters
    Dictionary<string, string> m_procedures = new Dictionary<string, string>();

    /// <summary>
    /// Prepare the project's directory, with masm files, and strings/procedures include files.
    /// Change to that directory
    /// </summary>
    /// <param name="projectName"></param>
    public FileWriter(string projectName) {
      if (!Directory.Exists(projectName)) {
        Directory.CreateDirectory(projectName);

        //copy all masm files into the project's new folder
        //If it already exists, assume the masm files are already there
        string[] files = System.IO.Directory.GetFiles(MASM_DIRECTORY);
        foreach (string s in files) {
          // Use static Path methods to extract only the file name from the path.
          string fileName = System.IO.Path.GetFileName(s);
          string destFile = System.IO.Path.Combine(projectName, fileName);
          System.IO.File.Copy(s, destFile, true);
        }
      }

      //change to the new project directory
      Directory.SetCurrentDirectory(projectName);

      //create strings and procedures files in the new directory
      m_stringIncludes = new StreamWriter(STRINGS_FILE_NAME + CODE_FILE_EXTENSION);
      m_procedureIncludes = new StreamWriter(PROCEDURES_FILE_NAME + CODE_FILE_EXTENSION);
      m_prototypeIncludes = new StreamWriter(PROTOTYPES_FILE_NAME + CODE_FILE_EXTENSION);

      //create and setup the main procedure
      NewProcedure(MAIN_PROC_NAME);

      //Write a windows batch file which contains the necessary shell commands to compile the project
      WriteCommandFile(projectName);

      m_projectName = projectName;
    }

    /// <summary>
    /// Write the top level program code to a file, that wraps and calls the main procedure
    /// </summary>
    /// <param name="mem"></param>
    public void WriteMainFile(int mem) {
      DateTime dt = DateTime.Now;//Generate a time stamp
      using (StreamWriter sw = new StreamWriter(m_projectName + ".asm"))
        sw.Write("COMMENT |\r\nTITLE Kompiler output: " + m_projectName + "\r\n| Created: "
        + dt.ToString("F") + "\r\n"//time stamp
        +".686"+"\r\n"
        + ".model flat, stdcall" + "\r\n"
        + "option casemap :none" + "\r\n"

        + "include windows.inc" + "\r\n"
        + "include kernel32.inc" + "\r\n"
        + "include masm32.inc" + "\r\n"

        + "includelib kernel32.lib" + "\r\n"
        + "includelib masm32.lib" + "\r\n"

        + "include prototypes.asm" + "\r\n"

        + ".data" + "\r\n"

        + "int_buffer db 10 DUP(0) ; buffer capable of holding 10 digits" + "\r\n"
        + "nwln_string dw 0D0Ah ; ASCII 13 & 10 carriage return & line feed" + "\r\n"
        + "end_ch  DB  ?          ; John Broere 2002 idea to 'pause' at end.\r\n"
        + "str0    DB  'Press a key...',0" + "\r\n"

        + ";===== string constants inserted here: ======\r\n"
        + "include " + STRINGS_FILE_NAME + CODE_FILE_EXTENSION + "\r\n"

        + ".code\r\n"
        + "start:\r\n"
        + "push    ebp            ; save EBP since we use it\r\n"
        + "sub     esp, " + mem  + "  ; Room for main proc local vars\r\n"
        + "mov     ebp,esp          ; set the stack pointer as the base pointer\r\n"
        + "invoke " + MAIN_PROC_NAME + "\r\n"

        // adds a "pause" to the end of the program - thanks to John Broere 2002 !

        // note that John added str0 to the string collection,
              // and he added a character (end_ch) in the data segment above.
        + "invoke StdOut, addr nwln_string\r\n"
        + "invoke StdOut, addr str0\r\n"
        + "invoke StdIn, addr int_buffer, 10\r\n"

        // end the program
        + "pop     ebp            ; restore EBP\r\n"
        + "invoke ExitProcess, 0" + "\r\n"
        
        + "include " + FileWriter.PROCEDURES_FILE_NAME + FileWriter.CODE_FILE_EXTENSION + "\r\n"

        + "end start" + "\r\n"
        );
    }

    /// <summary>
    /// Run the windows command batch file to build and link the project
    /// </summary>
    public void Make(){
      SystemCommand.SysCommand(m_projectName + ".cmd");
    }

    /// <summary>
    /// Open the main procedure file and read it into a string which is immediately returned
    /// </summary>
    public string MainProcedureCode{
      get{return new StreamReader(MAIN_PROC_NAME + CODE_FILE_EXTENSION).ReadToEnd();}
    }

    /// <summary>
    /// Writes code to the current procedure
    /// </summary>
    /// <param name="name"></param>
    /// <param name="code"></param>
    public void Add(string code) {
      m_procedures[m_curProc] += (code) + "\r\n";
    }

    /// <summary>
    /// Creates a procedure;
    /// Will add the proper header to the new procedure : "functionName PROC"
    /// Sets the current working procedure to this one, to be written to by sequential Add() calls
    /// </summary>
    /// <param name="procedureName"></param>
    public void NewProcedure(string procedureName) {
      m_procedures[procedureName] = (procedureName + " PROC") + "\r\n";

      m_procedureIncludes.WriteLine("INCLUDE " + procedureName + CODE_FILE_EXTENSION);
      m_prototypeIncludes.WriteLine(procedureName + " PROTO");

      m_curProc = procedureName;
      Add("push ebp");//save BP in case another procedure was using it
    }

    /// <summary>
    /// Add the necessary "ProcName ENDP" and close the current procedure's file
    /// Perform basic efficieny guarentee by eliminating extraneous push/pop combos
    /// </summary>
    public void EndProcedure(int memUse) {
      m_procedures[m_curProc] += ("pop ebp") + "\r\n";
      m_procedures[m_curProc] += ("ret " + memUse) + "\r\n";
      m_procedures[m_curProc] += (m_curProc + " endp") + "\r\n";
      m_procedures[m_curProc] = Regex.Replace(m_procedures[m_curProc], "push (.*)\r\npop \\1\r\n", "");
      using (StreamWriter sw = new StreamWriter(m_curProc + CODE_FILE_EXTENSION))
        sw.Write(m_procedures[m_curProc]);
      m_procedures.Remove(m_curProc);

      m_curProc = MAIN_PROC_NAME;//As we leave a procedure, return to the main scope
    }

    /// <summary>
    /// Writes a string declaration to the correct file
    /// </summary>
    /// <param name="stringDecl"></param>
    public void DeclString(string stringDecl) {
      m_stringIncludes.WriteLine(stringDecl);
    }

    /// <summary>
    /// Closes all files and sets the directory back up one level
    /// </summary>
    public void CloseFiles() {
      m_stringIncludes.Close();
      m_procedureIncludes.Close();
      m_prototypeIncludes.Close();
    }

    public void ResetWorkingDirectory() {
      Directory.SetCurrentDirectory("..");
    }

    private void WriteCommandFile(string projectName) {
      // Write the command string to the proper file.
      using (StreamWriter sw = new StreamWriter(projectName + ".cmd"))
        sw.Write("REM ===== Kompiler: auto-created command file ======\r\n"
          // assemble to create the object file
        + "ml.exe /c /coff " + projectName + ".asm\r\n"

        // link the files to create the executable
        + "link.exe /subsystem:console " + projectName + ".obj" + "\r\n"
        //+ "@ PAUSE\r\n"
        + projectName + ".exe\r\n"

         // add a pause, so we can see the results of the assembly and linking
          // Thanks to John Broere 2002 !
        + "@ PAUSE"
        );
    }
  }
}
