using NFive.SDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronaverse.IdentityManager.Shared
{
	public interface ICharacter : IIdentityModel
	{
		string FirstName { get; set; }
		string MiddleName { get; set; }
		string LastName { get; set; }
		DateTime DateOfBirth { get; set; }
		short Gender { get; set; }
		bool Alive { get; set; }
		int Health { get; set; }
		int Armor { get; set; }
		int SSN { get; set; }
		Position Position { get; set; }
		string Model { get; set; }
		string WalkingStyle { get; set; }

		DateTime? LastPlayed { get; set; }
	}
}
