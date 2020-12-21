using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
	[Schema.Table(nameof(Role))]
	public sealed class Role : Item
	{
	}
}
