namespace Kampai.Game.Mignette.View
{
	public class MignetteBuildingCooldownMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::Kampai.Game.Mignette.View.MignetteBuildingViewObject buildingViewObject;

		private global::Kampai.Game.View.MignetteBuildingObject mignetteBuildingObject;

		private global::Kampai.Game.MignetteBuilding mignetteBuilding;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingCooldownUpdateViewSignal cooldownUpdateViewSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingCooldownCompleteSignal cooldownCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayLocalAudioSignal localAudioSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			buildingViewObject = GetComponent<global::Kampai.Game.Mignette.View.MignetteBuildingViewObject>();
			mignetteBuildingObject = GetComponent<global::Kampai.Game.View.MignetteBuildingObject>();
			cooldownUpdateViewSignal.AddListener(OnUpdateCooldownView);
			cooldownCompleteSignal.AddListener(OnCooldownComplete);
			buildingViewObject.ResetCooldownView(localAudioSignal);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			cooldownUpdateViewSignal.RemoveListener(OnUpdateCooldownView);
			cooldownCompleteSignal.RemoveListener(OnCooldownComplete);
		}

		public void Start()
		{
			mignetteBuilding = playerService.GetByInstanceId<global::Kampai.Game.MignetteBuilding>(mignetteBuildingObject.ID);
		}

		public void OnCooldownComplete(int buildingID)
		{
			if (mignetteBuildingObject.ID == buildingID)
			{
				buildingViewObject.ResetCooldownView(localAudioSignal);
				buildingViewObject.DestroyDynamicCoolDownObjects();
			}
		}

		public void OnUpdateCooldownView(int buildingID)
		{
			if (mignetteBuildingObject.ID == buildingID)
			{
				if (!buildingViewObject.IsDynamicCooldownObjectsLoaded() && mignetteBuilding.State == global::Kampai.Game.BuildingState.Cooldown)
				{
					LoadDynamicCooldownObjects();
				}
				int num = timeService.CurrentTime();
				int stateStartTime = mignetteBuilding.StateStartTime;
				int num2 = num - stateStartTime;
				float pctDone = (float)num2 / (float)mignetteBuilding.GetCooldown();
				if (mignetteBuilding.State == global::Kampai.Game.BuildingState.Idle)
				{
					pctDone = 1f;
				}
				buildingViewObject.UpdateCooldownView(localAudioSignal, mignetteBuilding.MignetteData, pctDone);
			}
		}

		private void LoadDynamicCooldownObjects()
		{
			global::Kampai.Game.MignetteBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MignetteBuilding>(mignetteBuildingObject.ID);
			global::Kampai.Game.MignetteBuildingDefinition mignetteBuildingDefinition = byInstanceId.MignetteBuildingDefinition;
			global::UnityEngine.Transform parent = base.gameObject.transform;
			if (mignetteBuildingDefinition.CooldownObjects == null)
			{
				return;
			}
			foreach (global::Kampai.Game.MignetteChildObjectDefinition cooldownObject in mignetteBuildingDefinition.CooldownObjects)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(cooldownObject.Prefab));
				buildingViewObject.AddDynamicCoolDownObject(gameObject);
				global::UnityEngine.Transform transform = gameObject.transform;
				transform.parent = parent;
				if (cooldownObject.IsLocal)
				{
					transform.localPosition = cooldownObject.Position;
					transform.localRotation = global::UnityEngine.Quaternion.Euler(0f, cooldownObject.Rotation, 0f);
				}
				else
				{
					transform.position = cooldownObject.Position;
					transform.rotation = global::UnityEngine.Quaternion.Euler(0f, cooldownObject.Rotation, 0f);
				}
			}
		}
	}
}
