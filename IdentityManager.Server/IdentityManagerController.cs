using Coronaverse.IdentityManager.Server.Models;
using Coronaverse.IdentityManager.Server.Storage;
using Coronaverse.IdentityManager.Shared.Events;
using Coronaverse.IdentityManager.Shared;
using JetBrains.Annotations;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Helpers;
using NFive.SDK.Core.Models;
using NFive.SDK.Core.Plugins;
using NFive.SDK.Server.Communications;
using NFive.SDK.Server.Controllers;
using NFive.SDK.Server.Events;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System;
using Z.EntityFramework.Plus;
using NFive.SDK.Core.Models.Player;

namespace Coronaverse.IdentityManager.Server
{
	[PublicAPI]
	public class IdentityManagerController : ConfigurableController<IdentityConfiguration>
	{
		private readonly List<CharacterSession> characterSessions = new List<CharacterSession>();

		private readonly ICommunicationManager Comms;

		public IdentityManagerController(ILogger logger, IdentityConfiguration configuration, ICommunicationManager comms) : base(logger, configuration)
		{
			this.Comms = comms;

			comms.Event(IdentityManagerEvents.IdentityConfiguration).FromClients().OnRequest(e => e.Reply(this.Configuration));
			
			comms.Event(IdentityManagerEvents.IdentityGetCharacters).FromClients().OnRequest(GetCharactersForUser);

			comms.Event(IdentityManagerEvents.IdentityCreateCharacter).FromClients().OnRequest<Character>(CreateCharacter);

			comms.Event(IdentityManagerEvents.IdentityDeleteCharacter).FromClients().OnRequest<Guid>(DeleteCharacter);

			comms.Event(IdentityManagerEvents.IdentityUpdateCharacter).FromClients().OnRequest<Character>(UpdateCharacter);

			comms.Event(IdentityManagerEvents.IdentityLoginCharacter).FromClients().OnRequest<Guid>(LoginCharacter);

			comms.Event(IdentityManagerEvents.IdentitySavePosition).FromClients().OnRequest<Guid, Position>(UpdatePosition);

			comms.Event(SessionEvents.ClientDisconnected).FromServer().OnRequest<IClient, Session>(OnClientDisconnect);
		}

		public override Task Started()
		{
			CleanupOldSessions();

			return base.Started();
		}

		public async void CleanupOldSessions()
		{
			using (var context = new StorageContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				var activeSessions = context.CharacterSessions.Where(s => s.Disconnected == null).ToList();
				var lastServerActiveTime = await this.Comms.Event(BootEvents.GetLastActiveTime).ToServer().Request<DateTime?>() ?? DateTime.UtcNow;
				foreach(var session in activeSessions)
				{
					session.Connected = null;
					session.Disconnected = lastServerActiveTime;
					context.CharacterSessions.AddOrUpdate(session);
				}

				context.SaveChanges();
				transaction.Commit();
			}
		}

		public void GetCharactersForUser(ICommunicationMessage e)
		{
			using(var context = new StorageContext())
			{
				var characters = context.Characters.Where(c => c.Deleted == null && c.UserId == e.User.Id).ToList();

				e.Reply(characters);
			}
		}

