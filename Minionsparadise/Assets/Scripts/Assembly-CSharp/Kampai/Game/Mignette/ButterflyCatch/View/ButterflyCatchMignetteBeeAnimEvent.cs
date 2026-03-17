namespace Kampai.Game.Mignette.ButterflyCatch.View
{
	public class ButterflyCatchMignetteBeeAnimEvent : global::UnityEngine.MonoBehaviour
	{
		public global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchButterflyViewObject beeViewObject;

		public void PlayStingFX()
		{
			if (beeViewObject != null)
			{
				beeViewObject.BeeFireStingFX();
			}
		}

		public void CompleteStingAnim()
		{
			if (beeViewObject != null)
			{
				beeViewObject.CompleteStingAnim();
			}
		}
	}
}
