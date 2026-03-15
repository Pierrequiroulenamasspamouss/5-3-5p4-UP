namespace Kampai.Game
{
	public class QuestDialogSetting
	{
		public int definitionID { get; set; }

		public string dialogSound { get; set; }

		public string additionalStringParameter { get; set; }

		public global::Kampai.UI.View.QuestDialogType type { get; set; }

		public QuestDialogSetting()
		{
			dialogSound = string.Empty;
			type = global::Kampai.UI.View.QuestDialogType.NORMAL;
			additionalStringParameter = string.Empty;
		}
	}
}
