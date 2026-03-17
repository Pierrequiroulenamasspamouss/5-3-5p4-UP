namespace Kampai.UI.View
{
	public interface IWorldToGlassView
	{
		int TrackedId { get; }

		global::UnityEngine.GameObject GameObject { get; }

		void SetForceHide(bool forceHide);

		global::UnityEngine.Vector3 GetIndicatorPosition();
	}
}
