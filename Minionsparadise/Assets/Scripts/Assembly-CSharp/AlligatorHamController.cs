public class AlligatorHamController : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.SkinnedMeshRenderer PoleMeshRenderer;

	private global::UnityEngine.Shader currentShader;

	private global::UnityEngine.Shader hiddenShader;

	public void Awake()
	{
		currentShader = PoleMeshRenderer.materials[1].shader;
		hiddenShader = global::UnityEngine.Shader.Find("Kampai/Standard/Hidden");
	}

	public void DisplayHam(bool display)
	{
		if (display)
		{
			PoleMeshRenderer.materials[1].shader = currentShader;
		}
		else
		{
			PoleMeshRenderer.materials[1].shader = hiddenShader;
		}
	}
}
