using Coronaverse.IdentityManager.Shared.Objects;
using NFive.SDK.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronaverse.IdentityManager.Server.Tables
{
	public class StyleTable : Style
	{
		[Key] public Guid Id { get; set; }

		public StyleTable() : base()
		{
			this.Id = GuidGenerator.GenerateTimeBasedGuid();
		}

		public void UpdateStyle(Style style)
		{
			this.Face = style.Face;
			this.Head = style.Head;
			this.Hair = style.Hair;
			this.Torso = style.Torso;
			this.Legs = style.Legs;
			this.Hands = style.Hands;
			this.Shoes = style.Shoes;
			this.Special1 = style.Special1;
			this.Special2 = style.Special2;
			this.Special3 = style.Special3;

			this.Hat = style.Hat;
			this.Glasses = style.Glasses;
			this.Unknown3 = style.Unknown3;
			this.Unknown4 = style.Unknown4;
			this.Unknown5 = style.Unknown5;
			this.Watch = style.Watch;
			this.Wristband = style.Wristband;
			this.Unknown8 = style.Unknown8;
			this.Unknown9 = style.Unknown9;
		}
	}
}
