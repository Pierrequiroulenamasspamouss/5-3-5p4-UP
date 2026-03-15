namespace Kampai.Game
{
	public class MagnetFingerPickController : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MagnetFingerPickController") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int pickEvent { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 inputPosition { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal stateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.View.CameraUtils cameraUtils { get; set; }

		[Inject]
		public global::Kampai.Common.SelectMinionSignal selectMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displaySignal { get; set; }

		public override void Execute()
		{
			switch (pickEvent)
			{
			case 1:
				Initialize();
				break;
			case 2:
				model.CurrentMagnetFingerTimer += global::UnityEngine.Time.deltaTime;
				model.DurationBetweenMinions -= 0.06f * global::UnityEngine.Time.deltaTime;
				if (model.CurrentMagnetFingerTimer > model.DurationBetweenMinions)
				{
					SelectMinion();
				}
				break;
			case 3:
				if (model.ValidLocation)
				{
					MinionFreeze(false);
					Reset();
					int count = model.SelectedMinions.Count;
					LocalPersistMagnetFingerAction(count);
					routineRunner.StartCoroutine(DelayPlayerTraining());
				}
				break;
			}
		}

		private void Initialize()
		{
			if (model.MMView == null)
			{
				model.MMView = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			}
			global::UnityEngine.Vector3 xZProjection = cameraUtils.GroundPlaneRaycast(inputPosition);
			global::Kampai.Util.Point p = new global::Kampai.Util.Point
			{
				XZProjection = xZProjection
			};
			if (!environment.Contains(p) || !environment.IsWalkable(p.x, p.y))
			{
				model.ValidLocation = false;
				return;
			}
			model.ValidLocation = true;
			model.Points = environment.GetMagnetFingerGridSquares(p.x, p.y);
			SendSelectedSignals();
			model.Minions = model.MMView.GetMinionListSortedByDistanceAndState(inputPosition, false);
			MinionFreeze(true);
			SelectMinion();
		}

		private void SendSelectedSignals()
		{
			foreach (global::System.Collections.Generic.KeyValuePair<int, global::Kampai.Common.SelectedMinionModel> selectedMinion in model.SelectedMinions)
			{
				int key = selectedMinion.Key;
				global::Kampai.Util.Point point = model.Points.Dequeue();
				selectMinionSignal.Dispatch(key, new global::Kampai.Util.Boxed<global::UnityEngine.Vector3>(new global::UnityEngine.Vector3(point.x, 0f, point.y)), true);
			}
		}

		private void MinionFreeze(bool freeze)
		{
			foreach (int minion in model.Minions)
			{
				global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minion);
				if (freeze)
				{
					if (byInstanceId.State != global::Kampai.Game.MinionState.Leisure)
					{
						stateChangeSignal.Dispatch(minion, global::Kampai.Game.MinionState.WaitingOnMagnetFinger);
						global::Kampai.Game.View.MinionObject minionObject = model.MMView.Get(minion);
						minionObject.EnqueueAction(new global::Kampai.Game.View.RotateAction(minionObject, global::UnityEngine.Camera.main.transform.eulerAngles.y - 180f, 360f, logger), true);
					}
				}
				else if (byInstanceId.State == global::Kampai.Game.MinionState.WaitingOnMagnetFinger)
				{
					stateChangeSignal.Dispatch(minion, global::Kampai.Game.MinionState.Idle);
				}
			}
		}

		private void SelectMinion()
		{
			if (model.Minions.Count != 0)
			{
				int param = model.Minions.Dequeue();
				global::Kampai.Util.Point point = model.Points.Dequeue();
				int num = 200 / (model.SelectedMinions.Count + 1);
				int num2 = randomService.NextInt(0, 100);
				bool param2 = ((num < num2) ? true : false);
				selectMinionSignal.Dispatch(param, new global::Kampai.Util.Boxed<global::UnityEngine.Vector3>(new global::UnityEngine.Vector3(point.x, 0f, point.y)), param2);
				model.CurrentMagnetFingerTimer = 0f;
			}
		}

		private void Reset()
		{
			model.CurrentMagnetFingerTimer = 0f;
			model.DurationBetweenMinions = 0.2f;
		}

		private void LocalPersistMagnetFingerAction(int selectedMinionCount)
		{
			if (selectedMinionCount > 0 && !localPersistService.HasKey("didyouknow_MagnetFinger"))
			{
				localPersistService.PutDataInt("didyouknow_MagnetFinger", 1);
			}
		}

		private global::System.Collections.IEnumerator DelayPlayerTraining()
		{
			yield return new global::UnityEngine.WaitForSeconds(1f);
			displaySignal.Dispatch(19000015, false, new global::strange.extensions.signal.impl.Signal<bool>());
		}
	}
}
