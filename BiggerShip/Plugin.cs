using BepInEx;
using BepInEx.Logging;
using BiggerShip.Compatibility;
using HarmonyLib;
using UnityEngine;

namespace BiggerShip
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
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

		internal static Harmony harmony = new(PluginInfo.PLUGIN_GUID);

		internal static TooManySuitsCompat TooManySuitsCompat = new("TooManySuits");
		internal static ScienceBirdTweaksCompat ScienceBirdTweaksCompat = new("ScienceBird.ScienceBirdTweaks");
		internal static UniversalRadarCompat UniversalRadarCompat = new("ScienceBird.UniversalRadar");

		private void Awake()
		{
			logger = Logger;
			harmony.PatchAll();

			ConfigManager.Init(Config);

			// #if DEBUG
			Plugin.logger.LogWarning("Enabling full debug logging!");
			ConfigManager.Debug.Value = MrovLib.LoggingType.Developer;
			// #endif


			MrovLib.EventManager.SceneLoaded.AddListener(scene =>
			{
				if (scene == "SampleSceneRelay")
				{
					ReplaceVanillaShip.Init();
				}
			});

			ScienceBirdTweaksCompat.Init();
			TooManySuitsCompat.Init();
			UniversalRadarCompat.Init();

			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
