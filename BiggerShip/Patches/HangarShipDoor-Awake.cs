using HarmonyLib;
using UnityEngine;

namespace BiggerShip.Patches
{
	[HarmonyPatch(typeof(HangarShipDoor))]
	public static class HangarShipDoorStartPatch
	{
		[HarmonyPostfix]
		[HarmonyPatch("Start")]
		public static void PatchMethod(HangarShipDoor __instance)
		{
			Plugin.debugLogger.LogDebug("Applying HangarShipDoor Start Patch");

			if (Variables.BiggerShipBundle == null)
			{
				Plugin.logger.LogError("BiggerShipBundle is null in HangarShipDoor Start Patch!");
				return;
			}

			RuntimeAnimatorController shipDoorsAnimator = Variables.BiggerShipBundle.LoadAsset<RuntimeAnimatorController>(
				"assets/lethalcompany/biggership/assets/animatedshipdoor.controller"
			);

			if (shipDoorsAnimator == null)
			{
				Plugin.logger.LogError("Failed to load AnimatedShipDoor Animator Controller from AssetBundle!");
				return;
			}

			__instance.gameObject.GetComponent<Animator>().runtimeAnimatorController = shipDoorsAnimator;

			// retrigger rack update
			if (Plugin.TooManySuitsCompat.IsModPresent)
			{
				Plugin.debugLogger.LogDebug("Reloading TooManySuits!");

				Plugin.TooManySuitsCompat.ReloadSuitRack();
			}
		}
	}
}
