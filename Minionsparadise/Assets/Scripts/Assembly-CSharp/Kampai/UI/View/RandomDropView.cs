namespace Kampai.UI.View
{
	public class RandomDropView : global::Kampai.Util.KampaiView
	{
		private const float heightOffset = 2f;

		private const float endOffset = 4f;

		public global::Kampai.UI.View.KampaiImage image;

		public global::Kampai.UI.View.ButtonView button;

		private global::UnityEngine.Vector3 localScale;

		private float timeTillTween = 1.5f;

		private float pulseValue;

		private global::UnityEngine.Vector3 scaleDelta;

		public global::strange.extensions.signal.impl.Signal<global::UnityEngine.Vector3, int> timeToTweenSignal = new global::strange.extensions.signal.impl.Signal<global::UnityEngine.Vector3, int>();

		public float InitialPulseValue { get; set; }

		public int ItemDefinitionId { get; set; }

		internal void Init()
		{
			localScale = base.gameObject.transform.localScale;
			scaleDelta = default(global::UnityEngine.Vector3);
			global::UnityEngine.GameObject gameObject = base.gameObject;
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> nodes = CreatePath();
			GoSpline path = new GoSpline(nodes);
			Go.to(gameObject.transform, 0.5f, new GoTweenConfig().setEaseType(GoEaseType.Linear).positionPath(path).onComplete(delegate(AbstractGoTween thisTween)
			{
				thisTween.destroy();
			}));
			Go.to(this, 0.2f, new GoTweenConfig().floatProp("InitialPulseValue", 0.2f).setIterations(-1, GoLoopType.PingPong).setUpdateType(GoUpdateType.LateUpdate)
				.onBegin(delegate
				{
					pulseValue = InitialPulseValue;
				})
				.onUpdate(delegate
				{
					float f = InitialPulseValue - pulseValue;
					UpdatePulse(f);
					pulseValue = InitialPulseValue;
				}));
		}

		public void Update()
		{
			timeTillTween -= global::UnityEngine.Time.deltaTime;
			if (timeTillTween <= 0f)
			{
				timeToTweenSignal.Dispatch(base.gameObject.transform.position, ItemDefinitionId);
			}
		}

		private void UpdatePulse(float f)
		{
			scaleDelta.x = f;
			scaleDelta.y = f;
			scaleDelta.z = f;
			base.gameObject.transform.localScale += scaleDelta;
		}

		internal void UpdateScale(float percentage)
		{
			base.gameObject.transform.localScale = localScale * (1f - percentage);
		}

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3> CreatePath()
		{
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
			global::UnityEngine.Vector3 position = base.gameObject.transform.position;
			global::UnityEngine.Vector3 item = new global::UnityEngine.Vector3(position.x + 2f, position.y + 2f, position.z);
			global::UnityEngine.Vector3 item2 = new global::UnityEngine.Vector3(position.x + 4f, position.y - 2f, position.z);
			list.Add(position);
			list.Add(item);
			list.Add(item2);
			return list;
		}

		internal void KillTweens()
		{
			Go.killAllTweensWithTarget(base.gameObject.transform);
		}
	}
}
