namespace Kampai.Game.View.Audio
{
	public class PositionalAudioListenerView : global::Kampai.Util.KampaiView
	{
		public void UpdatePosition(global::UnityEngine.Vector3 newPosition)
		{
			base.transform.position = newPosition;
		}
	}
}
