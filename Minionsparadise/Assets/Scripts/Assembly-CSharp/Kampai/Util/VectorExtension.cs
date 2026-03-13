namespace Kampai.Util
{
	public static class VectorExtension
	{
		public static global::UnityEngine.Vector3 Truncate(this global::UnityEngine.Vector3 v, float maxLength)
		{
			float num = maxLength * maxLength;
			float sqrMagnitude = v.sqrMagnitude;
			if (sqrMagnitude <= num)
			{
				return v;
			}
			if ((double)sqrMagnitude <= 1E-08)
			{
				return global::UnityEngine.Vector3.zero;
			}
			return v * (maxLength / global::UnityEngine.Mathf.Sqrt(sqrMagnitude));
		}
	}
}
