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
      System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      this.CmdImport = new System.Windows.Forms.Button();
      this.TxtRepository = new System.Windows.Forms.TextBox();
      this.TxtDataFileName = new System.Windows.Forms.TextBox();
      this.TxtLog = new System.Windows.Forms.TextBox();
      this.CmdBrowseDataFile = new System.Windows.Forms.Button();
      this.BrowseDataFileDlg = new System.Windows.Forms.OpenFileDialog();
      label1 = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
      tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(3, 0);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(89, 20);
      label1.TabIndex = 1;
      label1.Text = "Repository:";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(3, 32);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(77, 20);
      label2.TabIndex = 1;
      label2.Text = "Data File:";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point(3, 68);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(40, 20);
      label3.TabIndex = 1;
      label3.Text = "Log:";
      // 
      // tableLayoutPanel1
      // 
      tableLayoutPanel1.AutoSize = true;
      tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      tableLayoutPanel1.ColumnCount = 4;
      tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      tableLayoutPanel1.Controls.Add(this.CmdImport, 2, 3);
      tableLayoutPanel1.Controls.Add(this.TxtLog, 1, 2);
      tableLayoutPanel1.Controls.Add(this.CmdBrowseDataFile, 3, 1);
      tableLayoutPanel1.Controls.Add(label3, 0, 2);
      tableLayoutPanel1.Controls.Add(label2, 0, 1);
      tableLayoutPanel1.Controls.Add(this.TxtDataFileName, 1, 1);
      tableLayoutPanel1.Controls.Add(label1, 0, 0);
      tableLayoutPanel1.Controls.Add(this.TxtRepository, 1, 0);
      tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      tableLayoutPanel1.Name = "tableLayoutPanel1";
      tableLayoutPanel1.RowCount = 4;
      tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      tableLayoutPanel1.Size = new System.Drawing.Size(700, 549);
      tableLayoutPanel1.TabIndex = 4;
      // 
      // CmdImport
      // 
      this.CmdImport.AutoSize = true;
      tableLayoutPanel1.SetColumnSpan(this.CmdImport, 2);
      this.CmdImport.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CmdImport.Enabled = false;
      this.CmdImport.Location = new System.Drawing.Point(382, 516);
      this.CmdImport.Name = "CmdImport";
      this.CmdImport.Size = new System.Drawing.Size(315, 30);
      this.CmdImport.TabIndex = 0;
      this.CmdImport.Text = "Import";
      this.CmdImport.UseVisualStyleBackColor = true;
      this.CmdImport.Click += new System.EventHandler(this.CmdImport_Click);
      // 
      // TxtRepository
      // 
      tableLayoutPanel1.SetColumnSpan(this.TxtRepository, 3);
      this.TxtRepository.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TxtRepository.Location = new System.Drawing.Point(98, 3);
      this.TxtRepository.Name = "TxtRepository";
      this.TxtRepository.ReadOnly = true;
      this.TxtRepository.Size = new System.Drawing.Size(599, 26);
      this.TxtRepository.TabIndex = 2;
      // 
      // TxtDataFileName
      // 
      tableLayoutPanel1.SetColumnSpan(this.TxtDataFileName, 2);
      this.TxtDataFileName.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TxtDataFileName.Location = new System.Drawing.Point(98, 35);
      this.TxtDataFileName.Name = "TxtDataFileName";
      this.TxtDataFileName.ReadOnly = true;
      this.TxtDataFileName.Size = new System.Drawing.Size(562, 26);
      this.TxtDataFileName.TabIndex = 2;
      // 
      // TxtLog
      // 
      tableLayoutPanel1.SetColumnSpan(this.TxtLog, 3);
      this.TxtLog.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TxtLog.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TxtLog.Location = new System.Drawing.Point(98, 71);
      this.TxtLog.Multiline = true;
      this.TxtLog.Name = "TxtLog";
      this.TxtLog.ReadOnly = true;
      this.TxtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TxtLog.Size = new System.Drawing.Size(599, 439);
      this.TxtLog.TabIndex = 2;
      this.TxtLog.WordWrap = false;
      // 
      // CmdBrowseDataFile
      // 
      this.CmdBrowseDataFile.AutoSize = true;
      this.CmdBrowseDataFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.CmdBrowseDataFile.Location = new System.Drawing.Point(666, 35);
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
      // Import
      // 
      this.ClientSize = new System.Drawing.Size(700, 549);
      this.Controls.Add(tableLayoutPanel1);
      this.Name = "Import";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "Import Data";
      tableLayoutPanel1.ResumeLayout(false);
      tableLayoutPanel1.PerformLayout();
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