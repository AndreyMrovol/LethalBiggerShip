using UnityEngine;

namespace BiggerShip.Enums
{
    public static class MagnetLeverManager
    {
        public enum MagnetLeverPlacement
        {
            Front,
            Back,
            Inside
        }

        public static void SetMagnetLeverPlacement(GameObject hangarShip, MagnetLeverPlacement placement)
        {
            var chargeStation = hangarShip.transform.Find(".../MagnetLever");
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