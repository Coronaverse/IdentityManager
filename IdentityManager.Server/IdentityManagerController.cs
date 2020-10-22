using JetBrains.Annotations;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Server.Communications;
using NFive.SDK.Server.Controllers;
using CRP.IdentityManager.Shared;
using CRP.IdentityManager.Server.Storage;
using CRP.IdentityManager.Server.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRP.IdentityManager.Server
{
	[PublicAPI]
	public class IdentityManagerController : Controller
	{
		public IdentityManagerController(ILogger logger, ICommunicationManager comms) : base(logger)
		{
			comms.Event(IdentityManagerEvents.Identity).FromClients().OnRequest(e => e.Reply(GetIdentity(e)));
		}

		public Identity GetIdentity(ICommunicationMessage e)
		{
			UserPrivilege up = null;
			List<Character> Characters = new List<Character>();

			using (var context = new StorageContext())
			{
				up = new UserPrivilege(context, e.User.License);

				try
				{
					context.UserPrivileges.Add(up);
					context.SaveChanges();
				}
				catch (Exception ex)
				{
					this.Logger.Debug($"MySql Error 1: {ex.Message}"); // Likely thrown by a dupiclate value... logging just in case
				}

				try
				{
					Characters = context.Characters.Where(a => a.UserId == up.UserId).ToList();
				}
				catch (Exception ex)
				{
					this.Logger.Debug($"MySql Error 2: {ex.Message}");
				}
			}

			List<Shared.CharacterData> CharactersData = MapTo.CharacterData(Characters);
			Identity id = MapTo.Identity(up, CharactersData);

			return id;
		}
	}
}
