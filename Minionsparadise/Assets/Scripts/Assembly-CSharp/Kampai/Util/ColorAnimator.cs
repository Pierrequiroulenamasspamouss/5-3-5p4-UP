namespace Kampai.Util
{
	public class ColorAnimator : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.Color color = global::UnityEngine.Color.white;

		private void Update()
		{
			GetComponent<global::UnityEngine.Renderer>().material.color = color;
		}
	}
}
