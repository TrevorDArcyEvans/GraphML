using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphML.UI.Desktop
{
  public partial class CreateEdit : Form
  {
    private readonly Repository _repo;

    public CreateEdit()
    {
      InitializeComponent();
    }

    public CreateEdit(Repository repo) :
      this()
    {
      _repo = repo;
      TxtRepoName.Text = repo.Name;
    }

    private void CreateEdit_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (DialogResult == DialogResult.OK)
      {
        _repo.Name = TxtRepoName.Text;
      }
    }
  }
}
