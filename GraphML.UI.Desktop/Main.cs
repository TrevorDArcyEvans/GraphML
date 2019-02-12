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

namespace GraphML.UI.Desktop
{
  public partial class Main : Form
  {
    private readonly string _server;
    private readonly string _userName;
    private readonly string _password;

    public Main()
    {
      InitializeComponent();

      _server = ConfigurationManager.AppSettings["GraphML_Server"];
      _userName = ConfigurationManager.AppSettings["GraphML_UserName"];
      _password = ConfigurationManager.AppSettings["GraphML_Password"];
    }
  }
}
