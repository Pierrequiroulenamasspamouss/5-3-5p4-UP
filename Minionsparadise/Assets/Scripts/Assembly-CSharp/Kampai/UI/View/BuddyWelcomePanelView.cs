namespace Kampai.UI.View
{
	public class BuddyWelcomePanelView : global::Kampai.UI.View.PopupMenuView
	{
		public global::Kampai.UI.View.LocalizeView WelcomeTitle;

		public global::Kampai.UI.View.LocalizeView Name;

		public float FadeOutTime = 2f;

		public global::UnityEngine.Vector3 height = new global::UnityEngine.Vector3(0f, 1.1f, 0f);

		private global::Kampai.UI.IPositionService positionService;

		private global::Kampai.Game.View.CharacterObject characterObject;

		public bool Initialized { get; set; }

		public void SetUpInjections(global::Kampai.UI.IPositionService positionService)
		{
			this.positionService = positionService;
		}

		public void SetUpCharacterObject(global::Kampai.Game.View.CharacterObject characterObject)
		{
			this.characterObject = characterObject;
		}

		public void Init(string title, string name)
		{
			base.Init();
			WelcomeTitle.LocKey = title;
			Name.LocKey = name;
		}

		internal void OnUpdatePosition(global::Kampai.UI.PositionData positionData)
		{
			base.gameObject.transform.position = positionData.WorldPositionInUI;
			base.gameObject.transform.localPosition = VectorUtils.ZeroZ(base.gameObject.transform.localPosition);
		}

		internal void LateUpdate()
		{
			if (Initialized && !(characterObject == null))
			{
				global::Kampai.UI.PositionData positionData = ((!(characterObject is global::Kampai.Game.View.VillainView)) ? positionService.GetPositionData(characterObject.GetIndicatorPosition() + height) : positionService.GetPositionData(characterObject.GetIndicatorPosition() + global::Kampai.Util.GameConstants.UI.VILLAIN_UI_OFFSET));
				OnUpdatePosition(positionData);
			}
		}
	}
}
