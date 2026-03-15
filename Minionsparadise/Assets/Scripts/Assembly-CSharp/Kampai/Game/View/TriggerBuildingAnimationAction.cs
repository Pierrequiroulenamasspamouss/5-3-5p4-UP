namespace Kampai.Game.View
{
	public class TriggerBuildingAnimationAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.AnimatingBuildingObject obj;

		private global::System.Collections.Generic.Dictionary<string, object> animationParams;

		private string mecanimState;

		public TriggerBuildingAnimationAction(global::Kampai.Game.View.AnimatingBuildingObject obj, global::System.Collections.Generic.Dictionary<string, object> animationParams, global::Kampai.Util.IKampaiLogger logger, string mecanimState = null)
			: base(logger)
		{
			this.obj = obj;
			this.animationParams = animationParams;
			this.mecanimState = mecanimState;
		}

		public TriggerBuildingAnimationAction(global::Kampai.Game.View.AnimatingBuildingObject obj, string triggerName, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.obj = obj;
			animationParams = new global::System.Collections.Generic.Dictionary<string, object>();
			animationParams.Add(triggerName, string.Empty);
		}

		public override void Execute()
		{
			if (animationParams != null)
			{
				foreach (global::System.Collections.Generic.KeyValuePair<string, object> animationParam in animationParams)
				{
					if (animationParam.Value is bool)
					{
						obj.SetAnimBool(animationParam.Key, (bool)animationParam.Value);
						if ((bool)animationParam.Value)
						{
							obj.SetVFXState(animationParam.Key, mecanimState);
						}
					}
					else if (animationParam.Value.ToString().Length == 0)
					{
						obj.SetAnimBool(animationParam.Key, true);
						obj.SetVFXState(animationParam.Key, mecanimState);
					}
					else if (animationParam.Value is string)
					{
						bool result;
						if (bool.TryParse(animationParam.Value.ToString(), out result))
						{
							obj.SetAnimBool(animationParam.Key, result);
							if (result)
							{
								obj.SetVFXState(animationParam.Key, mecanimState);
							}
						}
						else
						{
							logger.Error(string.Concat("Unknown animation argument: ", animationParam.Value, " (type=", animationParam.Value.GetType(), ")"));
						}
					}
					else
					{
						logger.Error(string.Concat("Unknown animation argument: ", animationParam.Value, " (type=", animationParam.Value.GetType(), ")"));
					}
				}
			}
			base.Done = true;
		}
	}
}
