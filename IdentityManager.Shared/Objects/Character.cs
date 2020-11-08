using Coronaverse.IdentityManager.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronaverse.IdentityManager.Shared
{
	public class Character
	{
		public Guid CharacterId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string DateOfBirth { get; set; }
		public string Gender { get; set; }
		public Style Style { get; set; }
	}
}
