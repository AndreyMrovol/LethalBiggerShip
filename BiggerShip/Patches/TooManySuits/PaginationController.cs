using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using TooManySuits;
using UnityEngine;

namespace BiggerShip.Patches.TooManySuit
{
	internal partial class TooManySuitsPatches
	{
		internal static bool DisplaySuitsPrefix(PaginationController __instance)
		{
			var startIndex = __instance.CurrentPage * __instance.SuitsPerPage;
			var endIndex = Mathf.Min(startIndex + __instance.SuitsPerPage, __instance._allSuits.Length);

			var SuitRack = StartOfRound.Instance.rightmostSuitPosition.parent.Find("NurbsPath.002");
			float ColliderSize = SuitRack.GetComponent<MeshRenderer>().bounds.size.x - 0.3f;
			var suitThickness = ColliderSize / __instance.SuitsPerPage;

			var num = 0;
			for (var i = 0; i < __instance._allSuits.Length; i++)
			{
				var suit = __instance._allSuits[i];
				var autoParent = suit.gameObject.GetComponent<AutoParentToShip>();
				if (autoParent == null)
					continue;

				var shouldShow = i >= startIndex && i < endIndex;

				foreach (var renderer in suit.gameObject.GetComponentsInChildren<Renderer>())
				{
					renderer.enabled = shouldShow;
				}

				foreach (var collider in suit.gameObject.GetComponentsInChildren<Collider>())
				{
					collider.enabled = shouldShow;
				}

				var interactTrigger = suit.gameObject.GetComponent<InteractTrigger>();
				interactTrigger.enabled = shouldShow;
				interactTrigger.interactable = shouldShow;

				if (!shouldShow)
					continue;

				autoParent.overrideOffset = true;
				autoParent.positionOffset =
					StartOfRound.Instance.rightmostSuitPosition.localPosition
					+ StartOfRound.Instance.rightmostSuitPosition.forward * (suitThickness * num);
				autoParent.rotationOffset = new Vector3(0f, 90f, 0f);

				num++;
			}

			__instance.StartCoroutine(__instance.UpdateLabel());
			return false;
		}
	}
}
