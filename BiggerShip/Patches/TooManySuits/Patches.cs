using TooManySuits;
using UnityEngine;

namespace BiggerShip.Patches.TooManySuit
{
	internal partial class TooManySuitsPatches
	{
		internal static bool PatchesPrefix(TooManySuits.Patches __instance)
		{
			var pageLabelGO = new GameObject("TooManySuitsPageLabel");
			pageLabelGO.SetActive(false);

			var pageLabelTransform = pageLabelGO.AddComponent<RectTransform>();
			pageLabelTransform.SetParent(StartOfRound.Instance.rightmostSuitPosition, false);
			pageLabelTransform.localPosition = Vector3.zero;
			pageLabelTransform.localEulerAngles = Vector3.zero;
			pageLabelTransform.localScale = Vector3.one * TooManySuits.TooManySuits.Config.LabelScale * 0.05f;

			var paginationController = pageLabelGO.AddComponent<PaginationController>();
			paginationController.SuitsPerPage = (int)(TooManySuits.TooManySuits.Config.SuitsPerPage * 1.75);

			var autoParentToShip = pageLabelGO.AddComponent<AutoParentToShip>();
			autoParentToShip.overrideOffset = true;
			autoParentToShip.positionOffset = new Vector3(-2.125f, 2.958f, -9.902f); // i also hardcoded this bullshit
			autoParentToShip.rotationOffset = new Vector3(0f, 180f, 0f);

			pageLabelGO.SetActive(true);
			TooManySuits.TooManySuits.SuitManager.UpdateSuits();

			return false;
		}
	}
}
