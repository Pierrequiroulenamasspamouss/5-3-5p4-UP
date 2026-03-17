namespace Kampai.UI.View
{
	public static class VertexHelperExtensions
	{
		private static readonly global::UnityEngine.Vector4 s_DefaultTangent = new global::UnityEngine.Vector4(1f, 0f, 0f, -1f);

		private static readonly global::UnityEngine.Vector3 s_DefaultNormal = global::UnityEngine.Vector3.back;

		public static void AddVert(this global::UnityEngine.UI.VertexHelper vh, global::UnityEngine.Vector3 position, global::UnityEngine.Color32 color, global::UnityEngine.Vector2 uv0, global::UnityEngine.Vector2 uv1)
		{
			vh.AddVert(position, color, uv0, uv1, s_DefaultNormal, s_DefaultTangent);
		}
	}
}
