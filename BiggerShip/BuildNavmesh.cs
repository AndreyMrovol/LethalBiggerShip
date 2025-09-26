using System.Collections.Generic;
using System.Diagnostics;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace BiggerShip
{
	internal class BuildNavmesh
	{
		private static Dictionary<GameObject, Vector3> OriginalOffMeshLinkPositions = [];
		private static GameObject NavmeshBlockers;

		public static void ReplaceNavmeshes()
		{
			Stopwatch stopwatch = new();
			stopwatch.Start();

			GameObject EnvironmentObject = GameObject.FindGameObjectWithTag("OutsideLevelNavMesh");
			if (EnvironmentObject == null)
			{
				Plugin.debugLogger.LogWarning("No GameObject with tag 'OutsideLevelNavMesh' found");
				return;
			}

			RemoveVanillaNavmesh(EnvironmentObject);
			ChangeOffMeshLinks(EnvironmentObject);
			SpawnNavmeshBlockers(EnvironmentObject);

			RebuildNavmesh(EnvironmentObject);

			stopwatch.Stop();
			Plugin.debugLogger.LogDebug($"Navmesh rebuilt in {stopwatch.ElapsedMilliseconds} ms");
		}

		public static void RemoveVanillaNavmesh(GameObject Environment)
		{
			GameObject NavmeshColliders = Environment.transform.Find("NavMeshColliders").gameObject;

			// remove old ship offlinks
			Transform ShipOffMeshLinks = NavmeshColliders.transform.Find("OffMeshLinks");

			if (ShipOffMeshLinks == null)
			{
				Plugin.debugLogger.LogWarning("No OffMeshLinks found under NavMeshColliders");
				return;
			}

			ShipOffMeshLinks.Find("ShipLadder")?.gameObject.SetActive(false);
			ShipOffMeshLinks.Find("ShipLadder2")?.gameObject.SetActive(false);

			// remove old ship navmesh
			Transform PlayerShipNavmesh = NavmeshColliders.transform.Find("PlayerShipNavmesh");
			PlayerShipNavmesh?.gameObject.SetActive(false);
		}

		public static void ChangeOffMeshLinks(GameObject Environment)
		{
			Transform OffLinkParent = Variables.HangarShip.transform.Find("ShipExtended(Clone)/ShipExtended/ShipNavmesh/OffMeshLinks");

			foreach (Transform child in OffLinkParent)
			{
				GameObject BottomPoint = child.Find("Bottom").gameObject;
				OffMeshLink link = child.GetComponent<OffMeshLink>();

				Vector3 OldEndPos = link.endTransform.localPosition;
				OriginalOffMeshLinkPositions[child.gameObject] = OldEndPos;

				if (
					Physics.Raycast(
						BottomPoint.transform.position,
						Vector3.down,
						out RaycastHit hitInfo,
						5f,
						LayerMask.GetMask("Default", "Room")
					)
				)
				{
					BottomPoint.transform.position = hitInfo.point;
					link.endTransform = BottomPoint.transform;
					Plugin.debugLogger.LogDebug($"Adjusted OffMeshLink {child.name} from {OldEndPos} to {link.endTransform.localPosition}");
				}

				if (link.costOverride != 0)
				{
					link.costOverride = 0;
				}

				link.UpdatePositions();
			}
		}

		public static void SpawnNavmeshBlockers(GameObject Environment)
		{
			GameObject navmeshBlockerPrefab = null;

			if (NavmeshBlockers == null)
			{
				navmeshBlockerPrefab = Variables.BiggerShipBundle.LoadAsset<GameObject>(
					"assets/lethalcompany/biggership/navmeshblockerextended.prefab"
				);

				NavmeshBlockers = navmeshBlockerPrefab;
			}

			if (navmeshBlockerPrefab != null)
			{
				NavmeshBlockers = GameObject.Instantiate(navmeshBlockerPrefab);
				NavmeshBlockers.transform.SetParent(Environment.transform);
				NavmeshBlockers.transform.position = new Vector3(3.5811f, 0.8754f, -14.5813f);
				NavmeshBlockers.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
				NavmeshBlockers.transform.localScale = new Vector3(100f, 100f, 100f);

				Plugin.debugLogger.LogDebug($"Instantiated NavmeshBlockers at position {NavmeshBlockers.transform.position}");
			}
		}

		public static void RebuildNavmesh(GameObject Environment)
		{
			Environment.GetComponent<NavMeshSurface>().BuildNavMesh();
		}

		public static void RestoreOffMeshLinks()
		{
			foreach (var kvp in OriginalOffMeshLinkPositions)
			{
				if (kvp.Key != null)
				{
					OffMeshLink link = kvp.Key.GetComponent<OffMeshLink>();
					if (link != null)
					{
						link.endTransform.localPosition = kvp.Value;
						link.UpdatePositions();
					}
				}
			}
		}
	}
}
