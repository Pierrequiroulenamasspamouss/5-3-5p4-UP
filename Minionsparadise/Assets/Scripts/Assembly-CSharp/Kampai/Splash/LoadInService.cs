namespace Kampai.Splash
{
	public class LoadInService : global::Kampai.Splash.ILoadInService
	{
		private const int NB_TIPS_TO_SAVE = 10;

		public global::Kampai.Common.IRandomService randomService;

		private float defaultTipCycleTime = 3f;

		private global::System.Collections.Generic.IList<string> defaultLoadTips;

		private global::System.Collections.Generic.IList<global::Kampai.Splash.TipToShow> savedTips;

		private global::Kampai.Splash.SavedTipsModel model = new global::Kampai.Splash.SavedTipsModel();

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		public LoadInService()
		{
			InitDefaultTips();
			InitSavedTips();
			long seed = global::System.DateTime.Now.Second;
			randomService = new global::Kampai.Common.RandomService(seed);
		}

		private void InitDefaultTips()
		{
			defaultLoadTips = new global::System.Collections.Generic.List<string>();
			defaultLoadTips.Add("defaultTip001");
			defaultLoadTips.Add("defaultTip002");
			defaultLoadTips.Add("defaultTip003");
			defaultLoadTips.Add("defaultTip004");
			defaultLoadTips.Add("defaultTip005");
			defaultLoadTips.Add("defaultTip006");
			defaultLoadTips.Add("defaultTip007");
			defaultLoadTips.Add("defaultTip008");
			defaultLoadTips.Add("defaultTip009");
			defaultLoadTips.Add("defaultTip010");
			defaultLoadTips.Add("defaultTip011");
		}

		private void InitSavedTips()
		{
			savedTips = new global::System.Collections.Generic.List<global::Kampai.Splash.TipToShow>(model.Tips);
		}

		public global::Kampai.Splash.TipToShow GetNextTip()
		{
			if (model.Tips.Count > 0 && model.TipsLocale.Equals(localizationService.GetLanguage()))
			{
				if (savedTips.Count == 0)
				{
					InitSavedTips();
				}
				int index = randomService.NextInt(savedTips.Count);
				global::Kampai.Splash.TipToShow result = savedTips[index];
				savedTips.RemoveAt(index);
				return result;
			}
			if (defaultLoadTips.Count == 0)
			{
				InitDefaultTips();
			}
			int index2 = randomService.NextInt(defaultLoadTips.Count);
			global::Kampai.Splash.TipToShow result2 = new global::Kampai.Splash.TipToShow(localizationService.GetString(defaultLoadTips[index2]), defaultTipCycleTime);
			defaultLoadTips.RemoveAt(index2);
			return result2;
		}

		public void SaveTipsForNextLaunch(int level)
		{
			global::System.Collections.Generic.List<global::Kampai.Splash.TipToShow> randomTipsFromCurrentBucket = GetRandomTipsFromCurrentBucket(level, 10);
			model.SaveTipsToDevice(randomTipsFromCurrentBucket, localizationService.GetLanguage());
		}

		private global::System.Collections.Generic.List<global::Kampai.Splash.TipToShow> GetRandomTipsFromCurrentBucket(int level, int nbTips)
		{
			global::Kampai.Splash.LoadinTipBucketDefinition bucket = GetBucket(level);
			global::System.Collections.Generic.List<global::Kampai.Splash.LoadInTipDefinition> all = definitionService.GetAll<global::Kampai.Splash.LoadInTipDefinition>();
			global::System.Collections.Generic.List<global::Kampai.Splash.TipToShow> list = new global::System.Collections.Generic.List<global::Kampai.Splash.TipToShow>();
			foreach (global::Kampai.Splash.LoadInTipDefinition item in all)
			{
				foreach (global::Kampai.Splash.BucketAssignment bucket2 in item.Buckets)
				{
					if (bucket2.BucketId == bucket.ID)
					{
						list.Add(new global::Kampai.Splash.TipToShow(localizationService.GetString(item.Text), bucket2.Time));
						break;
					}
				}
			}
			if (list.Count <= nbTips)
			{
				return list;
			}
			global::System.Collections.Generic.List<global::Kampai.Splash.TipToShow> list2 = new global::System.Collections.Generic.List<global::Kampai.Splash.TipToShow>();
			for (int i = 0; i < nbTips; i++)
			{
				int index = randomService.NextInt(list.Count);
				list2.Add(list[index]);
				list.RemoveAt(index);
			}
			return list2;
		}

		private global::Kampai.Splash.LoadinTipBucketDefinition GetBucket(int level)
		{
			global::System.Collections.Generic.IList<global::Kampai.Splash.LoadinTipBucketDefinition> all = definitionService.GetAll<global::Kampai.Splash.LoadinTipBucketDefinition>();
			global::Kampai.Splash.LoadinTipBucketDefinition loadinTipBucketDefinition = null;
			foreach (global::Kampai.Splash.LoadinTipBucketDefinition item in all)
			{
				if (loadinTipBucketDefinition == null || loadinTipBucketDefinition.Max < item.Max)
				{
					loadinTipBucketDefinition = item;
				}
				if (level >= item.Min && level <= item.Max)
				{
					return item;
				}
			}
			return loadinTipBucketDefinition;
		}
	}
}
