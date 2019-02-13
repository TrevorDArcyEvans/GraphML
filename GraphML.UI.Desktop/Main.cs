using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json;

namespace GraphML.UI.Desktop
{
  public partial class Main : Form
  {
    private readonly Server _server;

    public Main()
    {
      InitializeComponent();

      _server = new Server(
        ConfigurationManager.AppSettings["GraphML_ServerUrl"],
        ConfigurationManager.AppSettings["GraphML_UserName"],
        ConfigurationManager.AppSettings["GraphML_Password"]);
    }

    private void CmdRepositoryManager_Click(object sender, EventArgs e)
    {
      TxtResults.Text = JsonConvert.SerializeObject(_server.RepositoryManager_GetAll(), new JsonSerializerSettings { Formatting = Formatting.Indented });
    }
  }
}
