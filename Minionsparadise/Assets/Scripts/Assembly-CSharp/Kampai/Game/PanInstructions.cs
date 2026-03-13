namespace Kampai.Game
{
	public class PanInstructions
	{
		public int InstanceId { get; set; }

		public global::Kampai.Game.Instance Instance { get; set; }

		public global::Kampai.Util.Boxed<float> ZoomDistance { get; set; }

		public global::Kampai.Util.Boxed<global::UnityEngine.Vector3> Offset { get; set; }

		public global::Kampai.Game.CameraMovementSettings CameraMovementSettings { get; set; }

		public PanInstructions(int instanceId)
		{
			InstanceId = instanceId;
		}

		public PanInstructions(global::Kampai.Game.Instance instance)
		{
			Instance = instance;
		}
	}
}
