using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace BiggerShip.Compatibility
{
	internal class UniversalRadarCompat(string guid, string version = null) : MrovLib.Compatibility.CompatibilityBase(guid, version)
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public void Init()
		{
			if (!this.IsModPresent)
			{
				return;
			}

			Plugin.debugLogger.LogDebug("UniversalRadar detected, applying patches.");
			PatchThings();
		}

		public void PatchThings()
		{
			Plugin.harmony.Patch(
				AccessTools.Method(typeof(UniversalRadar.Patches.RadarExtraPatches), "SetCamParameters"),
				transpiler: new HarmonyMethod(typeof(UniversalRadarCompat), nameof(ShipContourTranspiler))
			);

			Plugin.harmony.Patch(
				AccessTools.Method(typeof(UniversalRadar.Patches.RadarExtraPatches), "SetShipSpriteColour"),
				transpiler: new HarmonyMethod(typeof(UniversalRadarCompat), nameof(ShipContourTranspiler))
			);
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static IEnumerable<CodeInstruction> ShipContourTranspiler(IEnumerable<CodeInstruction> instructions)
		{
			CodeMatcher matcher = new(instructions);

			try
			{
				// look for: ldstr	"Environment/HangarShip/Square" and replace it with "Environment/HangarShip/ShipExtended(Clone)/ShipExtended/ShipContour"
				matcher.MatchForward(false, new CodeMatch(OpCodes.Ldstr, "Environment/HangarShip/Square"));

				// get all matches and replace instructions
				matcher.Repeat(match =>
				{
					match.SetOperandAndAdvance("Environment/HangarShip/ShipExtended(Clone)/ShipExtended/ShipContour");
					Plugin.debugLogger.LogInfo("ShipContourTranspiler: Replaced GameObject path");
				});
			}
			catch (System.Exception ex)
			{
				Plugin.debugLogger.LogError($"ShipContourTranspiler: Exception occurred - {ex.Message}");
				return instructions;
			}

			return matcher.InstructionEnumeration();
		}
	}
}
