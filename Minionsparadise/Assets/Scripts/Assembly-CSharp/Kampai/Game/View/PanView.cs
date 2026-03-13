namespace Kampai.Game.View
{
	public class PanView : global::Kampai.Util.KampaiView, global::Kampai.Game.View.CameraView
	{
		protected global::UnityEngine.Vector3 currentPosition;

		protected global::UnityEngine.Vector3 previousPosition;

		protected global::UnityEngine.Vector3 velocity;

		protected float decayAmount;

		protected global::UnityEngine.Camera mainCamera;

		protected global::UnityEngine.Ray mouseRay;

		protected global::UnityEngine.Plane groundPlane;

		protected float hitDistance;

		protected global::UnityEngine.Vector3 hitPosition;

		protected bool initialized;

		protected global::UnityEngine.Vector3 xVector = new global::UnityEngine.Vector3(1f, 0f, 0f);

		protected global::UnityEngine.Vector3 zVector = new global::UnityEngine.Vector3(0f, 0f, 1f);

		private global::UnityEngine.Vector3 multiplier;

		public global::UnityEngine.Vector3 Velocity
		{
			get
			{
				return velocity;
			}
			set
			{
				velocity = value;
			}
		}

		public float DecayAmount
		{
			get
			{
				return decayAmount;
			}
			set
			{
				decayAmount = value;
			}
		}

		protected override void Start()
		{
			mainCamera = base.gameObject.GetComponent<global::UnityEngine.Camera>();
			decayAmount = 0.925f;
			global::UnityEngine.Vector3 inNormal = new global::UnityEngine.Vector3(0f, 1f, 0f);
			global::UnityEngine.Vector3 inPoint = new global::UnityEngine.Vector3(0f, 0f, 0f);
			groundPlane = new global::UnityEngine.Plane(inNormal, inPoint);
			global::UnityEngine.Quaternion quaternion = global::UnityEngine.Quaternion.Euler(0f, base.transform.eulerAngles.y, 0f);
			xVector = quaternion * xVector;
			zVector = quaternion * zVector;
			base.Start();
		}

		public virtual void PerformBehaviour(global::Kampai.Game.View.CameraUtils cameraUtils)
		{
			base.transform.position = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Clamp(base.transform.position.x + velocity.x, 25f, 233f), base.transform.position.y + velocity.y, global::UnityEngine.Mathf.Clamp(base.transform.position.z + velocity.z, -9f, 205f));
		}

		public virtual void CalculateBehaviour(global::UnityEngine.Vector3 position)
		{
		}

		public virtual void ResetBehaviour()
		{
			initialized = false;
		}

		public virtual void Decay()
		{
			velocity *= decayAmount;
		}

		public virtual void SetupAutoPan(global::UnityEngine.Vector3 panTo)
		{
			multiplier = panTo - base.transform.position;
		}

		public virtual void PerformAutoPan(float delta)
		{
			global::UnityEngine.Vector3 vector = delta * multiplier;
			base.transform.position += new global::UnityEngine.Vector3(vector.x, 0f, vector.z);
			velocity = global::UnityEngine.Vector3.zero;
		}
	}
}
