using CRP.IdentityManager.Shared;
using NFive.SDK.Client.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRP.IdentityManager.Client.Overlays
{
	class IdentityManagerOverlay : Overlay
	{
		public event EventHandler<CharacterCreationEventArgs> CharacterCreate;
		public event EventHandler<CharacterLoginEventArgs> CharacterLogin;

		public IdentityManagerOverlay(IOverlayManager manager) : base(manager, "login.html")
		{
			On("create", new Action<Character>(charData => this.CharacterCreate?.Invoke(this, new CharacterCreationEventArgs(this, charData))));
			On("login", new Action<int>(charId => this.CharacterLogin?.Invoke(this, new CharacterLoginEventArgs(this, charId))));
		}

		public void SendCharacters(List<Character> characters)
		{
			this.Emit("characters", characters);
		}

		public void SendCharacterCreationError()
		{
			this.Emit("createfailed");
		}
	}

	public class CharacterCreationEventArgs : OverlayEventArgs
	{
		public string FirstName;
		public string LastName;
		public string DateOfBirth;
		public string Gender;

		public CharacterCreationEventArgs(Overlay overlay, Character character) : base(overlay)
		{
			FirstName = character.FirstName;
			LastName = character.LastName;
			DateOfBirth = character.DateOfBirth;
			Gender = character.Gender;
		}
	}

	public class CharacterLoginEventArgs : OverlayEventArgs
	{
		public int CharacterID;

		public CharacterLoginEventArgs(Overlay overlay, int characterId) : base(overlay)
		{
			CharacterID = characterId;
		}
	}

	
}
