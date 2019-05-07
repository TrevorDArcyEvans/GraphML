namespace GraphML.UI.Desktop
{
  partial class Main
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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("System");
      this.Overview = new System.Windows.Forms.TreeView();
      this.RepositoryMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.CmdImport = new System.Windows.Forms.ToolStripMenuItem();
      this.RepositoryMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // Overview
      // 
      this.Overview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.Overview.Location = new System.Drawing.Point(12, 12);
      this.Overview.Name = "Overview";
      treeNode1.Name = "Node0";
      treeNode1.Text = "System";
      this.Overview.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
      this.Overview.Size = new System.Drawing.Size(776, 426);
      this.Overview.TabIndex = 3;
      this.Overview.DoubleClick += new System.EventHandler(this.Overview_DoubleClick);
      this.Overview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Overview_MouseDown);
      // 
      // RepositoryMenu
      // 
      this.RepositoryMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.RepositoryMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdImport});
      this.RepositoryMenu.Name = "RepositoryMenu";
      this.RepositoryMenu.Size = new System.Drawing.Size(152, 36);
      // 
      // CmdImport
      // 
      this.CmdImport.Name = "CmdImport";
      this.CmdImport.Size = new System.Drawing.Size(151, 32);
      this.CmdImport.Text = "Import...";
      this.CmdImport.Click += new System.EventHandler(this.CmdImport_Click);
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.Overview);
      this.Name = "Main";
      this.Text = "GraphML for Desktop";
      this.RepositoryMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.TreeView Overview;
    private System.Windows.Forms.ContextMenuStrip RepositoryMenu;
    private System.Windows.Forms.ToolStripMenuItem CmdImport;
  }
}