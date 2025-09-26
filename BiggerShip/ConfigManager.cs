using BepInEx.Configuration;
using BiggerShip.Enums;
using MrovLib;

namespace BiggerShip;

internal class ConfigManager
{
	internal static ConfigEntry<LoggingType> Debug { get; private set; }

	internal static ConfigEntry<ChargeStationPlacement> ChargeStation { get; private set; }
	internal static ConfigEntry<MagnetLeverPlacement> MagnetLever { get; private set; }
	internal static ConfigEntry<DoorControlPanelPlacement> DoorControlPanel { get; private set; }

	internal static ConfigEntry<bool> ShipPillars { get; private set; }
	internal static ConfigEntry<bool> SuitRack { get; private set; }

	public static ConfigManager Instance { get; private set; }
	public static ConfigFile configFile;

	private ConfigManager(ConfigFile config)
	{
		configFile = config;
		Debug = configFile.Bind("General", "Logging levels", LoggingType.Basic, "Enable debug logging");

		ChargeStation = configFile.Bind("Placement", "Charge Station", ChargeStationPlacement.Right, "Change the charge station's placement");
		MagnetLever = configFile.Bind("Placement", "Magnet Lever", MagnetLeverPlacement.Front, "Change the magnet lever's placement");
		DoorControlPanel = configFile.Bind(
			"Placement",
			"Door Control Panel",
			DoorControlPanelPlacement.Vanilla,
			"Change the door control panel's placement"
		);

		ShipPillars = configFile.Bind("Placement", "Pillars", true, "Should the ship have pillars inside?");
		SuitRack = configFile.Bind(
			"Placement",
			"Suit Rack",
			true,
			"Should the ship have a suit rack? This is only visual, the suits will still spawn!"
		);
	}

	internal static void Init(ConfigFile config)
	{
		Instance = new ConfigManager(config);
	}
}
