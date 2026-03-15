namespace Kampai.Game.View
{
	public class MuteAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.ActionableObject obj;

		private bool muteStatus;

		public MuteAction(global::Kampai.Game.View.ActionableObject obj, bool muteStatus, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.obj = obj;
			this.muteStatus = muteStatus;
		}

		public override void Abort()
		{
			base.Done = true;
		}

		public override void Execute()
		{
			global::Kampai.Game.View.AnimEventHandler[] componentsInChildren = obj.gameObject.GetComponentsInChildren<global::Kampai.Game.View.AnimEventHandler>();
			global::Kampai.Game.View.AnimEventHandler[] array = componentsInChildren;
			foreach (global::Kampai.Game.View.AnimEventHandler animEventHandler in array)
			{
				animEventHandler.mute = muteStatus;
			}
			base.Done = true;
		}
	}
}
