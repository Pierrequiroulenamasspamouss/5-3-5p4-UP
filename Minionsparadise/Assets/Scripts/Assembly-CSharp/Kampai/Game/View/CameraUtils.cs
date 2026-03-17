namespace Kampai.Game.View
{
	public class CameraUtils : global::Kampai.Util.KampaiView
	{
		private const float NearClipPlane = 0.3f;

		private const float FarClipPlane = 60f;

		private global::UnityEngine.Camera mainCamera;

		private global::UnityEngine.Plane groundPlane;

		private global::UnityEngine.Ray ray;

		private float hitDistance;

		private float angle;

		private float width;

		private float depth;

		private global::UnityEngine.Vector3 center;

		private global::UnityEngine.Vector2 xBounds;

		private global::UnityEngine.Vector2 zBounds;

		private int currentFrame;

		private global::UnityEngine.Vector3 mainCameraRaycast;

		protected override void Start()
		{
			mainCamera = GetComponent<global::UnityEngine.Camera>();
			mainCamera.nearClipPlane = 0.3f;
			mainCamera.farClipPlane = 60f;
			global::UnityEngine.Vector3 inNormal = new global::UnityEngine.Vector3(0f, 1f, 0f);
			global::UnityEngine.Vector3 inPoint = new global::UnityEngine.Vector3(0f, 0f, 0f);
			groundPlane = new global::UnityEngine.Plane(inNormal, inPoint);
			xBounds = new global::UnityEngine.Vector2(10f, 100f);
			zBounds = new global::UnityEngine.Vector2(-20f, 70f);
			angle = base.transform.eulerAngles.y * ((float)global::System.Math.PI / 180f);
			center = new global::UnityEngine.Vector3((xBounds.y + xBounds.x) / 2f, 0f, (zBounds.y + zBounds.x) / 2f);
			width = (xBounds.y - xBounds.x) / 2f;
			depth = (zBounds.y - zBounds.x) / 2f;
			base.Start();
		}

		public bool Contains(global::UnityEngine.Vector3 point)
		{
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(point.x - center.x, point.z - center.z);
			float num = global::UnityEngine.Mathf.Cos(angle) * vector.x - global::UnityEngine.Mathf.Sin(angle) * vector.y;
			float num2 = global::UnityEngine.Mathf.Sin(angle) * vector.x + global::UnityEngine.Mathf.Cos(angle) * vector.y;
			num += center.x;
			num2 += center.z;
			return num > center.x - width && num < center.x + width && num2 > center.z - depth && num2 < center.z + depth;
		}

		public global::UnityEngine.Vector3 CameraCenterRaycast()
		{
			int frameCount = global::UnityEngine.Time.frameCount;
			if (currentFrame != frameCount)
			{
				mainCameraRaycast = CameraCenterRaycast(mainCamera);
				currentFrame = frameCount;
			}
			return mainCameraRaycast;
		}

		public global::UnityEngine.Vector3 CameraCenterRaycast(global::UnityEngine.Camera camera)
		{
			return GroundPlaneRaycast(new global::UnityEngine.Vector3((float)camera.pixelWidth * 0.5f, (float)camera.pixelHeight * 0.5f, 0f));
		}

		public global::UnityEngine.Vector3 GroundPlaneRaycast(float xPercentage, float yPercentage)
		{
			return GroundPlaneRaycast(new global::UnityEngine.Vector3((float)mainCamera.pixelWidth * xPercentage, (float)mainCamera.pixelHeight * yPercentage, 0f));
		}

		public global::UnityEngine.Vector3 GroundPlaneRaycast(global::UnityEngine.Vector3 screenPosition)
		{
			return GroundPlaneRaycast(screenPosition, mainCamera);
		}

		public global::UnityEngine.Vector3 GroundPlaneRaycast(global::UnityEngine.Vector3 screenPosition, global::UnityEngine.Camera camera)
		{
			ray = camera.ScreenPointToRay(screenPosition);
			groundPlane.Raycast(ray, out hitDistance);
			return ray.GetPoint(hitDistance);
		}

		public global::UnityEngine.Vector3 GroundPlaneRaycastFromPoint(global::UnityEngine.Vector3 pointInSpace)
		{
			ray = new global::UnityEngine.Ray(pointInSpace, mainCamera.transform.forward);
			groundPlane.Raycast(ray, out hitDistance);
			return ray.GetPoint(hitDistance);
		}

		public global::UnityEngine.Vector3 UIToWorldCoords(global::UnityEngine.Vector3 uiPosition)
		{
			global::UnityEngine.Vector3 vector = GroundPlaneRaycast(uiPosition);
			return new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Round(vector.x), global::UnityEngine.Mathf.Round(vector.y), global::UnityEngine.Mathf.Round(vector.z));
		}
	}
}
