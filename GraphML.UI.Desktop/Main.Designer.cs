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
      System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
      System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("System");
      this.Overview = new System.Windows.Forms.TreeView();
      this.RepositoryMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.CmdRepositoryEdit = new System.Windows.Forms.ToolStripMenuItem();
      this.CmRepositoryDelete = new System.Windows.Forms.ToolStripMenuItem();
      this.CmdRepositoryImport = new System.Windows.Forms.ToolStripMenuItem();
      this.RepositoryManagerMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.CmdRepositoryCreate = new System.Windows.Forms.ToolStripMenuItem();
      this.GraphMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.CmdGraphDelete = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.RepositoryMenu.SuspendLayout();
      this.RepositoryManagerMenu.SuspendLayout();
      this.GraphMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new System.Drawing.Size(148, 6);
      // 
      // Overview
      // 
      this.Overview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.Overview.Location = new System.Drawing.Point(12, 12);
      this.Overview.Name = "Overview";
      treeNode2.Name = "Node0";
      treeNode2.Text = "System";
      this.Overview.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
      this.Overview.Size = new System.Drawing.Size(776, 426);
      this.Overview.TabIndex = 3;
      this.Overview.DoubleClick += new System.EventHandler(this.Overview_DoubleClick);
      this.Overview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Overview_MouseDown);
      // 
      // RepositoryMenu
      // 
      this.RepositoryMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.RepositoryMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdRepositoryEdit,
            this.CmRepositoryDelete,
            toolStripSeparator1,
            this.CmdRepositoryImport});
      this.RepositoryMenu.Name = "RepositoryMenu";
      this.RepositoryMenu.Size = new System.Drawing.Size(152, 106);
      // 
      // CmdRepositoryEdit
      // 
      this.CmdRepositoryEdit.Name = "CmdRepositoryEdit";
      this.CmdRepositoryEdit.Size = new System.Drawing.Size(151, 32);
      this.CmdRepositoryEdit.Text = "Edit...";
      this.CmdRepositoryEdit.Click += new System.EventHandler(this.CmdRepositoryEdit_Click);
      // 
      // CmRepositoryDelete
      // 
      this.CmRepositoryDelete.Name = "CmRepositoryDelete";
      this.CmRepositoryDelete.Size = new System.Drawing.Size(151, 32);
      this.CmRepositoryDelete.Text = "Delete...";
      this.CmRepositoryDelete.Click += new System.EventHandler(this.CmdRepositoryDelete_Click);
      // 
      // CmdRepositoryImport
      // 
      this.CmdRepositoryImport.Name = "CmdRepositoryImport";
      this.CmdRepositoryImport.Size = new System.Drawing.Size(151, 32);
      this.CmdRepositoryImport.Text = "Import...";
      this.CmdRepositoryImport.Click += new System.EventHandler(this.CmdRepositoryImport_Click);
      // 
      // RepositoryManagerMenu
      // 
      this.RepositoryManagerMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.RepositoryManagerMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdRepositoryCreate});
      this.RepositoryManagerMenu.Name = "RepositoryMenu";
      this.RepositoryManagerMenu.Size = new System.Drawing.Size(147, 36);
      // 
      // CmdRepositoryCreate
      // 
      this.CmdRepositoryCreate.Name = "CmdRepositoryCreate";
      this.CmdRepositoryCreate.Size = new System.Drawing.Size(146, 32);
      this.CmdRepositoryCreate.Text = "Create...";
      this.CmdRepositoryCreate.Click += new System.EventHandler(this.CmdRepositoryCreate_Click);
      // 
      // GraphMenu
      // 
      this.GraphMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.GraphMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdGraphDelete});
      this.GraphMenu.Name = "GraphMenu";
      this.GraphMenu.Size = new System.Drawing.Size(147, 36);
      // 
      // CmdGraphDelete
      // 
      this.CmdGraphDelete.Name = "CmdGraphDelete";
      this.CmdGraphDelete.Size = new System.Drawing.Size(146, 32);
      this.CmdGraphDelete.Text = "Delete...";
      this.CmdGraphDelete.Click += new System.EventHandler(this.CmdGraphDelete_Click);
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
      this.RepositoryManagerMenu.ResumeLayout(false);
      this.GraphMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.TreeView Overview;
    private System.Windows.Forms.ContextMenuStrip RepositoryMenu;
    private System.Windows.Forms.ToolStripMenuItem CmdRepositoryImport;
    private System.Windows.Forms.ContextMenuStrip RepositoryManagerMenu;
    private System.Windows.Forms.ToolStripMenuItem CmdRepositoryCreate;
    private System.Windows.Forms.ToolStripMenuItem CmdRepositoryEdit;
    private System.Windows.Forms.ToolStripMenuItem CmRepositoryDelete;
    private System.Windows.Forms.ContextMenuStrip GraphMenu;
    private System.Windows.Forms.ToolStripMenuItem CmdGraphDelete;
  }
}