using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BiggerShip.Compatibility;
using HarmonyLib;
using MrovLib;

namespace BiggerShip
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	[BepInIncompatibility("mborsh.WiderShipMod")]
	[BepInIncompatibility("MelanieMelicious.2StoryShip")]
	[BepInDependency("MrovLib", BepInDependency.DependencyFlags.HardDependency)]
	public class Plugin : BaseUnityPlugin
	{
		internal static ManualLogSource logger;
		internal static Logger debugLogger = new("Debug", MrovLib.LoggingType.Developer);

		internal static Harmony harmony = new(PluginInfo.PLUGIN_GUID);

		// internal static TooManySuitsCompat TooManySuitsCompat = new("TooManySuits");

		private void Awake()
		{
			logger = Logger;
			harmony.PatchAll();

			LocalConfigManager.Init(Config);

			debugLogger.LogFatal("Debuglogger test");

			MrovLib.EventManager.SceneLoaded.AddListener(scene =>
			{
				if (scene == "SampleSceneRelay")
				{
					ReplaceVanillaShip.Init();
				}
			});

			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}
	}

	internal class LocalConfigManager : MrovLib.ConfigManager
	{
		public static ConfigEntry<LoggingType> Debug { get; private set; }

		private LocalConfigManager(ConfigFile config)
			: base(config)
		{
			Debug = configFile.Bind("General", "Logging levels", LoggingType.Basic, "Enable debug logging");
		}

		public static new void Init(ConfigFile config)
		{
			Instance = new LocalConfigManager(config);
		}
	}
}
