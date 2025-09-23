using System.Collections.Generic;
using System.Linq;
using BiggerShip.Enums;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BiggerShip.Patches
{
	[HarmonyPatch(typeof(RoundManager))]
	public static class SpawnOutsideHazardsPatch
	{
		[HarmonyPostfix]
		[HarmonyPatch(typeof(RoundManager), "SpawnOutsideHazards")]
		public static void RebakeNavmeshOnLanding(RoundManager __instance)
		{
			Plugin.debugLogger.LogDebug("Patching RoundManager.SpawnOutsideHazards");

			BuildNavmesh.ReplaceNavmeshes();
		}
	}
}
