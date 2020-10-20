namespace CRP.IdentityManager.Shared.Definitions
{
	public class PedComponent
	{
		#region Enum Definitions

		public enum BodyComponentID
		{
			Face,
			Beard,
			Hair,
			Shirt,
			Pants,
			Hands,
			Shoes,
			Eyes,
			Accessories,
			MissionItems,
			Decals,
			InnerShirt
		}

		public enum PropComponentID
		{
			Hat,
			Glasses,
			Ear,
			Watch,
			Bracelet
		}

		public enum HeadOverlay //used with SetPedHeadOverlay http://www.kronzky.info/fivemwiki/index.php?title=SetPedHeadOverlay
		{
			Blemishes,
			FacialHair,
			Eyebrows,
			Ageing,
			Makeup,
			Blush,
			Complexion,
			SunDamage,
			Lipstick,
			Freckles,
			ChestHair,
			BodyBlemishes,
			AddBodyBlemishes
		}

		public enum HeadOverlayColorType
		{
			Otherwise,
			Hair,
			Makeup
		}

		#endregion
	}
}
