public class WaterslideSpawnObject : global::UnityEngine.MonoBehaviour
{
	public enum SpawnType
	{
		Collectable = 0,
		Obstacle = 1
	}

	public WaterslideSpawnObject.SpawnType WaterslideObjectType = WaterslideSpawnObject.SpawnType.Obstacle;

	public float PointValue = 10f;
}
