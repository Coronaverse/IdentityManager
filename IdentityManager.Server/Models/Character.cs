using Coronaverse.IdentityManager.Server.Storage;
using Coronaverse.IdentityManager.Shared;
using Coronaverse.IdentityManager.Shared.Objects;
using Newtonsoft.Json;
using NFive.SDK.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Linq;
using Coronaverse.IdentityManager.Server.Models;
using NFive.SDK.Core.Models.Player;

namespace Coronaverse.IdentityManager.Server.Models
{
	[Table("Character")]
	public class Character : IdentityModel, ICharacter
	{
		[Required]
		[ForeignKey("User")]
		public Guid UserId { get; set; }

		public virtual User User { get; set; }

		[Key]
		public Guid CharacterId { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 2)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 0)]
		public string MiddleName { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 2)]
		public string LastName { get; set; }

		[Required]
		public DateTime DateOfBirth { get; set; }

		[Required]
		[Range(0, 1)]
		public short Gender { get; set; }

		[Required]
		public bool Alive { get; set; }

		[Required]
		[Range(0, 10000)]
		public int Health { get; set; }

		[Required]
		[Range(0, 100)]
		public int Armor { get; set; }

		[Required]
		[Range(100000000, 999999999)]
		public int SSN { get; set; }

		[Required]
		public Position Position { get; set; }

		[Required]
		[StringLength(200)]
		public string WalkingStyle { get; set; }

		[Required]
		[StringLength(200)] // TODO
		public string Model { get; set; }

		[Required]
		[ForeignKey("Style")]
		public Guid StyleId { get; set; }

		public virtual Style Style { get; set; }

		[Required]
		[ForeignKey("Appearance")]
		public Guid AppearanceId { get; set; }

		public virtual Appearance Appearance { get; set; }

		[Required]
		[ForeignKey("FaceShape")]
		public Guid FaceShapeId { get; set; }

		public virtual FaceShape FaceShape { get; set; }

		[Required]
		[ForeignKey("Heritage")]
		public Guid HeritageId { get; set; }

		public virtual Heritage Heritage { get; set; }

		public DateTime? LastPlayed { get; set; }

		[JsonIgnore]
		public string FullName => $"{this.FirstName} {this.MiddleName} {this.LastName}".Replace("  ", " ");

		public static int GenerateSSN()
		{
			var rng = new Random();
			int ssn;
			using (StorageContext db = new StorageContext())
			{
				
				do
				{
					ssn = rng.Next(100000000, 999999999);
					if (db.Characters.Any(o => o.SSN == ssn)) {
						ssn = 0;
						continue;
					}
				} while (Regex.IsMatch(ssn.ToString(), "^(?!b(d)1+b)(?!123456789|219099999|078051120)(?!666|000|9d{2})d{3}(?!00)d{2}(?!0{4})d{4}$"));
			}
			

			return ssn;
		}
	}
}
