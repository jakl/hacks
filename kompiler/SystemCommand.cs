using System;
// Note that I had to add the reference System.dll to access System.Diagnostics
using System.Diagnostics;
using System.ComponentModel;    // to handle Win32 file access errors, not needed otherwise
using System.Windows.Forms;     // MessageBox


/// <summary>
/// SystemCommand is a singleton class that lets us run commands on the local
///    system. The idea and about half the command code is from:
///    http://msdn2.microsoft.com/en-us/library/system.diagnostics.process.aspx
/// </summary>
namespace kompiler {
  public class SystemCommand {
    // Instead of a constructor, we offer a static method to run a command.
    private SystemCommand() { } // private constructor so no one else can create one.

    /// <summary>
    /// PRE:  A valid command file name is given.
    /// POST: SysCommand executes the given string on the local system.
    ///    If an error is detected false is returned, otherwise true.
    /// </summary>
    public static bool SysCommand(string strCommand) {
      // These are the Win32 error codes for these two specific errors.
      const int ERROR_FILE_NOT_FOUND = 2;
      const int ERROR_ACCESS_DENIED = 5;

      System.Diagnostics.Process m_p = new Process();
      m_p.StartInfo.RedirectStandardOutput = false;
      m_p.StartInfo.CreateNoWindow = true;
      m_p.StartInfo.FileName = strCommand;
      m_p.StartInfo.UseShellExecute = true;

      // attempt to run the command:
      try {
        m_p.Start();
        m_p.WaitForExit();
        m_p.Dispose();
      } catch (Win32Exception ex) {
        // check for known errors first
        if (ex.NativeErrorCode == ERROR_FILE_NOT_FOUND) {
          MessageBox.Show(ex.Message + ". Check the path ('" + strCommand + "').",
              "TomPiler - SystemCommand",
              MessageBoxButtons.OK,
              MessageBoxIcon.Exclamation);

          return false;
        } else if (ex.NativeErrorCode == ERROR_ACCESS_DENIED) {
          MessageBox.Show(ex.Message +
              ". You do not have permission to execute this file ('" + strCommand + "').",
              "TomPiler - SystemCommand",
              MessageBoxButtons.OK,
              MessageBoxIcon.Exclamation);

          return false;
        }

        // unknown error - just report
        MessageBox.Show(ex.Message + " ('" + strCommand + "').",
            "TomPiler - SystemCommand",
            MessageBoxButtons.OK,
            MessageBoxIcon.Exclamation);

        return false;
      }

      return true; // all must be well
    } // SysCommand
  }
}