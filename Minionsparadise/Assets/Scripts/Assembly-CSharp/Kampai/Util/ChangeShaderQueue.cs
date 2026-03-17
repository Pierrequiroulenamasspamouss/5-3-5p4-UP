namespace Kampai.Util
{
	public class ChangeShaderQueue : global::UnityEngine.MonoBehaviour
	{
		public global::Kampai.Util.Graphics.RenderQueue Queue = global::Kampai.Util.Graphics.RenderQueue.Background;

		public int Offset;

		public int MaterialIndex;

		private void Awake()
		{
			global::UnityEngine.Renderer component = GetComponent<global::UnityEngine.Renderer>();
			if (component != null && component.materials != null && component.materials[MaterialIndex] != null)
			{
				component.materials[MaterialIndex].renderQueue = (int)(Queue + Offset);
			}
		}
	}
}
