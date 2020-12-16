namespace Coronaverse.IdentityManager.Shared
{
	public static class IdentityManagerEvents
	{
		public const string IdentityConfiguration = "crp:identitymanager:configuration";
		public const string Identity = "crp:identitymanager:identity";
		public const string GetLocalIdentity = "crp:identitymanager:getLocalIdentity";

		// Client -> Client events
		public const string CharacterCreated = "crp:identitymanager:charactercreated";
		public const string CharacterLogin = "crp:identitymanager:identitylogin";
		public const string CharacterGet = "crp:identitymanager:characterget";
		public const string CharacterSync = "crp:identitymanager:charactersync"; // Trigger a manual sync of character data

		// Client -> Server events
		public const string IdentityGetCharacters = "crp:identitymanager:getcharacters";
		public const string IdentityCreateCharacter = "crp:identitymanager:createcharacter";
		public const string IdentityDeleteCharacter = "crp:identitymanager:deletecharacter";
		public const string IdentityLoginCharacter = "crp:identitymanager:logincharacter";
		public const string IdentityUpdateCharacter = "crp:identitymanager:updatecharacter";
		public const string IdentitySavePosition = "crp:identitymanager:saveposition";
	}
}
