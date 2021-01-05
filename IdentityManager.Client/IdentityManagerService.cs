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
using CitizenFX.Core;
using System.Collections.Generic;
using CitizenFX.Core.UI;
using NFive.SDK.Client.Extensions;
using CitizenFX.Core.Native;
using Coronaverse.IdentityManager.Client.Overlays;
using Coronaverse.IdentityManager.Client.Models;
using NFive.SDK.Core.Extensions;
using Coronaverse.IdentityManager.Shared.Events;
using Coronaverse.IdentityManager.Client.Events;

namespace Coronaverse.IdentityManager.Client
{
	[PublicAPI]
	public class IdentityManagerService : Service
	{
		private IdentityManagerOverlay overlay;
		private List<Character> Characters;
		private CharacterSession CurrentSession = null;
		private IdentityConfiguration config;

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
			while (this.CurrentSession == null) await Delay(100);
		}


		private async void OnCharacterCreate(object sender, CharacterCreationEventArgs character)
		{
			CreateCharacterEvent newCharacter = await this.Comms.Event(IdentityManagerEvents.IdentityCreateCharacter).ToServer().Request<CreateCharacterEvent>(new Character()
			{
				FirstName = character.FirstName,
				MiddleName = "MiddleName",
				LastName = character.LastName,
				DateOfBirth = character.DateOfBirth,
				Gender = character.Gender,
				WalkingStyle = null,
				Model = ((uint)(character.Gender == 0 ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01)).ToString()
		});
			if (newCharacter.Status != CreateCharacterStatus.GOOD)
			{
				//TODO: Return overlay with error message
			} else
			{

				this.CurrentSession = await this.Comms.Event(IdentityManagerEvents.IdentityLoginCharacter).ToServer().Request<CharacterSession>(newCharacter.Character.Id);
				this.Characters = await this.Comms.Event(IdentityManagerEvents.IdentityGetCharacters).ToServer().Request<List<Character>>();
				this.CurrentSession.Character = this.Characters.Find(c => this.CurrentSession.CharacterId == c.Id);
				this.OnPlay();
				this.OverlayManager.Focus(false, false);
				character.Overlay.Dispose();
			}
		}

		private async void OnCharacterLogin(object sender, CharacterLoginEventArgs character)
		{
			this.CurrentSession = await this.Comms.Event(IdentityManagerEvents.IdentityLoginCharacter).ToServer().Request<CharacterSession>(character.CharacterId);
			this.CurrentSession.Character = this.Characters.Find(c => this.CurrentSession.CharacterId == c.Id);
			this.OnPlay();
			this.OverlayManager.Focus(false, false);
			character.Overlay.Dispose();

		}

		public override async Task Started()
		{
			this.config = await this.Comms.Event(IdentityManagerEvents.IdentityConfiguration).ToServer().Request<IdentityConfiguration>();

			// Create events
			this.Comms.Event(IdentityManagerEvents.CharacterGet).FromClient().OnRequest(e =>
			{
				e.Reply(this.CurrentSession.Character);
			});
		}

		private async void OnPlay()
		{
			Game.Player.Character.Position = this.CurrentSession.Character.Position.ToVector3().ToCitVector3();

			await this.CurrentSession.Character.Render(this.Logger);

			Game.Player.Unfreeze();

			Screen.Hud.IsVisible = true;

			API.SwitchInPlayer(API.PlayerPedId());

			//API.SetCloudHatOpacity(1f);

			this.Comms.Event(IdentityManagerEvents.CharacterLogin).ToClient().Emit(this.CurrentSession);
			this.Ticks.On(OnSavePosition);
		}

		public async Task OnSavePosition()
		{
			SavePosition();

			await Delay(this.config.Autosave.PositionInterval);
		}

		private void SavePosition()
		{
			if (this.CurrentSession == null)
				return;

			this.Comms.Event(IdentityManagerEvents.IdentitySavePosition).ToServer().Emit(this.CurrentSession.CharacterId, Game.Player.Character.Position.ToVector3().ToPosition());
		}
	}
}
