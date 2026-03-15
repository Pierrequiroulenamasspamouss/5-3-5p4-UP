public class TrackSpawnManager : global::UnityEngine.MonoBehaviour
{
	[global::System.Serializable]
	public struct ObjectToSpawn
	{
		public global::UnityEngine.GameObject SpawnPrefab;

		public int MaxCount;

		public float Weight;
	}

	public PathController Path;

	public global::System.Collections.Generic.List<float> SpawerLocations = new global::System.Collections.Generic.List<float>();

	public TrackSpawnManager.ObjectToSpawn[] PossibleSpawns;

	public global::UnityEngine.Transform SpawnedObjectsParent;

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject> spawnedObjects = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	private global::System.Collections.Generic.Dictionary<int, int> ObjectCounts = new global::System.Collections.Generic.Dictionary<int, int>();

	private void Start()
	{
		InititalizeIds();
		GenerateSpawnsOnTrack();
	}

	private void InititalizeIds()
	{
		int num = PossibleSpawns.Length;
		for (int i = 0; i < num; i++)
		{
			ObjectCounts.Add(i, 0);
		}
	}

	public void GenerateSpawnsOnTrack()
	{
		int max = PossibleSpawns.Length;
		int count = SpawerLocations.Count;
		for (int i = 0; i < count; i++)
		{
			global::UnityEngine.Vector3 positionOnSpline = Path.GetPositionOnSpline(SpawerLocations[i]);
			int num = 0;
			do
			{
				num = global::UnityEngine.Random.Range(0, max);
			}
			while (ObjectCounts[num] >= PossibleSpawns[num].MaxCount && PossibleSpawns[num].MaxCount > -1);
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(PossibleSpawns[num].SpawnPrefab);
			gameObject.transform.position = positionOnSpline;
			gameObject.transform.parent = SpawnedObjectsParent;
			spawnedObjects.Add(gameObject);
			global::System.Collections.Generic.Dictionary<int, int> objectCounts;
			global::System.Collections.Generic.Dictionary<int, int> dictionary = (objectCounts = ObjectCounts);
			int key2;
			int key = (key2 = num);
			key2 = objectCounts[key2];
			dictionary[key] = key2 + 1;
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
