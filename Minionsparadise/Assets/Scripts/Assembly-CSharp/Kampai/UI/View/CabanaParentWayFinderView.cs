namespace Kampai.UI.View
{
	public class CabanaParentWayFinderView : global::Kampai.UI.View.AbstractParentWayFinderView
	{
		protected override string UIName
		{
			get
			{
				return "CabanaParentWayFinder";
			}
		}

		protected override string WayFinderDefaultIcon
		{
			get
			{
				return wayFinderDefinition.CabanaDefaultIcon;
			}
		}

		protected override bool OnCanUpdate()
		{
			if (zoomCameraModel.ZoomedIn)
			{
				return false;
			}
			bool flag = false;
			foreach (global::Kampai.UI.View.IChildWayFinderView value in ChildrenWayFinders.Values)
			{
				if (value.IsTargetObjectVisible())
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				foreach (global::Kampai.UI.View.IChildWayFinderView value2 in ChildrenWayFinders.Values)
				{
					value2.SetForceHide(false);
				}
				return false;
			}
			foreach (global::Kampai.UI.View.IChildWayFinderView value3 in ChildrenWayFinders.Values)
			{
				value3.SetForceHide(true);
			}
			return true;
		}

		public override global::UnityEngine.Vector3 GetIndicatorPosition()
		{
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			foreach (global::Kampai.UI.View.IChildWayFinderView value in ChildrenWayFinders.Values)
			{
				zero += value.GetIndicatorPosition();
			}
			int num = ChildrenWayFinders.Values.Count;
			if (num < 1)
			{
				num = 1;
			}
			return zero / num;
		}
	}
}
