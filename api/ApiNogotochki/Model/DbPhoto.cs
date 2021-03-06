﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
	[Table("photos")]
	public class DbPhoto
	{
		[Key]
		[Column("id")]
		public string Id { get; set; }

		[Column("content")]
		public byte[] Content { get; set; }
	}
}