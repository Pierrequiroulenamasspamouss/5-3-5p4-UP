namespace Kampai.Game.View
{
	public class VillainManagerMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		private global::Kampai.Game.View.BuildingManagerView buildingManagerView;

		[Inject]
		public global::Kampai.Game.View.VillainManagerView view { get; set; }

		[Inject]
		public global::Kampai.Game.VillainPlayWelcomeSignal welcomeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainGotoCarpetSignal carpetSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainGotoCabanaSignal cabanaSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CreateVillainViewSignal createViewSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Util.INamedCharacterBuilder builder { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		[Inject]
		public global::Kampai.Common.RecreateBuildingSignal recreateSignal { get; set; }

		[PostConstruct]
		public void Init()
		{
			buildingManagerView = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
		}

		public override void OnRegister()
		{
			welcomeSignal.AddListener(PlayWelcome);
			carpetSignal.AddListener(GotoCarpet);
			cabanaSignal.AddListener(GotoCabana);
			createViewSignal.AddListener(CreateView);
		}

		public override void OnRemove()
		{
			welcomeSignal.RemoveListener(PlayWelcome);
			carpetSignal.RemoveListener(GotoCarpet);
			cabanaSignal.RemoveListener(GotoCabana);
			createViewSignal.RemoveListener(CreateView);
		}

		private void PlayWelcome(int id)
		{
			view.Get(id).PlayWelcome();
		}

		private void GotoCabana(int id, int cabanaId)
		{
			global::Kampai.Game.CabanaBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.CabanaBuilding>(cabanaId);
			byInstanceId.Occupied = true;
			global::Kampai.Game.View.CabanaBuildingObject cabanaBuildingObject = buildingManagerView.GetBuildingObject(byInstanceId.ID) as global::Kampai.Game.View.CabanaBuildingObject;
			view.Get(id).GotoCabana(byInstanceId.ID, cabanaBuildingObject.GetRoutingPoint());
			buildingChangeStateSignal.Dispatch(byInstanceId.ID, global::Kampai.Game.BuildingState.Working);
			recreateSignal.Dispatch(byInstanceId);
		}

		private void GotoCarpet(int id)
		{
			global::Kampai.Game.View.NamedCharacterObject orCreateView = GetOrCreateView(id);
			orCreateView.transform.parent = base.transform;
			global::Kampai.Game.NoOpPlot noOpPlot = global::System.Linq.Enumerable.FirstOrDefault(playerService.GetInstancesByType<global::Kampai.Game.NoOpPlot>());
			orCreateView.setLocation(new global::UnityEngine.Vector3(noOpPlot.Location.x, 0f, noOpPlot.Location.y));
		}

		private global::Kampai.Game.View.VillainView GetOrCreateView(int id)
		{
			global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = view.Get(id);
			if (namedCharacterObject == null)
			{
				global::Kampai.Game.Villain byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Villain>(id);
				namedCharacterObject = builder.Build(byInstanceId, base.gameObject);
				view.Add(namedCharacterObject);
			}
			else
			{
				namedCharacterObject.gameObject.SetActive(true);
			}
			return namedCharacterObject as global::Kampai.Game.View.VillainView;
		}

		private void CreateView(int id)
		{
			GetOrCreateView(id);
		}
	}
}
