using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GraphML.UI.Desktop
{
  public partial class Main : Form
  {
    private readonly RepositoryManagerServer _repoMgrServer;
    private readonly RepositoryServer _repoServer;

    public Main()
    {
      InitializeComponent();

      var serverUrl = ConfigurationManager.AppSettings["GraphML_ServerUrl"];
      var userName = ConfigurationManager.AppSettings["GraphML_UserName"];
      var password = ConfigurationManager.AppSettings["GraphML_Password"];
      var client = new RestClient(serverUrl)
      {
        Authenticator = new HttpBasicAuthenticator(userName, password)
      };
      client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
      var logFact = new LoggerFactory();

      _repoMgrServer = new RepositoryManagerServer(client);
      _repoServer = new RepositoryServer(client);
    }

    private void Overview_DoubleClick(object sender, EventArgs e)
    {
      using (new AutoCursor())
      {
        var selNode = Overview.SelectedNode;

        if (selNode.Parent == null)
        {
          selNode.Nodes.Clear();
          var repoMgrs = _repoMgrServer.GetAll();
          var childNodes = repoMgrs.Select(x => new TreeNode(x.Name) { Tag = x });
          selNode.Nodes.AddRange(childNodes.ToArray());
        }

        if (selNode.Tag is RepositoryManager repoMgr)
        {
          selNode.Nodes.Clear();
          var repos = _repoServer.ByOwners(new[] { repoMgr.Id });
          var childNodes = repos.Select(x => new TreeNode(x.Name) { Tag = x });
          selNode.Nodes.AddRange(childNodes.ToArray());
        }

        if (selNode.Tag is Repository repo)
        {
          // TODO   expand nodes+edges?
        }
      }
    }
  }
}
