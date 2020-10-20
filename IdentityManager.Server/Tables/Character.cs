using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRP.IdentityManager.Server.Tables
{
	public class Character
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
		public int Age { get; set; }

		#region Methods

		public Character()
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
