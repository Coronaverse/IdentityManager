using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronaverse.IdentityManager.Shared.Models.Apparel
{
	public class Component
	{
		public ComponentType Type { get; set; } = ComponentType.Face;
		public int Index { get; set; } = 0;
		public int Texture { get; set; } = 0;
	}
}
