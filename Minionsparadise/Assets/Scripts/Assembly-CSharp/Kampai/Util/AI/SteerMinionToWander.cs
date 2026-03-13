namespace Kampai.Util.AI
{
	[global::UnityEngine.RequireComponent(typeof(global::Kampai.Game.View.MinionObject))]
	public class SteerMinionToWander : global::Kampai.Util.AI.SteerToWander
	{
		public const float minRestTime = 5f;

		public const float maxRestTime = 8f;

		private global::Kampai.Game.View.MinionObject obj;

		private float timer;

		[Inject]
		public global::Kampai.Game.IncidentalAnimationSignal animSignal { get; set; }

		public override global::UnityEngine.Vector3 Force
		{
			get
			{
				timer -= global::UnityEngine.Time.deltaTime;
				if (timer < 0f)
				{
					timer = global::UnityEngine.Random.Range(5f, 8f);
					animSignal.Dispatch(obj.ID);
					return global::UnityEngine.Vector3.zero;
				}
				return base.Force;
			}
		}

		protected override void Start()
		{
			base.Start();
			obj = GetComponent<global::Kampai.Game.View.MinionObject>();
			timer = global::UnityEngine.Random.Range(5f, 8f);
		}
	}
}
