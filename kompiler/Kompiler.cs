/*
 * Program: Kompiler
 * Version: alpha
 * Author: James Koval
 * Date: 2010/01/22
 * Purpose: Develop a compiler for some of Modula2, and gain precious points in my compilers class
 * Input: Everything is documented through the GUI. In general, paste code in the big text box.
 * Output: Again, everything is documented in the GUI.
 * Assumptions: Those who want to test my program will reboot into some edition of Microsoft Windows (for now)
 * 
 * Summary of Tests conducted:
 * 
 * When the Test Symbols button is clicked, the method symbolsbtn_Click is called and this happens:
 * I add several variables to my symbols handler, assign them values, change the scope around, and dump
 * all the data from the symbols handler to prove that the variables were handled correctly.
 * 
 * It does something to the effect of:
 * 
 * int x=42
 * int y=443
 * 
 * {  //nesting
 * 
 *   y=22
 *   string x = "twenty one"
 *   
 *   { //nesting
 *     string tempscope = "going to evaporate on unnest"
 *   } //unnesting
 *   
 *   x = "fourty two"
 *   
 * DUMP DATA
 *   
 
  Copyright 2011 James Koval
  License GPLv3+: GNU GPL version 3 or later
  <http://gnu.org/licenses/gpl.html>
  This is free software; you are free to change and redistribute it
  There is NO WARRANTY, to the extent permitted by law
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace kompiler {
  public partial class form : Form {
    private String m_filePath = null;

    private Facade facade = Facade.GetFacade();

    public form() {
      InitializeComponent();
    }

    private void openbtn_Click(object sender, EventArgs e) {
      //Handles a popup for the user to select a file
      OpenFileDialog openPopup = new OpenFileDialog();

      openPopup.Filter = "modula2 files (*.mod)|*.mod|All files (*.*)|*.*";

      //initiate the popup
      if (openPopup.ShowDialog() == DialogResult.OK) {
        //Fill our central code text box with all the contents from the file
        codebox.Text = new StreamReader(openPopup.OpenFile()).ReadToEnd();

        m_filePath = openPopup.FileName;
      }
      //lineNumbers_For_RichTextBox1.Refresh(); Why don't line numbers work when opening a file?
    }

    private void savebtn_Click(object sender, EventArgs e) {
      if (m_filePath != null) saveCodeboxToPath();
      else saveasbtn_Click(null, null);
    }

    private void saveasbtn_Click(object sender, EventArgs e) {
      SaveFileDialog savePopup = new SaveFileDialog();
      if (savePopup.ShowDialog() == DialogResult.OK) {
        m_filePath = savePopup.FileName;
        saveCodeboxToPath();
      }
    }

    /// <summary>
    /// Save the contents of codebox to m_filePath
    /// </summary>
    private void saveCodeboxToPath() {
      using (StreamWriter sw = new StreamWriter(m_filePath))
        sw.Write(codebox.Text);
    }

    private void newbtn_Click(object sender, EventArgs e) {
      codebox.Text = "";
      m_filePath = null;
    }

    private void parsebtn_Click(object sender, EventArgs e) {
      facade.m_comments = commentcheck.Checked;
      if(m_filePath == null)
        facade.parse(codebox.Text, "unnamed");
      else
        facade.parse(codebox.Text, Path.GetFileNameWithoutExtension(m_filePath));
      tokensbox.Text = facade.TokenDump;
      errorbox.Text = facade.Errors;
      assemblybox.Text = facade.AssemblyDump;//Make the main procedure viewable
    }
  }
}