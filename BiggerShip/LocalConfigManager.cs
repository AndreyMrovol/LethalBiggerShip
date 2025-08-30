using BepInEx.Configuration;
using BiggerShip.Enums;
using MrovLib;

namespace BiggerShip;
internal class LocalConfigManager : MrovLib.ConfigManager
{
    internal static ConfigEntry<LoggingType> Debug { get; private set; }

    internal static ConfigEntry<ChargeStationPlacement> ChargeStation { get; private set; }
    internal static ConfigEntry<MagnetLeverPlacement> MagnetLever { get; private set; }
    private LocalConfigManager(ConfigFile config)
        : base(config)
    {
        Debug = configFile.Bind("General", "Logging levels", LoggingType.Basic, "Enable debug logging");
        ChargeStation = configFile.Bind("Placements", "Charge Station", ChargeStationPlacement.Right, "Change the charge station's placement");
        MagnetLever = configFile.Bind("Placements", "Magnet Lever", MagnetLeverPlacement.Front, "Change the magnet lever's placement");
    }

    internal static new void Init(ConfigFile config)
    {
        Instance = new LocalConfigManager(config);
    }
}