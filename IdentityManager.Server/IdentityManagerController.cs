using JetBrains.Annotations;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Server.Communications;
using NFive.SDK.Server.Controllers;
using Coronaverse.IdentityManager.Shared;
using Coronaverse.IdentityManager.Server.Storage;
using Coronaverse.IdentityManager.Server.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using NFive.SDK.Core.Models.Player;
using NFive.SDK.Core.Helpers;
using Z.EntityFramework.Plus;

namespace Coronaverse.IdentityManager.Server
{
	[PublicAPI]
	public class IdentityManagerController : Controller
	{
		public IdentityManagerController(ILogger logger, ICommunicationManager comms) : base(logger)
		{
			comms.Event(IdentityManagerEvents.IdentityGetCharacters).FromClients().OnRequest(e =>
			{
				e.Reply((GetCharacters(e.User)));
			});

			comms.Event(IdentityManagerEvents.IdentityCreateCharacter).FromClients().OnRequest<Character>((e, CharData) =>
			{
				e.Reply(CreateCharacter(e.User, CharData));
			});

			comms.Event(IdentityManagerEvents.IdentityUpdateCharacter).FromClients().OnRequest<Character>((e, CharData) =>
			{
				this.UpdateCharacter(CharData);
			});
		}

		public Character CreateCharacter(User user, Character data)
		{
			using (var context = new StorageContext())
			{
				try
				{
					CharacterTable newChar = new CharacterTable();
					newChar.CharacterId = GuidGenerator.GenerateTimeBasedGuid();
					newChar.UserId = user.Id;
					newChar.FirstName = data.FirstName;
					newChar.LastName = data.LastName;
					newChar.DateOfBirth = data.DateOfBirth;
					newChar.Gender = data.Gender;
					newChar.Style = new StyleTable();
					context.Characters.Add(newChar);
					context.SaveChanges();
					return MapTo.Character(newChar);
				}
				catch (Exception ex)
				{
					this.Logger.Debug($"Character creation failed: {ex}");
				}
			}
			return null;

		}

		public void UpdateCharacter(Character data)
		{
			using (var context = new StorageContext())
			{
				try
				{
					var character = context.Characters.Include("Style").Where(e => e.CharacterId.Equals(data.CharacterId)).First();
					character.FirstName = data.FirstName;
					character.LastName = data.LastName;
					character.DateOfBirth = data.DateOfBirth;
					character.Gender = data.Gender;
					character.Style.UpdateStyle(data.Style);
					context.SaveChanges();
				}
				catch (Exception e)
				{
					this.Logger.Debug($"Character update failed: {e}");
				}
			}
		}

		public List<Character> GetCharacters(User user)
		{
			List<CharacterTable> Characters = new List<CharacterTable>();
			using (var context = new StorageContext())
			{
				try 
				{
					Characters = context.Characters.Include("Style").Where(a => a.UserId == user.Id).ToList();
				} catch (Exception ex)
				{
					this.Logger.Debug($"MySQL Error: {ex.Message}");
				}
			}
			return Characters.Select(x => MapTo.Character(x)).ToList();
		}
	}
}
