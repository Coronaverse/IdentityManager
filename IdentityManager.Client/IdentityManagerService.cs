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
using CRP.IdentityManager.Shared;
using NFive.SDK.Core.Utilities;
using CitizenFX.Core;
using System.Collections.Generic;
using CitizenFX.Core.UI;
using NFive.SDK.Client.Extensions;
using CitizenFX.Core.Native;
using CRP.IdentityManager.Client.Overlays;
using Coronaverse.Notifications.Shared;

namespace CRP.IdentityManager.Client
{
	[PublicAPI]
	public class IdentityManagerService : Service
	{
		private IdentityManagerOverlay overlay;
		private List<Character> Characters;
		private Character LoggedInCharacter;

		public IdentityManagerService(ILogger logger, ITickManager ticks, ICommunicationManager comms, ICommandManager commands, IOverlayManager overlay, User user) : base(logger, ticks, comms, commands, overlay, user)
		{
			this.overlay = new IdentityManagerOverlay(overlay);
		}

		public override async Task HoldFocus()
		{
			// Grab User Characters
			this.Characters = await this.Comms.Event(IdentityManagerEvents.IdentityGetCharacters).ToServer().Request<List<Character>>();

			this.overlay.SendCharacters(this.Characters);

			Screen.Hud.IsVisible = false;

			Game.Player.Character.Position = Vector3.Zero;

			Game.Player.Freeze();

			if (!API.IsPlayerSwitchInProgress())
			{
				API.SwitchOutPlayer(Game.PlayerPed.Handle, 0, 1);
			}

			API.SetCloudHatOpacity(0.01f);

			while (API.GetPlayerSwitchState() != 5)
				await Delay(10);

			API.ShutdownLoadingScreen();

			Screen.Fading.FadeOut(0);
			while (Screen.Fading.IsFadedOut)
			{
				await Delay(10);
			}

			this.overlay.CharacterCreate += OnCharacterCreate;
			this.overlay.CharacterLogin += OnCharacterLogin;
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
				this.Comms.Event(NotificationsEvents.Notification).ToClient().Emit(new Coronaverse.Notifications.Shared.Notification()
				{
					Icon = "exclamation-triangle",
					Title = "Character Creation Failed",
					Message = "Please try again at a later time to create your character, or contact an admin if you continue to have issues",
					Variant = Variant.Warning
				});
			}
		}

		private async void OnCharacterLogin(object sender, CharacterLoginEventArgs character)
		{
			this.LoggedInCharacter = this.Characters.Find(c => c.CharacterId == character.CharacterID);
			this.Comms.Event(IdentityManagerEvents.CharacterLogin).ToClient().Emit(this.LoggedInCharacter);
			this.overlay.Dispose();
		}

		public override async Task Started()
		{
			// Create events
			this.Comms.Event(IdentityManagerEvents.CharacterGet).FromClient().OnRequest(e =>
			{
				e.Reply(this.LoggedInCharacter);
			});
		}
	}
}
