using System.Data.Entity;
using NFive.SDK.Core.Models.Player;
using NFive.SDK.Server.Storage;
using CRP.IdentityManager.Server.Tables;

namespace CRP.IdentityManager.Server.Storage
{
	public class StorageContext : EFContext<StorageContext>
	{
		public DbSet<User> Users { get; set; }
		public DbSet<CharacterTable> Characters { get; set; }
	}
}
