using System;
using System.Windows.Forms;

namespace GraphML.UI.Desktop
{
  public partial class Import : Form
  {
    private readonly Repository _repo;

    public Import()
    {
      InitializeComponent();
    }

    public Import(Repository repo) :
      this()
    {
      _repo = repo;
      TxtRepository.Text = _repo.Name;
    }

    private void CmdBrowseDataFile_Click(object sender, EventArgs e)
    {
      CmdImport.Enabled = false;
      if (BrowseDataFileDlg.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      TxtDataFileName.Text = BrowseDataFileDlg.FileName;
      CmdImport.Enabled = true;
    }

    private void CmdImport_Click(object sender, EventArgs e)
    {
      TxtLog.Clear();
      using (new AutoCursor())
      {
        new GraphML.Datastore.Database.Importer.CSV.Program(_repo.Name, TxtDataFileName.Text, msg => TxtLog.AppendText(msg)).Run();
      }
    }
  }
}
