namespace Kampai.Game.View
{
	public class OpenOrderBoardCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.OrderBoard orderBoard { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDReminderSignal showHUDReminderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.OrderBoard byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.OrderBoard>(309);
			if (byInstanceId == null || byInstanceId.State != global::Kampai.Game.BuildingState.Inaccessible)
			{
				OpenGUI();
			}
		}

		private void OpenGUI()
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingZoomSignal>().Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.IN, global::Kampai.Game.BuildingZoomType.ORDERBOARD, ZoomComplete));
		}

		public void ZoomComplete()
		{
			showHUDReminderSignal.Dispatch(false);
			closeSignal.Dispatch(null);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "screen_OrderBoard");
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(orderBoard.ID);
			if (!(buildingObject == null))
			{
				OrderBoardBuildingTicketsView component2 = buildingObject.GetComponent<OrderBoardBuildingTicketsView>();
				iGUICommand.Args.Add(component2);
				iGUICommand.Args.Add(orderBoard);
				iGUICommand.skrimScreen = "OrderBoardSkrim";
				iGUICommand.darkSkrim = true;
				guiService.Execute(iGUICommand);
			}
		}
	}
}
