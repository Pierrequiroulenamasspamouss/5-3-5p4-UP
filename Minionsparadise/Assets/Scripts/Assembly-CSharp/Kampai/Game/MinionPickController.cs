namespace Kampai.Game
{
	public class MinionPickController : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Game.View.CharacterObject characterObject;

		[Inject]
		public global::UnityEngine.GameObject selectedGameObject { get; set; }

		[Inject]
		public global::Kampai.Game.TapMinionSignal tapMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TSMCharacterSelectedSignal tsmCharacterSelectedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.NamedCharacterSelectedSignal namedCharacterSelectedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.GetWayFinderSignal getWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			if (minionPartyInstance.IsPartyHappening)
			{
				return;
			}
			characterObject = selectedGameObject.GetComponentInParent(typeof(global::Kampai.Game.View.CharacterObject)) as global::Kampai.Game.View.CharacterObject;
			if (!(characterObject != null))
			{
				return;
			}
			if (characterObject is global::Kampai.Game.View.VillainView)
			{
				global::Kampai.Game.Villain byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Villain>(characterObject.ID);
				if (byInstanceId != null)
				{
					getWayFinderSignal.Dispatch(byInstanceId.CabanaBuildingId, OnGetWayFinder);
				}
			}
			else if (characterObject is global::Kampai.Game.View.TSMCharacterView)
			{
				tsmCharacterSelectedSignal.Dispatch();
			}
			else
			{
				getWayFinderSignal.Dispatch(characterObject.ID, OnGetWayFinder);
				namedCharacterSelectedSignal.Dispatch(characterObject);
			}
		}

		private void OnGetWayFinder(int trackedId, global::Kampai.UI.View.IWayFinderView wayFinderView)
		{
			if (wayFinderView != null)
			{
				wayFinderView.SimulateClick();
				return;
			}
			global::Kampai.Game.View.MinionObject minionObject = characterObject as global::Kampai.Game.View.MinionObject;
			if (minionObject != null)
			{
				tapMinionSignal.Dispatch(minionObject.ID);
			}
		}
	}
}
