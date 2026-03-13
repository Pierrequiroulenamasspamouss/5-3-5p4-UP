namespace Kampai.Common
{
	public class PickControllerModel
	{
		public enum Mode
		{
			None = 0,
			Building = 1,
			DragAndDrop = 2,
			MagnetFinger = 3,
			Minion = 4,
			EnvironmentalMignette = 5,
			LandExpansion = 6,
			TikiBarView = 7,
			StageView = 8,
			VillainIsland = 9,
			VillainLair = 10
		}

		public const float maxTouchDelta = 15f;

		public const float MagnetFingerTheshold = 1f;

		public const float BaseDurationBetweenMinions = 0.2f;

		public const float DurationReductionPerSecond = 0.06f;

		public const float TimeForDoubleClick = 0.2f;

		public float DurationBetweenMinions = 0.2f;

		private global::System.Collections.Generic.List<int> ignoredInstances = new global::System.Collections.Generic.List<int>();

		private int SkrimCounter;

		private bool forceDisabled;

		public global::Kampai.Common.PickControllerModel.Mode CurrentMode { get; set; }

		public bool PreviousPressState { get; set; }

		public global::UnityEngine.Vector3 StartTouchPosition { get; set; }

		public global::System.Collections.Generic.Dictionary<int, global::Kampai.Common.SelectedMinionModel> SelectedMinions { get; set; }

		public int? SelectedBuilding { get; set; }

		public bool activitySpinnerExists { get; set; }

		public global::UnityEngine.GameObject StartHitObject { get; set; }

		public global::UnityEngine.GameObject EndHitObject { get; set; }

		public float StartTouchTimeMs { get; set; }

		public bool InvalidateMovement { get; set; }

		public bool DetectedMovement { get; set; }

		public bool ValidLocation { get; set; }

		public bool Blocked { get; set; }

		public float HeldTimer { get; set; }

		public global::Kampai.Game.View.MinionManagerView MMView { get; set; }

		public global::System.Collections.Generic.Queue<int> Minions { get; set; }

		public float CurrentMagnetFingerTimer { get; set; }

		public global::System.Collections.Generic.Queue<global::Kampai.Util.Point> Points { get; set; }

		public global::Kampai.Util.Point MainPoint { get; set; }

		public global::UnityEngine.Vector3 DragPreviousPosition { get; set; }

		public global::Kampai.Game.DragOffsetType OffsetType { get; set; }

		public float LastClickTime { get; set; }

		public global::UnityEngine.Vector2 LastBuildingStorePosition { get; set; }

		public bool WaitingForDouble { get; set; }

		public bool HasPlayedGacha { get; set; }

		public bool HasPlayedSFX { get; set; }

		public global::UnityEngine.Object MinionMoveToIndicator { get; set; }

		public bool Enabled
		{
			get
			{
				return SkrimCounter == 0 && !forceDisabled;
			}
		}

		public bool PanningCameraBlocked { get; set; }

		public bool ZoomingCameraBlocked { get; set; }

		public global::UnityEngine.GameObject minionMoveIndicator { get; set; }

		public bool ForceDisabled
		{
			get
			{
				return forceDisabled;
			}
			set
			{
				forceDisabled = value;
			}
		}

		public PickControllerModel()
		{
			SelectedMinions = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Common.SelectedMinionModel>();
			SelectedBuilding = null;
		}

		public void SetIgnoreInstance(int instanceId, bool ignored)
		{
			if (ignored)
			{
				if (!IsInstanceIgnored(instanceId))
				{
					ignoredInstances.Add(instanceId);
				}
			}
			else
			{
				ignoredInstances.Remove(instanceId);
			}
		}

		public bool IsInstanceIgnored(int instanceId)
		{
			return ignoredInstances.Contains(instanceId);
		}

		public void IncreaseSkrimCounter()
		{
			SkrimCounter++;
		}

		public void DecreaseSkrimCounter()
		{
			SkrimCounter--;
			if (SkrimCounter <= 0)
			{
				SkrimCounter = 0;
			}
		}
	}
}
