using BepInEx.Configuration;
using BiggerShip.Enums;
using MrovLib;

namespace BiggerShip;

internal class ConfigManager
{
	internal static ConfigEntry<LoggingType> Debug { get; private set; }
	internal static ConfigEntry<ChargeStationPlacement> ChargeStation { get; private set; }
	internal static ConfigEntry<MagnetLeverPlacement> MagnetLever { get; private set; }

	public static ConfigManager Instance { get; private set; }
	public static ConfigFile configFile;

	private ConfigManager(ConfigFile config)
	{
		configFile = config;
		Debug = configFile.Bind("General", "Logging levels", LoggingType.Basic, "Enable debug logging");

		ChargeStation = configFile.Bind("Placement", "Charge Station", ChargeStationPlacement.Right, "Change the charge station's placement");
		MagnetLever = configFile.Bind("Placement", "Magnet Lever", MagnetLeverPlacement.Back, "Change the magnet lever's placement");
	}

	internal static void Init(ConfigFile config)
	{
		Instance = new ConfigManager(config);
	}
}
