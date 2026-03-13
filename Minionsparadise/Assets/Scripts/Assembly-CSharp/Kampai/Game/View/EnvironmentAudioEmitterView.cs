namespace Kampai.Game.View
{
	public class EnvironmentAudioEmitterView : global::strange.extensions.mediation.impl.View
	{
		public string AudioName;

		public float FadeDuration = 2f;

		private global::UnityEngine.Camera mainCamera;

		private global::UnityEngine.Transform m_transform;

		internal readonly global::strange.extensions.signal.impl.Signal<bool> OnTargetVisible = new global::strange.extensions.signal.impl.Signal<bool>();

		private bool volcanoWasVisible;

		internal void Init(global::UnityEngine.Camera mainCamera)
		{
			this.mainCamera = mainCamera;
			m_transform = base.transform;
			OnTargetVisible.Dispatch(false);
		}

		private void Update()
		{
			if (!(mainCamera == null))
			{
				global::UnityEngine.Vector3 vector = mainCamera.WorldToViewportPoint(m_transform.position);
				global::UnityEngine.Vector3 vector2 = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Clamp01(vector.x), global::UnityEngine.Mathf.Clamp01(vector.y), vector.z);
				bool flag = vector == vector2;
				if (flag != volcanoWasVisible)
				{
					OnTargetVisible.Dispatch(flag);
					volcanoWasVisible = flag;
				}
			}
		}
	}
}
