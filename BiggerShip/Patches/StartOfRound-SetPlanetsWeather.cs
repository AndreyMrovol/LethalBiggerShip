using System.Collections.Generic;
using HarmonyLib;
using Newtonsoft.Json;

namespace BiggerShip.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	class SetPlanetsWeatherPatch
	{
		[HarmonyPatch("SetPlanetsWeather")]
		[HarmonyPostfix]
		public static void RestoreOffLinks()
		{
			BuildNavmesh.RestoreOffMeshLinks();
		}
	}
}
