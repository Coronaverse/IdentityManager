using CitizenFX.Core;
using Coronaverse.IdentityManager.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronaverse.IdentityManager.Client.Extensions
{
	public static class StyleExtensions
	{
		public static void UpdateStyle(this Shared.Objects.Style style)
		{
			style.Face = PedComponentToComponent(ComponentType.Face);
			style.Head = PedComponentToComponent(ComponentType.Head);
			style.Hair = PedComponentToComponent(ComponentType.Hair);
			style.Torso = PedComponentToComponent(ComponentType.Torso);
			style.Torso2 = PedComponentToComponent(ComponentType.Torso2);
			style.Legs = PedComponentToComponent(ComponentType.Legs);
			style.Hands = PedComponentToComponent(ComponentType.Hands);
			style.Shoes = PedComponentToComponent(ComponentType.Shoes);
			style.Special1 = PedComponentToComponent(ComponentType.Special1);
			style.Special2 = PedComponentToComponent(ComponentType.Special2);
			style.Special3 = PedComponentToComponent(ComponentType.Special3);
			style.Textures = PedComponentToComponent(ComponentType.Textures);

			style.Hat = PedPropToProp(PropType.Hats);
			style.Glasses = PedPropToProp(PropType.Glasses);
			style.EarPiece = PedPropToProp(PropType.EarPieces);
			style.Unknown3 = PedPropToProp(PropType.Unknown3);
			style.Unknown4 = PedPropToProp(PropType.Unknown4);
			style.Unknown5 = PedPropToProp(PropType.Unknown5);
			style.Watch = PedPropToProp(PropType.Watches);
			style.Wristband = PedPropToProp(PropType.Wristbands);
			style.Unknown8 = PedPropToProp(PropType.Unknown8);
			style.Unknown9 = PedPropToProp(PropType.Unknown9);
		}

		public static void RenderStyle(this Shared.Objects.Style style)
		{
			Game.PlayerPed.Style[PedComponents.Face].SetVariation(style.Face.Index, style.Face.Texture);
			Game.PlayerPed.Style[PedComponents.Head].SetVariation(style.Head.Index, style.Head.Texture);
			Game.PlayerPed.Style[PedComponents.Hair].SetVariation(style.Hair.Index, style.Hair.Texture);
			Game.PlayerPed.Style[PedComponents.Torso].SetVariation(style.Torso.Index, style.Torso.Texture);
			Game.PlayerPed.Style[PedComponents.Torso2].SetVariation(style.Torso2.Index, style.Torso2.Texture);
			Game.PlayerPed.Style[PedComponents.Legs].SetVariation(style.Legs.Index, style.Legs.Texture);
			Game.PlayerPed.Style[PedComponents.Hands].SetVariation(style.Hands.Index, style.Hands.Texture);
			Game.PlayerPed.Style[PedComponents.Shoes].SetVariation(style.Shoes.Index, style.Shoes.Texture);
			Game.PlayerPed.Style[PedComponents.Special1].SetVariation(style.Special1.Index, style.Special1.Texture);
			Game.PlayerPed.Style[PedComponents.Special2].SetVariation(style.Special2.Index, style.Special2.Texture);
			Game.PlayerPed.Style[PedComponents.Special3].SetVariation(style.Special3.Index, style.Special3.Texture);
			Game.PlayerPed.Style[PedComponents.Textures].SetVariation(style.Textures.Index, style.Textures.Texture);

			Game.PlayerPed.Style[PedProps.Hats].SetVariation(style.Hat.Index, style.Hat.Texture);
			Game.PlayerPed.Style[PedProps.Glasses].SetVariation(style.Glasses.Index, style.Glasses.Texture);
			Game.PlayerPed.Style[PedProps.EarPieces].SetVariation(style.EarPiece.Index, style.EarPiece.Texture);
			Game.PlayerPed.Style[PedProps.Unknown3].SetVariation(style.Unknown3.Index, style.Unknown3.Texture);
			Game.PlayerPed.Style[PedProps.Unknown4].SetVariation(style.Unknown4.Index, style.Unknown4.Texture);
			Game.PlayerPed.Style[PedProps.Unknown5].SetVariation(style.Unknown5.Index, style.Unknown5.Texture);
			Game.PlayerPed.Style[PedProps.Watches].SetVariation(style.Watch.Index, style.Watch.Texture);
			Game.PlayerPed.Style[PedProps.Wristbands].SetVariation(style.Wristband.Index, style.Wristband.Texture);
			Game.PlayerPed.Style[PedProps.Unknown8].SetVariation(style.Unknown8.Index, style.Unknown8.Texture);
			Game.PlayerPed.Style[PedProps.Unknown9].SetVariation(style.Unknown9.Index, style.Unknown9.Texture);
		}

		public static Component PedComponentToComponent(ComponentType type)
		{
			return new Component()
			{
				Type = type,
				Index = Game.PlayerPed.Style[(PedComponents)((int)type)].Index,
				Texture = Game.PlayerPed.Style[(PedComponents)((int)type)].TextureIndex
			};
		}

		public static Shared.Objects.Prop PedPropToProp(PropType type)
		{
			return new Shared.Objects.Prop()
			{
				Type = type,
				Index = Game.PlayerPed.Style[(PedProps)((int)type)].Index,
				Texture = Game.PlayerPed.Style[(PedProps)((int)type)].TextureIndex
			};
		}
	}
}
