namespace Kampai.Game.View
{
	public class TSMCharacterView : global::Kampai.Game.View.NamedMinionView
	{
		private global::strange.extensions.signal.impl.Signal<string, global::System.Type, object> animSignal = new global::strange.extensions.signal.impl.Signal<string, global::System.Type, object>();

		internal global::strange.extensions.signal.impl.Signal RemoveCharacterSignal = new global::strange.extensions.signal.impl.Signal();

		internal global::strange.extensions.signal.impl.Signal NextPartyAnimSignal = new global::strange.extensions.signal.impl.Signal();

		internal global::strange.extensions.signal.impl.Signal DestinationReachedSignal = new global::strange.extensions.signal.impl.Signal();

		private global::strange.extensions.signal.impl.Signal GrabTreasureChestSignal = new global::strange.extensions.signal.impl.Signal();

		internal global::strange.extensions.signal.impl.Signal ChestReadyToOpen = new global::strange.extensions.signal.impl.Signal();

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3> introPath;

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3> reverseIntroPath;

		private float defaultIntroTime;

		private string treasureController;

		private int idleStateHash;

		private int belloStateHash;

		private global::System.Collections.Generic.Dictionary<string, object> celebrateParams;

		private global::System.Collections.Generic.Dictionary<string, object> wavingParams;

		private bool showed;

		public override global::strange.extensions.signal.impl.Signal<string, global::System.Type, object> AnimSignal
		{
			get
			{
				return animSignal;
			}
		}

		public override global::Kampai.Game.View.NamedCharacterObject Build(global::Kampai.Game.NamedCharacter character, global::UnityEngine.GameObject parent, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Util.IMinionBuilder minionBuilder)
		{
			idleStateHash = global::UnityEngine.Animator.StringToHash("Base Layer.Idle");
			belloStateHash = global::UnityEngine.Animator.StringToHash("Base Layer.Bello");
			celebrateParams = new global::System.Collections.Generic.Dictionary<string, object>();
			celebrateParams.Add("isCelebrating", true);
			wavingParams = new global::System.Collections.Generic.Dictionary<string, object>();
			wavingParams.Add("isWaving", true);
			global::Kampai.Game.TSMCharacter tSMCharacter = character as global::Kampai.Game.TSMCharacter;
			global::Kampai.Game.TSMCharacterDefinition definition = tSMCharacter.Definition;
			introPath = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(definition.IntroPath);
			reverseIntroPath = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(definition.IntroPath);
			reverseIntroPath.Reverse();
			defaultIntroTime = definition.IntroTime;
			treasureController = definition.TreasureController;
			defaultController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(definition.CharacterAnimations.StateMachine);
			base.Build(character, parent, logger, minionBuilder);
			base.gameObject.SetLayerRecursively(8);
			GrabTreasureChestSignal.AddListener(GrabTreasureChest);
			return this;
		}

		internal void ShowTSMCharacter()
		{
			AnimatePosition(true, defaultIntroTime, DestinationReachedSignal);
		}

		internal void HideTSMCharacter(global::Kampai.Game.View.TSMCharacterHideState hideState)
		{
			switch (hideState)
			{
			case global::Kampai.Game.View.TSMCharacterHideState.Celebrate:
			case global::Kampai.Game.View.TSMCharacterHideState.CelebrateAndReturn:
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, defaultController, logger));
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, null, logger, celebrateParams));
				EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, idleStateHash, logger));
				break;
			case global::Kampai.Game.View.TSMCharacterHideState.Chest:
				TeleportHome();
				break;
			}
			AnimatePosition(false, defaultIntroTime, RemoveCharacterSignal);
		}

		internal void SayCheese(global::System.Action callback)
		{
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, null, logger, wavingParams));
			EnqueueAction(new global::Kampai.Game.View.RotateAction(this, global::UnityEngine.Camera.main.transform.eulerAngles.y - 180f, 360f, logger));
			EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, belloStateHash, logger));
			EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, idleStateHash, logger));
			EnqueueAction(new global::Kampai.Game.View.DelegateAction(callback, logger));
		}

		private void AnimatePosition(bool show, float animationDuration, global::strange.extensions.signal.impl.Signal callback)
		{
			if (show != showed)
			{
				showed = show;
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> path;
				if (show)
				{
					path = introPath;
				}
				else
				{
					path = reverseIntroPath;
					EnableCollider(false);
				}
				EnqueueAction(new global::Kampai.Game.View.PathAction(this, path, animationDuration, logger));
				EnqueueAction(new global::Kampai.Game.View.SendSignalAction(callback, logger));
			}
		}

		public void PlayPartyAnimation(global::Kampai.Game.MinionAnimationDefinition def)
		{
			global::UnityEngine.RuntimeAnimatorController controller = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(def.StateMachine);
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, controller, logger, def.arguments), true);
			if (def.FaceCamera)
			{
				EnqueueAction(new global::Kampai.Game.View.RotateAction(this, global::UnityEngine.Camera.main.transform.eulerAngles.y - 180f, 360f, logger));
			}
			EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.Exit"), logger));
			EnqueueAction(new global::Kampai.Game.View.SendSignalAction(NextPartyAnimSignal, logger));
		}

		public void ChestIntroPostParty()
		{
			global::UnityEngine.RuntimeAnimatorController controller = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(treasureController);
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, controller, logger).SetInstant(), true);
			EnqueueAction(new global::Kampai.Game.View.PlayMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.captain_idle_02"), logger));
		}

		public void StartChestIntro()
		{
			if (actionQueue.Count > 0)
			{
				ClearActionQueue();
				GrabTreasureChest();
			}
			else
			{
				AnimatePosition(false, defaultIntroTime / 3f, GrabTreasureChestSignal);
			}
		}

		public void TeleportHome()
		{
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, defaultController, logger));
		}

		private void GrabTreasureChest()
		{
			showed = true;
			global::UnityEngine.RuntimeAnimatorController controller = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(treasureController);
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, controller, logger).SetInstant());
			EnqueueAction(new global::Kampai.Game.View.TeleportAction(this, introPath[introPath.Count - 1], new global::UnityEngine.Vector3(0f, 130f, 0f), logger));
			EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.captain_idle_02"), logger));
			EnqueueAction(new global::Kampai.Game.View.SendSignalAction(ChestReadyToOpen, logger));
		}

		public void OpenChest(global::System.Action callback)
		{
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "open", null));
			EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.end"), logger));
			EnqueueAction(new global::Kampai.Game.View.DelegateAction(callback, logger));
		}
	}
}
