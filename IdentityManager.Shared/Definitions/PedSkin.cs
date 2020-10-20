using System.Collections.Generic;

namespace CRP.IdentityManager.Shared.Definitions
{
	public struct PedSkin
	{
		public enum Model
		{
			MaleDefault,
			FemaleDefault
		}
	}

	public static class PedSkinModel
	{
		public static Dictionary<int, string> ModelString = new Dictionary<int, string>
		{
			{ (int)PedSkin.Model.MaleDefault, "mp_m_freemode_01" },
			{ (int)PedSkin.Model.FemaleDefault, "mp_f_freemode_01" }
		};
	}
}
