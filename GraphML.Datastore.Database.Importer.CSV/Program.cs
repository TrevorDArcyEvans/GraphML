using CsvHelper.Configuration;
using Dapper.Contrib.Extensions;
using GraphML.Common;
using Microsoft.Extensions.Configuration;
using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Globalization;
using Dapper;

namespace GraphML.Datastore.Database.Importer.CSV
{
	public sealed class Program
	{
		public static void Main(string[] args)
		{
			// check import spec file
			if (args.Length == 1 && !File.Exists(args[0]))
			{
				Usage();
				return;
			}

			var importSpecPath = args.Length == 0 ? "import.json" : args[0];
			new Program(importSpecPath, LogInformation).Run();
		}

		private readonly ImportSpecification _importSpec = new ImportSpecification();
		private readonly IConfiguration _config;
		private readonly Action<string> _logInfoAction;

		public Program(string importSpecPath, Action<string> logInfoAction)
		{
			_config = new ConfigurationBuilder()
			  .AddJsonFile(importSpecPath)
			  .AddEnvironmentVariables()
			  .Build();
			_config.GetSection("ImportSpecification").Bind(_importSpec);
			_logInfoAction = logInfoAction;
		}

		public void Run()
		{
			SqlMapper.AddTypeHandler(new GuidTypeHandler());
			SqlMapper.RemoveTypeMap(typeof(Guid));
			SqlMapper.RemoveTypeMap(typeof(Guid?));

			DumpSettings(_config);
			_logInfoAction(Environment.NewLine);

			var sw = Stopwatch.StartNew();
			using (var tr = File.OpenText(_importSpec.DataFile))
			{
				var csvCfg = new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					Delimiter = Path.GetExtension(_importSpec.DataFile).ToLowerInvariant() == ".csv" ? "," : "\t",
					AllowComments = true,
					HeaderValidated = null,
					HasHeaderRecord = _importSpec.HasHeaderRecord
				};
				using (var csv = new CsvHelper.CsvReader(tr, csvCfg))
				{
					csv.Context.RegisterClassMap<ImportEdgeClassMap>();

					var edges = csv.GetRecords<ImportEdge>().ToList();
					var nodes = edges.SelectMany(edge => new[] { edge.FromNode, edge.ToNode }).Distinct();

					var dbConnFact = new DbConnectionFactory(_config);
					using (var conn = dbConnFact.Get())
					{
						var org = conn.GetAll<Organisation>().Single(o => o.Name == _importSpec.Organisation);
						var repoMgr = conn.GetAll<RepositoryManager>().Single(rm => rm.Name == _importSpec.RepositoryManager && rm.OrganisationId == org.Id);
						var repo = conn.GetAll<Repository>().SingleOrDefault(r => r.Name == _importSpec.Repository);
						if (repo is null)
						{
							repo = new Repository
							{
								Name = _importSpec.Repository,
								OrganisationId = org.Id,
								RepositoryManagerId = repoMgr.Id
							};
							conn.Insert(repo);
						}

						// TODO	  GetOrCreate	  graph
            var graph = new Graph(repo.Id, repo.OrganisationId, _importSpec.Graph)
            {
                Directed = _importSpec.Directed
            };

            _logInfoAction($"Importing from:  {_importSpec.DataFile}");
						_logInfoAction($"          into:  {conn.ConnectionString}");
						_logInfoAction($"    repository:  {repo.Name}");
						_logInfoAction($"         graph:  {graph.Name}");
						_logInfoAction(Environment.NewLine);

						var modelNodes = nodes.Select(node => new Node(graph.Id, graph.OrganisationId, node)).ToList();
						var modelNodesMap = modelNodes.ToDictionary(node => node.Name);
						var modelEdges = edges.Select(edge =>
						  new Edge(
							graph.Id,
							graph.OrganisationId,
							$"{edge.FromNode}-->{edge.ToNode}",
							modelNodesMap[edge.FromNode].Id,
							modelNodesMap[edge.ToNode].Id)).ToList();

						_logInfoAction($"Transformed data at         : {sw.ElapsedMilliseconds} ms");

						using (var trans = conn.BeginTransaction())
						{
							if (conn is SqlConnection && trans is SqlTransaction)
							{
								var bulky = new BulkUploadToMsSqlServer((SqlConnection)conn, (SqlTransaction)trans);

								bulky.Commit(new[] { graph }, GetTableName<Graph>());

								_logInfoAction($"Started node import at      : {sw.ElapsedMilliseconds} ms");
								bulky.Commit(modelNodes, GetTableName<Node>());
								_logInfoAction($"  finished at               : {sw.ElapsedMilliseconds} ms");

								_logInfoAction($"Started edge import at      : {sw.ElapsedMilliseconds} ms");
								bulky.Commit(modelEdges, GetTableName<Edge>());
								_logInfoAction($"  finished at               : {sw.ElapsedMilliseconds} ms");
							}
							else
							{
								conn.Insert(graph, trans);

								_logInfoAction($"Started node import at      : {sw.ElapsedMilliseconds} ms");
								conn.Insert(modelNodes, trans);
								_logInfoAction($"  finished at               : {sw.ElapsedMilliseconds} ms");

								_logInfoAction($"Started edge import at      : {sw.ElapsedMilliseconds} ms");
								conn.Insert(modelEdges, trans);
								_logInfoAction($"  finished at               : {sw.ElapsedMilliseconds} ms");
							}

							_logInfoAction($"Started database commit     : {sw.ElapsedMilliseconds} ms");
							trans.Commit();
							_logInfoAction($"  finished at               : {sw.ElapsedMilliseconds} ms");

							_logInfoAction(Environment.NewLine);
						}
						_logInfoAction($"Finished!");
						_logInfoAction($"  Imported {modelNodes.Count()} nodes and {modelEdges.Count()} edges in {sw.ElapsedMilliseconds} ms");
					}
				}
			}
		}

