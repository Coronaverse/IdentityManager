using CitizenFX.Core;
using CitizenFX.Core.Native;
using Coronaverse.IdentityManager.Shared;
using Coronaverse.IdentityManager.Shared.Models.Appearance;
using Newtonsoft.Json;
using NFive.SDK.Client.Extensions;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Extensions;
using NFive.SDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronaverse.IdentityManager.Client.Models
{
	public class Character : IdentityModel, ICharacter
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public short Gender { get; set; }
		public bool Alive { get; set; }
		public int Health { get; set; }
		public int Armor { get; set; }
		public int SSN { get; set; }
		public Position Position { get; set; }
		public string Model { get; set; }
		public string WalkingStyle { get; set; }
		public Guid StyleId { get; set; }
		public Style Style { get; set; }
		public Guid AppearanceId { get; set; }
		public Appearance Appearance { get; set; }
		public Guid FaceShapeId { get; set; }
		public FaceShape FaceShape { get; set; }
		public Guid HeritageId { get; set; }
		public Heritage Heritage { get; set; }
		public DateTime? LastPlayed { get; set; }
		public Guid UserId { get; set; }

		[JsonIgnore] public string FullName => $"{this.FirstName} {this.MiddleName} {this.LastName}".Replace("  ", " ");

		[JsonIgnore] public PedHash ModelHash => (PedHash)Convert.ToUInt32(this.Model);

		public void RenderCustom(ILogger logger)
		{
			if (this.ModelHash != PedHash.FreemodeFemale01 && this.ModelHash != PedHash.FreemodeMale01) return;

			var player = Game.Player.Character.Handle;

			API.SetPedHeadBlendData(player, this.Heritage.Parent1, this.Heritage.Parent2, 0, this.Heritage.Parent1, this.Heritage.Parent2, 0, this.Heritage.Resemblance, this.Heritage.SkinTone, 0f, true);

			API.SetPedHairColor(player, this.Appearance.HairColorId, this.Appearance.HairHighlightColor);
			API.SetPedHeadOverlay(player, (int)FeatureTypes.Age, this.Appearance.Aging.Index, this.Appearance.Aging.Opacity);
			API.SetPedHeadOverlay(player, (int)FeatureTypes.Beard, this.Appearance.Beard.Index, this.Appearance.Beard.Opacity);
			API.SetPedEyeColor(player, this.Appearance.EyeColorId);
			API.SetPedHeadOverlay(player, (int)FeatureTypes.Eyebrows, this.Appearance.Eyebrows.Index, this.Appearance.Eyebrows.Opacity);
			API.SetPedHeadOverlay(player, (int)FeatureTypes.Makeup, this.Appearance.Makeup.Index, this.Appearance.Makeup.Opacity);
			API.SetPedHeadOverlay(player, (int)FeatureTypes.Lipstick, this.Appearance.Lipstick.Index, this.Appearance.Lipstick.Opacity);
			API.SetPedHeadOverlayColor(player, (int)FeatureTypes.Beard, (int)this.Appearance.Beard.ColorType, this.Appearance.Beard.ColorId, this.Appearance.Beard.SecondColorId);
			API.SetPedHeadOverlayColor(player, (int)FeatureTypes.Eyebrows, (int)this.Appearance.Eyebrows.ColorType, this.Appearance.Eyebrows.ColorId, this.Appearance.Eyebrows.SecondColorId);
			API.SetPedHeadOverlayColor(player, (int)FeatureTypes.Makeup, (int)this.Appearance.Makeup.ColorType, this.Appearance.Makeup.ColorId, this.Appearance.Makeup.SecondColorId);
			API.SetPedHeadOverlayColor(player, (int)FeatureTypes.Lipstick, (int)this.Appearance.Lipstick.ColorType, this.Appearance.Lipstick.ColorId, this.Appearance.Lipstick.SecondColorId);
			API.SetPedHeadOverlay(player, (int)FeatureTypes.Blush, this.Appearance.Blush.Index, this.Appearance.Blush.Opacity);
			API.SetPedHeadOverlayColor(player, (int)FeatureTypes.Blush, (int)this.Appearance.Blush.ColorType, this.Appearance.Blush.ColorId, this.Appearance.Blush.SecondColorId);
			API.SetPedHeadOverlay(player, (int)FeatureTypes.Complexion, this.Appearance.Complexion.Index, this.Appearance.Complexion.Opacity);
			API.SetPedHeadOverlay(player, (int)FeatureTypes.SunDamage, this.Appearance.SunDamage.Index, this.Appearance.SunDamage.Opacity);
			API.SetPedHeadOverlay(player, (int)FeatureTypes.MolesAndFreckles, this.Appearance.MolesAndFreckles.Index, this.Appearance.MolesAndFreckles.Opacity);
			API.SetPedHeadOverlay(player, (int)FeatureTypes.Chest, this.Appearance.Chest.Index, this.Appearance.Chest.Opacity);
			API.SetPedHeadOverlayColor(player, (int)FeatureTypes.Chest, (int)this.Appearance.Chest.ColorType, this.Appearance.Chest.ColorId, this.Appearance.Chest.SecondColorId);
			API.SetPedHeadOverlay(player, (int)FeatureTypes.Blemishes, this.Appearance.Blemishes.Index, this.Appearance.Blemishes.Opacity);

			API.SetPedFaceFeature(player, 0, this.FaceShape.NoseWidth);
			API.SetPedFaceFeature(player, 1, this.FaceShape.NosePeakHeight);
			API.SetPedFaceFeature(player, 2, this.FaceShape.NosePeakLength);
			API.SetPedFaceFeature(player, 3, this.FaceShape.NoseBoneHeight);
			API.SetPedFaceFeature(player, 4, this.FaceShape.NosePeakLowering);
			API.SetPedFaceFeature(player, 5, this.FaceShape.NoseBoneTwist);
			API.SetPedFaceFeature(player, 6, this.FaceShape.EyeBrowHeight);
			API.SetPedFaceFeature(player, 7, this.FaceShape.EyeBrowLength);
			API.SetPedFaceFeature(player, 8, this.FaceShape.CheekBoneHeight);
			API.SetPedFaceFeature(player, 9, this.FaceShape.CheekBoneWidth);
			API.SetPedFaceFeature(player, 10, this.FaceShape.CheekWidth);
			API.SetPedFaceFeature(player, 11, this.FaceShape.EyeOpenings);
			API.SetPedFaceFeature(player, 12, this.FaceShape.LipThickness);
			API.SetPedFaceFeature(player, 13, this.FaceShape.JawBoneWidth);
			API.SetPedFaceFeature(player, 14, this.FaceShape.JawBoneLength);
			API.SetPedFaceFeature(player, 15, this.FaceShape.ChinBoneLowering);
			API.SetPedFaceFeature(player, 16, this.FaceShape.ChinBoneLength);
			API.SetPedFaceFeature(player, 17, this.FaceShape.ChinBoneWidth);
			API.SetPedFaceFeature(player, 18, this.FaceShape.ChinDimple);
			API.SetPedFaceFeature(player, 19, this.FaceShape.NeckThickness);
		}

		public async Task Render(ILogger logger)
		{

			Model model = this.Gender == 0 ? new Model(PedHash.FreemodeMale01) : new Model(PedHash.FreemodeFemale01);
			while (!await Game.Player.ChangeModel(model)) await BaseScript.Delay(10);

			// Apparently this _must_ be called
			Game.Player.Character.Style.SetDefaultClothes();

			Game.Player.Character.Position = this.Position.ToVector3().ToCitVector3();
			Game.Player.Character.Armor = this.Armor;

			API.RequestClipSet(this.WalkingStyle);
			await BaseScript.Delay(100); // Required to load
			Game.Player.Character.MovementAnimationSet = this.WalkingStyle;

			Game.Player.Character.Style[PedComponents.Face].SetVariation(this.Style.Face.Index, this.Style.Face.Texture);
			Game.Player.Character.Style[PedComponents.Head].SetVariation(this.Style.Head.Index, this.Style.Head.Texture);

			// Temporary network visibility fix workaround
			Game.Player.Character.Style[PedComponents.Hair].SetVariation(1, 1);

			Game.Player.Character.Style[PedComponents.Hair].SetVariation(this.Style.Hair.Index, this.Style.Hair.Texture);

			Game.Player.Character.Style[PedComponents.Torso].SetVariation(this.Style.Torso.Index, this.Style.Torso.Texture);
			Game.Player.Character.Style[PedComponents.Legs].SetVariation(this.Style.Legs.Index, this.Style.Legs.Texture);
			Game.Player.Character.Style[PedComponents.Hands].SetVariation(this.Style.Hands.Index, this.Style.Hands.Texture);
			Game.Player.Character.Style[PedComponents.Shoes].SetVariation(this.Style.Shoes.Index, this.Style.Shoes.Texture);
			Game.Player.Character.Style[PedComponents.Special1].SetVariation(this.Style.Special1.Index, this.Style.Special1.Texture);
			Game.Player.Character.Style[PedComponents.Special2].SetVariation(this.Style.Special2.Index, this.Style.Special2.Texture);
			Game.Player.Character.Style[PedComponents.Special3].SetVariation(this.Style.Special3.Index, this.Style.Special3.Texture);
			Game.Player.Character.Style[PedComponents.Textures].SetVariation(this.Style.Textures.Index, this.Style.Textures.Texture);
			Game.Player.Character.Style[PedComponents.Torso2].SetVariation(this.Style.Torso2.Index, this.Style.Torso2.Texture);

			Game.Player.Character.Style[PedProps.Hats].SetVariation(this.Style.Hat.Index, this.Style.Hat.Texture);
			Game.Player.Character.Style[PedProps.Glasses].SetVariation(this.Style.Glasses.Index, this.Style.Glasses.Texture);
			Game.Player.Character.Style[PedProps.EarPieces].SetVariation(this.Style.EarPiece.Index, this.Style.EarPiece.Texture);
			Game.Player.Character.Style[PedProps.Unknown3].SetVariation(this.Style.Unknown3.Index, this.Style.Unknown3.Texture);
			Game.Player.Character.Style[PedProps.Unknown4].SetVariation(this.Style.Unknown4.Index, this.Style.Unknown4.Texture);
			Game.Player.Character.Style[PedProps.Unknown5].SetVariation(this.Style.Unknown5.Index, this.Style.Unknown5.Texture);
			Game.Player.Character.Style[PedProps.Watches].SetVariation(this.Style.Watch.Index, this.Style.Watch.Texture);
			Game.Player.Character.Style[PedProps.Wristbands].SetVariation(this.Style.Wristband.Index, this.Style.Wristband.Texture);
			Game.Player.Character.Style[PedProps.Unknown8].SetVariation(this.Style.Unknown8.Index, this.Style.Unknown8.Texture);
			Game.Player.Character.Style[PedProps.Unknown9].SetVariation(this.Style.Unknown9.Index, this.Style.Unknown9.Texture);
		}
	}
}
