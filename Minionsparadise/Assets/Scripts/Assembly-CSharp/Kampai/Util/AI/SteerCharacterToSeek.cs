namespace Kampai.Util.AI
{
	public class SteerCharacterToSeek : global::Kampai.Util.AI.SteerToSeek
	{
		public float Threshold;

		private global::Kampai.Game.View.CharacterObject obj;

		[Inject]
		public global::Kampai.Game.CharacterArrivedAtDestinationSignal arrivedSignal { get; set; }

		public override global::UnityEngine.Vector3 Force
		{
			get
			{
				if (agent == null)
				{
					agent = GetComponent<global::Kampai.Util.AI.Agent>();
				}
				if (agent == null)
				{
					return global::UnityEngine.Vector3.zero;
				}
				global::UnityEngine.Vector3 vector = Target - agent.Position;
				float magnitude = vector.magnitude;
				if (magnitude > Threshold)
				{
					return vector / magnitude * agent.MaxForce;
				}
				if (arrivedSignal != null && obj != null)
				{
					arrivedSignal.Dispatch(obj.ID);
				}
				base.enabled = false;
				return global::UnityEngine.Vector3.zero;
			}
		}

		protected override void Start()
		{
			base.Start();
			obj = GetComponentInParent<global::Kampai.Game.View.CharacterObject>();
		}
	}
}
