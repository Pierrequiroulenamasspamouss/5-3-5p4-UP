namespace Kampai.UI.View
{
	public class TikiBarChildWayFinderView : global::Kampai.UI.View.AbstractChildWayFinderView
	{
		protected override string UIName
		{
			get
			{
				return "TikiBarChildWayFinder";
			}
		}

		protected override bool OnCanUpdate()
		{
			return true;
		}
	}
}
