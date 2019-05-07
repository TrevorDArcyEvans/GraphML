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
      System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("System");
      this.Overview = new System.Windows.Forms.TreeView();
      this.SuspendLayout();
      // 
      // Overview
      // 
      this.Overview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.Overview.Location = new System.Drawing.Point(0, 0);
      this.Overview.Name = "Overview";
      treeNode1.Name = "Node0";
      treeNode1.Text = "System";
      this.Overview.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
      this.Overview.Size = new System.Drawing.Size(800, 450);
      this.Overview.TabIndex = 3;
      this.Overview.DoubleClick += new System.EventHandler(this.Overview_DoubleClick);
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.Overview);
      this.Name = "Main";
      this.Text = "GraphML for Desktop";
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.TreeView Overview;
  }
}