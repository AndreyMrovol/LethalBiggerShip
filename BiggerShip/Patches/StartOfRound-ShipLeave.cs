using System.Collections.Generic;
using BiggerShip.Enums;
using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;

namespace BiggerShip.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	class ShipLeavePatch
	{
		[HarmonyPatch("ShipLeave")]
		[HarmonyPostfix]
		public static void DisableShipNavmesh()
		{
			GameObject ShipExtended = Variables.HangarShip.transform.Find("ShipExtended(Clone)/ShipExtended/").gameObject;
			GameObject NavmeshParent = ShipExtended.transform.Find("ShipNavmesh").gameObject;

			NavmeshParent.SetActive(false);
			BuildNavmesh.RebuildNavmesh(ShipExtended);
		}

		[HarmonyPatch("ShipHasLeft")]
		[HarmonyPostfix]
		public static void ReenableShipNavmesh()
		{
			GameObject ShipExtended = Variables.HangarShip.transform.Find("ShipExtended(Clone)/ShipExtended/").gameObject;
			GameObject NavmeshParent = ShipExtended.transform.Find("ShipNavmesh").gameObject;

			NavmeshParent.SetActive(true);
			BuildNavmesh.RebuildNavmesh(ShipExtended);
		}
	}
}
