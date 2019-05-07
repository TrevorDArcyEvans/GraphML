using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    }

    public Main(IServiceProvider sp) :
      this()
    {
      var config = sp.GetService<IConfiguration>();

      var serverUrl= Settings.API_URI(config);
      var userName = Settings.API_USERNAME(config);
      var password = Settings.API_PASSWORD(config);
      var client = new RestClient(serverUrl)
      {
        Authenticator = new HttpBasicAuthenticator(userName, password)
      };
      client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

      var logger = sp.GetService<ILogger<Main>>();
      logger.LogInformation("Hello, world!");

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
