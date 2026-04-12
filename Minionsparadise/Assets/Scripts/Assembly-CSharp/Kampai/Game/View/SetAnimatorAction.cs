namespace Kampai.Game.View
{
	public class SetAnimatorAction : global::Kampai.Game.View.KampaiAction
	{
		protected global::Kampai.Game.View.ActionableObject obj;

		private global::UnityEngine.RuntimeAnimatorController controller;

		private global::System.Collections.Generic.Dictionary<string, object> animationParams;

		public SetAnimatorAction(global::Kampai.Game.View.ActionableObject obj, global::UnityEngine.RuntimeAnimatorController controller, global::Kampai.Util.IKampaiLogger logger, global::System.Collections.Generic.Dictionary<string, object> animationParams = null)
			: base(logger)
		{
			this.obj = obj;
			this.controller = controller;
			this.animationParams = animationParams;
		}

		public SetAnimatorAction(global::Kampai.Game.View.ActionableObject obj, global::UnityEngine.RuntimeAnimatorController controller, string paramName, global::Kampai.Util.IKampaiLogger logger, object paramValue = null)
			: base(logger)
		{
			this.obj = obj;
			this.controller = controller;
			animationParams = new global::System.Collections.Generic.Dictionary<string, object>();
			animationParams.Add(paramName, paramValue);
		}

		public override void Execute()
		{
			if (controller != null)
			{
				obj.SetAnimController(controller);
			}
			if (animationParams != null)
			{
				foreach (global::System.Collections.Generic.KeyValuePair<string, object> animationParam in animationParams)
				{
					global::Kampai.Game.View.SetAnimatorArgumentsAction.SetAnimationParameter(obj, logger, animationParam.Key, animationParam.Value);
				}
			}
			base.Done = true;
		}

		public override string ToString()
		{
			string arg = "null";
			if (controller != null)
			{
				arg = controller.name;
			}
			string arg2 = "null";
			if (animationParams != null)
			{
				global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
				foreach (string key in animationParams.Keys)
				{
					object obj = animationParams[key];
					if (obj == null)
					{
						obj = "trigger";
					}
					stringBuilder.Append(key).Append("=").Append(obj.ToString())
						.Append(" ");
				}
				arg2 = stringBuilder.ToString();
			}
			return string.Format("{0} - {1} params: {2}", GetType().Name, arg, arg2);
		}
	}
}
