using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRP.IdentityManager.Shared.Definitions
{
	public class Privileges
	{
		#region Enum Definitions

		public enum AuthorityLevel
		{
			Basic,
			Priority,
			Moderator,
			Administrator,
			Super
		}

		public enum PoliceLevel
		{
			None,
			Rookie,
			Officer,
			Sergeant,
			Lieutenant,
			Captain,
			Chief
		}

		public enum EMSLevel
		{
			None,
			Rookie,
			Medic,
			Doctor,
			Surgeon,
			Chief
		}

		public enum AccessLevel
		{
			Banned,
			Default,
			Whitelisted
		}

		#endregion
	}
}
