using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
	[Table("search_index_record")]
	public class DbSearchIndexRecord
	{
		[Column("target_id")]
		public string TargetId { get; set; }
		
		[Column("target_type")]
		public string TargetType { get; set; }
		
		[Column("value_type")]
		public string ValueType { get; set; }

		[Column("value")]
		public string Value { get; set; }
	}
}