namespace Kampai.Game.View
{
	public class OpenSellBuildingModelCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
			if (playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) < marketplaceDefinition.LevelGate)
			{
				string type = localService.GetString("MarketplaceUnlock", marketplaceDefinition.LevelGate);
				popupMessageSignal.Dispatch(type, global::Kampai.UI.View.PopupMessageType.NORMAL);
				return;
			}
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "screen_StorageBuilding");
			iGUICommand.skrimScreen = "StorageSkrim";
			iGUICommand.darkSkrim = true;
			iGUICommand.Args.Add(GetStorageBuilding());
			iGUICommand.Args.Add(global::Kampai.UI.View.StorageBuildingModalTypes.SELL);
			guiService.Execute(iGUICommand);
		}

		private global::Kampai.Game.StorageBuilding GetStorageBuilding()
		{
			global::Kampai.Game.StorageBuilding result = null;
			using (global::System.Collections.Generic.IEnumerator<global::Kampai.Game.StorageBuilding> enumerator = playerService.GetByDefinitionId<global::Kampai.Game.StorageBuilding>(3018).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					global::Kampai.Game.StorageBuilding current = enumerator.Current;
					result = current;
				}
			}
			return result;
		}
	}
}
