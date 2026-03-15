namespace Kampai.Util
{
	public class LocalizedImageBehaviour : global::UnityEngine.MonoBehaviour
	{
		public global::System.Collections.Generic.List<global::UnityEngine.Texture> Alternates;

		private void Start()
		{
			global::UnityEngine.UI.RawImage component = GetComponent<global::UnityEngine.UI.RawImage>();
			string text = component.texture.name;
			global::UnityEngine.Texture texture = null;
			if (Alternates != null)
			{
				string resourcePath = global::Kampai.Main.HALService.GetResourcePath(global::Kampai.Util.Native.GetDeviceLanguage());
				foreach (global::UnityEngine.Texture alternate in Alternates)
				{
					if (alternate != null && alternate.name != null && alternate.name.Equals(text + "_" + resourcePath))
					{
						texture = alternate;
						break;
					}
				}
			}
			if (texture != null)
			{
				component.texture = texture;
			}
		}
	}
}
