namespace Kampai.Game.Mignette.BalloonBarrage.View
{
	public class BalloonBarrageTargetAnimatorViewObject : global::UnityEngine.MonoBehaviour
	{
		public enum MinionAndBalloonStates
		{
			None = 0,
			Floating = 1,
			Falling = 2,
			WalkingOff = 3
		}

		private const string WALK_OFF_ANIM_STATE = "Base Layer.WalkOff";

		private const string IDLE_ANIM_STATE = "Base Layer.Idle";

		private const string WAVE_ANIM_TRIGGER_NAME = "OnWave";

		private const string FLYING_ANIM_TRIGGER_NAME = "OnFlying";

		private const string HIT_ANIM_TRIGGER_NAME = "OnMinionHit";

		private const string FALLING_ANIM_TRIGGER_NAME = "OnFalling";

		private const string BOUNCE_ANIM_TRIGGER_NAME = "OnBounce";

		private const float WAVE_TIMER_MIN = 3f;

		private const float WAVE_TIMER_MAX = 10f;

		public global::Kampai.Game.View.MinionObject MinionToAnimate;

		public global::UnityEngine.Animator BalloonAnimator;

		public global::UnityEngine.SkinnedMeshRenderer BasketRenderer;

		public int Score = 1;

		public global::UnityEngine.GameObject[] Mangoes;

		public global::UnityEngine.GameObject BasketTarget;

		public global::UnityEngine.Renderer BasketTargetRenderer;

		public global::UnityEngine.Material[] TargetMaterials;

		public float WalkoffAnimDelay = 3f;

		public float WalkoffZValue = 4f;

		public global::UnityEngine.GameObject BlobShadow;

		private bool isAFlyer;

		private global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject.MinionAndBalloonStates MinionAndBalloonState;

		private float zMinValue;

		private float zMaxValue;

		private bool directionLeft;

		private float targetSpeed = 10f;

		private float currentSpeed = 10f;

		private float initialY;

		private float velocityY;

		private bool needsReset;

		private global::UnityEngine.GameObject mangoSplatGameObject;

		private float splatTimer;

		private float waveTimer;

		public global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageBuildingViewObject MyParentBuildingViewObject;

		public global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageGameController MyParentGameControllerObject;

		public bool HoldPosition = true;

		private void Start()
		{
			initialY = base.transform.position.y;
			velocityY = 3f;
			waveTimer = global::UnityEngine.Random.Range(3f, 10f);
		}

		public void SyncAnimationStates(string stateName)
		{
			BalloonAnimator.Play(global::UnityEngine.Animator.StringToHash(stateName));
			MinionToAnimate.PlayAnimation(global::UnityEngine.Animator.StringToHash(stateName), 0, 0f);
		}

		public void SetAnimTriggers(string newTrigger)
		{
			MinionToAnimate.SetAnimTrigger(newTrigger);
			BalloonAnimator.SetTrigger(newTrigger);
		}

		public bool IsInState(string stateName)
		{
			return MinionToAnimate.IsInAnimatorState(global::UnityEngine.Animator.StringToHash(stateName));
		}

		public void AddTarget(int materialIndex)
		{
			BasketTarget.SetActive(true);
			BasketTargetRenderer.material = TargetMaterials[materialIndex];
		}

		public void ShowModel(bool show)
		{
			MinionToAnimate.EnableRenderers(show);
			global::UnityEngine.Renderer[] componentsInChildren = BalloonAnimator.gameObject.GetComponentsInChildren<global::UnityEngine.Renderer>();
			foreach (global::UnityEngine.Renderer renderer in componentsInChildren)
			{
				renderer.enabled = show;
			}
			MinionToAnimate.EnableBlobShadow(false);
			BlobShadow.SetActive(show);
		}

		public void ShowMango(bool show)
		{
			if (show)
			{
				global::UnityEngine.GameObject[] mangoes = Mangoes;
				foreach (global::UnityEngine.GameObject gameObject in mangoes)
				{
					if (!gameObject.activeSelf)
					{
						gameObject.SetActive(true);
						BasketTarget.SetActive(false);
						break;
					}
				}
			}
			else
			{
				global::UnityEngine.GameObject[] mangoes2 = Mangoes;
				foreach (global::UnityEngine.GameObject gameObject2 in mangoes2)
				{
					gameObject2.SetActive(false);
				}
				BasketTarget.SetActive(true);
			}
		}

		public void Update()
		{
			if (MinionToAnimate != null)
			{
				MinionToAnimate.transform.position = base.transform.position;
				MinionToAnimate.transform.rotation = base.transform.rotation;
			}
			if (isAFlyer)
			{
				UpdateFlyingMinion();
			}
		}

		public bool IsAFlyer()
		{
			return isAFlyer;
		}

