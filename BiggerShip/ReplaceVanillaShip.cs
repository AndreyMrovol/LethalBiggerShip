using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BiggerShip.Definitions;
using BiggerShip.Enums;
using UnityEngine;

namespace BiggerShip
{
	public static class ReplaceVanillaShip
	{
		internal static GameObject HangarShip
		{
			get => Variables.HangarShip;
			set => Variables.HangarShip = value;
		}

		internal static Dictionary<ShipPart, GameObject> vanillaObjects = [];

		internal static Dictionary<ShipPart, GameObject> replacementObjects = [];

		public static void Init()
		{
			HangarShip = GameObject.Find("HangarShip");

			vanillaObjects[ShipPart.Plane_001] = HangarShip.transform.Find("Plane.001").gameObject;
			vanillaObjects[ShipPart.ShipElectricLights] = HangarShip.transform.Find("ShipElectricLights").gameObject;
			// vanillaObjects[ShipPart.AnimatedShipDoor] = HangarShip.transform.Find("AnimatedShipDoor").gameObject;

			if (Variables.BiggerShipBundle == null)
			{
				string pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				string assetBundlePath = Path.Combine(pluginPath, "biggership.assetbundle");
				Variables.BiggerShipBundle = AssetBundle.LoadFromFile(assetBundlePath);

				if (Variables.BiggerShipBundle == null)
				{
					Plugin.logger.LogError("Failed to load AssetBundle!!!");
					return;
				}
			}

			List<string> assetNames = Variables.BiggerShipBundle.GetAllAssetNames().ToList();
			foreach (string assetName in assetNames)
			{
				Plugin.debugLogger.LogWarning($"assetname: {assetName}");
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

				GameObject newPart = Variables.BiggerShipBundle.LoadAsset<GameObject>(assetName);
				if (newPart != null)
				{
					GameObject instantiatedPart = GameObject.Instantiate(newPart, HangarShip.transform);

					instantiatedPart.AddComponent<MeshCollider>();
					instantiatedPart.GetComponent<MeshCollider>().convex = false;

					replacementObjects[shipPart] = instantiatedPart;

					if (vanillaObject == null)
					{
						Plugin.debugLogger.LogCustom(
							$"Vanilla object for {shipPart} is null, skipping replacement.",
							BepInEx.Logging.LogLevel.Warning,
							MrovLib.LoggingType.Debug
						);
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

			if (ConfigManager.ShipRibs.Value == false)
			{
				replacementObjects[ShipPart.Ship].transform.Find("ShipExtended/ShipWalls/ShipRibs").gameObject.SetActive(false);
			}

			Plugin.logger.LogInfo("Replaced vanilla ship parts with bigger versions.");
		}

		public static void AlignShipPartsToBiggerShip()
		{
			List<ObjectNewPosition> newPositions =
			[
				new() { Name = "AnimatedShipDoor/HangarDoorButtonPanel", Position = new Vector3(-6f, 2.188f, -10.446f) },
				new() { Name = "CatwalkRailLining" },
				new() { Name = "CatwalkRailLiningB" },
				new() { Name = "CatwalkShip" },
				new() { Name = "CatwalkUnderneathSupports" },
				new() { Name = "Cube.005" },
				new() { Name = "Cube.006" },
				new() { Name = "Cube.007" },
				new() { Name = "Cube.008" },
				new() { Name = "GiantCylinderMagnet", Position = new Vector3(2.5f, 2.44f, -11.1f) },
				new() { Name = "LadderShort", Position = new Vector3(6.63f, -2.48f, -0.38f) },
				new() { Name = "LadderShort (1)", Position = new Vector3(-7f, -2.4955f, -12.2756f) },
				new() { Name = "LightSwitchContainer" },
				new() { Name = "MeterBoxDevice.001" },
				new() { Name = "NurbsPath.002" },
				new() { Name = "NurbsPath.004" },
				new()
				{
					Name = "OutsideShipRoom/Ladder",
					Position = new Vector3(-0.1785f, -0.1324f, -2.65f),
					Scale = new Vector3(0.5805f, 0.541f, 0.5805f)
				},
				new() { Name = "Pipework2.002" },
				new() { Name = "Railing" },
				new() { Name = "RightmostSuitPlacement" },
				new() { Name = "ScavengerModelSuitParts/Circle.004", Position = new Vector3(-7.26f, -3.37f, -3.98f) },
				new() { Name = "ShipElectricLights/LightSwitchContainer", Position = new Vector3(0.9935f, 1.4172f, -3.036f) },
				new()
				{
					Name = "ShipInnerRoomBoundsTrigger",
					Position = new Vector3(1.4367f, 1.781f, -6.7f),
					Scale = new Vector3(17.523226f, 5.341722f, 7.51f)
				},
				new() { Name = "ShipInside" },
				new() { Name = "ShipInside.001" },
				new() { Name = "ShipModels2b/ChargeStation", Position = new Vector3(0.957f, 1.2561f, -4.802f) },
				new() { Name = "ShipModels2b/Cube.005 (1)", Position = new Vector3(-6.192f, 2.637f, -5.18f) },
				new() { Name = "ShipModels2b/Cube.005 (2)", Position = new Vector3(-6.29f, 2.64f, -5.18f) },
				new()
				{
					Name = "ShipModels2b/Light",
					Position = new Vector3(-6.1517f, 3.23f, -4.5148f),
					Rotation = new Vector3(270, 180, 0)
				},
				new() { Name = "ShipModels2b/Light (1)", Position = new Vector3(1.0879f, 3.0727f, -11.8577f) },
				new() { Name = "ShipModels2b/Light (2)", Position = new Vector3(-6.1443f, 3.25f, -10.9433f) },
				new()
				{
					Name = "ShipModels2b/Light (3)",
					Position = new Vector3(1.0957f, 3.13f, -4.399f),
					Rotation = new Vector3(270, 0, 0)
				},
				new() { Name = "ShipModels2b/MonitorWall/SingleScreen", Position = new Vector3(-12.269f, -1.267f, -5.525f) },
				new() { Name = "ShipRailPosts" },
				new() { Name = "ShipRails" },
				new() { Name = "SideMachineryLeft" },
				new() { Name = "SideMachineryRight" },
				new() { Name = "StorageCloset", Position = new Vector3(-3.2237f, -0.1303f, -3.3999f) },
				new() { Name = "Terminal", Position = new Vector3(6.2022f, 0.9266f, -9.3018f) },
				new() { Name = "VentEntrance", Position = new Vector3(1.499f, 0.693f, -10.473f) },
				new() { Name = "WallInsulator" },
				new() { Name = "WallInsulator2" },
			];

			foreach (ObjectNewPosition newPos in newPositions)
			{
				PlacementManager.SetNewPlacement(newPos);
			}

			// get positions of magnet and charger from config
			if (ConfigManager.MagnetLever.Value != MagnetLeverPlacement.Back)
			{
				MagnetLeverPlacement placement = ConfigManager.MagnetLever.Value;

				if (PlacementManager.MagnetLeverPositions.TryGetValue(placement, out ObjectNewPosition newPos))
				{
					PlacementManager.SetNewPlacement(newPos);
				}
			}

			if (ConfigManager.ChargeStation.Value != ChargeStationPlacement.Right)
			{
				ChargeStationPlacement placement = ConfigManager.ChargeStation.Value;

				if (PlacementManager.ChargeStationPositions.TryGetValue(placement, out ObjectNewPosition newPos))
				{
					PlacementManager.SetNewPlacement(newPos);
				}
			}

			DoorControlPanelPlacement doorPlacement = ConfigManager.DoorControlPanel.Value;
			if (PlacementManager.DoorControlPanelPositions.TryGetValue(doorPlacement, out List<ObjectNewPosition> newDoorPos))
			{
				foreach (ObjectNewPosition pos in newDoorPos)
				{
					PlacementManager.SetNewPlacement(pos);
				}
			}

			return;
		}
	}
}