		public async void CreateCharacter(ICommunicationMessage e, Character character)
		{
			character.Id = GuidGenerator.GenerateTimeBasedGuid();
			character.UserId = e.User.Id;
			character.Alive = true;
			character.Health = 10000;
			character.Armor = 0;
			character.SSN = Character.GenerateSSN();
			character.LastPlayed = DateTime.UtcNow;
			character.Position = new Position(0f, 0f, 71f);
			character.Style = new Style();
			character.Appearance = new Appearance();
			character.FaceShape = new FaceShape();
			character.Heritage = new Heritage();

			using (var context = new StorageContext())
				using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					context.Characters.Add(character);
					await context.SaveChangesAsync();
					transaction.Commit();

					this.Logger.Debug($"Saved new character: {character.FullName}");

					e.Reply(new CreateCharacterEvent()
					{
						Status = CreateCharacterStatus.GOOD,
						Character = character
					});
				} catch (Exception ex)
				{
					this.Logger.Error(ex);
					transaction.Rollback();
					e.Reply(new CreateCharacterEvent()
					{
						Status = CreateCharacterStatus.ERROR,
						Character = null
					});
				}
			}
		}

		public async void DeleteCharacter(ICommunicationMessage e, Guid id)
		{
			using (var context = new StorageContext())
			{
				var character = context.Characters.First(c => c.UserId == e.User.Id && c.CharacterId == id);

				character.Deleted = DateTime.UtcNow;

				await context.SaveChangesAsync();

				GetCharactersForUser(e);
			}
		}

		public async Task LogoutAll(Guid id)
		{
			using (var context = new StorageContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				var activeSessions = context.CharacterSessions.Where(s => s.CharacterId == id && s.Disconnected == null).ToList();

				foreach(var session in activeSessions)
				{
					session.Connected = null;
					session.Disconnected = DateTime.UtcNow;
					context.CharacterSessions.AddOrUpdate(session);
				}

				await context.SaveChangesAsync();
				transaction.Commit();

				foreach (var session in activeSessions)
				{
					this.characterSessions.RemoveAll(c => c.Id == session.Id);
				}
			}
		}


		public async void LoginCharacter(ICommunicationMessage e, Guid id)
		{
			await LogoutAll(e.User.Id);

			using (var context = new StorageContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				var character = context.Characters.Include(c => c.User).FirstOrDefault(c => c.Id == id);

				if (character == null)
				{
					e.Reply(null);
					throw new Exception($"No character found for Id: {id}");
				}

				var newSession = new CharacterSession
				{
					Id = GuidGenerator.GenerateTimeBasedGuid(),
					CharacterId = character.Id,
					Character = character,
					Created = DateTime.UtcNow,
					Connected = DateTime.UtcNow,
					SessionId = e.Session.Id
				};

				context.CharacterSessions.Add(newSession);

				await context.SaveChangesAsync();
				transaction.Commit();

				newSession.Session = e.Session;

				e.Reply(newSession);

				this.characterSessions.Add(newSession);

				
			}
		}

		public async void UpdateCharacter(ICommunicationMessage e, Character characterUpdate)
		{
			using (var context = new StorageContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					// Grab character from database to ensure we aren't trying to update someone else's character
					var character = context.Characters.FirstOrDefault(c => c.Id == characterUpdate.Id && c.UserId == e.User.Id);
					if (character == null || character == default(Character))
					{
						e.Reply(false);
						return;
					}

					context.Characters.AddOrUpdate(characterUpdate);
					await context.SaveChangesAsync();
					transaction.Commit();

				} catch (Exception ex)
				{
					this.Logger.Error(ex);
					transaction.Rollback();
				}

				var activeSession = this.characterSessions.FirstOrDefault(s => s.Character.Id == characterUpdate.Id);
				if (activeSession == null || activeSession == default(CharacterSession)) return;
				activeSession.Character = characterUpdate;
			}
		}

		public async void UpdatePosition(ICommunicationMessage e, Guid characterGuid, Position position)
		{
			using (var context = new StorageContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					var character = context.Characters.FirstOrDefault(c => c.Id == characterGuid && c.UserId == e.User.Id);
					if (character == null || character == default(Character))
					{
						e.Reply(false);
						return;
					}
					character.Position = position;

					await context.SaveChangesAsync();
					transaction.Commit();
				}
				catch(Exception ex)
				{
					this.Logger.Error(ex);

					transaction.Rollback();
				}

				var activeSession = this.characterSessions.FirstOrDefault(s => s.Character.Id == characterGuid);
				if (activeSession == null || activeSession == default(CharacterSession)) return;
				activeSession.Character.Position = position;
			}
		}

		public async void OnClientDisconnect(ICommunicationMessage e, IClient client, Session session)
		{
			await LogoutAll(session.UserId);
		}
	}
}
