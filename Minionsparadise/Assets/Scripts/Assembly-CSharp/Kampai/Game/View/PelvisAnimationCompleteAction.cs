namespace Kampai.Game.View
{
	public class PelvisAnimationCompleteAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.CharacterObject characterObj;

		private global::UnityEngine.RuntimeAnimatorController controller;

		public PelvisAnimationCompleteAction(global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.View.CharacterObject characterObj, global::UnityEngine.RuntimeAnimatorController controller = null)
			: base(logger)
		{
			this.characterObj = characterObj;
			this.controller = controller;
		}

		public override void Execute()
		{
			characterObj.MoveToPelvis();
			if (controller != null)
			{
				characterObj.SetAnimController(controller);
			}
		}

		public override void LateUpdate()
		{
			characterObj.UpdateBlobShadowPosition();
			base.Done = true;
		}
	}
}
