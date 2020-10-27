namespace Coronaverse.IdentityManager.Shared
{
	public static class IdentityManagerEvents
	{
		public const string Identity = "crp:identitymanager:identity";
		public const string GetLocalIdentity = "crp:identitymanager:getLocalIdentity";

		// Client -> Client events
		public const string CharacterCreated = "crp:identitymanager:charactercreated";
		public const string CharacterLogin = "crp:identitymanager:identitylogin";
		public const string CharacterGet = "crp:identitymanager:characterget";

		// Client -> Server events
		public const string IdentityGetCharacters = "crp:identitymanager:getcharacters";
		public const string IdentityCreateCharacter = "crp:identitymanager:createcharacter";
	}
}
