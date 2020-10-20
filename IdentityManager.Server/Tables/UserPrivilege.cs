using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CitizenFX.Core;
using CRP.IdentityManager.Shared.Definitions;
using CRP.IdentityManager.Server.Storage;

namespace CRP.IdentityManager.Server.Tables
{
	public class UserPrivilege
	{
		[Key]
		[Required]
		[Index(IsUnique = true)]
		public Guid UserId { get; set; }

		[Required]
		[EnumDataType(typeof(Privileges.AuthorityLevel))]
		public Privileges.AuthorityLevel AuthorityLevel { get; set; }

		[Required]
		[EnumDataType(typeof(Privileges.PoliceLevel))]
		public Privileges.PoliceLevel PoliceRank { get; set; }

		[Required]
		[EnumDataType(typeof(Privileges.EMSLevel))]
		public Privileges.EMSLevel EMSRank { get; set; }

		[Required]
		[EnumDataType(typeof(Privileges.AccessLevel))]
		public Privileges.AccessLevel AccessLevel { get; set; }

		public UserPrivilege(StorageContext context, string license)
		{
			var uid = context.Users.Single(a => a.License == license);
			this.UserId = uid.Id;
			this.AuthorityLevel = Privileges.AuthorityLevel.Basic;
			this.PoliceRank = Privileges.PoliceLevel.None;
			this.EMSRank = Privileges.EMSLevel.None;
			this.AccessLevel = Privileges.AccessLevel.Default;
		}
	}
}
