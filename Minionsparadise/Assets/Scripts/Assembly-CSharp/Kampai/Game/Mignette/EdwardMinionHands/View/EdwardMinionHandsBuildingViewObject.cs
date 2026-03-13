namespace Kampai.Game.Mignette.EdwardMinionHands.View
{
	public class EdwardMinionHandsBuildingViewObject : global::Kampai.Game.Mignette.View.MignetteBuildingViewObject
	{
		[global::System.Serializable]
		public class CollectableData
		{
			public int pointValue;

			public int numberInPool;
		}

		public global::UnityEngine.GameObject DefaultBush;

		public global::UnityEngine.Animation ShakeAnimation;

		public global::UnityEngine.GameObject BushLocator;

		public global::UnityEngine.GameObject CollectableGrabbedVfxPrefab;

		public global::UnityEngine.GameObject[] TopiaryPrefabs;

		public global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsBuildingViewObject.CollectableData[] CollectablePoolData;

		public global::UnityEngine.GameObject CollectablePrefab;

		public global::UnityEngine.GameObject CuttingToolPrefab;

		public global::UnityEngine.AnimationCurve TimeBetweenCollectables;

		public bool HasRequestedExit;

		private global::System.Collections.Generic.List<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsTopiaryViewObject> Topiaries = new global::System.Collections.Generic.List<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsTopiaryViewObject>();

		private global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsTopiaryViewObject activeTopiary;

		public void Start()
		{
			Topiaries.Clear();
			global::UnityEngine.GameObject[] topiaryPrefabs = TopiaryPrefabs;
			foreach (global::UnityEngine.GameObject original in topiaryPrefabs)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = BushLocator.transform.position;
				global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsTopiaryViewObject component = gameObject.GetComponent<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsTopiaryViewObject>();
				Topiaries.Add(component);
			}
			Reset();
			base.gameObject.AddComponent<global::Kampai.Game.Mignette.View.MignetteBuildingCooldownView>();
		}

		public int DisplayRandomTopiary()
		{
			int num = global::UnityEngine.Random.Range(0, Topiaries.Count);
			DisplayTopiary(num);
			return num;
		}

		private void DisplayTopiary(int index)
		{
			if (activeTopiary == Topiaries[index])
			{
				return;
			}
			DefaultBush.SetActive(false);
			foreach (global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsTopiaryViewObject topiary in Topiaries)
			{
				topiary.Reset();
			}
			activeTopiary = Topiaries[index];
			activeTopiary.ShowDefaultModel();
		}

		public void Reset()
		{
			HasRequestedExit = false;
			activeTopiary = null;
			DefaultBush.SetActive(true);
			foreach (global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsTopiaryViewObject topiary in Topiaries)
			{
				topiary.Reset();
			}
		}

		public override void ResetCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal)
		{
			Reset();
		}

		public override void UpdateCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal, int buildingData, float pctDone)
		{
			if (pctDone < 1f)
			{
				if (!HasRequestedExit)
				{
					DisplayTopiary(buildingData);
				}
				if (activeTopiary != null)
				{
					activeTopiary.SetCooldownViewState(pctDone);
				}
			}
		}
	}
}
