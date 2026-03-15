namespace Kampai.Game
{
	public class VillainLairModel
	{
		public enum LairPrefabType
		{
			LAIR = 0,
			LOCKED_PLOT = 1,
			UNLOCKED_PLOT = 2
		}

		public global::System.Collections.Generic.IDictionary<int, global::UnityEngine.GameObject> villainLairInstances;

		public global::System.Collections.Generic.Dictionary<int, global::UnityEngine.GameObject> asyncLoadedPrefabs = new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.GameObject>();

		public global::Kampai.Game.VillainLair currentActiveLair { get; set; }

		public bool goingToLair { get; set; }

		public bool leavingLair { get; set; }

		public bool isPortalResourceModalOpen { get; set; }

		public bool areLairAssetsLoaded
		{
			get
			{
				return asyncLoadedPrefabs.Keys.Count >= global::System.Enum.GetValues(typeof(global::Kampai.Game.VillainLairModel.LairPrefabType)).Length;
			}
		}

		public bool seenCooldownAlert { get; set; }

		public GoTweenFlow cameraFlow { get; set; }

		public VillainLairModel()
		{
			villainLairInstances = new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.GameObject>();
			currentActiveLair = null;
			seenCooldownAlert = false;
		}
	}
}