		private static void Usage()
		{
			Console.WriteLine("Usage:");
			Console.WriteLine("  GraphML.Datastore.Database.Importer.CSV.exe <import.json>");
			Console.WriteLine();
			Console.WriteLine("Notes:");
			Console.WriteLine("  Database connection is contained in import.json");
		}

		private void DumpSettings(IConfiguration config)
		{
			_logInfoAction("Settings:");
			_logInfoAction($"  REPOSITORY:");
			_logInfoAction($"    NAME                        : {_importSpec.Repository}");
			_logInfoAction($"  DATASTORE:");
			_logInfoAction($"    DATASTORE_CONNECTION        : {config.DATASTORE_CONNECTION()}");
			_logInfoAction($"    DATASTORE_CONNECTION_TYPE   : {config.DATASTORE_CONNECTION_TYPE(config.DATASTORE_CONNECTION())}");
			_logInfoAction($"    DATASTORE_CONNECTION_STRING : {config.DATASTORE_CONNECTION_STRING(config.DATASTORE_CONNECTION())}");
		}

		private static string GetTableName<T>()
		{
			var tableName = typeof(T).GetCustomAttribute<Schema.TableAttribute>().Name;

			return tableName;
		}

		private static void LogInformation(string message = null)
		{
			Console.WriteLine(message ?? Environment.NewLine);
		}

		private sealed class ImportEdge
		{
			public string FromNode { get; set; }
			public string ToNode { get; set; }
		}

		private sealed class ImportEdgeClassMap : ClassMap<ImportEdge>
		{
			public ImportEdgeClassMap()
			{
				Map(m => m.FromNode).Index(0);
				Map(m => m.ToNode).Index(1);
			}
		}

		public sealed class ImportSpecification
		{
			public string Organisation { get; set; }
			public string RepositoryManager { get; set; }
			public string Repository { get; set; }
			public string Graph { get; set; }
			public bool Directed { get; set; }

			public string DataFile { get; set; }
			public bool HasHeaderRecord { get; set; }
		}
	}
}
