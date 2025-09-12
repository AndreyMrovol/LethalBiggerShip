using System.Collections.Generic;
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
					MagnetLeverPlacement.FrontDoor,
					new ObjectNewPosition
					{
						Name = "MagnetLever",
						Position = new Vector3(-7.4955f, 2.114f, -9.733f),
						Rotation = new Vector3(90f, 180f, 0f),
					}
				},
				// {
				// 	MagnetLeverPlacement.FrontLadder,
				// 	new ObjectNewPosition
				// 	{
				// 		Name = "MagnetLever",
				// 		Position = new Vector3(-5.2773f, 3.014f, -10.62f),
				// 		Rotation = new Vector3(90f, 90f, 0f),
				// 		Scale = new Vector3(1.2141f, 1.4141f, 1.4141f)
				// 	}
				// },
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

		public static Dictionary<ChargeStationPlacement, ObjectNewPosition> ChargeStationPositions = new() { };

		public static void SetNewPlacement(ObjectNewPosition position)
		{
			Transform transform = Variables.HangarShip.transform.Find(position.Name);
			if (transform == null)
			{
				Plugin.logger.LogWarning($"Object {position.Name} not found on ship, skipping.");
				return;
			}

			if (position.ToRemove)
			{
				Plugin.debugLogger.LogDebug($"Removing object: {position.Name}");
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

				Plugin.debugLogger.LogDebug($"Moved {position.Name} to {position.Position}");
			}

			if (position.Rotation != Vector3.zero)
			{
				transform.localEulerAngles = position.Rotation;
				Plugin.debugLogger.LogDebug($"Rotated {position.Name} to {position.Rotation}");
			}

			if (position.Scale != Vector3.zero)
			{
				transform.localScale = position.Scale;
				Plugin.debugLogger.LogDebug($"Scaled {position.Name} to {position.Scale}");
			}
		}
	}
}
