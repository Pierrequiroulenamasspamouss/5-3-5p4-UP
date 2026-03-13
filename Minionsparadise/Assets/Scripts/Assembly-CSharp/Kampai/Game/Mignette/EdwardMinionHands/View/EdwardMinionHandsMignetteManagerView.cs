namespace Kampai.Game.Mignette.EdwardMinionHands.View
{
	public class EdwardMinionHandsMignetteManagerView : global::Kampai.Game.Mignette.View.MignetteManagerView
	{
		private const string CHAT_LEFT1_ANIM_STATE_NAME = "Base Layer.ChatLeft01";

		private const string NOD_RIGHT1_ANIM_STATE_NAME = "Base Layer.NodRight01";

		private const string NOD_LEFT2_ANIM_STATE_NAME = "Base Layer.NodLeft02";

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("EdwardMinionHandsMignetteManagerView") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.List<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject> ToolsWithMinionsList = new global::System.Collections.Generic.List<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject>();

		private global::System.Collections.Generic.Dictionary<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCollectableViewObject, global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject> CollectableDictionary = new global::System.Collections.Generic.Dictionary<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCollectableViewObject, global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject>();

		private global::System.Collections.Generic.List<int> CollectablePointPool = new global::System.Collections.Generic.List<int>();

		private global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsBuildingViewObject BuildingViewReference;

		private global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsGameController gameController;

		private float TimeTillCollectableEmit;

		private bool TrimmingInProgress;

		private float currentProgress;

		private global::System.Collections.Generic.List<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject> ToolsWaitingForStateChangeQueue = new global::System.Collections.Generic.List<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject>();

		public int prevStateNameHash;

		private CustomFMOD_StudioEventEmitter trimEmitter;

		private bool trimSoundPlaying;

		private bool hidePolesIfInIdle;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.ChangeMignetteScoreSignal changeScoreSignal { get; set; }

		[Inject]
		public global::Kampai.Common.Service.Audio.IFMODService fmodService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnMignetteDooberSignal spawnMignetteDooberSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RequestStopMignetteSignal requestStopMignetteSignal { get; set; }

		protected override void Start()
		{
			base.Start();
			BuildingViewReference = MignetteBuildingObject.GetComponent<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsBuildingViewObject>();
			BuildingViewReference.Reset();
			gameController = base.gameObject.GetComponentInChildren<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsGameController>();
			TimeElapsed = 0f;
			TotalEventTime = 45f;
			CollectablePointPool.Clear();
			global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsBuildingViewObject.CollectableData[] collectablePoolData = BuildingViewReference.CollectablePoolData;
			foreach (global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsBuildingViewObject.CollectableData collectableData in collectablePoolData)
			{
				for (int j = 0; j < collectableData.numberInPool; j++)
				{
					CollectablePointPool.Add(collectableData.pointValue);
				}
			}
			ToolsWithMinionsList.Clear();
			int num = 0;
			for (num = 0; num < MignetteBuildingObject.GetMignetteMinionCount(); num++)
			{
				global::Kampai.Game.View.TaskingMinionObject childMinion = MignetteBuildingObject.GetChildMinion(num);
				childMinion.Minion.EnableBlobShadow(true);
				if (childMinion.Minion.GetComponent<global::UnityEngine.Collider>() != null)
				{
					childMinion.Minion.GetComponent<global::UnityEngine.Collider>().enabled = true;
				}
				else
				{
					childMinion.Minion.gameObject.AddComponent<global::UnityEngine.CapsuleCollider>();
				}
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(BuildingViewReference.CuttingToolPrefab);
				gameObject.transform.SetParent(base.transform, false);
				global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject component = gameObject.GetComponent<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject>();
				component.Setup(childMinion.Minion, 2f, this);
				ToolsWithMinionsList.Add(component);
			}
			global::UnityEngine.Transform newTransform = gameController.CameraTransform16x9;
			if (base.mignetteCamera.aspect <= 1.4f)
			{
				newTransform = gameController.CameraTransform4x3;
			}
			RelocateCameraForMignette(newTransform, gameController.FieldOfView, gameController.NearClipPlane, 1f);
			trimEmitter = base.gameObject.AddComponent<CustomFMOD_StudioEventEmitter>();
			trimEmitter.shiftPosition = false;
			trimEmitter.staticSound = false;
			trimEmitter.startEventOnAwake = false;
			trimEmitter.path = fmodService.GetGuid("Play_minion_topiary_trim_01");
			StartCoroutine(IntroSequence());
		}

		private global::System.Collections.IEnumerator IntroSequence()
		{
			bool lookLeft = true;
			foreach (global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject tool in ToolsWithMinionsList)
			{
				tool.StartMinionChat(lookLeft);
				lookLeft = !lookLeft;
			}
			yield return new global::UnityEngine.WaitForSeconds(2f);
			bool everyoneIdle = false;
			while (!everyoneIdle)
			{
				everyoneIdle = true;
				foreach (global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject tool2 in ToolsWithMinionsList)
				{
					if (!tool2.IsMinionIdle())
					{
						everyoneIdle = false;
					}
				}
				if (!everyoneIdle)
				{
					yield return new global::UnityEngine.WaitForSeconds(0.1f);
				}
			}
			TrimmingInProgress = true;
			EveryoneCut();
			TimeTillCollectableEmit = BuildingViewReference.TimeBetweenCollectables.Evaluate(TimeElapsed);
		}

		public override void Update()
		{
			if (base.IsPaused)
			{
				return;
			}
			PlayAudioForStateChanges();
			if (hidePolesIfInIdle && ToolsWithMinionsList[0].IsMinionIdleWithPole())
			{
				foreach (global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject toolsWithMinions in ToolsWithMinionsList)
				{
					toolsWithMinions.ShowPole(false);
					hidePolesIfInIdle = false;
				}
			}
			base.Update();
			if (!TrimmingInProgress)
			{
				return;
			}
			TimeElapsed += global::UnityEngine.Time.deltaTime;
			if (TimeElapsed >= 45f)
			{
				TimeElapsed = 45f;
				int mignetteData = BuildingViewReference.DisplayRandomTopiary();
				global::Kampai.Game.MignetteBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MignetteBuilding>(MignetteBuildingObject.ID);
				byInstanceId.MignetteData = mignetteData;
				TrimmingInProgress = false;
				EveryoneCheer();
				hidePolesIfInIdle = true;
				Invoke("ExitMignette", 2f);
			}
			float num = 45f - TimeElapsed;
			TimeTillCollectableEmit -= global::UnityEngine.Time.deltaTime;
			if (!(TimeTillCollectableEmit <= 0f) || !(num >= 5f))
			{
				return;
			}
			currentProgress += 10f;
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(BuildingViewReference.CollectablePrefab);
			global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCollectableViewObject component = gameObject.GetComponent<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCollectableViewObject>();
			if (CollectablePointPool.Count != 0)
			{
				int num2 = CollectablePointPool[global::UnityEngine.Random.Range(0, CollectablePointPool.Count)];
				CollectablePointPool.Remove(num2);
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = BuildingViewReference.BushLocator.transform.position;
				global::UnityEngine.Vector3 vector = base.mignetteCamera.transform.position - BuildingViewReference.BushLocator.transform.position;
				vector.y = 0f;
				vector.Normalize();
				float num3 = global::UnityEngine.Random.Range(135f, 75f);
				if (global::UnityEngine.Random.Range(0f, 1f) >= 0.5f)
				{
					num3 *= -1f;
				}
				vector = global::UnityEngine.Quaternion.Euler(0f, num3, 0f) * vector;
				float num4 = global::UnityEngine.Random.Range(3.5f, 5f);
				global::UnityEngine.Vector3 targetPos = gameObject.transform.position + vector * num4;
				component.StartCollectable(targetPos, num2, 4f, this);
				globalAudioSignal.Dispatch("Play_dooberSpawn_whistle_01");
				TimeTillCollectableEmit = BuildingViewReference.TimeBetweenCollectables.Evaluate(TimeElapsed);
			}
		}

		public void CollectableHasTimedOut(global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCollectableViewObject viewObject)
		{
			if (CollectableDictionary.ContainsKey(viewObject))
			{
				global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject edwardMinionHandsCuttingToolViewObject = CollectableDictionary[viewObject];
				edwardMinionHandsCuttingToolViewObject.ClearCollectable();
			}
			global::UnityEngine.Object.Destroy(viewObject.gameObject);
		}

		public void OnInputDown(global::UnityEngine.Vector3 inputPosition)
		{
			global::UnityEngine.Ray ray = base.mignetteCamera.ScreenPointToRay(inputPosition);
			global::UnityEngine.RaycastHit hitInfo;
			if (global::UnityEngine.Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 8192))
			{
				global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCollectableViewObject component = hitInfo.collider.gameObject.transform.parent.GetComponent<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCollectableViewObject>();
				if (component.WasTapped())
				{
					globalAudioSignal.Dispatch("Play_mignette_collect");
					SendMinionToCollectDoober(component);
				}
			}
		}

		public void CollectableHasBeenCollected(global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCollectableViewObject collectableViewObject)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(BuildingViewReference.CollectableGrabbedVfxPrefab);
			gameObject.transform.SetParent(base.transform, false);
			global::UnityEngine.Vector3 position = collectableViewObject.transform.position;
			position.y += 1f;
			gameObject.transform.position = position;
			global::UnityEngine.Object.Destroy(gameObject, 5f);
			int pointValue = collectableViewObject.GetPointValue();
			spawnMignetteDooberSignal.Dispatch(mignetteHUD, collectableViewObject.transform.position, pointValue, true);
			changeScoreSignal.Dispatch(pointValue);
			global::UnityEngine.Object.Destroy(collectableViewObject.gameObject);
		}

		private void SendMinionToCollectDoober(global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCollectableViewObject collectableViewObject)
		{
			if (collectableViewObject == null)
			{
				logger.FatalNullArgument(global::Kampai.Util.FatalCode.MIGNETTE_BAD_COLLECTABLE_OBJECT);
			}
			else if (!CollectableDictionary.ContainsKey(collectableViewObject))
			{
				global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject toolClosestToDoober = GetToolClosestToDoober(collectableViewObject.transform.position);
				if (toolClosestToDoober != null && collectableViewObject != null)
				{
					toolClosestToDoober.GoPickupDoober(collectableViewObject);
					CollectableDictionary.Add(collectableViewObject, toolClosestToDoober);
				}
			}
		}

		private global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject GetToolClosestToDoober(global::UnityEngine.Vector3 dooberPos)
		{
			float num = 0f;
			global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject edwardMinionHandsCuttingToolViewObject = null;
			foreach (global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject toolsWithMinions in ToolsWithMinionsList)
			{
				if (!toolsWithMinions.IsCollecting())
				{
					float num2 = global::UnityEngine.Vector3.Distance(toolsWithMinions.transform.position, dooberPos);
					if (edwardMinionHandsCuttingToolViewObject == null || num2 < num)
					{
						edwardMinionHandsCuttingToolViewObject = toolsWithMinions;
						num = num2;
					}
				}
			}
			return edwardMinionHandsCuttingToolViewObject;
		}

		private void PlayAudioForStateChanges()
		{
			global::Kampai.Game.View.MinionObject myMinionToUpdate = ToolsWithMinionsList[0].myMinionToUpdate;
			global::UnityEngine.AnimatorStateInfo? animatorStateInfo = myMinionToUpdate.GetAnimatorStateInfo(0);
			if (!animatorStateInfo.HasValue)
			{
				return;
			}
			global::UnityEngine.AnimatorStateInfo value = animatorStateInfo.Value;
			if (value.fullPathHash != prevStateNameHash)
			{
				prevStateNameHash = value.fullPathHash;
				if (myMinionToUpdate.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.ChatLeft01")))
				{
					globalAudioSignal.Dispatch("Play_minion_topiary_chatter_01");
				}
				else if (myMinionToUpdate.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.NodRight01")))
				{
					globalAudioSignal.Dispatch("Play_minion_topiary_nod_01");
				}
				else if (myMinionToUpdate.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.NodLeft02")))
				{
					globalAudioSignal.Dispatch("Play_minion_topiary_nod_01");
				}
			}
		}

		private void EveryoneCut()
		{
			trimSoundPlaying = true;
			trimEmitter.Play();
			ToolsWaitingForStateChangeQueue.Clear();
			foreach (global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject toolsWithMinions in ToolsWithMinionsList)
			{
				ToolsWaitingForStateChangeQueue.Add(toolsWithMinions);
			}
			StartCoroutine(DequeueCuttingMinions());
		}

		private global::System.Collections.IEnumerator DequeueCuttingMinions()
		{
			while (ToolsWaitingForStateChangeQueue.Count > 0)
			{
				global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject tool = ToolsWaitingForStateChangeQueue[global::UnityEngine.Random.Range(0, ToolsWaitingForStateChangeQueue.Count)];
				ToolsWaitingForStateChangeQueue.Remove(tool);
				tool.StartCutting();
				yield return new global::UnityEngine.WaitForSeconds(0.1f);
			}
			BuildingViewReference.ShakeAnimation.Play();
		}

		private void EveryoneCheer()
		{
			ResetTree();
			globalAudioSignal.Dispatch("Play_mignette_group_cheer");
			foreach (global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCuttingToolViewObject toolsWithMinions in ToolsWithMinionsList)
			{
				toolsWithMinions.StopCutting();
				toolsWithMinions.Cheer();
			}
		}

		private void FadeTrimSound()
		{
			if (trimSoundPlaying)
			{
				trimSoundPlaying = false;
				trimEmitter.Fade(1f, 0f, 1f);
			}
		}

		public void ResetTree()
		{
			FadeTrimSound();
			BuildingViewReference.ShakeAnimation.Stop();
			BuildingViewReference.DefaultBush.transform.rotation = global::UnityEngine.Quaternion.identity;
			BuildingViewReference.DefaultBush.transform.localScale = global::UnityEngine.Vector3.one;
		}

		public new void ResetCameraAndStopMignette(bool showScore)
		{
			BuildingViewReference.HasRequestedExit = !showScore;
			base.ResetCameraAndStopMignette(showScore);
		}

		private void ExitMignette()
		{
			requestStopMignetteSignal.Dispatch(true);
		}
	}
}
