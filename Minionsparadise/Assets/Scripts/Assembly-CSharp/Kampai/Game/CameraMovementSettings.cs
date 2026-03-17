namespace Kampai.Game
{
	public class CameraMovementSettings
	{
		public enum Settings
		{
			None = 0,
			Default = 1,
			ShowMenu = 2,
			Quest = 3,
			KeepUIOpen = 4
		}

		public global::Kampai.Game.CameraMovementSettings.Settings settings;

		public global::Kampai.Game.Building building;

		public global::Kampai.Game.Quest quest;

		public float cameraSpeed;

		public bool bypassModal;

		public CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings settings, global::Kampai.Game.Building building, global::Kampai.Game.Quest quest)
		{
			this.settings = settings;
			this.building = building;
			this.quest = quest;
			cameraSpeed = 0.8f;
		}

		public CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings settings, global::Kampai.Game.Building building, global::Kampai.Game.Quest quest, bool bypassModal)
		{
			this.settings = settings;
			this.building = building;
			this.quest = quest;
			cameraSpeed = 0.8f;
			this.bypassModal = bypassModal;
		}
	}
}
