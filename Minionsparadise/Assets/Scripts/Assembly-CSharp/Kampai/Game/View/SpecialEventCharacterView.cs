namespace Kampai.Game.View
{
	public class SpecialEventCharacterView : global::Kampai.Game.View.FrolicCharacterView
	{
		internal global::strange.extensions.signal.impl.Signal RemoveCharacterSignal = new global::strange.extensions.signal.impl.Signal();

		internal global::strange.extensions.signal.impl.Signal NextPartyAnimSignal = new global::strange.extensions.signal.impl.Signal();

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3> introPath;

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3> reverseIntroPath;

		private float introTime;

		internal global::Kampai.Game.SpecialEventCharacter eventCharacter;

		public override global::Kampai.Game.View.NamedCharacterObject Build(global::Kampai.Game.NamedCharacter character, global::UnityEngine.GameObject parent, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Util.IMinionBuilder minionBuilder)
		{
			eventCharacter = character as global::Kampai.Game.SpecialEventCharacter;
			global::Kampai.Game.SpecialEventCharacterDefinition definition = eventCharacter.Definition;
			introPath = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(definition.IntroPath);
			reverseIntroPath = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(definition.IntroPath);
			reverseIntroPath.Reverse();
			introTime = definition.IntroTime;
			base.Build(character, parent, logger, minionBuilder);
			return this;
		}

		internal void ShowSpecialEventCharacter()
		{
			AnimatePosition(true);
		}

		internal void HideSpecialEventCharacter(bool completedQuest)
		{
			if (completedQuest)
			{
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, null, logger, CelebrateParams));
				EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, IdleStateHash, logger));
			}
			AnimatePosition(false);
		}

		private void AnimatePosition(bool show)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> path = ((!show) ? reverseIntroPath : introPath);
			EnqueueAction(new global::Kampai.Game.View.PathAction(this, path, introTime, logger));
			if (!show)
			{
				EnqueueAction(new global::Kampai.Game.View.SendSignalAction(RemoveCharacterSignal, logger));
			}
			else if (AnimationCallback != null)
			{
				EnqueueAction(new global::Kampai.Game.View.SendSignalAction(AnimationCallback, logger));
			}
		}

		public void PlayPartyAnimation(global::Kampai.Game.MinionAnimationDefinition def)
		{
			agent.MaxSpeed = 0f;
			global::UnityEngine.RuntimeAnimatorController controller = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(def.StateMachine);
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, controller, logger, def.arguments), true);
			if (def.FaceCamera)
			{
				EnqueueAction(new global::Kampai.Game.View.RotateAction(this, global::UnityEngine.Camera.main.transform.eulerAngles.y - 180f, 360f, logger));
			}
			EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.Exit"), logger));
			EnqueueAction(new global::Kampai.Game.View.SendSignalAction(NextPartyAnimSignal, logger));
		}
	}
}
