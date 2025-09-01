using UnityEngine;

namespace BiggerShip.Enums
{
    public static class ChargeStationManager
    {
        public enum ChargeStationPlacement
        {
            Right,
            Left,
            Front
        }
        // Set this up properly
        public static void SetChargeStationPlacement(GameObject hangarShip, ChargeStationPlacement placement)
        {
            var chargeStation = hangarShip.transform.Find(".../ChargeStation");
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
    }
}