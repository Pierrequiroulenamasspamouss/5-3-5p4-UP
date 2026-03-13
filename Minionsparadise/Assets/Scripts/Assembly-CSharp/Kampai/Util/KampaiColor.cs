namespace Kampai.Util
{
	public struct KampaiColor
	{
		public float r { get; set; }

		public float g { get; set; }

		public float b { get; set; }

		public float a { get; set; }

		public global::UnityEngine.Color GetColor()
		{
			return new global::UnityEngine.Color(r, g, b, a);
		}
	}
}
