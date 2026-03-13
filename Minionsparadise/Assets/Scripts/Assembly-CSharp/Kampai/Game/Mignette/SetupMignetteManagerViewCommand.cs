namespace Kampai.Game.Mignette
{
	public abstract class SetupMignetteManagerViewCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupMignetteManagerViewCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.MignetteGameModel mignetteModel { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		protected T CreateManagerView<T>(string viewName) where T : global::Kampai.Game.Mignette.View.MignetteManagerView
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject(viewName);
			gameObject.transform.parent = contextView.transform;
			T val = gameObject.AddComponent<T>();
			val.MignetteBuildingObject = GetMignetteBuildingObject();
			return val;
		}

		protected void InitializeChildObjects(global::Kampai.Game.Mignette.View.MignetteManagerView managerView)
		{
			global::Kampai.Game.MignetteBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MignetteBuilding>(managerView.MignetteBuildingObject.ID);
			global::Kampai.Game.MignetteBuildingDefinition mignetteBuildingDefinition = byInstanceId.MignetteBuildingDefinition;
			global::UnityEngine.Transform transform = managerView.transform;
			if (mignetteBuildingDefinition.ChildObjects == null)
			{
				return;
			}
			foreach (global::Kampai.Game.MignetteChildObjectDefinition childObject in mignetteBuildingDefinition.ChildObjects)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(childObject.Prefab), transform.position, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject;
				global::UnityEngine.Transform transform2 = gameObject.transform;
				transform2.parent = transform;
				if (childObject.IsLocal)
				{
					transform2.localPosition = childObject.Position;
					transform2.localRotation = global::UnityEngine.Quaternion.Euler(0f, childObject.Rotation, 0f);
				}
				else
				{
					transform2.position = childObject.Position;
					transform2.rotation = global::UnityEngine.Quaternion.Euler(0f, childObject.Rotation, 0f);
				}
			}
		}

		protected global::Kampai.Game.View.MignetteBuildingObject GetMignetteBuildingObject()
		{
			global::UnityEngine.GameObject instance = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER);
			global::Kampai.Game.View.BuildingManagerView component = instance.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(mignetteModel.BuildingId);
			global::Kampai.Game.View.MignetteBuildingObject mignetteBuildingObject = buildingObject as global::Kampai.Game.View.MignetteBuildingObject;
			if (mignetteBuildingObject == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.MIGNETTE_BAD_BUILDING);
			}
			return mignetteBuildingObject;
		}
	}
}
