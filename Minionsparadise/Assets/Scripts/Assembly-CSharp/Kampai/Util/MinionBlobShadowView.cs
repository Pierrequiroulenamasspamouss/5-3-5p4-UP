namespace Kampai.Util
{
	public class MinionBlobShadowView : global::UnityEngine.MonoBehaviour
	{
		private global::UnityEngine.Transform pelvis;

		private global::UnityEngine.Transform myTransform;

		private void Start()
		{
			myTransform = base.transform;
		}

		public void SetToTrack(global::UnityEngine.Transform pelvis)
		{
			this.pelvis = pelvis;
		}

		private void Update()
		{
			if ((bool)pelvis)
			{
				global::UnityEngine.Vector3 position = pelvis.position;
				myTransform.position = new global::UnityEngine.Vector3(position.x, 0f, position.z);
			}
		}

		public void ManualUpdate()
		{
			Update();
		}
	}
}
