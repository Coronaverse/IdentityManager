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
using NFive.SDK.Core.Models.Player;

namespace CRP.IdentityManager.Server
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
				e.Reply(CreateCharacter(CharData));
			});
		}

		public Character CreateCharacter(Character data)
		{
			using (var context = new StorageContext())
			{
				try
				{
					CharacterTable newChar = new CharacterTable();
					newChar.FirstName = data.FirstName;
					newChar.LastName = data.LastName;
					newChar.DateOfBirth = data.DateOfBirth;
					newChar.Gender = data.Gender;
					context.Characters.Add(newChar);
					context.SaveChanges();
					return MapTo.Character(newChar);
				}
				catch (Exception ex)
				{
					this.Logger.Debug($"Character creation failed: {ex.Message}");
				}
			}
			return null;

		}

		public List<Character> GetCharacters(User user)
		{
			List<CharacterTable> Characters = new List<CharacterTable>();
			using (var context = new StorageContext())
			{
				try
				{
					Characters = context.Characters.Where(a => a.UserId == user.Id).ToList();
				} catch (Exception ex)
				{
					this.Logger.Debug($"MySQL Error: {ex.Message}");
				}
			}
			return Characters.Select(x => MapTo.Character(x)).ToList();
		}
	}
}
