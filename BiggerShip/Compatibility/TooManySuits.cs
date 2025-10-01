using System.Runtime.CompilerServices;
using HarmonyLib;
using TooManySuits;

namespace BiggerShip.Compatibility
{
	internal class TooManySuitsCompat(string guid, string version = null) : MrovLib.Compatibility.CompatibilityBase(guid, version)
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public void Init()
		{
			if (!IsModPresent)
			{
				return;
			}

			Plugin.harmony.Patch(
				original: AccessTools.Method(typeof(PaginationController), "DisplayCurrentPage"),
				prefix: new HarmonyMethod(typeof(BiggerShip.Patches.TooManySuit.TooManySuitsPatches), "DisplaySuitsPrefix")
			);

			Plugin.harmony.Patch(
				original: AccessTools.Method(typeof(TooManySuits.Patches), "StartPatch"),
				prefix: new HarmonyMethod(typeof(BiggerShip.Patches.TooManySuit.TooManySuitsPatches), "PatchesPrefix")
			);
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public void ReloadSuitRack()
		{
			if (!IsModPresent)
			{
				return;
			}

			StartOfRound.Instance.rightmostSuitPosition.gameObject.GetComponentInChildren<PaginationController>()?.UpdateSuits();
		}
	}
}
