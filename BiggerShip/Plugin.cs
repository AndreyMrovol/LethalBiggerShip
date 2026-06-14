using BepInEx;
using BepInEx.Logging;
using BiggerShip.Compatibility;
using HarmonyLib;
using UnityEngine;

namespace BiggerShip
{
	[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
	[BepInIncompatibility("mborsh.WiderShipMod")]
	[BepInIncompatibility("MelanieMelicious.2StoryShip")]
	[BepInDependency("MrovLib", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("TestAccount666.ShipWindows", BepInDependency.DependencyFlags.SoftDependency)]
	[BepInDependency("ScienceBird.ScienceBirdTweaks", BepInDependency.DependencyFlags.SoftDependency)]
	[BepInDependency("ScienceBird.UniversalRadar", BepInDependency.DependencyFlags.SoftDependency)]
	[BepInDependency("darmuh.ShipColors", BepInDependency.DependencyFlags.SoftDependency)]
	[BepInDependency("TooManySuits", BepInDependency.DependencyFlags.SoftDependency)]
	public class Plugin : BaseUnityPlugin
	{
		internal static ManualLogSource logger;
		internal static Logger debugLogger = new("Debug", MrovLib.LoggingType.Developer);

		internal static Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

		internal static TooManySuitsCompat TooManySuitsCompat;
		internal static ScienceBirdTweaksCompat ScienceBirdTweaksCompat;
		internal static UniversalRadarCompat UniversalRadarCompat;

		private void Awake()
		{
			logger = Logger;
			harmony.PatchAll();

			ConfigManager.Init(Config);

#if DEBUG
			Plugin.logger.LogWarning("Enabling full debug logging!");
			ConfigManager.Debug.Value = MrovLib.LoggingType.Developer;
#endif

			MrovLib.EventManager.SceneLoaded.AddListener(scene =>
			{
				if (scene == "SampleSceneRelay")
				{
					ReplaceVanillaShip.Init();
				}
			});

			TooManySuitsCompat = new TooManySuitsCompat("TooManySuits");
			TooManySuitsCompat.Init();

			ScienceBirdTweaksCompat = new ScienceBirdTweaksCompat("ScienceBird.ScienceBirdTweaks");
			ScienceBirdTweaksCompat.Init();

			UniversalRadarCompat = new UniversalRadarCompat("ScienceBird.UniversalRadar");
			UniversalRadarCompat.Init();

			// Plugin startup logic
			Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
