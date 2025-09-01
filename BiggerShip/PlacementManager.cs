using BiggerShip.Enums;
using UnityEngine;

namespace BiggerShip
{
	public static class PlacementManager
	{
		public static void SetNewPlacement(GameObject objectToMove, Vector3 position, Vector3 rotation)
		{
			objectToMove.transform.localPosition = position;
			objectToMove.transform.localEulerAngles = rotation;
		}

		// Set this up properly
		public static void SetChargeStationPlacement(ChargeStationPlacement placement)
		{
			var chargeStation = Variables.HangarShip.transform.Find(".ShipModels2b/ChargeStation");
			if (chargeStation == null)
			{
				Plugin.logger.LogError("ChargeStation not found on ship!");
				return;
			}

			switch (placement)
			{
				case ChargeStationPlacement.Right:
					//chargeStation.localPosition = new Vector3();
					//chargeStation.localEulerAngles = new Vector3(0, -90, 0);
					break;

				case ChargeStationPlacement.Left:
					//chargeStation.localPosition = new Vector3();
					//chargeStation.localEulerAngles = new Vector3(0, 90, 0);
					break;

				case ChargeStationPlacement.Front:
					//chargeStation.localPosition = new Vector3();
					//chargeStation.localEulerAngles = new Vector3(0, 180, 0);
					break;

				default:
					Plugin.logger.LogWarning("Unknown ChargeStationPlacement, skipping.");
					break;
			}
		}

		public static void SetMagnetLeverPlacement(MagnetLeverPlacement placement)
		{
			var chargeStation = Variables.HangarShip.transform.Find("GiantCylinderMagnet");
			if (chargeStation == null)
			{
				Plugin.logger.LogError("Magnet Lever not found on ship!");
				return;
			}

			switch (placement)
			{
				case MagnetLeverPlacement.Front:
					//chargeStation.localPosition = new Vector3();
					//chargeStation.localEulerAngles = new Vector3(0, -90, 0);
					break;

				case MagnetLeverPlacement.Back:
					//chargeStation.localPosition = new Vector3();
					//chargeStation.localEulerAngles = new Vector3(0, 90, 0);
					break;

				case MagnetLeverPlacement.Inside:
					//chargeStation.localPosition = new Vector3();
					//chargeStation.localEulerAngles = new Vector3(0, 180, 0);
					break;

				default:
					Plugin.logger.LogWarning("Unknown MagnetLeverPlacement, skipping.");
					break;
			}
		}
	}
}
