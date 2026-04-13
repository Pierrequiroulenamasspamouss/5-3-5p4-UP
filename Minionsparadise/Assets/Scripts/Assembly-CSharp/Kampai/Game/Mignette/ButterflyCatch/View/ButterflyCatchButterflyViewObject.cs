namespace Kampai.Game.Mignette.ButterflyCatch.View
{
	public class ButterflyCatchButterflyViewObject : global::UnityEngine.MonoBehaviour
	{
		private enum StingState
		{
			None = 0,
			BounceToStartPoint = 1,
			StingIn = 2,
			Stinging = 3,
			StingOut = 4
		}

		public bool IsGettingCaught;

		public bool IsReallyABee;

		public global::UnityEngine.GameObject BeeOrButterflyModel;

		public float TimeTillFlyAway;

		public global::UnityEngine.Renderer RendererForMaterial;

		public global::UnityEngine.Animator ButterflyAnimator;

		public global::UnityEngine.Animator BeeAnimator;

		private global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView myParentView;

		private global::Kampai.Game.View.MinionObject MinionToSting;

		private GoSpline myPath;

		private float pathProgress;

		private float totalPathTime;

		private float timeTillExpire;

		private float mySpeed = 1f;

		public int myScore;

		private bool isFlyingAway;

		private global::UnityEngine.Vector3 flyAwayTarget;

		private float flapTimer = 2f;

		private float stingTimer;

		private global::UnityEngine.Vector3 stingStartPos;

		private global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject.StingState BeeStingState;

		public global::UnityEngine.ParticleSystem AngryBeeParticleSystem;

		public global::UnityEngine.ParticleSystem StingBurstParticleSystem;

		public float TimeToGetInStingPosition = 1.3f;

		private global::UnityEngine.Transform myTransform;

		public global::UnityEngine.AnimationCurve beeCurveZ;

		private bool stingTriggered;

		private void Start()
		{
			isFlyingAway = false;
			myTransform = base.transform;
		}

		public void FollowPath(global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView parentView, GoSpline path, float speed)
		{
			myParentView = parentView;
			myPath = path;
			base.transform.position = myPath.getPointOnPath(0f);
			mySpeed = speed;
			totalPathTime = myPath.pathLength / mySpeed;
			StartFlapping();
		}

		public void StingMinion(global::Kampai.Game.View.MinionObject mo)
		{
			MinionToSting = mo;
			BeeStingState = global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject.StingState.BounceToStartPoint;
			stingTimer = 0f;
			stingStartPos = base.transform.position;
			AngryBeeParticleSystem.Stop();
			AngryBeeParticleSystem.Clear();
			AngryBeeParticleSystem.Play();
		}

		private void Update()
		{
			if (myParentView.IsPaused || UpdateStingStatus() || UpdateFlyAwayStatus())
			{
				return;
			}
			if (myPath != null)
			{
				pathProgress += global::UnityEngine.Time.deltaTime * mySpeed;
				if (pathProgress >= totalPathTime)
				{
					pathProgress -= totalPathTime;
				}
				float t = pathProgress / totalPathTime;
				global::UnityEngine.Vector3 pointOnPath = myPath.getPointOnPath(t);
				global::UnityEngine.Vector3 vector = pointOnPath - myTransform.position;
				myTransform.forward = vector.normalized;
				myTransform.position = pointOnPath;
			}
			if (TimeTillFlyAway > 0f)
			{
				TimeTillFlyAway -= global::UnityEngine.Time.deltaTime;
				if (TimeTillFlyAway <= 0f)
				{
					isFlyingAway = true;
					flyAwayTarget = myTransform.position;
					if (myTransform.position.x >= myParentView.MignetteBuildingObject.GetComponent<global::UnityEngine.Collider>().bounds.center.x)
					{
						flyAwayTarget.x += 20f;
					}
					else
					{
						flyAwayTarget.x -= 20f;
					}
					flyAwayTarget.y += 10f;
					myTransform.forward = (flyAwayTarget - myTransform.position).normalized;
					timeTillExpire = 5f;
				}
			}
			if (!(flapTimer > 0f))
			{
				return;
			}
			flapTimer -= global::UnityEngine.Time.deltaTime;
			if (flapTimer <= 0f)
			{
				if (ButterflyAnimator != null)
				{
					ButterflyAnimator.SetBool("isFlapping", false);
				}
				Invoke("StartFlapping", global::UnityEngine.Random.Range(0f, 1.5f));
			}
		}

		public bool UpdateFlyAwayStatus()
		{
			if (isFlyingAway)
			{
				myTransform.position = global::UnityEngine.Vector3.MoveTowards(myTransform.position, flyAwayTarget, global::UnityEngine.Time.deltaTime * mySpeed * 5f);
				myTransform.LookAt(flyAwayTarget);
				timeTillExpire -= global::UnityEngine.Time.deltaTime;
				if (timeTillExpire <= 0f)
				{
					myParentView.RemoveMeFromLists(this);
					global::UnityEngine.Object.Destroy(base.gameObject);
				}
				return true;
			}
			return false;
		}

		public bool UpdateStingStatus()
		{
			if (BeeStingState == global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject.StingState.None)
			{
				return false;
			}
			global::UnityEngine.Transform transform = MinionToSting.transform;
			switch (BeeStingState)
			{
			case global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject.StingState.BounceToStartPoint:
			{
				stingTimer += global::UnityEngine.Time.deltaTime;
				global::UnityEngine.Vector3 position = transform.position;
				position.y += 1f;
				position.x -= 1f;
				global::UnityEngine.Vector3 position2 = global::UnityEngine.Vector3.Lerp(stingStartPos, position, stingTimer / TimeToGetInStingPosition);
				position2.z += beeCurveZ.Evaluate(stingTimer / TimeToGetInStingPosition);
				position.x += 2f;
				myTransform.position = position2;
				myTransform.LookAt(position);
				if (stingTimer >= TimeToGetInStingPosition)
				{
					BeeStingState = global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject.StingState.Stinging;
				}
				stingTriggered = false;
				break;
			}
			case global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject.StingState.Stinging:
			{
				if (BeeAnimator != null && !stingTriggered)
				{
					stingTriggered = true;
					BeeAnimator.Play("beeStinging");
				}
				break;
			}
			}
			global::UnityEngine.Vector3 vector = myParentView.mignetteCamera.transform.position - myTransform.position;
			AngryBeeParticleSystem.transform.position = myTransform.position + vector.normalized;
			return true;
		}

		public void StartFlapping()
		{
			if (ButterflyAnimator != null)
			{
				ButterflyAnimator.SetBool("isFlapping", true);
			}
			flapTimer = global::UnityEngine.Random.Range(1f, 3f);
		}

		public void BeeFireStingFX()
		{
			StingBurstParticleSystem.Stop();
			StingBurstParticleSystem.Clear();
			StingBurstParticleSystem.Play();
			myParentView.globalAudioSignal.Dispatch("Play_balloon_pop_01");
		}

		public void CompleteStingAnim()
		{
			isFlyingAway = true;
			timeTillExpire = 5f;
			stingTriggered = false;
			BeeStingState = global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject.StingState.None;
		}
	}
}
