using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Coronaverse.IdentityManager.Client.Models;
using Coronaverse.IdentityManager.Shared;
using NFive.SDK.Client.Extensions;
using NFive.SDK.Client.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronaverse.IdentityManager.Client.Overlays
{
	class IdentityManagerOverlay : Overlay
	{
		public event EventHandler<CharacterCreationEventArgs> CharacterCreate;
		public event EventHandler<CharacterLoginEventArgs> CharacterLogin;

		public IdentityManagerOverlay(IOverlayManager manager) : base(manager, "login.html")
		{
			On("create", new Action<CharacterJS>(charData => this.CharacterCreate?.Invoke(this, new CharacterCreationEventArgs(this, charData))));
			On("login", new Action<Guid>(charData => this.CharacterLogin?.Invoke(this, new CharacterLoginEventArgs(this, charData))));
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

	public class CharacterJS
	{
		public string FirstName;
		public string LastName;
		public string DateOfBirth;
		public short Gender;
	}

	public class CharacterCreationEventArgs : OverlayEventArgs
	{
		public string FirstName;
		public string LastName;
		public DateTime DateOfBirth;
		public short Gender;

		public CharacterCreationEventArgs(Overlay overlay, CharacterJS character) : base(overlay)
		{
			FirstName = character.FirstName;
			LastName = character.LastName;
			DateOfBirth = DateTime.Parse(character.DateOfBirth, null, System.Globalization.DateTimeStyles.RoundtripKind); 
			Gender = character.Gender;
		}
	}

	public class CharacterLoginEventArgs : OverlayEventArgs
	{
		public Guid CharacterId;

		public CharacterLoginEventArgs(Overlay overlay, Guid id) : base(overlay)
		{
			this.CharacterId = id;
		}
	}

	
}
