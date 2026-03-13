namespace Kampai.UI.View
{
	public class FaceCameraView : global::UnityEngine.MonoBehaviour
	{
		private global::UnityEngine.Camera faceCamera;

		private global::UnityEngine.Vector3 yFlip = new global::UnityEngine.Vector3(0f, 180f, 0f);

		public void Start()
		{
			faceCamera = global::UnityEngine.Camera.main;
		}

		public void Update()
		{
			base.gameObject.transform.LookAt(faceCamera.transform, faceCamera.transform.up);
			base.gameObject.transform.Rotate(yFlip);
		}
	}
}
