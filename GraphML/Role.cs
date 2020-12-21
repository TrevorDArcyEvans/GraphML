using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
	[Schema.Table(nameof(Role))]
	public sealed class Role
	{
		public string Id { get; set; }

		[Write(false)]
		public string Name
		{
			get => Id;
			set => Id = value;
		}

		public Role()
		{
		}

		public Role(string id)
		{
			Id = id;
		}
	}
}