using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Communications;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using Coronaverse.IdentityManager.Shared;
using NFive.SDK.Core.Utilities;
using CitizenFX.Core;
using System.Collections.Generic;
using CitizenFX.Core.UI;
using NFive.SDK.Client.Extensions;
using CitizenFX.Core.Native;
using Coronaverse.IdentityManager.Client.Overlays;
using Coronaverse.IdentityManager.Shared.Definitions;

namespace Coronaverse.IdentityManager.Client
{
	[PublicAPI]
	public class IdentityManagerService : Service
	{
		private IdentityManagerOverlay overlay;
		private List<Character> Characters;
		private Character LoggedInCharacter = null;

		public IdentityManagerService(ILogger logger, ITickManager ticks, ICommunicationManager comms, ICommandManager commands, IOverlayManager overlay, User user) : base(logger, ticks, comms, commands, overlay, user)
		{
			
		}

		public override async Task HoldFocus()
		{
			// Hide HUD
			Screen.Hud.IsVisible = false;

			// Disable the loading screen from automatically being dismissed
			// No longer works, requires "loadscreen_manual_shutdown 'yes'" in __resource.lua:
			// https://github.com/citizenfx/fivem/blob/7208a2a63fe5da65ce5ea785032d148ae9354ac1/code/components/loading-screens-five/src/LoadingScreens.cpp#L146
			//API.SetManualShutdownLoadingScreenNui(true);

			// Position character, required for switching
			Game.Player.Character.Position = Vector3.Zero;

			// Freeze player
			Game.Player.Freeze();

			// Switch out the player if it isn't already in a switch state
			if (!API.IsPlayerSwitchInProgress()) API.SwitchOutPlayer(API.PlayerPedId(), 0, 1);

			// Remove most clouds
			//API.SetCloudHatOpacity(0.01f);

			// Wait for switch
			while (API.GetPlayerSwitchState() != 5) await Delay(10);

			// Hide loading screen
			API.ShutdownLoadingScreen();

			// Fade out
			Screen.Fading.FadeOut(0);
			while (Screen.Fading.IsFadingOut) await Delay(10);

			// Create overlay
			var overlay = new IdentityManagerOverlay(this.OverlayManager);
			overlay.CharacterCreate += OnCharacterCreate;
			overlay.CharacterLogin += OnCharacterLogin;
			this.Characters = await this.Comms.Event(IdentityManagerEvents.IdentityGetCharacters).ToServer().Request<List<Character>>();
			overlay.SendCharacters(this.Characters);

			// Focus overlay
			this.OverlayManager.Focus(true, true);

			// Shut down the NUI loading screen
			API.ShutdownLoadingScreenNui();

			// Fade in
			Screen.Fading.FadeIn(500);
			while (this.LoggedInCharacter == null) await Delay(100);
		}


		private async void OnCharacterCreate(object sender, CharacterCreationEventArgs character)
		{
			Character newCharacter = await this.Comms.Event(IdentityManagerEvents.IdentityCreateCharacter).ToServer().Request<Character>(new Character()
			{
				FirstName = character.FirstName,
				LastName = character.LastName,
				DateOfBirth = character.DateOfBirth,
				Gender = character.Gender
			});
			if (newCharacter == null)
			{
				
			} else
			{
				this.LoggedInCharacter = newCharacter;
				this.OnPlay();
				this.OverlayManager.Focus(false, false);
				character.Overlay.Dispose();
			}
		}

		private async void OnCharacterLogin(object sender, CharacterLoginEventArgs character)
		{
			Debug.WriteLine($"Logging in character: {character.Character.CharacterId}");
			this.LoggedInCharacter = this.Characters.Find(c => c.CharacterId == character.Character.CharacterId);
			this.OnPlay();
			this.OverlayManager.Focus(false, false);
			character.Overlay.Dispose();

		}

		public override async Task Started()
		{
			// Create events
			this.Comms.Event(IdentityManagerEvents.CharacterGet).FromClient().OnRequest(e =>
			{
				e.Reply(this.LoggedInCharacter);
			});
		}

		private async void OnPlay()
		{
			Game.Player.Character.Position = new Vector3(0f, 0f, 71f);

			Model model = this.LoggedInCharacter.Gender == "male" ? new Model(PedHash.FreemodeMale01) : new Model(PedHash.FreemodeFemale01);
			while (!await Game.Player.ChangeModel(model)) await Delay(10);
			Game.Player.Character.Style.SetDefaultClothes();

			Game.Player.Unfreeze();

			Screen.Hud.IsVisible = true;

			API.SwitchInPlayer(API.PlayerPedId());

			//API.SetCloudHatOpacity(1f);

			this.Comms.Event(IdentityManagerEvents.CharacterLogin).ToClient().Emit(this.LoggedInCharacter);
		}
	}
}
