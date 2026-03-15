namespace Kampai.Game
{
	public class TikiBarViewPickController : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("TikiBarViewPickController") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::UnityEngine.GameObject EndHitObject { get; set; }

		[Inject]
		public global::Kampai.Game.ITikiBarService TikiBarService { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayStickerbookSignal DisplayStickerbookSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.GetWayFinderSignal GetWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowQuestPanelSignal ShowQuestPanelSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService QuestService { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject NamedCharacterManagerGO { get; set; }

		public override void Execute()
		{
			if (EndHitObject.name == "StampAlbum")
			{
				logger.Debug("StampAlbum was clicked in Tiki bar view!");
				DisplayStickerbookSignal.Dispatch();
				return;
			}
			if (EndHitObject.name == "Shelve" || EndHitObject.name == "building_313")
			{
				logger.Debug("Shelve was clicked in Tiki bar view, or Tikibar was clicked while Zoomed Out!");
				HandleClick(NamedCharacterManagerGO.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>().Get(78));
				return;
			}
			global::Kampai.Game.View.CharacterObject componentInParent = EndHitObject.GetComponentInParent<global::Kampai.Game.View.CharacterObject>();
			if (componentInParent != null)
			{
				logger.Debug("{0} was clicked in Tiki bar view!", componentInParent.name);
				HandleClick(componentInParent);
			}
			else
			{
				logger.Error("{0} clicked event was ignored!", EndHitObject.name);
			}
		}

		private void HandleClick(global::Kampai.Game.View.CharacterObject characterObject)
		{
			GetWayFinderSignal.Dispatch(78, delegate(int trackedId, global::Kampai.UI.View.IWayFinderView wayFinderView)
			{
				if (wayFinderView != null)
				{
					ClickedOnCharacter(characterObject, wayFinderView);
				}
				else
				{
					ShowQuestBook();
				}
			});
		}

		private void ShowQuestBook()
		{
			foreach (global::Kampai.Game.IQuestController value in QuestService.GetQuestMap().Values)
			{
				global::Kampai.Game.QuestDefinition definition = value.Definition;
				if (definition.SurfaceType != global::Kampai.Game.QuestSurfaceType.Automatic && definition.SurfaceType != global::Kampai.Game.QuestSurfaceType.ProcedurallyGenerated && value.State == global::Kampai.Game.QuestState.RunningTasks)
				{
					ShowQuestPanelSignal.Dispatch(value.ID);
					break;
				}
			}
		}

		private void ClickedOnCharacter(global::Kampai.Game.View.CharacterObject characterObject, global::Kampai.UI.View.IWayFinderView wayFinder)
		{
			int iD = characterObject.ID;
			global::Kampai.Game.Character byInstanceId = PlayerService.GetByInstanceId<global::Kampai.Game.Character>(iD);
			if (byInstanceId == null)
			{
				logger.Warning("Could not find named character for instance id:{0} ", iD);
			}
			else if (TikiBarService.IsCharacterSitting(byInstanceId))
			{
				wayFinder.SimulateClick();
			}
			else
			{
				logger.Warning("Ignoring clicks to {0} since they are not sitting", characterObject.name);
			}
		}
	}
}
