namespace Kampai.Game.View
{
	public class SetOrderBoardShader : global::UnityEngine.MonoBehaviour
	{
		private void Start()
		{
			GetComponent<global::UnityEngine.Renderer>().material.shader = global::UnityEngine.Shader.Find("Kampai/Background/(+4) Platform");
		}
	}
}
