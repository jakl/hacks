namespace kompiler
{
    partial class form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form));
          this.codebox = new System.Windows.Forms.RichTextBox();
          this.openbtn = new System.Windows.Forms.Button();
          this.newbtn = new System.Windows.Forms.Button();
          this.savebtn = new System.Windows.Forms.Button();
          this.saveasbtn = new System.Windows.Forms.Button();
          this.tabs = new System.Windows.Forms.TabControl();
          this.assemblytab = new System.Windows.Forms.TabPage();
          this.assemblybox = new System.Windows.Forms.RichTextBox();
          this.tokenstab = new System.Windows.Forms.TabPage();
          this.tokensbox = new System.Windows.Forms.RichTextBox();
          this.errorstab = new System.Windows.Forms.TabPage();
          this.errorbox = new System.Windows.Forms.RichTextBox();
          this.notetab = new System.Windows.Forms.TabPage();
          this.notebox = new System.Windows.Forms.RichTextBox();
          this.parsebtn = new System.Windows.Forms.Button();
          this.commentcheck = new System.Windows.Forms.CheckBox();
          this.lineNumbers_For_RichTextBox1 = new LineNumbers.LineNumbers_For_RichTextBox();
          this.tabs.SuspendLayout();
          this.assemblytab.SuspendLayout();
          this.tokenstab.SuspendLayout();
          this.errorstab.SuspendLayout();
          this.notetab.SuspendLayout();
          this.SuspendLayout();
          // 
          // codebox
          // 
          this.codebox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.codebox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.codebox.Location = new System.Drawing.Point(29, 62);
          this.codebox.Name = "codebox";
          this.codebox.Size = new System.Drawing.Size(702, 286);
          this.codebox.TabIndex = 0;
          this.codebox.Text = "";
          this.codebox.WordWrap = false;
          // 
          // openbtn
          // 
          this.openbtn.Location = new System.Drawing.Point(77, 12);
          this.openbtn.Name = "openbtn";
          this.openbtn.Size = new System.Drawing.Size(59, 44);
          this.openbtn.TabIndex = 1;
          this.openbtn.Text = "Open";
          this.openbtn.UseVisualStyleBackColor = true;
          this.openbtn.Click += new System.EventHandler(this.openbtn_Click);
          // 
          // newbtn
          // 
          this.newbtn.Location = new System.Drawing.Point(12, 12);
          this.newbtn.Name = "newbtn";
          this.newbtn.Size = new System.Drawing.Size(59, 44);
          this.newbtn.TabIndex = 2;
          this.newbtn.Text = "New";
          this.newbtn.UseVisualStyleBackColor = true;
          this.newbtn.Click += new System.EventHandler(this.newbtn_Click);
          // 
          // savebtn
          // 
          this.savebtn.Location = new System.Drawing.Point(142, 12);
          this.savebtn.Name = "savebtn";
          this.savebtn.Size = new System.Drawing.Size(59, 44);
          this.savebtn.TabIndex = 3;
          this.savebtn.Text = "Save";
          this.savebtn.UseVisualStyleBackColor = true;
          this.savebtn.Click += new System.EventHandler(this.savebtn_Click);
          // 
          // saveasbtn
          // 
          this.saveasbtn.Location = new System.Drawing.Point(207, 12);
          this.saveasbtn.Name = "saveasbtn";
          this.saveasbtn.Size = new System.Drawing.Size(40, 44);
          this.saveasbtn.TabIndex = 4;
          this.saveasbtn.Text = "Save As";
          this.saveasbtn.UseVisualStyleBackColor = true;
          this.saveasbtn.Click += new System.EventHandler(this.saveasbtn_Click);
          // 
          // tabs
          // 
          this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.tabs.Controls.Add(this.assemblytab);
          this.tabs.Controls.Add(this.tokenstab);
          this.tabs.Controls.Add(this.errorstab);
          this.tabs.Controls.Add(this.notetab);
          this.tabs.Location = new System.Drawing.Point(12, 354);
          this.tabs.Name = "tabs";
          this.tabs.SelectedIndex = 0;
          this.tabs.Size = new System.Drawing.Size(719, 235);
          this.tabs.TabIndex = 5;
          // 
          // assemblytab
          // 
          this.assemblytab.Controls.Add(this.assemblybox);
          this.assemblytab.Location = new System.Drawing.Point(4, 22);
          this.assemblytab.Name = "assemblytab";
          this.assemblytab.Size = new System.Drawing.Size(711, 209);
          this.assemblytab.TabIndex = 4;
          this.assemblytab.Text = "Assembly";
          this.assemblytab.UseVisualStyleBackColor = true;
          // 
          // assemblybox
          // 
          this.assemblybox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.assemblybox.Location = new System.Drawing.Point(3, 3);
          this.assemblybox.Name = "assemblybox";
          this.assemblybox.Size = new System.Drawing.Size(707, 205);
          this.assemblybox.TabIndex = 0;
          this.assemblybox.Text = "";
          // 
          // tokenstab
          // 
          this.tokenstab.Controls.Add(this.tokensbox);
          this.tokenstab.Location = new System.Drawing.Point(4, 22);
          this.tokenstab.Name = "tokenstab";
          this.tokenstab.Padding = new System.Windows.Forms.Padding(3);
          this.tokenstab.Size = new System.Drawing.Size(711, 209);
          this.tokenstab.TabIndex = 0;
          this.tokenstab.Text = "Tokens";
          this.tokenstab.UseVisualStyleBackColor = true;
          // 
          // tokensbox
          // 
          this.tokensbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.tokensbox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.tokensbox.Location = new System.Drawing.Point(6, 6);
          this.tokensbox.Name = "tokensbox";
          this.tokensbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
          this.tokensbox.Size = new System.Drawing.Size(701, 197);
          this.tokensbox.TabIndex = 0;
          this.tokensbox.Text = "No tokens yet lexed... maybe you want to click Lex";
          // 
          // errorstab
          // 
          this.errorstab.Controls.Add(this.errorbox);
          this.errorstab.Location = new System.Drawing.Point(4, 22);
          this.errorstab.Name = "errorstab";
          this.errorstab.Padding = new System.Windows.Forms.Padding(3);
          this.errorstab.Size = new System.Drawing.Size(711, 209);
          this.errorstab.TabIndex = 1;
          this.errorstab.Text = "Errors";
          this.errorstab.UseVisualStyleBackColor = true;
          // 
          // errorbox
          // 
          this.errorbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.errorbox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.errorbox.Location = new System.Drawing.Point(6, 6);
          this.errorbox.Name = "errorbox";
          this.errorbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
          this.errorbox.Size = new System.Drawing.Size(701, 197);
          this.errorbox.TabIndex = 0;
          this.errorbox.Text = "";
          // 
          // notetab
          // 
          this.notetab.Controls.Add(this.notebox);
          this.notetab.Location = new System.Drawing.Point(4, 22);
          this.notetab.Name = "notetab";
          this.notetab.Size = new System.Drawing.Size(711, 209);
          this.notetab.TabIndex = 3;
          this.notetab.Text = "Scratch Pad";
          this.notetab.UseVisualStyleBackColor = true;
          // 
          // notebox
          // 
          this.notebox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.notebox.Location = new System.Drawing.Point(3, 3);
          this.notebox.Name = "notebox";
          this.notebox.Size = new System.Drawing.Size(707, 203);
          this.notebox.TabIndex = 0;
          this.notebox.Text = "";
          // 
          // parsebtn
          // 
          this.parsebtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.parsebtn.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.parsebtn.ForeColor = System.Drawing.SystemColors.ControlText;
          this.parsebtn.Location = new System.Drawing.Point(529, 12);
          this.parsebtn.Name = "parsebtn";
          this.parsebtn.Size = new System.Drawing.Size(121, 44);
          this.parsebtn.TabIndex = 6;
          this.parsebtn.Text = "Parse";
          this.parsebtn.UseVisualStyleBackColor = true;
          this.parsebtn.Click += new System.EventHandler(this.parsebtn_Click);
          // 
          // commentcheck
          // 
          this.commentcheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.commentcheck.AutoSize = true;
          this.commentcheck.Location = new System.Drawing.Point(656, 12);
          this.commentcheck.Name = "commentcheck";
          this.commentcheck.Size = new System.Drawing.Size(75, 17);
          this.commentcheck.TabIndex = 10;
          this.commentcheck.Text = "Comments";
          this.commentcheck.UseVisualStyleBackColor = true;
          // 
          // lineNumbers_For_RichTextBox1
          // 
          this.lineNumbers_For_RichTextBox1._SeeThroughMode_ = false;
          this.lineNumbers_For_RichTextBox1.AutoSizing = true;
          this.lineNumbers_For_RichTextBox1.BackgroundGradient_AlphaColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
          this.lineNumbers_For_RichTextBox1.BackgroundGradient_BetaColor = System.Drawing.Color.LightSteelBlue;
          this.lineNumbers_For_RichTextBox1.BackgroundGradient_Direction = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
          this.lineNumbers_For_RichTextBox1.BorderLines_Color = System.Drawing.Color.SlateGray;
          this.lineNumbers_For_RichTextBox1.BorderLines_Style = System.Drawing.Drawing2D.DashStyle.Dot;
          this.lineNumbers_For_RichTextBox1.BorderLines_Thickness = 1F;
          this.lineNumbers_For_RichTextBox1.DockSide = LineNumbers.LineNumbers_For_RichTextBox.LineNumberDockSide.Left;
          this.lineNumbers_For_RichTextBox1.GridLines_Color = System.Drawing.Color.SlateGray;
          this.lineNumbers_For_RichTextBox1.GridLines_Style = System.Drawing.Drawing2D.DashStyle.Dot;
          this.lineNumbers_For_RichTextBox1.GridLines_Thickness = 1F;
          this.lineNumbers_For_RichTextBox1.LineNrs_Alignment = System.Drawing.ContentAlignment.TopRight;
          this.lineNumbers_For_RichTextBox1.LineNrs_AntiAlias = true;
          this.lineNumbers_For_RichTextBox1.LineNrs_AsHexadecimal = false;
          this.lineNumbers_For_RichTextBox1.LineNrs_ClippedByItemRectangle = true;
          this.lineNumbers_For_RichTextBox1.LineNrs_LeadingZeroes = true;
          this.lineNumbers_For_RichTextBox1.LineNrs_Offset = new System.Drawing.Size(0, 0);
          this.lineNumbers_For_RichTextBox1.Location = new System.Drawing.Point(11, 62);
          this.lineNumbers_For_RichTextBox1.Margin = new System.Windows.Forms.Padding(0);
          this.lineNumbers_For_RichTextBox1.MarginLines_Color = System.Drawing.Color.SlateGray;
          this.lineNumbers_For_RichTextBox1.MarginLines_Side = LineNumbers.LineNumbers_For_RichTextBox.LineNumberDockSide.Right;
          this.lineNumbers_For_RichTextBox1.MarginLines_Style = System.Drawing.Drawing2D.DashStyle.Solid;
          this.lineNumbers_For_RichTextBox1.MarginLines_Thickness = 1F;
          this.lineNumbers_For_RichTextBox1.Name = "lineNumbers_For_RichTextBox1";
          this.lineNumbers_For_RichTextBox1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
          this.lineNumbers_For_RichTextBox1.ParentRichTextBox = this.codebox;
          this.lineNumbers_For_RichTextBox1.Show_BackgroundGradient = true;
          this.lineNumbers_For_RichTextBox1.Show_BorderLines = true;
          this.lineNumbers_For_RichTextBox1.Show_GridLines = true;
          this.lineNumbers_For_RichTextBox1.Show_LineNrs = true;
          this.lineNumbers_For_RichTextBox1.Show_MarginLines = true;
          this.lineNumbers_For_RichTextBox1.Size = new System.Drawing.Size(17, 286);
          this.lineNumbers_For_RichTextBox1.TabIndex = 12;
          // 
          // form
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(743, 601);
          this.Controls.Add(this.lineNumbers_For_RichTextBox1);
          this.Controls.Add(this.commentcheck);
          this.Controls.Add(this.parsebtn);
          this.Controls.Add(this.tabs);
          this.Controls.Add(this.saveasbtn);
          this.Controls.Add(this.savebtn);
          this.Controls.Add(this.newbtn);
          this.Controls.Add(this.openbtn);
          this.Controls.Add(this.codebox);
          this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.Name = "form";
          this.Text = "Kompiler";
          this.tabs.ResumeLayout(false);
          this.assemblytab.ResumeLayout(false);
          this.tokenstab.ResumeLayout(false);
          this.errorstab.ResumeLayout(false);
          this.notetab.ResumeLayout(false);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox codebox;
        private System.Windows.Forms.Button openbtn;
        private System.Windows.Forms.Button newbtn;
        private System.Windows.Forms.Button savebtn;
        private System.Windows.Forms.Button saveasbtn;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tokenstab;
        private System.Windows.Forms.TabPage errorstab;
        private System.Windows.Forms.RichTextBox tokensbox;
        private System.Windows.Forms.Button parsebtn;
        private System.Windows.Forms.RichTextBox errorbox;
        private System.Windows.Forms.TabPage notetab;
        private System.Windows.Forms.RichTextBox notebox;
        private System.Windows.Forms.CheckBox commentcheck;
        private System.Windows.Forms.TabPage assemblytab;
        private System.Windows.Forms.RichTextBox assemblybox;
        private LineNumbers.LineNumbers_For_RichTextBox lineNumbers_For_RichTextBox1;
    }
}

