namespace Kampai.UI.View
{
	public class TikiBarParentWayFinderView : global::Kampai.UI.View.AbstractParentWayFinderView
	{
		protected override string WayFinderDefaultIcon
		{
			get
			{
				return wayFinderDefinition.TikibarDefaultIcon;
			}
		}

		protected override string UIName
		{
			get
			{
				return "TikiBarParentWayFinder";
			}
		}

		protected override bool OnCanUpdate()
		{
			if (ChildrenWayFinders.Values.Count < 1)
			{
				return true;
			}
			if (isForceHideEnabled)
			{
				foreach (global::Kampai.UI.View.IChildWayFinderView value in ChildrenWayFinders.Values)
				{
					value.SetForceHide(true);
				}
				return false;
			}
			foreach (global::Kampai.UI.View.IChildWayFinderView value2 in ChildrenWayFinders.Values)
			{
				value2.SetForceHide(false);
			}
			return false;
		}
	}
}
