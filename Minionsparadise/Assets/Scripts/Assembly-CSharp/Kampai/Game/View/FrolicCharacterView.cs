namespace Kampai.Game.View
{
	public class FrolicCharacterView : global::Kampai.Game.View.NamedMinionView
	{
		private global::strange.extensions.signal.impl.Signal<string, global::System.Type, object> animSignal = new global::strange.extensions.signal.impl.Signal<string, global::System.Type, object>();

		public int IdleStateHash;

		public global::System.Collections.Generic.Dictionary<string, object> CelebrateParams;

		public global::strange.extensions.signal.impl.Signal AnimationCallback;

		public global::Kampai.Util.AI.Agent agent;

		protected global::Kampai.Game.MinionAnimationDefinition currentFrolicAnimation;

		protected global::UnityEngine.RuntimeAnimatorController nextFrolicAnimationController;

		protected string wanderControllerName;

		protected global::UnityEngine.RuntimeAnimatorController wanderAnimationController;

		protected global::Kampai.Game.Angle currentFrolicRotation;

		public override global::strange.extensions.signal.impl.Signal<string, global::System.Type, object> AnimSignal
		{
			get
			{
				return animSignal;
			}
		}

		public override global::Kampai.Game.View.NamedCharacterObject Build(global::Kampai.Game.NamedCharacter character, global::UnityEngine.GameObject parent, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Util.IMinionBuilder minionBuilder)
		{
			global::Kampai.Game.FrolicCharacter frolicCharacter = character as global::Kampai.Game.FrolicCharacter;
			global::Kampai.Game.FrolicCharacterDefinition definition = frolicCharacter.Definition;
			wanderControllerName = definition.WanderStateMachine;
			IdleStateHash = global::UnityEngine.Animator.StringToHash("Base Layer.Idle");
			CelebrateParams = new global::System.Collections.Generic.Dictionary<string, object>();
			CelebrateParams.Add("isCelebrating", true);
			base.Build(character, parent, logger, minionBuilder);
			agent = base.gameObject.GetComponent<global::Kampai.Util.AI.Agent>();
			if (agent == null)
			{
				agent = base.gameObject.AddComponent<global::Kampai.Util.AI.Agent>();
			}
			agent.Radius = 0.5f;
			agent.Mass = 1f;
			agent.MaxForce = 8f;
			agent.MaxSpeed = 0f;
			base.gameObject.SetLayerRecursively(8);
			global::Kampai.Util.AI.SteerCharacterToFollowPath steerCharacterToFollowPath = base.gameObject.AddComponent<global::Kampai.Util.AI.SteerCharacterToFollowPath>();
			steerCharacterToFollowPath.enabled = false;
			steerCharacterToFollowPath.Threshold = 0.2f;
			steerCharacterToFollowPath.FinalThreshold = 0.1f;
			steerCharacterToFollowPath.Modifier = 4;
			return this;
		}

		internal void IdleInTownHall(global::Kampai.Game.LocationIncidentalAnimationDefinition animationDefinition, global::Kampai.Game.MinionAnimationDefinition mad)
		{
			logger.Info("Frolic Character Idle InTownHall animation:{0} location:{1} => {2}", mad.ID, animationDefinition.LocalizedKey, ((global::UnityEngine.Vector3)animationDefinition.Location).ToString());
			if (wanderAnimationController == null)
			{
				wanderAnimationController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(wanderControllerName);
			}
			if (!IsWandering())
			{
				global::Kampai.Util.GameObjectUtil.TryEnableBehaviour<global::Kampai.Util.AI.Agent>(base.gameObject, true);
				global::Kampai.Util.GameObjectUtil.TryEnableBehaviour<global::Kampai.Util.AI.SteerCharacterToFollowPath>(base.gameObject, true);
			}
			ClearActionQueue();
			SetAnimController(wanderAnimationController);
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
			logger.Info("Frolic Minion ArrivedAtWayPoint");
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
				float animationSeconds = currentFrolicAnimation.AnimationSeconds;
				if (animationSeconds > 0f)
				{
					EnqueueAction(new global::Kampai.Game.View.DelayAction(this, animationSeconds, logger));
				}
				if (nextFrolicAnimationController == wanderAnimationController)
				{
					EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "chill", false));
				}
				else
				{
					EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.Exit"), logger));
					EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, wanderAnimationController, logger));
				}
				if (AnimationCallback != null)
				{
					EnqueueAction(new global::Kampai.Game.View.SendSignalAction(AnimationCallback, logger));
				}
				currentFrolicAnimation = null;
			}
			else
			{
				logger.Error("No animaiton to play");
			}
		}

		public void SetAnimationCallback(global::strange.extensions.signal.impl.Signal callback)
		{
			AnimationCallback = callback;
		}

		internal bool IsWandering()
		{
			return GetCurrentAnimController() == wanderAnimationController;
		}

		internal virtual void SetIsInParty(bool enable)
		{
			agent.MaxSpeed = 0f;
			EnqueueAction(new global::Kampai.Game.View.RotateAction(this, global::UnityEngine.Camera.main.transform.eulerAngles.y - 180f, 360f, logger));
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "isInParty", enable));
		}

		public override void LateUpdate()
		{
			base.LateUpdate();
			if (IsWandering())
			{
				SetAnimBool("isMoving", agent.MaxSpeed > 0.0001f);
				SetAnimFloat("speed", agent.Speed);
			}
		}
	}
}
