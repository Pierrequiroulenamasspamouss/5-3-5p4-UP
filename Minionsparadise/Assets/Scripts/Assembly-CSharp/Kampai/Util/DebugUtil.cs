namespace Kampai.Util
{
	public static class DebugUtil
	{
		public static void DrawXZCircle(global::UnityEngine.Vector3 center, float radius)
		{
			DrawXZCircle(center, radius, 18, global::UnityEngine.Color.white);
		}

		public static void DrawXZCircle(global::UnityEngine.Vector3 center, float radius, global::UnityEngine.Color color)
		{
			DrawXZCircle(center, radius, 18, color);
		}

		public static void DrawXZCircle(global::UnityEngine.Vector3 center, float radius, int segments, global::UnityEngine.Color color)
		{
			global::UnityEngine.Vector3 start = center;
			start.x += radius * global::UnityEngine.Mathf.Cos(0f);
			start.z += radius * global::UnityEngine.Mathf.Sin(0f);
			int num = 360 / segments;
			for (int i = num; i <= 360; i += num)
			{
				global::UnityEngine.Vector3 vector = center;
				vector.x += radius * global::UnityEngine.Mathf.Cos((float)global::System.Math.PI / 180f * (float)i);
				vector.z += radius * global::UnityEngine.Mathf.Sin((float)global::System.Math.PI / 180f * (float)i);
				global::UnityEngine.Debug.DrawLine(start, vector, color);
				start = vector;
			}
		}
	}
}
