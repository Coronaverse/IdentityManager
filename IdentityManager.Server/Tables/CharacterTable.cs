using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coronaverse.IdentityManager.Server.Tables
{
	[Table("Character")]
	public class CharacterTable
	{
		[Required]
		public Guid UserId { get; set; }

		[Key]
		public Guid CharacterId { get; set; }

		[Required]
		[StringLength(60)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(60)]
		public string LastName { get; set; }

		[Required]
		public string DateOfBirth { get; set; }

		[Required]
		[StringLength(60)]
		public string Gender { get; set; }
	}
}
