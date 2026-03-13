namespace Kampai.Game.Mignette.ButterflyCatch.View
{
	public class ButterflyCatchGameController : global::UnityEngine.MonoBehaviour
	{
		[global::System.Serializable]
		public class SpawnedButterfly
		{
			public global::UnityEngine.Material wingMaterial;

			public int pointValue;

			public float probability = 0.1f;
		}

		public global::UnityEngine.Transform CameraTransform;

		public float CameraFieldOfView = 45f;

		public float CameraNearClipPlane = 0.3f;

		public global::UnityEngine.TextAsset[] MinionPaths;

		public global::UnityEngine.TextAsset[] ButterflyPaths;

		private global::System.Collections.Generic.List<GoSpline> minionSplines;

		private global::System.Collections.Generic.List<GoSpline> butterflySplines;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject ButterflyPrefab;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject BeePrefab;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject NetPrefab;

		public global::UnityEngine.GameObject ButterflyCaughtVfxPrefab;

		[global::UnityEngine.SerializeField]
		private global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchGameController.SpawnedButterfly[] spawnedButterflies;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve maxButterfliesCurve;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve maxBeesCurve;

		private float[] weightedButterfly;

		private void Awake()
		{
			InitSplines();
			InitButterflyWeights();
		}

		private void InitButterflyWeights()
		{
			float num = 0f;
			for (int i = 0; i < spawnedButterflies.Length; i++)
			{
				num += spawnedButterflies[i].probability;
			}
			weightedButterfly = new float[spawnedButterflies.Length];
			float num2 = 0f;
			for (int j = 0; j < spawnedButterflies.Length; j++)
			{
				num2 += spawnedButterflies[j].probability / num;
				weightedButterfly[j] = num2;
			}
		}

		private void InitSplines()
		{
			minionSplines = new global::System.Collections.Generic.List<GoSpline>();
			butterflySplines = new global::System.Collections.Generic.List<GoSpline>();
			BuildSplines(MinionPaths, minionSplines);
			BuildSplines(ButterflyPaths, butterflySplines);
		}

		private void BuildSplines(global::UnityEngine.TextAsset[] paths, global::System.Collections.Generic.List<GoSpline> splineList)
		{
			foreach (global::UnityEngine.TextAsset asset in paths)
			{
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = LoadPoints(asset);
				if (list.Count > 0)
				{
					list.Insert(0, global::UnityEngine.Vector3.zero);
					list.Add(global::UnityEngine.Vector3.zero);
					global::UnityEngine.Vector3 position = base.transform.position;
					for (int j = 0; j < list.Count; j++)
					{
						global::System.Collections.Generic.List<global::UnityEngine.Vector3> list3;
						global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = (list3 = list);
						int index2;
						int index = (index2 = j);
						global::UnityEngine.Vector3 vector = list3[index2];
						list2[index] = vector + position;
					}
					GoSpline goSpline = new GoSpline(list);
					goSpline.buildPath();
					goSpline.closePath();
					splineList.Add(goSpline);
				}
			}
		}

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3> LoadPoints(global::UnityEngine.TextAsset asset)
		{
			if (asset == null)
			{
				return new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
			}
			return GoSpline.bytesToVector3List(asset.bytes);
		}

		public GoSpline GetButterflySpline(int index)
		{
			return butterflySplines[index];
		}

		public GoSpline GetMinionSpline(int index)
		{
			return minionSplines[index];
		}

		public int GetButterflySplineCount()
		{
			return butterflySplines.Count;
		}

		public global::UnityEngine.GameObject SpawnNet()
		{
			return global::UnityEngine.Object.Instantiate(NetPrefab);
		}

		public global::UnityEngine.GameObject SpawnBee(int activeCount, float timeElapsed)
		{
			if (global::UnityEngine.Random.value < 0.1f && (float)activeCount < maxBeesCurve.Evaluate(timeElapsed))
			{
				return global::UnityEngine.Object.Instantiate(BeePrefab);
			}
			return null;
		}

		public global::UnityEngine.GameObject SpawnButterfly(int activeCount, float timeElapsed)
		{
			if ((float)activeCount >= maxButterfliesCurve.Evaluate(timeElapsed))
			{
				return null;
			}
			float value = global::UnityEngine.Random.value;
			int num = 0;
			for (int i = 0; i < weightedButterfly.Length; i++)
			{
				if (value <= weightedButterfly[i])
				{
					num = i;
					break;
				}
			}
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(ButterflyPrefab);
			global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject component = gameObject.GetComponent<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject>();
			component.RendererForMaterial.material = spawnedButterflies[num].wingMaterial;
			component.myScore = spawnedButterflies[num].pointValue;
			return gameObject;
		}
	}
}
