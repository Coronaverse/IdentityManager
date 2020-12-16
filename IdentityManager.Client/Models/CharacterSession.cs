using Coronaverse.IdentityManager.Shared.Models;
using System;

namespace Coronaverse.IdentityManager.Client.Models
{
	public class CharacterSession : ICharacterSession
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Connected { get; set; }
		public DateTime? Disconnected { get; set; }
		public Guid CharacterId { get; set; }
		public Character Character { get; set; }
	}
}
