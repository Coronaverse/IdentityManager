using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRP.IdentityManager.Shared
{
	public class Character
	{
		public int CharacterId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string DateOfBirth { get; set; }
		public string Gender { get; set; }
	}
}
