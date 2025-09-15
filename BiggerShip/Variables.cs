using UnityEngine;

namespace BiggerShip
{
	public static class Variables
	{
		public static AssetBundle BiggerShipBundle { get; internal set; }

		public static GameObject HangarShip { get; set; }
		public static Transform RightmostSuitPosition { get; internal set; }
		public static ShipLights ShipLights { get; internal set; }
	}
}
