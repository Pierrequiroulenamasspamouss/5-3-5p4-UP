namespace Kampai.UI.View
{
	public class FloatingTextSettings : global::Kampai.UI.View.WorldToGlassUISettings
	{
		public string descriptionText { get; set; }

		public float height { get; set; }

		public bool heightOverrideActive
		{
			get
			{
				return height > 0f;
			}
		}

		public FloatingTextSettings(int trackedId, string text, float heightOverrideValue = -1f)
			: base(trackedId)
		{
			descriptionText = text;
			height = heightOverrideValue;
		}
	}
}
