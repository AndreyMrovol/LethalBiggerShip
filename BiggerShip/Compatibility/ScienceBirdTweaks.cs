using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace BiggerShip.Compatibility
{
	internal class ScienceBirdTweaksCompat(string guid, string version = null) : MrovLib.Compatibility.CompatibilityBase(guid, version)
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public void Init()
		{
			if (!this.IsModPresent)
			{
				return;
			}

			Plugin.harmony.Patch(
				AccessTools.Method(typeof(ScienceBirdTweaks.Patches.OccupancyPatch), "UpdatePoster"),
				transpiler: new HarmonyMethod(typeof(ScienceBirdTweaksCompat), nameof(OccupancyTranspiler))
			);

			Plugin.debugLogger.LogDebug("ScienceBirdTweaks detected, applying patches.");
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static IEnumerable<CodeInstruction> OccupancyTranspiler(IEnumerable<CodeInstruction> instructions)
		{
			CodeMatcher matcher = new(instructions);

			try
			{
				// Look for the pattern:
				// stsfld playerCount field
				// ldstr "HangarShip/Plane.001"
				matcher.MatchForward(
					false,
					new CodeMatch(OpCodes.Stsfld, AccessTools.Field(typeof(ScienceBirdTweaks.Patches.OccupancyPatch), "playerCount")),
					new CodeMatch(OpCodes.Ldstr, "HangarShip/Plane.001")
				);

				if (matcher.IsInvalid)
				{
					// Pattern not found - log warning and return original instructions
					Plugin.debugLogger.LogWarning("OccupancyTranspiler: Could not find target pattern, returning original instructions");
					return instructions;
				}

				// Move to the ldstr instruction (second match)
				matcher.Advance(1);

				// Verify we're at the right instruction
				if (matcher.Opcode != OpCodes.Ldstr || (string)matcher.Operand != "HangarShip/Plane.001")
				{
					Plugin.debugLogger.LogWarning("OccupancyTranspiler: Unexpected instruction found, returning original instructions");
					return instructions;
				}

				// Replace the string operand
				matcher.SetOperandAndAdvance("HangarShip/Plane.001/Posters/Poster6");

				Plugin.debugLogger.LogInfo("OccupancyTranspiler: Successfully replaced GameObject path");
			}
			catch (System.Exception ex)
			{
				Plugin.debugLogger.LogError($"OccupancyTranspiler: Exception occurred - {ex.Message}");
				return instructions;
			}

			return matcher.InstructionEnumeration();
		}
	}
}
