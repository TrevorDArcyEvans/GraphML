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
      System.Windows.Forms.Button CmdRepositoryManager;
      this.TxtResults = new System.Windows.Forms.TextBox();
      CmdRepositoryManager = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // CmdRepositoryManager
      // 
      CmdRepositoryManager.Location = new System.Drawing.Point(687, 319);
      CmdRepositoryManager.Name = "CmdRepositoryManager";
      CmdRepositoryManager.Size = new System.Drawing.Size(100, 30);
      CmdRepositoryManager.TabIndex = 0;
      CmdRepositoryManager.Text = "Retrieve";
      CmdRepositoryManager.UseVisualStyleBackColor = true;
      CmdRepositoryManager.Click += new System.EventHandler(this.CmdRepositoryManager_Click);
      // 
      // TxtResults
      // 
      this.TxtResults.Location = new System.Drawing.Point(13, 13);
      this.TxtResults.Multiline = true;
      this.TxtResults.Name = "TxtResults";
      this.TxtResults.Size = new System.Drawing.Size(596, 344);
      this.TxtResults.TabIndex = 1;
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.TxtResults);
      this.Controls.Add(CmdRepositoryManager);
      this.Name = "Main";
      this.Text = "GraphML for Desktop";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox TxtResults;
  }
}