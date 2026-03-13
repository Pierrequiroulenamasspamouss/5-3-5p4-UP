namespace Kampai.UI.View
{
	public class WayFinderModal : global::Kampai.UI.View.WorldToGlassUIModal
	{
		public global::UnityEngine.Animator Animator;

		public global::Kampai.UI.View.ButtonView GoToButton;

		public global::Kampai.UI.View.WayFinderInnerModel GenericModel;

		public global::Kampai.UI.View.WayFinderInnerModel SpecificModel;

		public global::Kampai.Game.Prestige Prestige { get; set; }
	}
}
