namespace Kampai.Game
{
	public class PrefabLightmapView : global::UnityEngine.MonoBehaviour
	{
		[global::System.Serializable]
		public struct RendererInfo
		{
			public global::UnityEngine.Renderer renderer;

			public int lightmapIndex;

			public global::UnityEngine.Vector4 lightmapOffsetScale;

			public void SetValues(int lightmapOffsetIndex)
			{
				if (!(renderer == null))
				{
					renderer.lightmapIndex = lightmapOffsetIndex + lightmapIndex;
					renderer.lightmapScaleOffset = lightmapOffsetScale;
				}
			}

			public override string ToString()
			{
				return string.Format("renderer: {0}, lightmap index {1}, scale {2}", renderer, lightmapIndex, lightmapOffsetScale);
			}
		}

		public const int INVALID_LIGHTMAP_INDEX = -1;

		public const int NO_LIGHTMAP_INDEX = 65534;

		[global::UnityEngine.SerializeField]
		public global::Kampai.Game.PrefabLightmapView.RendererInfo[] m_RendererInfo;

		public int LightmapCount;

		public string SceneName;

		public static bool IsValidRenderer(global::UnityEngine.Renderer renderer)
		{
			return renderer != null && IsValidRenderer(renderer.lightmapIndex);
		}

		public static bool IsValidRenderer(global::Kampai.Game.PrefabLightmapView.RendererInfo renderer)
		{
			return IsValidRenderer(renderer.lightmapIndex);
		}

		public static bool IsValidRenderer(int lightmapIndex)
		{
			return lightmapIndex != -1 && lightmapIndex < 65534;
		}

		private void Awake()
		{
			if (m_RendererInfo != null && m_RendererInfo.Length != 0)
			{
				LoadLightMaps();
			}
		}

		private void LoadLightMaps()
		{
			int num = global::UnityEngine.LightmapSettings.lightmaps.Length;
			int num2 = num + LightmapCount;
			global::UnityEngine.LightmapData[] array = new global::UnityEngine.LightmapData[num2];
			global::UnityEngine.LightmapSettings.lightmaps.CopyTo(array, 0);
			for (int i = 0; i < LightmapCount; i++)
			{
				string path = string.Format("{0}_Lightmap-{1}_comp_light", SceneName, i);
				array[num + i] = new global::UnityEngine.LightmapData
				{
					lightmapColor = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.Texture2D>(path)
				};
			}
			ApplyRendererInfo(m_RendererInfo, num);
			global::UnityEngine.LightmapSettings.lightmaps = array;
		}

		private static void ApplyRendererInfo(global::Kampai.Game.PrefabLightmapView.RendererInfo[] infos, int lightmapOffsetIndex)
		{
			foreach (global::Kampai.Game.PrefabLightmapView.RendererInfo rendererInfo in infos)
			{
				rendererInfo.SetValues(lightmapOffsetIndex);
			}
		}
	}
}
