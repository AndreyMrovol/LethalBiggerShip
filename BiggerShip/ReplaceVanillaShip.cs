using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BiggerShip.Enums;
using UnityEngine;

namespace BiggerShip
{
	public static class ReplaceVanillaShip
	{
		internal static GameObject HangarShip;

		internal static Dictionary<ShipPart, GameObject> vanillaObjects =
			new()
			{
				{ ShipPart.CatwalkRailLiningB, null },
				{ ShipPart.CatwalkShip, null },
				{ ShipPart.ShipRailPosts, null },
				{ ShipPart.ShipRails, null },
				{ ShipPart.ShipInside, null }
			};

		internal static Dictionary<ShipPart, GameObject> replacementObjects = [];

		private static AssetBundle biggerShipBundle;

		public static void Init()
		{
			HangarShip = GameObject.Find("HangarShip");

			vanillaObjects[ShipPart.CatwalkRailLiningB] = HangarShip.transform.Find("CatwalkRailLiningB").gameObject;
			vanillaObjects[ShipPart.CatwalkRailLining] = HangarShip.transform.Find("CatwalkRailLining").gameObject;
			vanillaObjects[ShipPart.CatwalkShip] = HangarShip.transform.Find("CatwalkShip").gameObject;
			vanillaObjects[ShipPart.ShipInside] = HangarShip.transform.Find("ShipInside").gameObject;
			vanillaObjects[ShipPart.ShipRailPosts] = HangarShip.transform.Find("ShipRailPosts").gameObject;
			vanillaObjects[ShipPart.ShipRails] = HangarShip.transform.Find("ShipRails").gameObject;
			// vanillaObjects[ShipPart.SuitRack] = HangarShip.transform.Find("SuitRack").gameObject;
			vanillaObjects[ShipPart.Railing] = HangarShip.transform.Find("Railing").gameObject;
			vanillaObjects[ShipPart.Plane_001] = HangarShip.transform.Find("Plane.001").gameObject;
			vanillaObjects[ShipPart.ShipElectricLights] = HangarShip.transform.Find("ShipElectricLights").gameObject;

			string pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string assetBundlePath = Path.Combine(pluginPath, "biggership.assetbundle");
			biggerShipBundle = AssetBundle.LoadFromFile(assetBundlePath);

			if (biggerShipBundle == null)
			{
				Plugin.logger.LogError("Failed to load AssetBundle!");
				return;
			}

			List<string> assetNames = biggerShipBundle.GetAllAssetNames().ToList();
			foreach (string assetName in assetNames)
			{
				Plugin.logger.LogWarning($"assetname: {assetName}");
			}

			ReplaceVanillaObjects();
			AlignShipPartsToBiggerShip();
		}

		public static void ReplaceVanillaObjects()
		{
			string assetPath = "assets/lethalcompany/biggership/";

			foreach (ShipPart shipPart in System.Enum.GetValues(typeof(ShipPart)))
			{
				GameObject vanillaObject = vanillaObjects.TryGetValue(shipPart, out GameObject obj) ? obj : null;
				string assetName = assetPath + shipPart.ToString().ToLowerInvariant() + "extended.prefab";

				GameObject newPart = biggerShipBundle.LoadAsset<GameObject>(assetName);
				if (newPart != null)
				{
					GameObject instantiatedPart = GameObject.Instantiate(newPart, HangarShip.transform);

					instantiatedPart.AddComponent<MeshCollider>();
					instantiatedPart.GetComponent<MeshCollider>().convex = false;

					replacementObjects[shipPart] = instantiatedPart;

					if (vanillaObject == null)
					{
						Plugin.logger.LogError($"Vanilla object for {shipPart} is null, skipping replacement.");
						continue;
					}

					vanillaObject.SetActive(false);
					instantiatedPart.name = vanillaObject.name; // Maintain original name
					GameObject.Destroy(vanillaObject); // Remove the old part
				}
				else
				{
					Plugin.logger.LogError($"Failed to load asset for {assetName}");
					continue;
				}
			}

			Variables.RightmostSuitPosition = replacementObjects[ShipPart.SuitRack].transform.Find("RightmostSuitPlacement");
			Variables.ShipLights = replacementObjects[ShipPart.ShipElectricLights]
				.transform.Find("ShipElectricLights")
				.GetComponentInChildren<ShipLights>();

			Plugin.logger.LogInfo("Replaced vanilla ship parts with bigger versions.");
		}

