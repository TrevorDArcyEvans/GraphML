namespace GraphML.UI.Desktop
{
  partial class Import
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
      System.Windows.Forms.Label label1;
      System.Windows.Forms.Label label2;
      System.Windows.Forms.Label label3;
      this.CmdImport = new System.Windows.Forms.Button();
      this.TxtRepository = new System.Windows.Forms.TextBox();
      this.TxtDataFileName = new System.Windows.Forms.TextBox();
      this.CmdBrowseDataFile = new System.Windows.Forms.Button();
      this.BrowseDataFileDlg = new System.Windows.Forms.OpenFileDialog();
      this.TxtLog = new System.Windows.Forms.TextBox();
      label1 = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // CmdImport
      // 
      this.CmdImport.AutoSize = true;
      this.CmdImport.Enabled = false;
      this.CmdImport.Location = new System.Drawing.Point(310, 435);
      this.CmdImport.Name = "CmdImport";
      this.CmdImport.Size = new System.Drawing.Size(103, 30);
      this.CmdImport.TabIndex = 0;
      this.CmdImport.Text = "Import";
      this.CmdImport.UseVisualStyleBackColor = true;
      this.CmdImport.Click += new System.EventHandler(this.CmdImport_Click);
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(13, 13);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(89, 20);
      label1.TabIndex = 1;
      label1.Text = "Repository:";
      // 
      // TxtRepository
      // 
      this.TxtRepository.Location = new System.Drawing.Point(109, 6);
      this.TxtRepository.Name = "TxtRepository";
      this.TxtRepository.ReadOnly = true;
      this.TxtRepository.Size = new System.Drawing.Size(304, 26);
      this.TxtRepository.TabIndex = 2;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(12, 55);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(77, 20);
      label2.TabIndex = 1;
      label2.Text = "Data File:";
      // 
      // TxtDataFileName
      // 
      this.TxtDataFileName.Location = new System.Drawing.Point(109, 49);
      this.TxtDataFileName.Name = "TxtDataFileName";
      this.TxtDataFileName.ReadOnly = true;
      this.TxtDataFileName.Size = new System.Drawing.Size(254, 26);
      this.TxtDataFileName.TabIndex = 2;
      // 
      // CmdBrowseDataFile
      // 
      this.CmdBrowseDataFile.AutoSize = true;
      this.CmdBrowseDataFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.CmdBrowseDataFile.Location = new System.Drawing.Point(382, 45);
      this.CmdBrowseDataFile.Name = "CmdBrowseDataFile";
      this.CmdBrowseDataFile.Size = new System.Drawing.Size(31, 30);
      this.CmdBrowseDataFile.TabIndex = 3;
      this.CmdBrowseDataFile.Text = "...";
      this.CmdBrowseDataFile.UseVisualStyleBackColor = true;
      this.CmdBrowseDataFile.Click += new System.EventHandler(this.CmdBrowseDataFile_Click);
      // 
      // BrowseDataFileDlg
      // 
      this.BrowseDataFileDlg.AddExtension = false;
      this.BrowseDataFileDlg.Filter = "CSV files|*.csv|All files|*.*";
      this.BrowseDataFileDlg.Title = "Select data file";
      // 
      // TxtLog
      // 
      this.TxtLog.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TxtLog.Location = new System.Drawing.Point(109, 103);
      this.TxtLog.Multiline = true;
      this.TxtLog.Name = "TxtLog";
      this.TxtLog.ReadOnly = true;
      this.TxtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TxtLog.Size = new System.Drawing.Size(304, 298);
      this.TxtLog.TabIndex = 2;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point(13, 103);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(40, 20);
      label3.TabIndex = 1;
      label3.Text = "Log:";
      // 
      // Import
      // 
      this.ClientSize = new System.Drawing.Size(459, 495);
      this.Controls.Add(this.CmdBrowseDataFile);
      this.Controls.Add(this.TxtLog);
      this.Controls.Add(this.TxtDataFileName);
      this.Controls.Add(this.TxtRepository);
      this.Controls.Add(label3);
      this.Controls.Add(label2);
      this.Controls.Add(label1);
      this.Controls.Add(this.CmdImport);
      this.Name = "Import";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "Import Data";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button CmdImport;
    private System.Windows.Forms.TextBox TxtRepository;
    private System.Windows.Forms.TextBox TxtDataFileName;
    private System.Windows.Forms.Button CmdBrowseDataFile;
    private System.Windows.Forms.OpenFileDialog BrowseDataFileDlg;
    private System.Windows.Forms.TextBox TxtLog;
  }
}