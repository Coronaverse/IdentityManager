using System.Collections.Generic;
using CitizenFX.Core;
using Coronaverse.IdentityManager.Server.Tables;
using Coronaverse.IdentityManager.Shared;

namespace Coronaverse.IdentityManager.Server
{
	public static class MapTo
	{
		public static List<Shared.Character> CharacterData(List<Character> Characters)
		{
			List<Shared.Character> CharactersData = new List<Shared.Character>();

			foreach (Character Character in Characters)
			{
				Shared.Character CharacterData = new Shared.Character();

				CharacterData.CharacterId = Character.CharacterId;
				CharacterData.FirstName = Character.FirstName;
				CharacterData.LastName = Character.LastName;
				CharacterData.DateOfBirth = Character.DateOfBirth;
				CharacterData.Gender = Character.Gender;
				CharacterData.Style = Character.Style;

				CharactersData.Add(CharacterData);
			}

			return CharactersData;
		}

		public static Character Character(Character data)
		{
			Debug.WriteLine($"Character Style: {data.Style.Id}, FaceIndex: {data.Style.Face.Index}");
			return new Character()
			{
				CharacterId = data.CharacterId,
				FirstName = data.FirstName,
				LastName = data.LastName,
				DateOfBirth = data.DateOfBirth,
				Gender = data.Gender,
				Style = data.Style
			};
		}
	}
}
