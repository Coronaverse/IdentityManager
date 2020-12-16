using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronaverse.IdentityManager.Shared.Models
{
	public interface ICharacterSession
	{
		/// <summary>
		/// Gets or sets the session identifier
		/// </summary>
		Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of when the session was created
		/// </summary>
		DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of when the character connected to the session
		/// </summary>
		DateTime? Connected { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of when the character disconnected from the session
		/// </summary>
		DateTime? Disconnected { get; set; }

		/// <summary>
		/// Gets or sets the Character identifier
		/// </summary>
		Guid CharacterId { get; set; }
	}
}
