namespace Kampai.Util
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Renderer))]
	public class MovieMaterial : global::UnityEngine.MonoBehaviour
	{
		public float fps = 15f;

		public int frames = 1;

		private int rows = 1;

		private int cols = 1;

		public bool startAtTop = true;

		private global::UnityEngine.Vector2 scale = global::UnityEngine.Vector2.zero;

		private void Start()
		{
			Reset();
		}

		private void Reset()
		{
			scale = GetComponent<global::UnityEngine.Renderer>().material.mainTextureScale;
			cols = (int)global::UnityEngine.Mathf.Round(1f / scale.x);
			rows = (int)global::UnityEngine.Mathf.Round(1f / scale.y);
		}

		private void Update()
		{
			int num = (int)(global::UnityEngine.Time.time * fps);
			num %= frames;
			int num2 = num % cols;
			int num3 = num / cols;
			if (startAtTop)
			{
				num3 = rows - 1 - num3;
			}
			global::UnityEngine.Vector2 mainTextureOffset = new global::UnityEngine.Vector2((float)num2 * scale.x, (float)num3 * scale.y);
			GetComponent<global::UnityEngine.Renderer>().material.mainTextureOffset = mainTextureOffset;
		}
	}
}
