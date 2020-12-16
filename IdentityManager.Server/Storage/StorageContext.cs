using System.Data.Entity;
using Coronaverse.IdentityManager.Server.Models;
using NFive.SDK.Core.Models.Player;
using NFive.SDK.Server.Storage;

namespace Coronaverse.IdentityManager.Server.Storage
{
	public class StorageContext : EFContext<StorageContext>
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Character> Characters { get; set; }
		public DbSet<CharacterSession> CharacterSessions {get; set;}
	}
}
