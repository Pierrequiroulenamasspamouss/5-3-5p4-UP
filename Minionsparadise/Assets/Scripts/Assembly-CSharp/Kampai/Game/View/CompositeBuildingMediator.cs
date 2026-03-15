namespace Kampai.Game.View
{
	public class CompositeBuildingMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private const float MINION_REACT_RADIUS = 20f;

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CompositeBuildingMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.CompositeBuilding compositeBuilding;

		[Inject]
		public global::Kampai.Game.View.CompositeBuildingView view { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CompositeBuildingPieceAddedSignal compositeBuildingPieceAddedSignal { get; set; }

		[Inject]
		public global::Kampai.Common.MinionReactInRadiusSignal minionReactInRadiusSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			compositeBuilding = GetCompositeBuilding();
			view.SetupPieces(GetAttachedPieces());
			compositeBuildingPieceAddedSignal.AddListener(OnCompositePieceAdded);
		}

		public override void OnRemove()
		{
			compositeBuildingPieceAddedSignal.RemoveListener(OnCompositePieceAdded);
			base.OnRemove();
		}

		private global::Kampai.Game.CompositeBuilding GetCompositeBuilding()
		{
			global::Kampai.Game.View.CompositeBuildingObject component = GetComponent<global::Kampai.Game.View.CompositeBuildingObject>();
			if (component != null)
			{
				return component.compositeBuilding;
			}
			logger.Error("CompositeBuildingMediator: could not find CompositeBuilding for building " + base.gameObject.name);
			return null;
		}

		private global::System.Collections.Generic.IList<global::Kampai.Game.CompositeBuildingPiece> GetAttachedPieces()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.CompositeBuildingPiece> list = new global::System.Collections.Generic.List<global::Kampai.Game.CompositeBuildingPiece>();
			foreach (int attachedCompositePieceID in compositeBuilding.AttachedCompositePieceIDs)
			{
				global::Kampai.Game.CompositeBuildingPiece byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.CompositeBuildingPiece>(attachedCompositePieceID);
				if (byInstanceId == null)
				{
					logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "CompositeBuildingObject, Piece ID not owned: " + attachedCompositePieceID);
				}
				else
				{
					list.Add(byInstanceId);
				}
			}
			return list;
		}

		private void OnCompositePieceAdded(global::Kampai.Game.CompositeBuilding buildingAddedTo)
		{
			if (compositeBuilding == buildingAddedTo)
			{
				int numPieces = view.GetNumPieces();
				global::System.Collections.Generic.IList<global::Kampai.Game.CompositeBuildingPiece> attachedPieces = GetAttachedPieces();
				for (int i = numPieces; i < attachedPieces.Count; i++)
				{
					view.AddNewlyCreatedPiece(attachedPieces[i], playSFXSignal);
				}
				minionReactInRadiusSignal.Dispatch(20f, base.transform.position);
			}
		}

		public void PlayShuffleSequence(global::System.Collections.Generic.IList<int> newPieceOrder)
		{
			view.PlayShuffleSequence(newPieceOrder, playSFXSignal);
		}
	}
}
