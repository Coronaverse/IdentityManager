using Coronaverse.IdentityManager.Shared.Models;
using Coronaverse.IdentityManager.Shared.Models.Apparel;
using NFive.SDK.Core.Helpers;
using NFive.SDK.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronaverse.IdentityManager.Server.Models
{
	public class Style : IdentityModel, IStyle
	{
		public Component Face { get; set; }
		public Component Head { get; set; }
		public Component Hair { get; set; }
		public Component Torso { get; set; }
		public Component Torso2 { get; set; }
		public Component Legs { get; set; }
		public Component Hands { get; set; }
		public Component Shoes { get; set; }
		public Component Special1 { get; set; }
		public Component Special2 { get; set; }
		public Component Special3 { get; set; }
		public Component Textures { get; set; }
		public Prop Hat { get; set; }
		public Prop Glasses { get; set; }
		public Prop EarPiece { get; set; }
		public Prop Unknown3 { get; set; }
		public Prop Unknown4 { get; set; }
		public Prop Unknown5 { get; set; }
		public Prop Watch { get; set; }
		public Prop Wristband { get; set; }
		public Prop Unknown8 { get; set; }
		public Prop Unknown9 { get; set; }
	}
}
