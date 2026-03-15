namespace Kampai.Game.View
{
	public class BobView : global::Kampai.Game.View.NamedMinionView
	{
		private global::strange.extensions.signal.impl.Signal<string, global::System.Type, object> townSquareAnimSignal = new global::strange.extensions.signal.impl.Signal<string, global::System.Type, object>();

		private global::strange.extensions.signal.impl.Signal delaySignal = new global::strange.extensions.signal.impl.Signal();

		private global::Kampai.Game.MinionAnimationDefinition currentFrolicAnimation;

		private global::UnityEngine.RuntimeAnimatorController nextFrolicAnimationController;

		private global::UnityEngine.RuntimeAnimatorController defaultAnimationController;

		private global::Kampai.Game.MinionAnimationDefinition celebrateAnimation;

		private global::Kampai.Game.MinionAnimationDefinition attentionAnimation;

		private global::strange.extensions.signal.impl.Signal animationCallback;

		private global::Kampai.Util.AI.Agent agent;

		private global::UnityEngine.Vector3 defaultPosition;

		private global::Kampai.Game.Angle currentFrolicRotation;

		private bool waitingOnSignClear;

		public override global::strange.extensions.signal.impl.Signal<string, global::System.Type, object> AnimSignal
		{
			get
			{
				return townSquareAnimSignal;
			}
		}

		public override global::Kampai.Game.View.NamedCharacterObject Build(global::Kampai.Game.NamedCharacter character, global::UnityEngine.GameObject parent, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Util.IMinionBuilder minionBuilder)
		{
			base.Build(character, parent, logger, minionBuilder);
			global::Kampai.Util.AI.SteerCharacterToFollowPath steerCharacterToFollowPath = base.gameObject.AddComponent<global::Kampai.Util.AI.SteerCharacterToFollowPath>();
			steerCharacterToFollowPath.enabled = false;
			steerCharacterToFollowPath.Threshold = 0.2f;
			steerCharacterToFollowPath.FinalThreshold = 0.1f;
			steerCharacterToFollowPath.Modifier = 4;
			agent = base.gameObject.GetComponent<global::Kampai.Util.AI.Agent>();
			if (agent == null)
			{
				agent = base.gameObject.AddComponent<global::Kampai.Util.AI.Agent>();
			}
			agent.Radius = 0.5f;
			agent.Mass = 1f;
			agent.MaxForce = 8f;
			agent.MaxSpeed = 0f;
			defaultAnimationController = base.gameObject.GetComponent<global::UnityEngine.Animator>().runtimeAnimatorController;
			if (defaultAnimationController == null)
			{
				logger.Error("No default animation controller found for {0}", base.name);
			}
			global::Kampai.Game.BobCharacter bobCharacter = character as global::Kampai.Game.BobCharacter;
			celebrateAnimation = bobCharacter.Definition.CelebrateAnimation;
			attentionAnimation = bobCharacter.Definition.AttentionAnimation;
			base.gameObject.transform.position = (defaultPosition = (global::UnityEngine.Vector3)bobCharacter.Definition.WanderAnimations[0].Location);
			return this;
		}

		internal void PointAtSign(global::UnityEngine.Vector3 position)
		{
			if (!waitingOnSignClear)
			{
				logger.Info("Bob PointAtSign");
				SetAnimController(defaultAnimationController);
				base.transform.position = position;
				global::Kampai.Util.AI.Agent component = base.gameObject.GetComponent<global::Kampai.Util.AI.Agent>();
				component.MaxSpeed = 0f;
				ClearActionQueue();
				if (attentionAnimation.FaceCamera)
				{
					EnqueueAction(new global::Kampai.Game.View.RotateAction(this, global::UnityEngine.Camera.main.transform.eulerAngles.y - 180f, 360f, logger));
				}
				global::UnityEngine.RuntimeAnimatorController controller = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(attentionAnimation.StateMachine);
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, controller, logger, attentionAnimation.arguments));
			}
		}

		internal void CelebrateLandExpansion(global::strange.extensions.signal.impl.Signal callback)
		{
			waitingOnSignClear = true;
			logger.Info("Bob CelebrateLandExpansion");
			global::UnityEngine.RuntimeAnimatorController controller = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(celebrateAnimation.StateMachine);
			if (celebrateAnimation.FaceCamera)
			{
				EnqueueAction(new global::Kampai.Game.View.RotateAction(this, global::UnityEngine.Camera.main.transform.eulerAngles.y - 180f, 360f, logger));
			}
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, controller, logger, celebrateAnimation.arguments));
			EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.Exit"), logger));
			EnqueueAction(new global::Kampai.Game.View.SendSignalAction(delaySignal, logger));
			EnqueueAction(new global::Kampai.Game.View.SendSignalAction(callback, logger));
			delaySignal.AddOnce(ToggleBool);
		}

		private void ToggleBool()
		{
			waitingOnSignClear = false;
		}

		internal void IdleInTownHall(global::Kampai.Game.LocationIncidentalAnimationDefinition animationDefinition, global::Kampai.Game.MinionAnimationDefinition mad)
		{
			logger.Info("Bob Idle InTownHall animation:{0} location:{1} => {2}", mad.ID, animationDefinition.LocalizedKey, ((global::UnityEngine.Vector3)animationDefinition.Location).ToString());
			currentFrolicAnimation = mad;
			currentFrolicRotation = animationDefinition.Rotation;
			global::Kampai.Util.AI.SteerCharacterToFollowPath component = base.gameObject.GetComponent<global::Kampai.Util.AI.SteerCharacterToFollowPath>();
			component.SetTarget((global::UnityEngine.Vector3)animationDefinition.Location);
			component.enabled = true;
			nextFrolicAnimationController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(mad.StateMachine);
			if (nextFrolicAnimationController == null)
			{
				logger.Error("Unable to load animation controller {0} for {1}", mad.StateMachine, base.name);
			}
			agent.MaxSpeed = 1f;
		}

		internal void ArrivedAtWayPoint()
		{
			logger.Info("Bob ArrivedAtWayPoint");
			agent.MaxSpeed = 0f;
			if (currentFrolicAnimation != null)
			{
				ClearActionQueue();
				if (currentFrolicAnimation.FaceCamera)
				{
					EnqueueAction(new global::Kampai.Game.View.RotateAction(this, global::UnityEngine.Camera.main.transform.eulerAngles.y - 180f, 360f, logger));
				}
				else if (currentFrolicRotation != null)
				{
					EnqueueAction(new global::Kampai.Game.View.RotateAction(this, currentFrolicRotation.Degrees, 360f, logger));
				}
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, nextFrolicAnimationController, logger, currentFrolicAnimation.arguments));
				EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.Exit"), logger));
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, defaultAnimationController, logger));
				if (animationCallback != null)
				{
					EnqueueAction(new global::Kampai.Game.View.SendSignalAction(animationCallback, logger));
				}
				currentFrolicAnimation = null;
			}
			else
			{
				logger.Error("No animaiton to play");
			}
		}

		internal void ReturnToTown()
		{
			logger.Info("Bob ReturnToTown");
			agent.MaxSpeed = 1f;
			base.gameObject.transform.position = defaultPosition;
			SetAnimController(defaultAnimationController);
		}

		public void SetAnimationCallback(global::strange.extensions.signal.impl.Signal callback)
		{
			animationCallback = callback;
		}

		public override void LateUpdate()
		{
			base.LateUpdate();
			if (GetCurrentAnimControllerName() == defaultAnimationController.name)
			{
				SetAnimBool("isMoving", agent.MaxSpeed > 0.0001f);
				SetAnimFloat("speed", agent.Speed);
			}
		}
	}
}
