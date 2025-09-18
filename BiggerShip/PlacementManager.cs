using System.Collections.Generic;
using System.Text;
using BiggerShip.Definitions;
using BiggerShip.Enums;
using UnityEngine;
using UnityEngine.UIElements;

namespace BiggerShip
{
	public static class PlacementManager
	{
		public static Dictionary<MagnetLeverPlacement, ObjectNewPosition> MagnetLeverPositions =
			new()
			{
				{
					MagnetLeverPlacement.Front,
					new ObjectNewPosition
					{
						Name = "MagnetLever",
						Position = new Vector3(-7.4955f, 2.114f, -9.733f),
						Rotation = new Vector3(90f, 180f, 0f),
					}
				},
				{
					MagnetLeverPlacement.Back,
					new ObjectNewPosition
					{
						Name = "MagnetLever",
						Position = new Vector3(10.499f, 1.614f, -10.033f),
						Rotation = new Vector3(90f, 57.878f, 0f),
					}
				},
				{
					MagnetLeverPlacement.Inside,
					new ObjectNewPosition
					{
						Name = "MagnetLever",
						Position = new Vector3(9.85f, 2.7868f, -5.5257f),
						Rotation = new Vector3(90f, 180f, 0f),
						Scale = new Vector3(1.2141f, 1.3141f, 1.4141f)
					}
				},
			};

		public static Dictionary<ChargeStationPlacement, ObjectNewPosition> ChargeStationPositions =
			new()
			{
				{
					ChargeStationPlacement.Left,
					new ObjectNewPosition
					{
						Name = "ShipModels2b/ChargeStation",
						Position = new Vector3(-5.9898f, 1.2561f, -4.902f),
						Rotation = new Vector3(270f, 180f, 0f),
					}
				},
			};

		public static Dictionary<DoorControlPanelPlacement, List<ObjectNewPosition>> DoorControlPanelPositions =
			new()
			{
				{
					DoorControlPanelPlacement.Vanilla,
					new List<ObjectNewPosition>
					{
						new() { Name = "ShipModels2b/MonitorWall/SingleScreen", Position = new Vector3(-11.798f, -1.186f, -5.1814f), },
						new() { Name = "AnimatedShipDoor/HangarDoorButtonPanel", Position = new Vector3(-5.4f, 2.2f, -10.1f) }
					}
				},
				{
					DoorControlPanelPlacement.Compact,
					new List<ObjectNewPosition>
					{
						new() { Name = "AnimatedShipDoor/HangarDoorButtonPanel", Position = new Vector3(-6f, 2.188f, -10.446f) },
						new() { Name = "ShipModels2b/MonitorWall/SingleScreen", Position = new Vector3(-12.269f, -1.267f, -5.525f) },
					}
				}
			};

		public static void SetNewPlacement(ObjectNewPosition position)
		{
			Transform transform = Variables.HangarShip.transform.Find(position.Name);
			if (transform == null)
			{
				Plugin.logger.LogWarning($"Object {position.Name} not found on ship, skipping.");
				return;
			}

			StringBuilder logMessage = new();
			logMessage.Append($"{position.Name}: ");

			if (position.ToRemove)
			{
				logMessage.Append("has been removed.");
				GameObject.Destroy(transform.gameObject);
				return;
			}

			GameObject ShipPart = transform.gameObject;

			if (position.Position != Vector3.zero)
			{
				transform.localPosition = position.Position;

				if (ShipPart.GetComponent<AutoParentToShip>() != null)
				{
					ShipPart.GetComponent<AutoParentToShip>().positionOffset = position.Position;
				}

				logMessage.Append("position: " + position.Position.ToString() + "; ");
			}

			if (position.Rotation != Vector3.zero)
			{
				transform.localEulerAngles = position.Rotation;
				logMessage.Append("rotation: " + position.Rotation.ToString() + "; ");
			}

			if (position.Scale != Vector3.zero)
			{
				transform.localScale = position.Scale;
				logMessage.Append("scale: " + position.Scale.ToString() + "; ");
			}

			Plugin.debugLogger.LogDebug(logMessage.ToString());
		}
	}
}
