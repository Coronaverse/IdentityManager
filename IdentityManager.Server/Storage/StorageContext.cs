using System.Data.Entity;
using NFive.SDK.Core.Models.Player;
using NFive.SDK.Server.Storage;
using Coronaverse.IdentityManager.Server.Tables;

namespace Coronaverse.IdentityManager.Server.Storage
{
	public class StorageContext : EFContext<StorageContext>
	{
		public DbSet<User> Users { get; set; }
		public DbSet<CharacterTable> Characters { get; set; }
	}
}
