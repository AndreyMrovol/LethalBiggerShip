using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

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
}
