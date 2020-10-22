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

namespace CRP.IdentityManager.Client
{
	[PublicAPI]
	public class IdentityManagerService : Service
	{
		public Identity Identity { get; set; } = new Identity();

		public IdentityManagerService(ILogger logger, ITickManager ticks, ICommunicationManager comms, ICommandManager commands, IOverlayManager overlay, User user) : base(logger, ticks, comms, commands, overlay, user) { }

		public override async Task Started()
		{
			// Lookup local identity from server
			this.Identity = await this.Comms.Event(IdentityManagerEvents.Identity).ToServer().Request<Identity>();

			// Create events
			Comms.Event(IdentityManagerEvents.GetLocalIdentity).FromClient().OnRequest(e => e.Reply(GetLocalIdentity()));

			#region Example Debug
			this.Logger.Debug($"From server config: {this.Identity.UserId.ToString()}");
			foreach (CharacterData c in this.Identity.Characters)
			{
				this.Logger.Debug($"Character: {c.CharacterId.ToString()} - {c.FirstName} {c.LastName}");
			}
			#endregion
		}

		public string GetLocalIdentity()
		{
			Serializer s = new Serializer();
			string r = s.Serialize(this);
			return r;
		}
	}
}
