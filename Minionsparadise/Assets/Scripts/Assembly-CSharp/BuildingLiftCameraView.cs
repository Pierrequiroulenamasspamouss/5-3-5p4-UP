public class BuildingLiftCameraView : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Camera m_camera;

	public global::UnityEngine.Camera m_gameCamera;

	public global::UnityEngine.Transform m_gameCameraTransform;

	private void OnPreRender()
	{
		m_camera.fieldOfView = m_gameCamera.fieldOfView;
		base.gameObject.transform.localPosition = m_gameCameraTransform.localPosition;
		base.gameObject.transform.localRotation = m_gameCameraTransform.localRotation;
		base.gameObject.transform.localScale = m_gameCameraTransform.localScale;
	}
}
