using System.Collections.Generic;
using CRP.IdentityManager.Server.Tables;
using CRP.IdentityManager.Shared;

namespace CRP.IdentityManager.Server
{
	public static class MapTo
	{
		public static List<Shared.CharacterData> CharacterData(List<Character> Characters)
		{
			List<Shared.CharacterData> CharactersData = new List<Shared.CharacterData>();

			foreach (Character Character in Characters)
			{
				Shared.CharacterData CharacterData = new Shared.CharacterData();

				CharacterData.CharacterId = Character.CharacterId;
				CharacterData.FirstName = Character.FirstName;
				CharacterData.LastName = Character.LastName;
				CharacterData.Age = Character.Age;

				CharactersData.Add(CharacterData);
			}

			return CharactersData;
		}

		public static Identity Identity(UserPrivilege up, List<CharacterData> cd)
		{
			Identity id = new Identity();

			id.UserId = up.UserId;
			id.AuthorityLevel = up.AuthorityLevel;
			id.PoliceRank = up.PoliceRank;
			id.EMSRank = up.EMSRank;
			id.AccessLevel = up.AccessLevel;
			id.Characters = cd;

			return id;
		}
	}
}