		public static void AlignShipPartsToBiggerShip()
		{
			#region Remove parts
			List<GameObject> PartsToRemove =
			[
				HangarShip.transform.Find("RightmostSuitPlacement").gameObject,
				HangarShip.transform.Find("WallInsulator").gameObject,
				HangarShip.transform.Find("WallInsulator2").gameObject,
				HangarShip.transform.Find("StickyNoteItem").gameObject,
				// HangarShip.transform.Find("ClipboardManual").gameObject,
				HangarShip.transform.Find("Pipework2.002").gameObject,
				HangarShip.transform.Find("NurbsPath.002").gameObject,
				HangarShip.transform.Find("NurbsPath.004").gameObject,
				HangarShip.transform.Find("Cube.005").gameObject,
				HangarShip.transform.Find("Cube.006").gameObject,
				HangarShip.transform.Find("Cube.007").gameObject,
				HangarShip.transform.Find("Cube.008").gameObject,
				HangarShip.transform.Find("SideMachineryLeft").gameObject,
				HangarShip.transform.Find("SideMachineryRight").gameObject,
				HangarShip.transform.Find("LightSwitchContainer").gameObject,
				HangarShip.transform.Find("MeterBoxDevice.001").gameObject,
			];

			foreach (GameObject part in PartsToRemove)
			{
				if (part != null)
				{
					Plugin.debugLogger.LogDebug($"Removing part: {part.name}");
					GameObject.Destroy(part);
				}
				else
				{
					Plugin.logger.LogWarning("Part to remove not found, skipping.");
				}
			}

			#endregion



			#region Move parts

			// Align replacement parts to the bigger ship
			Dictionary<string, Vector3> objectsToMove =
				new()
				{
					{ "GiantCylinderMagnet", new(2.5f, 2.74f, -11.1f) },
					{ "ScavengerModelSuitParts/Circle.004", new(-7.26f, -3.37f, -3.98f) },
					{ "AnimatedShipDoor/HangarDoorButtonPanel", new(-5.548f, 2.188f, -10.446f) },
					{ "ShipModels2b/MonitorWall/SingleScreen", new(-11.92f, -1.267f, -5.525f) },
					{ "OutsideShipRoom/Ladder", new(-0.1785f, -0.1324f, -2.65f) },
					{ "StorageCloset", new(-3.5308f, -0.0982f, -2.9853f) },
					{ "ShipElectricLights/LightSwitchContainer", new(0.9935f, 1.4172f, -3.036f) },
					{ "ShipModels2b/ChargeStation", new(0.957f, 1.2561f, -4.802f) },
					{ "VentEntrance", new(1.499f, 0.693f, -10.473f) },
					{ "ShipModels2b/Cube.005 (2)", new(-6.29f, 2.64f, -5.18f) },
					{ "ShipModels2b/Cube.005 (1)", new(-6.192f, 2.637f, -5.18f) },
					{ "Terminal", new(6.3114f, 0.9263f, -9.2735f) },
					{ "LadderShort (1)", new(-7, -2.30100012f, -12.0609999f) },
					{ "ShipModels2b/Light", new(-6.1517f, 3.23f, -4.5148f) },
					{ "ShipModels2b/Light (1)", new(1.0879f, 3.0727f, -11.4066f) },
					{ "ShipModels2b/Light (2)", new(-6.1443f, 3.25f, -11.063f) },
					{ "ShipModels2b/Light (3)", new(1.0957f, 3.13f, -4.399f) },
				};

			foreach (KeyValuePair<string, Vector3> entry in objectsToMove)
			{
				GameObject part = HangarShip.transform.Find(entry.Key)?.gameObject;
				if (part != null)
				{
					if (part.GetComponent<AutoParentToShip>() != null)
					{
						part.GetComponent<AutoParentToShip>().positionOffset = entry.Value;
					}

					part.transform.localPosition = entry.Value;
				}
				else
				{
					Plugin.debugLogger.LogWarning($"Part not found for alignment: {entry.Key}");
				}
			}

			#endregion

			#region Rotate parts

			Dictionary<string, Vector3> objectsToRotate =
				new()
				{
					//
					{ "ShipModels2b/Light", new(270, 180, 0) },
					{ "ShipModels2b/Light (3)", new(270, 0, 0) },
				};

			foreach (KeyValuePair<string, Vector3> entry in objectsToRotate)
			{
				GameObject part = HangarShip.transform.Find(entry.Key)?.gameObject;
				if (part != null)
				{
					part.transform.localEulerAngles = entry.Value;
				}
				else
				{
					Plugin.debugLogger.LogWarning($"Part not found for alignment: {entry.Key}");
				}
			}

			#endregion


			return;
		}
	}
}
