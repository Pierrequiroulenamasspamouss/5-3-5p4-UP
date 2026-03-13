namespace Kampai.Main
{
	public class CheckDLCTierCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CheckDLCTierCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Common.IManifestService manifestService { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
			list.AddRange(checkPrestige());
			list.AddRange(checkUnlocks());
			int num = 0;
			foreach (string item in list)
			{
				string assetLocation = manifestService.GetAssetLocation(item);
				if (!string.IsNullOrEmpty(assetLocation))
				{
					int bundleTier = manifestService.GetBundleTier(assetLocation);
					if (bundleTier != int.MaxValue && bundleTier > num)
					{
						num = bundleTier;
					}
				}
			}
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.TIER_ID);
			int quantity2 = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.TIER_GATE_ID);
			if (num > quantity2)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Current Tier Gate " + quantity2 + " Is less than required Tier " + num);
				playerService.AlterQuantity(global::Kampai.Game.StaticItem.TIER_GATE_ID, num - quantity2);
			}
			if (num > quantity)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Current Tier " + quantity + " Is less than required Tier " + num);
				playerService.AlterQuantity(global::Kampai.Game.StaticItem.TIER_ID, num - quantity);
			}
		}

		private global::System.Collections.Generic.List<string> checkPrestige()
		{
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
			global::System.Collections.Generic.IList<global::Kampai.Game.Prestige> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.Prestige>();
			foreach (global::Kampai.Game.Prestige item2 in instancesByType)
			{
				global::Kampai.Game.NamedCharacterDefinition definition;
				if (definitionService.TryGet<global::Kampai.Game.NamedCharacterDefinition>(item2.Definition.TrackedDefinitionID, out definition))
				{
					string text = dlcService.GetDownloadQualityLevel().ToUpper();
					if (!string.IsNullOrEmpty(definition.Prefab) && !string.IsNullOrEmpty(text))
					{
						string item = string.Format("{0}_{1}", definition.Prefab, text);
						list.Add(item);
					}
				}
				else
				{
					global::Kampai.Game.CostumeItemDefinition definition2;
					if (!definitionService.TryGet<global::Kampai.Game.CostumeItemDefinition>(item2.Definition.TrackedDefinitionID, out definition2))
					{
						continue;
					}
					if (string.IsNullOrEmpty(definition2.Skeleton))
					{
						list.Add(definition2.Skeleton);
					}
					if (definition2.MeshList == null || definition2.MeshList.Count <= 0)
					{
						continue;
					}
					foreach (string mesh in definition2.MeshList)
					{
						list.Add(mesh);
					}
				}
			}
			return list;
		}

		private global::System.Collections.Generic.List<string> checkUnlocks()
		{
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
			global::System.Collections.Generic.IList<global::Kampai.Game.BuildingDefinition> unlockedDefsByType = playerService.GetUnlockedDefsByType<global::Kampai.Game.BuildingDefinition>();
			foreach (global::Kampai.Game.BuildingDefinition item in unlockedDefsByType)
			{
				global::Kampai.Game.ConnectableBuildingDefinition connectableBuildingDefinition = item as global::Kampai.Game.ConnectableBuildingDefinition;
				if (connectableBuildingDefinition != null && connectableBuildingDefinition.piecePrefabs != null)
				{
					for (int i = 0; i < connectableBuildingDefinition.GetNumPrefabs(); i++)
					{
						list.Add(connectableBuildingDefinition.GetPrefab(i));
					}
				}
				else if (!string.IsNullOrEmpty(item.GetPrefab()))
				{
					list.Add(item.GetPrefab());
				}
			}
			return list;
		}
	}
}
