using System.Linq;
using BiggerShip;
using HarmonyLib;
using MrovLib.ContentType;
using UnityEngine;

namespace BiggerShip.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	internal class SuitPositionPatch
	{
		[HarmonyPatch("PositionSuitsOnRack")]
		[HarmonyPostfix]
		public static void AdjustSuitPosition(StartOfRound __instance)
		{
			UnlockableSuit[] unlockableSuits = GameObject.FindObjectsOfType<UnlockableSuit>();
			UnlockableSuit[] array = unlockableSuits.OrderBy(suit => suit.syncedSuitID.Value).ToArray();

			for (int i = 0; i < array.Length; i++)
			{
				UnlockableItem unlockableItem = array[i].GetComponent<UnlockableItem>();
				// BuyableSuit suit = MrovLib.ContentManager.GetBuyable(unlockableItem) as BuyableSuit;

				AutoParentToShip component = array[i].gameObject.GetComponent<AutoParentToShip>();

				Vector3 newPosition = __instance.rightmostSuitPosition.localPosition + __instance.rightmostSuitPosition.forward * 0.18f * i;
				component.positionOffset = newPosition;

				if (unlockableItem == null)
				{
					// Plugin.logger.LogError($"unlockableItem is null for suitID {array[i].suitID}");
				}
				else
				{
					// Plugin.logger.LogWarning($"Moving suit {unlockableItem.unlockableName} to position {newPosition}");
				}
			}
		}
	}
}
