namespace Kampai.UI.View
{
	public interface IGUICommand
	{
		global::Kampai.UI.View.GUIOperation operation { get; set; }

		global::Kampai.UI.View.GUIPriority priority { get; }

		string prefab { get; }

		bool WorldCanvas { get; set; }

		global::Kampai.UI.View.GUIArguments Args { get; set; }

		string GUILabel { get; set; }

		string skrimScreen { get; set; }

		bool darkSkrim { get; set; }

		float alphaAmt { get; set; }

		global::Kampai.UI.View.SkrimBehavior skrimBehavior { get; set; }

		bool disableSkrimButton { get; set; }

		bool singleSkrimClose { get; set; }

		bool genericPopupSkrim { get; set; }

		global::Kampai.UI.View.ShouldShowPredicateDelegate ShouldShowPredicate { get; set; }
	}
}
