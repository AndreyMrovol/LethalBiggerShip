namespace BiggerShip.Compatibility
{
	internal class NavMeshLibCompat(string guid, string version = null) : MrovLib.CompatibilityHandler(guid, version)
	{
		public void RebakeExteriorNavmesh()
		{
			if (!IsModPresent)
			{
				return;
			}

			Plugin.debugLogger.LogDebug("NavMeshLib detected, rebaking exterior navmesh.");
			NavMeshLib.NavMeshUtil.RebakeExteriorNavMesh();
		}
	}
}
