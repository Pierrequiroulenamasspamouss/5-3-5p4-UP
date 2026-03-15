namespace Kampai.Game.View
{
	public class KevinView : global::Kampai.Game.View.FrolicCharacterView
	{
		private global::UnityEngine.RuntimeAnimatorController defaultAnimationController;

		private string WelcomeHutStateMachine;

		public override global::Kampai.Game.View.NamedCharacterObject Build(global::Kampai.Game.NamedCharacter character, global::UnityEngine.GameObject parent, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Util.IMinionBuilder minionBuilder)
		{
			base.Build(character, parent, logger, minionBuilder);
			global::Kampai.Game.View.MinionObject.SetEyes(base.transform, 2u);
			global::Kampai.Game.View.MinionObject.SetBody(base.transform, global::Kampai.Game.MinionBody.TALL);
			global::Kampai.Game.View.MinionObject.SetHair(base.transform, global::Kampai.Game.MinionHair.SPROUT);
			base.gameObject.AddComponent<global::Kampai.Util.AI.SteerToAvoidCollisions>();
			global::Kampai.Util.AI.SteerToAvoidEnvironment steerToAvoidEnvironment = base.gameObject.AddComponent<global::Kampai.Util.AI.SteerToAvoidEnvironment>();
			steerToAvoidEnvironment.Modifier = 8;
			global::Kampai.Util.AI.SteerCharacterToSeek steerCharacterToSeek = base.gameObject.AddComponent<global::Kampai.Util.AI.SteerCharacterToSeek>();
			steerCharacterToSeek.enabled = false;
			steerCharacterToSeek.Threshold = 0.1f;
			defaultAnimationController = base.gameObject.GetComponent<global::UnityEngine.Animator>().runtimeAnimatorController;
			if (defaultAnimationController == null)
			{
				logger.Error("No default animation controller found for {0}", base.name);
			}
			global::Kampai.Game.KevinCharacter kevinCharacter = character as global::Kampai.Game.KevinCharacter;
			WelcomeHutStateMachine = kevinCharacter.Definition.WelcomeHutStateMachine;
			return this;
		}

		internal void Walk(bool enable)
		{
			SetAnimBool("walk", enable);
		}

		public void GreetVillain(bool shouldGreet)
		{
			SetAnimBool("GreetVillain", shouldGreet);
		}

		public void WaveFarewell(bool shouldWave)
		{
			SetAnimBool("WaveFarewell", shouldWave);
		}

		internal void GotoWelcomeHut(global::UnityEngine.GameObject WelcomeHut, bool pop)
		{
			if (WelcomeHut == null)
			{
				return;
			}
			global::UnityEngine.Transform transform = WelcomeHut.transform.Find("route0");
			if (!(transform == null))
			{
				if (pop)
				{
					base.transform.position = transform.position;
					base.transform.rotation = transform.rotation;
				}
				ClearActionQueue();
				agent.MaxSpeed = 0f;
				global::UnityEngine.RuntimeAnimatorController controller = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(WelcomeHutStateMachine);
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, controller, logger));
				base.IsIdle = true;
			}
		}
	}
}
