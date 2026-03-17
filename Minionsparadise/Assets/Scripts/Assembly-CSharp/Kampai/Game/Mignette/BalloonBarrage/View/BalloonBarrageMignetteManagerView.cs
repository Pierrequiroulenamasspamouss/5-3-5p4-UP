namespace Kampai.Game.Mignette.BalloonBarrage.View
{
	public class BalloonBarrageMignetteManagerView : global::Kampai.Game.Mignette.View.MignetteManagerView
	{
		private const string GROUND_IDLE_ANIM_STATE = "Base Layer.IdleGround";

		private const string PILOT_TAKE_MANGO_ANIM_STATE = "Base Layer.PilotTakeMango";

		private const string BASKET_PILOT_WALK_ANIM_STATE = "Base Layer.IntroMinionPilot";

		private const string BASKET_COPILOT_WALK_ANIM_STATE = "Base Layer.IntroMinionCoPilot";

		private const string PILOT_IDLE_ANIM_STATE = "Base Layer.PilotIdle";

		private const string PILOT_IDLE_MANGO_ANIM_STATE = "Base Layer.PilotIdleWithMango";

		private const string PILOT_THROW_RELEASE_ANIM_STATE = "Base Layer.PilotThrowMangoB";

		private const string COPILOT_WAIT_ANIM_STATE = "Base Layer.CopilotWaiting";

		private const string COPILOT_IDLE_RETURN_ANIM_STATE = "Base Layer.Minion2MangoGrabC";

		private const string GROUND_CATCH_ANIM_STATE = "Base Layer.CatchGround";

		private const string CATCH_ANIM_TRIGGER_NAME = "OnMinionCatch";

		private const string TRANSFER_ANIM_TRIGGER_NAME = "OnTransferMango";

		private const string TRANSFER_COMPLETE_ANIM_TRIGGER_NAME = "OnTransferMangoComplete";

		private const string WAVE_GROUND_ANIM_TRIGGER_NAME = "OnWaveGround";

		private const string THROW_ANIM_TRIGGER_NAME = "OnThrow";

		private const string INTRO_ANIM_TRIGGER_NAME = "OnIntro";

		private const string OUTRO_ANIM_TRIGGER_NAME = "OnOutro";

		private global::Kampai.Game.View.MinionObject pilotMinion;

		private global::Kampai.Game.View.MinionObject coPilotMinion;

		private global::UnityEngine.Transform pilotThrowingHand;

		private bool isInPilotHand;

		private bool isThrowing;

		private global::UnityEngine.Transform coPilotThrowingHand;

		private global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageBuildingViewObject BuildingViewReference;

		private global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageGameController gameController;

		private bool introComplete;

		private bool forceCameraToFollowCameraLocator;

		private bool pilotShadowsEnabled;

		private bool balloonIsLanding;

		private global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject floatingMinionTarget;

		private global::System.Collections.Generic.List<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject> staticTargetsRemaining = new global::System.Collections.Generic.List<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject>();

		private int minionsUsedSoFar;

		private bool mangoInFlight;

		private global::UnityEngine.Vector3 dragStartPoint = global::UnityEngine.Vector3.zero;

		private global::UnityEngine.Vector3 throwInputVector = global::UnityEngine.Vector3.zero;

		private float groundMinionWaveTimer;

		private global::UnityEngine.Vector3 lastInputPos;

		private global::UnityEngine.Vector3 throwTarget;

		private global::UnityEngine.Vector3 mangoThrowStart;

		private float throwForce;

		private global::UnityEngine.Vector3 lastgroundPos;

		private global::System.Collections.Generic.List<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject> groundMinionsHit = new global::System.Collections.Generic.List<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject>();

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.ChangeMignetteScoreSignal changeScoreSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnMignetteDooberSignal spawnDooberSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayLocalAudioSignal localAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.StopLocalAudioSignal stopLocalAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RequestStopMignetteSignal requestStopMignetteSignal { get; set; }

		protected override void Start()
		{
			base.Start();
			BuildingViewReference = MignetteBuildingObject.GetComponent<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageBuildingViewObject>();
			gameController = base.gameObject.GetComponentInChildren<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageGameController>();
			BuildingViewReference.BalloonIsTakingOff = false;
			RelocateCameraForMignette(BuildingViewReference.CameraTransform, BuildingViewReference.FieldOfView, BuildingViewReference.NearClipPlane, 1f);
			pilotShadowsEnabled = true;
			TimeElapsed = 0f;
			TotalEventTime = 45f;
			minionsUsedSoFar = 0;
			pilotMinion = MignetteBuildingObject.GetChildMinion(minionsUsedSoFar).Minion;
			pilotThrowingHand = pilotMinion.gameObject.FindChild("minion:R_wrist_jnt").transform;
			minionsUsedSoFar++;
			pilotMinion.EnableRenderers(false);
			coPilotMinion = MignetteBuildingObject.GetChildMinion(minionsUsedSoFar).Minion;
			coPilotThrowingHand = coPilotMinion.gameObject.FindChild("minion:R_wrist_jnt").transform;
			minionsUsedSoFar++;
			coPilotMinion.EnableRenderers(false);
			SetupFloatingTarget();
			SetupStaticTargets();
			isInPilotHand = false;
			balloonIsLanding = false;
			groundMinionWaveTimer = 2f;
		}

		private void SetupFloatingTarget()
		{
			global::Kampai.Game.View.TaskingMinionObject childMinion = MignetteBuildingObject.GetChildMinion(minionsUsedSoFar);
			childMinion.Minion.EnableBlobShadow(false);
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(BuildingViewReference.BasketPrefab);
			gameObject.transform.parent = base.transform;
			global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject component = gameObject.GetComponent<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject>();
			component.Score = 25;
			component.GetComponent<global::UnityEngine.Collider>().enabled = false;
			component.ShowMango(false);
			component.AddTarget(2);
			global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject[] componentsInChildren = component.GetComponentsInChildren<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject>();
			global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject[] array = componentsInChildren;
			foreach (global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject balloonBarrageColliderViewObject in array)
			{
				balloonBarrageColliderViewObject.ParentTargetBalloonViewObject = component;
			}
			component.transform.position = gameController.FloatingMinionLocator.position;
			component.transform.forward = gameController.FloatingMinionLocator.forward;
			component.MinionToAnimate = childMinion.Minion;
			component.MyParentBuildingViewObject = BuildingViewReference;
			component.MyParentGameControllerObject = gameController;
			component.StartFloating(BuildingViewReference.transform.GetComponent<global::UnityEngine.Collider>().bounds.min.z, BuildingViewReference.transform.GetComponent<global::UnityEngine.Collider>().bounds.max.z, true, 1.5f);
			component.HoldPosition = true;
			component.ShowModel(false);
			floatingMinionTarget = component;
			minionsUsedSoFar++;
		}

		private void SetupStaticTargets()
		{
			groundMinionsHit.Clear();
			staticTargetsRemaining.Clear();
			for (int i = 0; i < gameController.MinionStaticTargetLocators.Length; i++)
			{
				global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageGameController.StaticBasketAndPoints staticBasketAndPoints = gameController.MinionStaticTargetLocators[i];
				global::UnityEngine.GameObject[] basketLocators = staticBasketAndPoints.BasketLocators;
				foreach (global::UnityEngine.GameObject gameObject in basketLocators)
				{
					if (MignetteBuildingObject.GetMignetteMinionCount() > minionsUsedSoFar)
					{
						global::Kampai.Game.View.TaskingMinionObject childMinion = MignetteBuildingObject.GetChildMinion(minionsUsedSoFar);
						childMinion.Minion.EnableBlobShadow(false);
						minionsUsedSoFar++;
						global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(BuildingViewReference.BasketPrefab);
						gameObject2.transform.parent = base.transform;
						gameObject2.transform.position = gameObject.transform.position;
						global::UnityEngine.Vector3 vector = BuildingViewReference.CameraTransform.position - gameObject2.transform.position;
						vector.y = 0f;
						gameObject2.transform.forward = vector.normalized;
						global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject[] componentsInChildren = gameObject2.GetComponentsInChildren<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject>();
						global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject[] array = componentsInChildren;
						foreach (global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject balloonBarrageColliderViewObject in array)
						{
							balloonBarrageColliderViewObject.gameObject.SetActive(false);
						}
						global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject component = gameObject2.GetComponent<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject>();
						component.Score = staticBasketAndPoints.ScoreValue;
						component.MinionToAnimate = childMinion.Minion;
						component.SyncAnimationStates("Base Layer.IdleGround");
						staticTargetsRemaining.Add(component);
						component.ShowMango(false);
						component.AddTarget(staticBasketAndPoints.BasketMaterialIndex);
					}
				}
			}
		}

		protected override void CameraTransitionComplete()
		{
			if (!introComplete)
			{
				introComplete = true;
				MignetteBuildingObject.GetComponent<global::UnityEngine.Animator>().SetTrigger("OnIntro");
				pilotMinion.PlayAnimation(global::UnityEngine.Animator.StringToHash("Base Layer.IntroMinionPilot"), 0, 0f);
				coPilotMinion.PlayAnimation(global::UnityEngine.Animator.StringToHash("Base Layer.IntroMinionCoPilot"), 0, 0f);
				forceCameraToFollowCameraLocator = true;
				Invoke("EnablePilots", 0.1f);
			}
		}

		public void EnablePilots()
		{
			pilotMinion.EnableRenderers(true);
			coPilotMinion.EnableRenderers(true);
		}

		public void ResetMignetteObjects()
		{
			MignetteBuildingObject.GetComponent<global::UnityEngine.Animator>().SetTrigger("OnOutro");
			balloonIsLanding = true;
			BuildingViewReference.UpdateCooldownView(localAudioSignal, 0, 0f);
		}

		public bool MangoHitMovingTarget(global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageMangoViewObject mango, global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject tvo)
		{
			if (tvo.ParentTargetBalloonViewObject == null || !tvo.ParentTargetBalloonViewObject.CanBeHit())
			{
				return false;
			}
			switch (tvo.TargetType)
			{
			case global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject.TargetTypes.Basket:
			{
				globalAudioSignal.Dispatch("Play_minion_balloon_catchMango_01");
				globalAudioSignal.Dispatch("Play_mignette_collect");
				tvo.ParentTargetBalloonViewObject.SetAnimTriggers("OnMinionCatch");
				changeScoreSignal.Dispatch(tvo.ParentTargetBalloonViewObject.Score);
				tvo.ParentTargetBalloonViewObject.ShowMango(true);
				spawnDooberSignal.Dispatch(mignetteHUD, mango.transform.position, tvo.ParentTargetBalloonViewObject.Score, true);
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(BuildingViewReference.MangoCaughtVfxPrefab);
				gameObject.transform.SetParent(base.transform, false);
				global::UnityEngine.Vector3 position = mango.transform.position;
				gameObject.transform.position = position;
				global::UnityEngine.Object.Destroy(gameObject, 5f);
				break;
			}
			case global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject.TargetTypes.Minion:
				globalAudioSignal.Dispatch("Play_minion_balloon_hitFace_01");
				tvo.ParentTargetBalloonViewObject.MinionWasHit();
				break;
			case global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject.TargetTypes.Balloon:
				globalAudioSignal.Dispatch("Play_balloon_pop_01");
				tvo.ParentTargetBalloonViewObject.PlayFallAnimation(mango.transform.position);
				break;
			}
			return true;
		}

		public void MangoHasBeenResolved(bool hitGround)
		{
			mangoInFlight = false;
			if (hitGround && !isThrowing && !isInPilotHand)
			{
				globalAudioSignal.Dispatch("Play_mango_splat_01");
			}
		}

		public void MangoHitStaticTarget(global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageMangoViewObject mango, global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject avo)
		{
			avo.SetAnimTriggers("OnMinionCatch");
			avo.GetComponent<global::UnityEngine.Collider>().enabled = false;
			avo.ShowMango(true);
			globalAudioSignal.Dispatch("Play_mignette_collect");
			globalAudioSignal.Dispatch("Play_minion_balloon_catchMango_01");
			groundMinionsHit.Add(avo);
			staticTargetsRemaining.Remove(avo);
			if (staticTargetsRemaining.Count <= 0)
			{
				floatingMinionTarget.ShowModel(true);
				floatingMinionTarget.HoldPosition = false;
			}
			changeScoreSignal.Dispatch(avo.Score);
			spawnDooberSignal.Dispatch(mignetteHUD, mango.transform.position, avo.Score, true);
		}

		public override void LateUpdate()
		{
			base.LateUpdate();
			if (!base.IsPaused)
			{
				UpdatePilotPosition();
				if (pilotMinion.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.PilotThrowMangoB")) && isThrowing)
				{
					ThrowAMango();
				}
			}
		}

		private void UpdatePilotPosition()
		{
			if (!shutdownInProgress)
			{
				if (pilotShadowsEnabled && BuildingViewReference.BalloonIsTakingOff)
				{
					pilotShadowsEnabled = false;
					pilotMinion.EnableBlobShadow(false);
					coPilotMinion.EnableBlobShadow(false);
				}
				global::UnityEngine.Vector3 position = BuildingViewReference.BalloonPilotIntroLocator.position;
				global::UnityEngine.Quaternion rotation = BuildingViewReference.BalloonPilotIntroLocator.rotation;
				pilotMinion.transform.position = position;
				pilotMinion.transform.rotation = rotation;
				coPilotMinion.transform.position = position;
				coPilotMinion.transform.rotation = rotation;
				global::UnityEngine.GameObject mangoToShowForPrepareThrow = GetMangoToShowForPrepareThrow();
				if (isInPilotHand)
				{
					mangoToShowForPrepareThrow.transform.position = pilotThrowingHand.position - pilotThrowingHand.up * 0.1f;
					mangoToShowForPrepareThrow.transform.rotation = pilotThrowingHand.rotation;
				}
				else
				{
					mangoToShowForPrepareThrow.transform.position = coPilotThrowingHand.position - coPilotThrowingHand.up * 0.1f;
					mangoToShowForPrepareThrow.transform.rotation = coPilotThrowingHand.rotation;
				}
			}
		}

		public override void Update()
		{
			base.Update();
			if (base.IsPaused)
			{
				return;
			}
			if (forceCameraToFollowCameraLocator)
			{
				base.mignetteCamera.transform.position = BuildingViewReference.CameraTransform.position;
				base.mignetteCamera.transform.rotation = BuildingViewReference.CameraTransform.rotation;
			}
			if (TimeElapsed >= TotalEventTime || shutdownInProgress || balloonIsLanding)
			{
				return;
			}
			if (introComplete)
			{
				if (pilotMinion.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.PilotTakeMango")) && coPilotMinion.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.Minion2MangoGrabC")))
				{
					isInPilotHand = true;
					TriggerForBothInBasket("OnTransferMangoComplete");
				}
				if (coPilotMinion.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.CopilotWaiting")))
				{
					GetMangoToShowForPrepareThrow().SetActive(true);
					if (pilotMinion.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.PilotIdle")))
					{
						TriggerForBothInBasket("OnTransferMango");
					}
				}
			}
			if (countdownTimer > 0f)
			{
				return;
			}
			if (staticTargetsRemaining.Count > 0)
			{
				groundMinionWaveTimer -= global::UnityEngine.Time.deltaTime;
				if (groundMinionWaveTimer <= 0f)
				{
					global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject balloonBarrageTargetAnimatorViewObject = staticTargetsRemaining[global::UnityEngine.Random.Range(0, staticTargetsRemaining.Count)];
					balloonBarrageTargetAnimatorViewObject.SetAnimTriggers("OnWaveGround");
					groundMinionWaveTimer = global::UnityEngine.Random.Range(2f, 10f);
				}
			}
			TimeElapsed += global::UnityEngine.Time.deltaTime;
			if (TimeElapsed >= TotalEventTime)
			{
				TimeElapsed = 45f;
				Invoke("ShutDownMignette", 2f);
			}
			UpdateGroundMinionLocations();
		}

		private void UpdateGroundMinionLocations()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject> list = new global::System.Collections.Generic.List<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject>();
			foreach (global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject item in groundMinionsHit)
			{
				if (item.IsInState("Base Layer.CatchGround"))
				{
					global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject thisAvo = item;
					list.Add(thisAvo);
					global::UnityEngine.Vector3 endValue = thisAvo.transform.position + thisAvo.transform.forward * 10f;
					Go.to(thisAvo.gameObject.transform, 3f, new GoTweenConfig().setEaseType(GoEaseType.SineIn).position(endValue).setDelay(thisAvo.WalkoffAnimDelay)
						.onComplete(delegate(AbstractGoTween thisTween)
						{
							thisAvo.ShowModel(false);
							thisTween.destroy();
						}));
				}
			}
			foreach (global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject item2 in list)
			{
				groundMinionsHit.Remove(item2);
			}
		}

		public void OnPress(global::UnityEngine.Vector3 pos, bool pressed)
		{
			if (!pilotMinion.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.PilotIdleWithMango")) || TimeElapsed >= TotalEventTime || isThrowing || countdownTimer > 0f)
			{
				return;
			}
			if (pressed)
			{
				global::UnityEngine.Ray ray = base.mignetteCamera.ScreenPointToRay(pos);
				global::UnityEngine.RaycastHit hitInfo;
				if (global::UnityEngine.Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 8192))
				{
					dragStartPoint = pos;
				}
			}
			else
			{
				if (!(dragStartPoint != global::UnityEngine.Vector3.zero))
				{
					return;
				}
				if (throwInputVector != global::UnityEngine.Vector3.zero)
				{
					isThrowing = true;
					TriggerForBothInBasket("OnThrow");
					lastgroundPos = gameContext.injectionBinder.GetInstance<global::Kampai.Game.View.CameraUtils>().GroundPlaneRaycast(lastInputPos);
					mangoThrowStart = base.mignetteCamera.transform.position;
					throwTarget = lastgroundPos;
					throwTarget.y = mangoThrowStart.y;
					global::UnityEngine.Vector3 throwVector = throwTarget - mangoThrowStart;
					throwForce = CalculateForceForInput(throwVector);
					float num = global::UnityEngine.Vector3.Dot(pilotMinion.transform.forward, throwVector.normalized);
					num = 1f - num;
					if (global::UnityEngine.Vector3.Dot(pilotMinion.transform.right, throwVector.normalized) < 0f)
					{
						num *= -1f;
					}
					float state = MapInterval(num, -0.5f, 0.5f, -1f, 1f);
					pilotMinion.SetAnimFloat("Direction", state);
				}
				dragStartPoint = global::UnityEngine.Vector3.zero;
			}
		}

		public void OnPressed(global::UnityEngine.Vector3 pos)
		{
			if (TimeElapsed >= TotalEventTime || isThrowing || countdownTimer > 0f)
			{
				return;
			}
			throwInputVector = pos - dragStartPoint;
			bool flag = true;
			if (throwInputVector.magnitude < 2f)
			{
				flag = false;
			}
			switch (BuildingViewReference.BalloonBarrageThrowType)
			{
			case global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageBuildingViewObject.BalloonBarrageThrowTypes.Pull:
				if (throwInputVector.y > 0f)
				{
					flag = false;
				}
				break;
			case global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageBuildingViewObject.BalloonBarrageThrowTypes.Push:
				if (throwInputVector.y < 0f)
				{
					flag = false;
				}
				break;
			}
			if (mangoInFlight || !pilotMinion.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.PilotIdleWithMango")))
			{
				flag = false;
			}
			if (!flag)
			{
				throwInputVector = global::UnityEngine.Vector3.zero;
			}
			else
			{
				lastInputPos = pos;
			}
		}

		private float MapInterval(float val, float srcMin, float srcMax, float dstMin, float dstMax)
		{
			if (val >= srcMax)
			{
				return dstMax;
			}
			if (val <= srcMin)
			{
				return dstMin;
			}
			return dstMin + (val - srcMin) / (srcMax - srcMin) * (dstMax - dstMin);
		}

		public float CalculateForceForInput(global::UnityEngine.Vector3 throwVector)
		{
			return MapInterval(throwVector.magnitude, 2f, 8f, 2000f, 7000f);
		}

		public void ThrowAMango()
		{
			isThrowing = false;
			if (!mangoInFlight)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(BuildingViewReference.MangoPrefab);
				gameObject.transform.parent = base.transform;
				global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageMangoViewObject component = gameObject.GetComponent<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageMangoViewObject>();
				isInPilotHand = false;
				component.ThrowMango(this, GetMangoToShowForPrepareThrow(), lastgroundPos, throwForce);
				mangoInFlight = true;
			}
		}

		public void TriggerForBothInBasket(string triggerName)
		{
			pilotMinion.SetAnimTrigger(triggerName);
			coPilotMinion.SetAnimTrigger(triggerName);
		}

		public void ShutDownMignette()
		{
			forceCameraToFollowCameraLocator = false;
			requestStopMignetteSignal.Dispatch(true);
		}

		public void SilencePilots()
		{
			stopLocalAudioSignal.Dispatch(coPilotMinion.gameObject.GetComponent<CustomFMOD_StudioEventEmitter>());
			stopLocalAudioSignal.Dispatch(pilotMinion.gameObject.GetComponent<CustomFMOD_StudioEventEmitter>());
		}

		public global::UnityEngine.GameObject GetMangoToShowForPrepareThrow()
		{
			return gameController.MangoToShowForPrepareThrow;
		}
	}
}
