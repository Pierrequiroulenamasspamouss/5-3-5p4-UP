namespace Kampai.Game.View
{
	public class DelayAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.ActionableObject obj;

		private float delay;

		public DelayAction(global::Kampai.Game.View.ActionableObject obj, float delay, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.obj = obj;
			this.delay = delay;
		}

		public override void Execute()
		{
			obj.StartCoroutine(Delay(delay));
		}

		private global::System.Collections.IEnumerator Delay(float t)
		{
			yield return new global::UnityEngine.WaitForSeconds(t);
			base.Done = true;
		}

		public override string ToString()
		{
			return string.Format("{0} Delay:{1}", GetType().Name, delay);
		}
	}
}
