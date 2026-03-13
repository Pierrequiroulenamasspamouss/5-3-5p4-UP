namespace Kampai.Game.View
{
	public interface CameraView
	{
		global::UnityEngine.Vector3 Velocity { get; set; }

		float DecayAmount { get; set; }

		void PerformBehaviour(global::Kampai.Game.View.CameraUtils cameraUtils);

		void CalculateBehaviour(global::UnityEngine.Vector3 position);

		void ResetBehaviour();

		void Decay();
	}
}