		private void UpdateFlyingMinion()
		{
			if (needsReset)
			{
				needsReset = false;
				global::UnityEngine.Vector3 position = base.transform.position;
				position.y = initialY;
				base.transform.position = position;
				base.transform.forward = MyParentGameControllerObject.FloatingMinionLocator.forward;
				splatTimer = 0f;
			}
			if (splatTimer >= 0f)
			{
				splatTimer -= global::UnityEngine.Time.deltaTime;
				if (splatTimer <= 1f && mangoSplatGameObject != null)
				{
					mangoSplatGameObject.transform.localScale = global::UnityEngine.Vector3.one * splatTimer;
				}
			}
			else if (mangoSplatGameObject != null && mangoSplatGameObject.activeSelf)
			{
				mangoSplatGameObject.SetActive(false);
			}
			waveTimer -= global::UnityEngine.Time.deltaTime;
			if (waveTimer <= 0f)
			{
				SetAnimTriggers("OnWave");
				waveTimer = global::UnityEngine.Random.Range(3f, 10f);
			}
			switch (MinionAndBalloonState)
			{
			case global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject.MinionAndBalloonStates.Floating:
				UpdateForFlying();
				break;
			case global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject.MinionAndBalloonStates.Falling:
			{
				global::UnityEngine.Vector3 position2 = base.transform.position;
				position2.y += global::UnityEngine.Time.deltaTime * velocityY;
				velocityY -= global::UnityEngine.Time.deltaTime * 16f;
				if (position2.y <= 0.1f)
				{
					PlayBounceAnimation();
				}
				base.transform.position = position2;
				break;
			}
			case global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject.MinionAndBalloonStates.WalkingOff:
				UpdateForWalkingOff();
				break;
			}
		}

		public void UpdateForWalkingOff()
		{
			if (IsInState("Base Layer.WalkOff"))
			{
				global::UnityEngine.Vector3 position = base.transform.position;
				position.z += global::UnityEngine.Time.deltaTime * 2f;
				base.transform.position = position;
				if (position.z >= zMaxValue + WalkoffZValue)
				{
					directionLeft = false;
					PlayFlyingAnimation();
				}
				base.transform.position = base.transform.position;
				base.transform.rotation = global::UnityEngine.Quaternion.Slerp(base.transform.rotation, global::UnityEngine.Quaternion.LookRotation(global::UnityEngine.Vector3.forward), global::UnityEngine.Time.deltaTime * 2f);
			}
		}

		public void UpdateForFlying()
		{
			if (HoldPosition)
			{
				return;
			}
			global::UnityEngine.Vector3 position = base.transform.position;
			if (directionLeft)
			{
				currentSpeed = global::UnityEngine.Mathf.MoveTowards(currentSpeed, 0f - targetSpeed, global::UnityEngine.Time.deltaTime * 5f);
				position.z += global::UnityEngine.Time.deltaTime * currentSpeed;
				if (position.z <= zMinValue)
				{
					directionLeft = false;
				}
			}
			else
			{
				currentSpeed = global::UnityEngine.Mathf.MoveTowards(currentSpeed, targetSpeed, global::UnityEngine.Time.deltaTime * 5f);
				position.z += global::UnityEngine.Time.deltaTime * currentSpeed;
				if (position.z >= zMaxValue)
				{
					directionLeft = true;
				}
			}
			base.transform.position = position;
		}

		public bool CanBeHit()
		{
			if (MinionAndBalloonState == global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject.MinionAndBalloonStates.Floating && !HoldPosition)
			{
				return true;
			}
			return false;
		}

		public void CleanUp()
		{
			global::UnityEngine.Object.Destroy(mangoSplatGameObject);
			MinionAndBalloonState = global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject.MinionAndBalloonStates.None;
		}

		private void StartSplat()
		{
			CreateVfxAtLocation(MyParentBuildingViewObject.MangoHitBodyVFXPrefab, base.transform.position);
			splatTimer = 3f;
			mangoSplatGameObject.SetActive(true);
			mangoSplatGameObject.transform.localScale = global::UnityEngine.Vector3.one;
		}

		public void MinionWasHit()
		{
			StartSplat();
			SetAnimTriggers("OnMinionHit");
		}

		public void PlayBounceAnimation()
		{
			MinionAndBalloonState = global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject.MinionAndBalloonStates.WalkingOff;
			SetAnimTriggers("OnBounce");
			CreateVfxAtLocation(MyParentBuildingViewObject.MinionHitGroundVFXPrefab, base.transform.position);
		}

		private void CreateVfxAtLocation(global::UnityEngine.GameObject vfxPrefab, global::UnityEngine.Vector3 pos)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(vfxPrefab);
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.position = pos;
			global::UnityEngine.Object.Destroy(gameObject, 5f);
		}

		public void PlayFallAnimation(global::UnityEngine.Vector3 impactPoint)
		{
			velocityY = 3f;
			CreateVfxAtLocation(MyParentBuildingViewObject.BalloonPopVFXPrefab, impactPoint);
			MinionAndBalloonState = global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject.MinionAndBalloonStates.Falling;
			SetAnimTriggers("OnFalling");
		}

		public void PlayFlyingAnimation()
		{
			needsReset = true;
			MinionAndBalloonState = global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject.MinionAndBalloonStates.Floating;
			SetAnimTriggers("OnFlying");
		}

		public void StartFloating(float zMin, float zMax, bool lookLeft, float speed)
		{
			isAFlyer = true;
			SyncAnimationStates("Base Layer.Idle");
			zMinValue = zMin;
			zMaxValue = zMax;
			directionLeft = lookLeft;
			targetSpeed = speed;
			currentSpeed = targetSpeed;
			if (directionLeft)
			{
				currentSpeed *= -1f;
			}
			global::UnityEngine.Vector3 position = base.transform.position;
			if (directionLeft)
			{
				position.z = zMax + 4f;
			}
			else
			{
				position.z = zMin - 4f;
			}
			base.transform.position = position;
			PlayFlyingAnimation();
			global::UnityEngine.GameObject gameObject = MinionToAnimate.gameObject.FindChild("minion:head_jnt");
			mangoSplatGameObject = global::UnityEngine.Object.Instantiate(MyParentBuildingViewObject.MinionFaceSplatVFXPrefab);
			mangoSplatGameObject.transform.SetParent(gameObject.transform, false);
		}
	}
}
