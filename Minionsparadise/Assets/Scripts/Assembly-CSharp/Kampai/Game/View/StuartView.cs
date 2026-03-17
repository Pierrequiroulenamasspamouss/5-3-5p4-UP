namespace Kampai.Game.View
{
	public class StuartView : global::Kampai.Game.View.FrolicCharacterView
	{
		private const string Base_Layer_OnStage_Idle = "Base Layer.OnStage_Idle";

		private const string Base_Layer_OnStage_Celebrate = "Base Layer.OnStage_Celebrate";

		private const string Base_Layer_OnStage_Perform = "Base Layer.OnStage_Perform";

		private const string BOOL_ISPERFORMING = "IsPerforming";

		private const string BOOL_ISCELEBRATING = "IsCelebrating";

		private const int SPIN_MIX_INDEX = 0;

		private string stageControllerName;

		private global::UnityEngine.RuntimeAnimatorController stageController;

		private global::Kampai.Game.MinionAnimationDefinition onStageAnimation;

		private bool onStage;

		private global::Kampai.Game.Transaction.WeightedInstance onStageIdleWeightedInstance;

		private global::Kampai.Game.Transaction.WeightedInstance onStageTicketFilledWeightedInstance;

		private global::Kampai.Game.Transaction.WeightedInstance onStagePerformWeightedInstance;

		private int lastIndex = -1;

		public global::System.Action OnSpinMic { get; set; }

		public override global::Kampai.Game.View.NamedCharacterObject Build(global::Kampai.Game.NamedCharacter character, global::UnityEngine.GameObject parent, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Util.IMinionBuilder minionBuilder)
		{
			base.Build(character, parent, logger, minionBuilder);
			global::Kampai.Game.StuartCharacter stuartCharacter = character as global::Kampai.Game.StuartCharacter;
			global::Kampai.Game.StuartCharacterDefinition definition = stuartCharacter.Definition;
			onStageAnimation = definition.OnStageAnimation;
			stageControllerName = definition.OnStageAnimation.StateMachine;
			int onStageIdleAnimationCount = definition.OnStageIdleAnimationCount;
			int onStageTicketFilledAnimationCount = definition.OnStageTicketFilledAnimationCount;
			int onStagePerformAnimationCount = definition.OnStagePerformAnimationCount;
			onStageIdleWeightedInstance = CreateWeightedInstance(onStageIdleAnimationCount);
			onStageTicketFilledWeightedInstance = CreateWeightedInstance(onStageTicketFilledAnimationCount);
			onStagePerformWeightedInstance = CreateWeightedInstance(onStagePerformAnimationCount);
			return this;
		}

		internal void AddToStage(global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation, global::Kampai.Game.StuartStageAnimationType animType)
		{
			global::Kampai.Util.GameObjectUtil.TryEnableBehaviour<global::Kampai.Util.AI.Agent>(base.gameObject, false);
			global::Kampai.Util.GameObjectUtil.TryEnableBehaviour<global::Kampai.Util.AI.SteerCharacterToFollowPath>(base.gameObject, false);
			base.transform.position = position;
			base.transform.localRotation = rotation;
			if (stageController == null)
			{
				stageController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(stageControllerName);
			}
			switch (animType)
			{
			case global::Kampai.Game.StuartStageAnimationType.CELEBRATE:
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, stageController, logger, onStageAnimation.arguments), true);
				GetOnStage(true);
				StartingState(global::Kampai.Game.StuartStageAnimationType.CELEBRATE);
				break;
			case global::Kampai.Game.StuartStageAnimationType.IDLEONSTAGE:
				GetOnStage(true);
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, stageController, logger, onStageAnimation.arguments), true);
				StartingState(global::Kampai.Game.StuartStageAnimationType.IDLEONSTAGE);
				break;
			default:
				GetOnStage(false);
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, stageController, logger), true);
				break;
			}
			base.IsIdle = true;
		}

		internal void GetOnStage(bool enable, bool clear = false)
		{
			if (onStage != enable)
			{
				onStage = enable;
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "goToStage", enable), clear);
				base.IsIdle = !enable;
			}
		}

		internal void GetOnStageImmediate(bool enable)
		{
			if (onStage != enable)
			{
				onStage = enable;
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "Immediate", null, "goToStage", enable), true);
				base.IsIdle = !enable;
				if (enable)
				{
					StartingState(global::Kampai.Game.StuartStageAnimationType.IDLEONSTAGE);
				}
			}
		}

		internal void Perform(global::Kampai.Util.SignalCallback<global::strange.extensions.signal.impl.Signal> finishedCallback)
		{
			base.IsIdle = false;
			SetAnimController(stageController);
			SetAnimBool("goToStage", true);
			StartingState(global::Kampai.Game.StuartStageAnimationType.PERFORM, true);
			EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.OffStage_Idle"), logger));
			EnqueueAction(new global::Kampai.Game.View.DelegateAction(SetToIdle, logger));
			EnqueueAction(new global::Kampai.Game.View.SendSignalAction(finishedCallback.PromiseDispatch(), logger));
		}

		private void SetToIdle()
		{
			base.IsIdle = true;
		}

		internal void GetAttention(bool enable)
		{
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "isGetAttention", enable));
		}

		internal override void SetIsInParty(bool enable)
		{
			if (IsWandering())
			{
				base.SetIsInParty(enable);
				return;
			}
			SetAnimBool("isInParty", enable);
			SetAnimBool("stageIsBuilt", onStage);
		}

		internal void TuneGuitar(global::System.Action onDone)
		{
			StartingState(global::Kampai.Game.StuartStageAnimationType.IDLEONSTAGE, false, onDone);
		}

		public bool IsOnStage()
		{
			return onStage;
		}

		private global::Kampai.Game.Transaction.WeightedInstance CreateWeightedInstance(int weightedCount)
		{
			global::Kampai.Game.Transaction.WeightedDefinition weightedDefinition = new global::Kampai.Game.Transaction.WeightedDefinition();
			weightedDefinition.Entities = new global::System.Collections.Generic.List<global::Kampai.Game.Transaction.WeightedQuantityItem>();
			for (int i = 0; i < weightedCount; i++)
			{
				weightedDefinition.Entities.Add(new global::Kampai.Game.Transaction.WeightedQuantityItem(i, 0u, 1u));
			}
			return new global::Kampai.Game.Transaction.WeightedInstance(weightedDefinition);
		}

		public void StartingState(global::Kampai.Game.StuartStageAnimationType targetState, bool clearQueue = false, global::System.Action onDone = null)
		{
			int num = 0;
			switch (targetState)
			{
			case global::Kampai.Game.StuartStageAnimationType.IDLEONSTAGE:
				num = onStageIdleWeightedInstance.NextPick(base.randomService).ID;
				num = ((num == lastIndex) ? onStageIdleWeightedInstance.NextPick(base.randomService).ID : num);
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "IsPerforming", false, "IsCelebrating", false), clearQueue);
				EnqueueAction(new global::Kampai.Game.View.DelayAction(this, 0.5f, logger));
				EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.OnStage_Idle"), logger));
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "randomizer", num, "TriggerIdle", null));
				if (num == 0)
				{
					EnqueueAction(new global::Kampai.Game.View.DelegateAction(OnSpinMic, logger));
				}
				if (onDone != null)
				{
					EnqueueAction(new global::Kampai.Game.View.DelegateAction(onDone, logger));
				}
				EnqueueAction(new global::Kampai.Game.View.PickNextStuartAnimationAction(this, targetState, logger));
				lastIndex = num;
				break;
			case global::Kampai.Game.StuartStageAnimationType.CELEBRATE:
				num = onStageTicketFilledWeightedInstance.NextPick(base.randomService).ID;
				num = ((num == lastIndex) ? onStageTicketFilledWeightedInstance.NextPick(base.randomService).ID : num);
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "IsPerforming", false, "IsCelebrating", true), clearQueue);
				EnqueueAction(new global::Kampai.Game.View.DelayAction(this, 0.5f, logger));
				EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.OnStage_Celebrate"), logger));
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "randomizer", num));
				if (onDone != null)
				{
					EnqueueAction(new global::Kampai.Game.View.DelegateAction(onDone, logger));
				}
				EnqueueAction(new global::Kampai.Game.View.PickNextStuartAnimationAction(this, global::Kampai.Game.StuartStageAnimationType.IDLEONSTAGE, logger));
				lastIndex = num;
				break;
			case global::Kampai.Game.StuartStageAnimationType.PERFORM:
				num = onStagePerformWeightedInstance.NextPick(base.randomService).ID;
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "randomizer", num, "IsPerforming", true, "IsCelebrating", false), true);
				EnqueueAction(new global::Kampai.Game.View.PlayMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.OnStage_Perform"), logger));
				EnqueueAction(new global::Kampai.Game.View.DelayAction(this, 0.5f, logger));
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "goToStage", false, "IsPerforming", false));
				lastIndex = -1;
				break;
			case global::Kampai.Game.StuartStageAnimationType.IDLEOFFSTAGE:
				break;
			}
		}
	}
}
