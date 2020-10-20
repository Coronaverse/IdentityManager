using System;
using NFive.SDK.Core.Controllers;
using System.Collections.Generic;
using CRP.IdentityManager.Shared.Definitions;


namespace CRP.IdentityManager.Shared
{
	public class Identity : ControllerConfiguration
	{
		public string Example { get; set; } = "Hello World";

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
