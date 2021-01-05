using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronaverse.IdentityManager.Shared.Events
{
	public enum CreateCharacterStatus
	{
		GOOD = 0,
		ERROR = 1
	}
	public interface ICreateCharacterEvent
	{
		CreateCharacterStatus Status { get; set; }
	}
}
