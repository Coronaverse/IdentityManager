using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRP.IdentityManager.Server.Tables
{
	[Table("Character")]
	public class CharacterTable
	{
		[Required]
		public Guid UserId { get; set; }

		[Key]
		[Required]
		[Index(IsUnique = true)]
		public int CharacterId { get; set; }

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

		#region Methods

		public CharacterTable()
		{
			this.UserId = Guid.NewGuid();
			this.GenerateId();
		}

		public void GenerateId()
		{
			Random r = new Random();
			this.CharacterId = r.Next(1000000, 9999999);
		}

		#endregion
	}
}
