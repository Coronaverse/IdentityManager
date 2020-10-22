using System;
using System.Collections.Generic;
using System.Diagnostics;
using CRP.IdentityManager.Shared.Definitions;
using NFive.SDK.Core.Utilities;

namespace CRP.IdentityManager.Shared
{
	public interface IIdentity
	{
		Guid UserId { get; set; }
		List<CharacterData> Characters { get; set; }
		Privileges.AuthorityLevel AuthorityLevel { get; set; }
		Privileges.PoliceLevel PoliceRank { get; set; }
		Privileges.EMSLevel EMSRank { get; set; }
		Privileges.AccessLevel AccessLevel { get; set; }
		CharacterData Character { get; }
	}

	public class Identity : IIdentity
	{
		//public string Example { get; set; } = "Hello World";

		public Guid UserId { get; set; }
		public List<CharacterData> Characters { get; set; } = new List<CharacterData>();
		public Privileges.AuthorityLevel AuthorityLevel { get; set; }
		public Privileges.PoliceLevel PoliceRank { get; set; }
		public Privileges.EMSLevel EMSRank { get; set; }
		public Privileges.AccessLevel AccessLevel { get; set; }

		private CharacterData _character = new CharacterData();
		public CharacterData Character { get { return _character; } } 

		public void SelectCharacter(CharacterData c)
		{
			_character = c;
			//TODO need to complete character selection
		}
	}
}
