public class AlligatorTrackSpawnManager : global::UnityEngine.MonoBehaviour
{
	public enum SpawnConfig
	{
		Collectable = 0,
		Obstacle = 1,
		ObstaclesAndCollectables = 2
	}

	[global::System.Serializable]
	public struct SpawnerConfiguration
	{
		public float Position;

		public AlligatorTrackSpawnManager.SpawnConfig Config;
	}

	public AlligatorWaypointController Path;

	public global::System.Collections.Generic.List<AlligatorTrackSpawnManager.SpawnerConfiguration> Spawners = new global::System.Collections.Generic.List<AlligatorTrackSpawnManager.SpawnerConfiguration>();

	public global::System.Collections.Generic.List<AlligatorTrackSpawnObject> LootTable = new global::System.Collections.Generic.List<AlligatorTrackSpawnObject>();

	public global::UnityEngine.Transform SpawnedObjectsParent;

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject> spawnedObjects = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	private void Start()
	{
		GenerateSpawnsFromLootTable();
	}

	public AlligatorTrackSpawnObject GetSpawnFromLootTable()
	{
		int index = global::UnityEngine.Random.Range(0, LootTable.Count);
		return LootTable[index];
	}

	public void GenerateSpawnsFromLootTable()
	{
		int count = Spawners.Count;
		for (int i = 0; i < count; i++)
		{
			global::UnityEngine.Vector3 positionOnMinionSpline = Path.GetPositionOnMinionSpline(Spawners[i].Position);
			int num = 0;
			AlligatorTrackSpawnObject alligatorTrackSpawnObject = null;
			do
			{
				alligatorTrackSpawnObject = GetSpawnFromLootTable();
				num++;
				if (num > 100)
				{
					alligatorTrackSpawnObject = null;
					break;
				}
			}
			while (alligatorTrackSpawnObject.TrackObjectType != Spawners[i].Config && Spawners[i].Config != AlligatorTrackSpawnManager.SpawnConfig.ObstaclesAndCollectables);
			if (!(alligatorTrackSpawnObject == null))
			{
				LootTable.Remove(alligatorTrackSpawnObject);
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(alligatorTrackSpawnObject.gameObject);
				gameObject.transform.position = positionOnMinionSpline;
				gameObject.transform.parent = SpawnedObjectsParent;
				spawnedObjects.Add(gameObject);
			}
		}
	}

	public void GenerateSpawnSeed(int spawns)
	{
		for (int i = 0; i < spawns; i++)
		{
			AlligatorTrackSpawnManager.SpawnerConfiguration item = new AlligatorTrackSpawnManager.SpawnerConfiguration
			{
				Config = AlligatorTrackSpawnManager.SpawnConfig.ObstaclesAndCollectables,
				Position = global::UnityEngine.Random.Range(0.25f, 0.85f)
			};
			Spawners.Add(item);
		}
	}

	public void ClearSpawns()
	{
		int count = spawnedObjects.Count;
		for (int i = 0; i < count; i++)
		{
			if (spawnedObjects[i] != null)
			{
				global::UnityEngine.Object.Destroy(spawnedObjects[i]);
			}
		}
		spawnedObjects.Clear();
	}
}
