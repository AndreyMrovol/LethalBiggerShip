using UnityEngine;

namespace BiggerShip.Definitions
{
	public struct ObjectNewPosition
	{
		public string Name { get; set; }
		public bool ToRemove => Position == Vector3.zero && Rotation == Vector3.zero && Scale == Vector3.zero;

		public Vector3 Position;
		public Vector3 Rotation;
		public Vector3 Scale;
	}
}
