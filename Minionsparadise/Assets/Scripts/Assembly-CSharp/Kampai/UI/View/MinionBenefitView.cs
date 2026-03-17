namespace Kampai.UI.View
{
	public class MinionBenefitView : global::Kampai.Util.KampaiView
	{
		private const string MINION_ABILITY_BAR_GLOW_MASK = "img_minion_glow_abilities_mask";

		private const string MINION_ABILITY_BAR_MASK = "img_minion_abilities_mask";

		public global::Kampai.UI.View.LocalizeView Ability;

		public global::Kampai.UI.View.KampaiImage AbilityImage;

		public global::UnityEngine.Color disabledBarColor;

		public global::UnityEngine.Color highlightedBarColor;

		public global::Kampai.UI.View.KampaiImage[] LevelBarImages;

		public global::UnityEngine.GameObject ItemBackgroundPanel;

		public global::Kampai.UI.View.KampaiImage ItemBlueRingBackground;

		public float levelBarPulseScaler = 1.2f;

		public float levelBarPulseTime = 1.25f;

		private int currentLevel;

		private global::Kampai.Game.MinionBenefitLevelBandDefintion LevelBandDef;

		private global::Kampai.UI.View.Benefit m_category;

		private global::Kampai.Game.IDefinitionService definitionService;

		public int levelBarAnimInterations = 4;

		internal global::strange.extensions.signal.impl.Signal triggerAbilityBarAudio = new global::strange.extensions.signal.impl.Signal();

		public global::Kampai.UI.View.Benefit category
		{
			get
			{
				return m_category;
			}
			set
			{
				if (m_category != value)
				{
					m_category = value;
					SetImage();
				}
			}
		}

		public void Init(global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService)
		{
			this.definitionService = definitionService;
			LevelBandDef = definitionService.Get<global::Kampai.Game.MinionBenefitLevelBandDefintion>(89898);
			SetImage();
			UpdateBenefits(global::Kampai.UI.View.MinionLevelSelectorView.GetLowestLevel(playerService, definitionService), null);
		}

		internal global::Kampai.UI.View.KampaiImage GetLevelBar(int level)
		{
			if (level < 0 || level >= LevelBarImages.Length)
			{
				return null;
			}
			return LevelBarImages[level];
		}

		internal void RefreshAllOfTypeArgsCallback(global::System.Type type, global::Kampai.UI.View.GUIArguments args)
		{
			if (type == GetType() && args.Contains<int>() && (!args.Contains<global::Kampai.UI.View.Benefit>() || args.Get<global::Kampai.UI.View.Benefit>() == category))
			{
				UpdateBenefits(args.Get<int>(), args);
			}
		}

		internal void SetLevel(int level)
		{
			currentLevel = level;
			for (int i = 0; i < LevelBarImages.Length; i++)
			{
				if (i < level)
				{
					LevelBarImages[i].maskSprite = UIUtils.LoadSpriteFromPath("img_minion_glow_abilities_mask");
					LevelBarImages[i].color = highlightedBarColor;
				}
				else
				{
					LevelBarImages[i].maskSprite = UIUtils.LoadSpriteFromPath("img_minion_abilities_mask");
					LevelBarImages[i].color = disabledBarColor;
				}
			}
		}

		private void SetValues(int level, global::Kampai.UI.View.GUIArguments args)
		{
			int num = currentLevel;
			if (currentLevel >= level || args == null || !args.Get<bool>() || !args.Contains<global::strange.extensions.signal.impl.Signal<float, global::Kampai.Util.Tuple<int, int>>>())
			{
				SetLevel(level);
				return;
			}
			for (int i = num; i < level; i++)
			{
				args.Get<global::strange.extensions.signal.impl.Signal<float, global::Kampai.Util.Tuple<int, int>>>().Dispatch(levelBarPulseTime * (float)levelBarAnimInterations, new global::Kampai.Util.Tuple<int, int>((int)category, i + 1));
			}
		}

		internal void AnimateLevelBar(int level)
		{
			SetLevel(level);
			global::Kampai.UI.View.KampaiImage levelBar = GetLevelBar(level - 1);
			if (!(levelBar == null))
			{
				global::UnityEngine.RectTransform target = levelBar.transform as global::UnityEngine.RectTransform;
				global::UnityEngine.Vector3 originalScale = global::UnityEngine.Vector3.one;
				triggerAbilityBarAudio.Dispatch();
				global::Kampai.Util.TweenUtil.Throb(target, levelBarPulseScaler, levelBarPulseTime, out originalScale, levelBarAnimInterations);
			}
		}

		private void SetImage()
		{
			if (LevelBandDef != null)
			{
				Ability.LocKey = LevelBandDef.benefitDescriptions[(int)category].localizedKey;
				int itemIconId = LevelBandDef.benefitDescriptions[(int)category].itemIconId;
				if (itemIconId == 0 || itemIconId == 1)
				{
					ItemBackgroundPanel.SetActive(false);
					UIUtils.SetItemIcon(AbilityImage, definitionService.Get<global::Kampai.Game.DisplayableDefinition>(itemIconId));
					return;
				}
				global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(itemIconId);
				UIUtils.SetItemIcon(AbilityImage, itemDefinition);
				ItemBackgroundPanel.SetActive(true);
				ItemBlueRingBackground.gameObject.SetActive(!(itemDefinition is global::Kampai.Game.DropItemDefinition));
			}
		}

		private void UpdateBenefits(int index, global::Kampai.UI.View.GUIArguments args)
		{
			int level = 0;
			switch (category)
			{
			case global::Kampai.UI.View.Benefit.DOUBLE_DROP:
				level = LevelBandDef.minionBenefitLevelBands[index].doubleDropLevel;
				break;
			case global::Kampai.UI.View.Benefit.PREMIUM:
				level = LevelBandDef.minionBenefitLevelBands[index].premiumDropLevel;
				break;
			case global::Kampai.UI.View.Benefit.RARE_DROP:
				level = LevelBandDef.minionBenefitLevelBands[index].rareDropLevel;
				break;
			}
			SetValues(level, args);
		}
	}
}
