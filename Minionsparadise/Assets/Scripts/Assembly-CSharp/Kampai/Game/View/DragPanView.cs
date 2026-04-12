namespace Kampai.Game.View
{
	public class DragPanView : global::Kampai.Util.KampaiView
	{
		private const float speed = 0.25f;

		protected bool pan;

		protected global::UnityEngine.Vector3 center;

		protected float xThreshold;

		protected float yThreshold;

		private float speedDelta;

		private global::UnityEngine.Quaternion rotation;

		protected float screenWidth;

		protected float screenHeight;

		protected override void Start()
		{
			screenWidth = global::UnityEngine.Screen.width;
			screenHeight = global::UnityEngine.Screen.height;
			pan = false;
			speedDelta = 1f;
			center = new global::UnityEngine.Vector3(screenWidth / 2f, screenHeight / 2f, 0f);
			xThreshold = screenWidth * 0.3f;
			yThreshold = screenHeight * 0.3f;
			rotation = global::UnityEngine.Quaternion.AngleAxis(base.transform.eulerAngles.y, global::UnityEngine.Vector3.up);
			base.Start();
		}

		public virtual void PerformBehaviour(global::UnityEngine.Vector3 position)
		{
			if (pan)
			{
				global::UnityEngine.Vector3 normalized = (position - center).normalized;
				calculateSpeedDelta(position);
				global::UnityEngine.Vector3 vector = normalized * 0.25f * speedDelta;
				vector = new global::UnityEngine.Vector3(vector.x, 0f, vector.y);
				vector = rotation * vector;
				base.transform.position += vector;
				base.transform.position = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Clamp(base.transform.position.x, 25f, 233f), base.transform.position.y, global::UnityEngine.Mathf.Clamp(base.transform.position.z, -9f, 205f));
			}
		}

		public virtual void ResetBehaviour()
		{
			pan = false;
		}

		private void calculateSpeedDelta(global::UnityEngine.Vector3 position)
		{
			if (position.x < xThreshold / 2f || position.x > screenWidth - xThreshold / 2f || position.y < yThreshold / 2f || position.y > screenHeight - yThreshold / 2f)
			{
				speedDelta = 2f;
			}
			else
			{
				speedDelta = 1f;
			}
		}

		public virtual void CalculateBehaviour(global::UnityEngine.Vector3 position)
		{
		}
	}
}
