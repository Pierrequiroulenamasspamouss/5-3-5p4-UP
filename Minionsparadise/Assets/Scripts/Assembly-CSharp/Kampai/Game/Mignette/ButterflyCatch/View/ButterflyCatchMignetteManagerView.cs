namespace Kampai.Game.Mignette.ButterflyCatch.View
{
	public class ButterflyCatchMignetteManagerView : global::Kampai.Game.Mignette.View.MignetteManagerView
	{
		public class MinionAndTimer
		{
			public global::Kampai.Game.View.MinionObject mo;

			public float timer;

			public float fallDownTimer;

			public float beeStingTimer;

			public ButterflyCatchMignetteNetViewObject netViewObject;
		}

		private const string SWING_ANIM_TRIGGER_NAME = "Swing";

		private const string BEE_CAUGHT_ANIM_FLOAT_NAME = "BeeStingTimer";

		private const string SWING_MISS_ANIM_FLOAT_NAME = "SwingAndMissTimer";

		private const string RUN_ANIM_TRIGGER_NAME = "Run";

		private const string SWING_ANIM_STATE_NAME = "Base Layer.Swing";

		private const string SUCCESS_ANIM_STATE_NAME = "Base Layer.Success";

		private const string RUN_ANIM_STATE_NAME = "Base Layer.Run";

		private const string IDLE_ANIM_STATE_NAME = "Base Layer.Idle";

		private global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchBuildingViewObject BuildingViewReference;

		private float butterflySpawnTimer;

		private bool minionsIdle = true;

		public int totalScore;

		private global::System.Collections.Generic.List<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer> activeMinionsList = new global::System.Collections.Generic.List<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer>();

		private global::System.Collections.Generic.List<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject> activeButterflies = new global::System.Collections.Generic.List<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject>();

		private global::System.Collections.Generic.List<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject> activeBees = new global::System.Collections.Generic.List<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject>();

		private global::Kampai.Game.View.TaskingMinionObject ptmo;

		private global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchGameController gameController;

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.ChangeMignetteScoreSignal changeScoreSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnMignetteDooberSignal spawnDooberSignal { get; set; }

		[Inject]
		public global::Kampai.Common.MinionReactInRadiusSignal minionReactInRadiusSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.RequestStopMignetteSignal requestStopMignetteSignal { get; set; }

		protected override void Start()
		{
			base.Start();
			BuildingViewReference = MignetteBuildingObject.GetComponent<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchBuildingViewObject>();
			gameController = base.transform.parent.GetComponentInChildren<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchGameController>();
			butterflySpawnTimer = 2f;
			TimeElapsed = 0f;
			minionsIdle = true;
			TotalEventTime = 45f;
			int num = 0;
			for (num = 0; num < MignetteBuildingObject.GetMignetteMinionCount(); num++)
			{
				global::Kampai.Game.View.TaskingMinionObject childMinion = MignetteBuildingObject.GetChildMinion(num);
				AddColliderToMinion(childMinion);
				childMinion.Minion.EnableBlobShadow(true);
				global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer minionAndTimer = new global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer();
				minionAndTimer.mo = childMinion.Minion;
				minionAndTimer.timer = 0f;
				GoSpline minionSpline = gameController.GetMinionSpline(num);
				global::UnityEngine.Vector3 pointOnPath = minionSpline.getPointOnPath(0f);
				minionAndTimer.mo.transform.position = pointOnPath;
				global::UnityEngine.Vector3 vector = gameController.CameraTransform.position - minionAndTimer.mo.transform.position;
				vector.y = 0f;
				minionAndTimer.mo.transform.forward = vector.normalized;
				global::UnityEngine.GameObject gameObject = gameController.SpawnNet();
				minionAndTimer.netViewObject = gameObject.GetComponent<ButterflyCatchMignetteNetViewObject>();
				minionAndTimer.netViewObject.transform.parent = base.transform;
				minionAndTimer.netViewObject.transform.position = minionAndTimer.mo.transform.position;
				minionAndTimer.netViewObject.transform.rotation = minionAndTimer.mo.transform.rotation;
				activeMinionsList.Add(minionAndTimer);
			}
			BuildingViewReference.ToggleAmbientButterflies(false);
			RelocateCameraForMignette(gameController.CameraTransform, gameController.CameraFieldOfView, gameController.CameraNearClipPlane, 1f);
		}

		private void AddColliderToMinion(global::Kampai.Game.View.TaskingMinionObject tmo)
		{
			global::UnityEngine.Collider component = tmo.Minion.GetComponent<global::UnityEngine.Collider>();
			if (component != null)
			{
				component.enabled = true;
			}
			else
			{
				tmo.Minion.gameObject.AddComponent<global::UnityEngine.CapsuleCollider>();
			}
			ptmo = tmo;
		}

		public void OnInputDown(global::UnityEngine.Vector3 inputPosition)
		{
			if (TimeElapsed >= 45f)
			{
				return;
			}
			global::UnityEngine.Ray ray = base.mignetteCamera.ScreenPointToRay(inputPosition);
			global::UnityEngine.RaycastHit hitInfo;
			if (!global::UnityEngine.Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 8192))
			{
				return;
			}
			global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer minionObjectForCollider = GetMinionObjectForCollider(hitInfo.collider);
			bool flag = false;
			if (minionObjectForCollider.mo != null)
			{
				if (minionObjectForCollider.mo.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.Run")) || minionObjectForCollider.mo.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.Success")))
				{
					flag = true;
				}
				if (flag)
				{
					StartCoroutine(CollectFliersAroundMinion(minionObjectForCollider));
				}
			}
		}

		private global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer GetMinionObjectForCollider(global::UnityEngine.Collider collider)
		{
			int num = 0;
			for (num = 0; num < activeMinionsList.Count; num++)
			{
				global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer minionAndTimer = activeMinionsList[num];
				if (collider.gameObject == minionAndTimer.netViewObject.gameObject)
				{
					return minionAndTimer;
				}
			}
			return null;
		}

		private global::System.Collections.IEnumerator CollectFliersAroundMinion(global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer mat)
		{
			global::Kampai.Game.View.MinionObject mo = mat.mo;
			mat.netViewObject.StartSwipe();
			mo.SetAnimTrigger("Swing");
			mat.netViewObject.NetAnimator.SetTrigger("Swing");
			yield return new global::UnityEngine.WaitForSeconds(mat.netViewObject.CatchDelay);
			int index = 0;
			for (index = 0; index < activeMinionsList.Count; index++)
			{
				global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer mat2 = activeMinionsList[index];
				if (mat2 != mat)
				{
					global::UnityEngine.Vector3 minion2AtMinionY = mat2.mo.transform.position;
					minion2AtMinionY.y = mo.transform.position.y;
					float dist = global::UnityEngine.Vector3.Distance(mo.transform.position, minion2AtMinionY);
					if (dist <= 2f && mat2.beeStingTimer <= 0f)
					{
						mat2.fallDownTimer = 1.5f;
						globalAudioSignal.Dispatch("Play_balloon_pop_01");
					}
				}
			}
			global::System.Collections.Generic.List<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject> caughtFliers = new global::System.Collections.Generic.List<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject>();
			bool beeCaught = false;
			foreach (global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject bvo in activeBees)
			{
				if (!bvo.IsGettingCaught)
				{
					global::UnityEngine.Vector3 butterflyAtMinionY = bvo.transform.position;
					butterflyAtMinionY.y = mo.transform.position.y;
					float dist2 = global::UnityEngine.Vector3.Distance(mo.transform.position, butterflyAtMinionY);
					if (dist2 <= 2f && !beeCaught)
					{
						bvo.StingMinion(mo);
						caughtFliers.Add(bvo);
						bvo.IsGettingCaught = true;
						beeCaught = true;
					}
				}
			}
			int butterfliesCaught = 0;
			foreach (global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject bvo2 in activeButterflies)
			{
				if (!bvo2.IsGettingCaught)
				{
					global::UnityEngine.Vector3 butterflyAtMinionY2 = bvo2.transform.position;
					butterflyAtMinionY2.y = mo.transform.position.y;
					float dist3 = global::UnityEngine.Vector3.Distance(mo.transform.position, butterflyAtMinionY2);
					if (dist3 <= 2f)
					{
						caughtFliers.Add(bvo2);
						bvo2.IsGettingCaught = true;
						butterfliesCaught++;
					}
				}
			}
			if (beeCaught)
			{
				mat.beeStingTimer = 3f;
			}
			else if (caughtFliers.Count > 0)
			{
				minionReactInRadiusSignal.Dispatch(15f, mo.transform.position);
			}
			else
			{
				mat.fallDownTimer = 1.5f;
				globalAudioSignal.Dispatch("Play_balloon_pop_01");
			}
			int addedScore = 0;
			if (butterfliesCaught >= 4)
			{
				globalAudioSignal.Dispatch("Play_minon_butterfly_celebrate_01");
				addedScore += 15;
			}
			foreach (global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject bvo3 in caughtFliers)
			{
				if (bvo3.IsReallyABee)
				{
					addedScore += -1;
					continue;
				}
				addedScore += bvo3.myScore;
				global::UnityEngine.GameObject vfx = global::UnityEngine.Object.Instantiate(gameController.ButterflyCaughtVfxPrefab);
				vfx.transform.SetParent(base.transform, false);
				vfx.transform.position = bvo3.transform.position;
				global::UnityEngine.Object.Destroy(vfx, 5f);
			}
			foreach (global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject bvo4 in caughtFliers)
			{
				RemoveMeFromLists(bvo4);
				if (!bvo4.IsReallyABee)
				{
					global::UnityEngine.Object.Destroy(bvo4.gameObject);
				}
			}
			if (addedScore > 0)
			{
				spawnDooberSignal.Dispatch(mignetteHUD, mo.transform.position, addedScore, true);
				changeScoreSignal.Dispatch(addedScore);
				globalAudioSignal.Dispatch("Play_mignette_collect");
				totalScore += addedScore;
			}
			else if (addedScore < 0)
			{
				if (totalScore + addedScore >= 0)
				{
					changeScoreSignal.Dispatch(addedScore);
					totalScore += addedScore;
				}
				else
				{
					changeScoreSignal.Dispatch(-totalScore);
					totalScore = 0;
				}
			}
		}

		public void CleanupMignette()
		{
			if (ptmo != null)
			{
				global::UnityEngine.Object.Destroy(ptmo.Minion.gameObject.GetComponent<global::UnityEngine.Collider>());
			}
			BuildingViewReference.ToggleAmbientButterflies(playerService.HasPurchasedMinigamePack());
		}

		public override void Update()
		{
			base.Update();
			if (base.IsPaused || TimeElapsed >= 45f)
			{
				return;
			}
			butterflySpawnTimer -= global::UnityEngine.Time.deltaTime;
			if (butterflySpawnTimer <= 0f)
			{
				butterflySpawnTimer = 2f;
				for (int i = 0; i < gameController.GetButterflySplineCount(); i++)
				{
					SpawnButterflyOrBee(gameController.GetButterflySpline(i), 1.1f);
				}
			}
			if (countdownTimer > 0f)
			{
				return;
			}
			if (minionsIdle)
			{
				minionsIdle = false;
				int num = 0;
				for (num = 0; num < activeMinionsList.Count; num++)
				{
					global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer minionAndTimer = activeMinionsList[num];
					minionAndTimer.mo.SetAnimTrigger("Run");
					minionAndTimer.netViewObject.NetAnimator.SetTrigger("Run");
				}
			}
			UpdateMinionLocations();
			TimeElapsed += global::UnityEngine.Time.deltaTime;
			if (TimeElapsed >= 45f)
			{
				TimeElapsed = 45f;
				int num2 = 0;
				for (num2 = 0; num2 < activeMinionsList.Count; num2++)
				{
					global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer minionAndTimer2 = activeMinionsList[num2];
					minionAndTimer2.mo.PlayAnimation(global::UnityEngine.Animator.StringToHash("Base Layer.Idle"), 0, 0f);
					minionAndTimer2.netViewObject.NetAnimator.Play(global::UnityEngine.Animator.StringToHash("Base Layer.Idle"), 0, 0f);
				}
				Invoke("ShutDownMignette", 2f);
			}
			for (int j = 0; j < activeMinionsList.Count; j++)
			{
				global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer minionAndTimer3 = activeMinionsList[j];
				minionAndTimer3.mo.SetAnimFloat("SwingAndMissTimer", minionAndTimer3.fallDownTimer);
				minionAndTimer3.netViewObject.NetAnimator.SetFloat("SwingAndMissTimer", minionAndTimer3.fallDownTimer);
				if (minionAndTimer3.fallDownTimer > 0f)
				{
					minionAndTimer3.fallDownTimer -= global::UnityEngine.Time.deltaTime;
				}
				minionAndTimer3.mo.SetAnimFloat("BeeStingTimer", minionAndTimer3.beeStingTimer);
				minionAndTimer3.netViewObject.NetAnimator.SetFloat("BeeStingTimer", minionAndTimer3.beeStingTimer);
				if (minionAndTimer3.beeStingTimer > 0f)
				{
					minionAndTimer3.beeStingTimer -= global::UnityEngine.Time.deltaTime;
					global::UnityEngine.Vector3 vector = base.mignetteCamera.transform.position - minionAndTimer3.mo.transform.position;
					vector.y = 0f;
					minionAndTimer3.mo.transform.forward = vector.normalized;
					minionAndTimer3.netViewObject.transform.forward = vector.normalized;
				}
			}
		}

		private void SpawnButterflyOrBee(GoSpline path, float speed)
		{
			global::UnityEngine.GameObject gameObject = gameController.SpawnBee(activeBees.Count, TimeElapsed);
			if (gameObject == null)
			{
				gameObject = gameController.SpawnButterfly(activeButterflies.Count, TimeElapsed);
			}
			if (gameObject != null)
			{
				gameObject.transform.parent = base.transform;
				global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject component = gameObject.GetComponent<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject>();
				component.FollowPath(this, path, speed);
				if (component.IsReallyABee)
				{
					activeBees.Add(component);
				}
				else
				{
					activeButterflies.Add(component);
				}
			}
		}

		private void UpdateMinionLocations()
		{
			int num = 0;
			for (num = 0; num < activeMinionsList.Count; num++)
			{
				global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView.MinionAndTimer minionAndTimer = activeMinionsList[num];
				global::Kampai.Game.View.MinionObject mo = minionAndTimer.mo;
				bool flag = false;
				if (mo.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.Run")) || mo.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.Success")))
				{
					flag = true;
				}
				if (flag)
				{
					minionAndTimer.timer += global::UnityEngine.Time.deltaTime * 1.5f;
					GoSpline minionSpline = gameController.GetMinionSpline(num);
					if (minionAndTimer.timer >= minionSpline.pathLength)
					{
						minionAndTimer.timer -= minionSpline.pathLength;
					}
					float t = minionAndTimer.timer / minionSpline.pathLength;
					global::UnityEngine.Vector3 pointOnPath = minionSpline.getPointOnPath(t);
					global::UnityEngine.Vector3 vector = pointOnPath - mo.transform.position;
					mo.transform.position = pointOnPath;
					mo.transform.rotation = global::UnityEngine.Quaternion.Slerp(mo.transform.rotation, global::UnityEngine.Quaternion.LookRotation(vector.normalized), global::UnityEngine.Time.deltaTime * 10f);
					minionAndTimer.netViewObject.transform.position = mo.transform.position;
					minionAndTimer.netViewObject.transform.rotation = mo.transform.rotation;
				}
			}
		}

		public void ShutDownMignette()
		{
			requestStopMignetteSignal.Dispatch(true);
		}

		public void RemoveMeFromLists(global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject butterflyOrBee)
		{
			if (activeBees.Contains(butterflyOrBee))
			{
				activeBees.Remove(butterflyOrBee);
			}
			if (activeButterflies.Contains(butterflyOrBee))
			{
				activeButterflies.Remove(butterflyOrBee);
			}
		}
	}
}
