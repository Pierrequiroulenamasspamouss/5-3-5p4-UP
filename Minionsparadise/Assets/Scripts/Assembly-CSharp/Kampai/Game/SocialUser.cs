namespace Kampai.Game
{
	public class SocialUser
	{
		public string name { get; set; }

		public string email { get; set; }

		protected global::UnityEngine.Texture image { get; set; }

		protected global::UnityEngine.Vector2 uvOffset { get; set; }

		public void SetTexture(global::UnityEngine.Texture image, global::UnityEngine.Vector2 uvOffset)
		{
			this.image = image;
			this.uvOffset = uvOffset;
		}

		public global::UnityEngine.Texture GetTexture()
		{
			return image;
		}
	}
}
