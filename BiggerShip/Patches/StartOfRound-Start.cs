using BiggerShip.Enums;
using HarmonyLib;

namespace BiggerShip.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	public static class StartOfRoundStartPatch
	{
		[HarmonyPrefix]
		[HarmonyPatch(typeof(StartOfRound), "Start")]
		public static void ChangeRightMostPosition(StartOfRound __instance)
		{
			Plugin.debugLogger.LogDebug("Patching StartOfRound.Start");

			__instance.rightmostSuitPosition = ReplaceVanillaShip.replacementObjects[ShipPart.SuitRack].transform.Find("RightmostSuitPlacement");
			__instance.shipRoomLights = Variables.ShipLights;
		}
	}
}
