namespace Kampai.UI.View
{
	public class MinionLevelSelectorView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.GameObject SelectedImage;

		public global::UnityEngine.UI.Text Level;

		public global::UnityEngine.UI.Text Count;

		public global::UnityEngine.UI.Text SelectedLevel;

		public global::UnityEngine.UI.Text SelectedCount;

		public ScrollableButtonView SelectionButton;

		public float levelupAnimPulseScalar = 1.2f;

		public float levelupAnimPulseTime = 0.5f;

		private global::Kampai.Game.IPlayerService playerService;

		private global::Kampai.Game.IDefinitionService definitionService;

		public int index { get; set; }

		public void Init(global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService definitionService)
		{
			this.playerService = playerService;
			this.definitionService = definitionService;
			SetSelectedColor(GetLowestLevel(playerService, definitionService), false);
			UpdateMinionCountText();
		}

		internal void SetSelectedColor(int i, bool tryAnim)
		{
			bool flag = index == i;
			SelectedImage.SetActive(flag);
			if (index < GetLowestLevel(playerService, definitionService))
			{
				ToggleButtonInteractive(false);
			}
			else if (tryAnim && flag)
			{
				global::UnityEngine.RectTransform target = base.transform as global::UnityEngine.RectTransform;
				global::UnityEngine.Vector3 originalScale = global::UnityEngine.Vector3.one;
				global::Kampai.Util.TweenUtil.Throb(target, levelupAnimPulseScalar, levelupAnimPulseTime, out originalScale);
			}
		}

		internal void RefreshAllOfTypeArgsCallback(global::System.Type type, int index, global::Kampai.UI.View.GUIArguments args)
		{
			if (type == GetType())
			{
				SetSelectedColor(index, args.Contains<bool>() && args.Get<bool>());
			}
		}

		internal void UpdateMinionCountText()
		{
			int minionCountByLevel = playerService.GetMinionCountByLevel(index);
			global::UnityEngine.UI.Text selectedCount = SelectedCount;
			string text = minionCountByLevel.ToString();
			Count.text = text;
			selectedCount.text = text;
		}

		internal void ToggleButtonInteractive(bool isEnabled)
		{
			global::UnityEngine.UI.Button component = SelectionButton.GetComponent<global::UnityEngine.UI.Button>();
			if (!(component == null))
			{
				component.interactable = isEnabled;
			}
		}

		public static int GetLowestLevel(global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService definitionService)
		{
			int result = 0;
			int count = definitionService.Get<global::Kampai.Game.MinionBenefitLevelBandDefintion>(89898).minionBenefitLevelBands.Count;
			for (int i = 0; i < count; i++)
			{
				int minionCountByLevel = playerService.GetMinionCountByLevel(i);
				if (minionCountByLevel > 0)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public void SetLevelText(string levelText)
		{
			global::UnityEngine.UI.Text selectedLevel = SelectedLevel;
			Level.text = levelText;
			selectedLevel.text = levelText;
		}
	}
}
