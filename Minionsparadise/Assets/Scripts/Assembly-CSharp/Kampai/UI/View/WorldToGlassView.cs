namespace Kampai.UI.View
{
	public abstract class WorldToGlassView : global::strange.extensions.mediation.impl.View, global::Kampai.Game.IDefinitionsHotSwapHandler, global::Kampai.UI.View.IWorldToGlassView
	{
		public bool isHidden;

		public bool isForceHideEnabled;

		protected int m_trackedId;

		protected global::UnityEngine.Vector3 UIOffset;

		protected global::Kampai.Util.IKampaiLogger logger;

		protected global::Kampai.Game.IPlayerService playerService;

		protected global::Kampai.Main.ILocalizationService localizationService;

		protected global::Kampai.UI.IPositionService positionService;

		protected global::Kampai.Game.View.ActionableObject targetObject;

		protected global::strange.extensions.context.api.ICrossContextCapable gameContext;

		protected global::Kampai.UI.View.WorldToGlassUISettings m_Settings;

		protected global::UnityEngine.Transform m_transform;

		protected global::Kampai.UI.View.WorldToGlassUIModal m_modal;

		public int TrackedId
		{
			get
			{
				return m_trackedId;
			}
		}

		public global::UnityEngine.GameObject GameObject
		{
			get
			{
				return base.gameObject;
			}
		}

		protected abstract string UIName { get; }

		internal void Init(global::Kampai.UI.IPositionService positionService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IPlayerService playerService, global::Kampai.Main.ILocalizationService localizationService)
		{
			this.positionService = positionService;
			this.playerService = playerService;
			this.localizationService = localizationService;
			this.logger = logger;
			this.gameContext = gameContext;
			m_transform = base.transform;
			m_modal = GetComponent<global::Kampai.UI.View.WorldToGlassUIModal>();
			m_Settings = m_modal.Settings;
			m_trackedId = m_Settings.TrackedId;
			UIOffset = global::UnityEngine.Vector3.zero;
			global::Kampai.Game.View.BuildingManagerView component = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER).GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetScaffoldingBuildingObject(m_trackedId) ?? component.GetBuildingObject(m_trackedId);
			if (buildingObject != null)
			{
				targetObject = buildingObject;
			}
			else
			{
				targetObject = global::Kampai.Game.View.ActionableObjectManagerView.GetFromAllObjects(m_trackedId);
			}
			base.name = string.Format("{0}-{1}", UIName, TrackedId);
			LoadModalData(m_modal);
			Hide();
		}

		public virtual void OnDefinitionsHotSwap(global::Kampai.Game.IDefinitionService definitionService)
		{
			LoadModalData(m_modal);
		}

		protected abstract void LoadModalData(global::Kampai.UI.View.WorldToGlassUIModal modal);

		protected virtual void OnHide()
		{
			global::UnityEngine.UI.Image[] componentsInChildren = m_transform.GetComponentsInChildren<global::UnityEngine.UI.Image>(true);
			foreach (global::UnityEngine.UI.Image image in componentsInChildren)
			{
				image.enabled = false;
			}
		}

		protected void Hide()
		{
			if (!isHidden)
			{
				isHidden = true;
				OnHide();
			}
		}

		public virtual void SetForceHide(bool forceHide)
		{
			isForceHideEnabled = forceHide;
		}

		protected virtual void OnShow()
		{
			global::UnityEngine.UI.Image[] componentsInChildren = m_transform.GetComponentsInChildren<global::UnityEngine.UI.Image>(true);
			foreach (global::UnityEngine.UI.Image image in componentsInChildren)
			{
				image.enabled = true;
			}
		}

		protected void Show()
		{
			if (isHidden)
			{
				isHidden = false;
				OnShow();
			}
		}

		public virtual global::UnityEngine.Vector3 GetIndicatorPosition()
		{
			global::Kampai.Game.View.BuildingObject buildingObject = targetObject as global::Kampai.Game.View.BuildingObject;
			if (buildingObject != null)
			{
				return buildingObject.IndicatorPosition;
			}
			global::Kampai.Game.View.CharacterObject characterObject = targetObject as global::Kampai.Game.View.CharacterObject;
			if (characterObject != null)
			{
				if (characterObject.DefinitionID == 70000)
				{
					global::Kampai.Game.View.BuildingManagerView component = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER).GetComponent<global::Kampai.Game.View.BuildingManagerView>();
					global::Kampai.Game.View.BuildingObject buildingObject2 = component.GetScaffoldingBuildingObject(313) ?? component.GetBuildingObject(313);
					return buildingObject2.IndicatorPosition;
				}
				return characterObject.GetIndicatorPosition();
			}
			global::Kampai.Game.View.MasterPlanObject masterPlanObject = targetObject as global::Kampai.Game.View.MasterPlanObject;
			if (masterPlanObject != null)
			{
				UIOffset = global::Kampai.Util.GameConstants.UI.MASTER_PLAN_COOLDOWN_OFFSET;
				return masterPlanObject.GetIndicatorPosition();
			}
			return global::UnityEngine.Vector3.zero;
		}

		internal virtual bool CanUpdate()
		{
			if (targetObject == null)
			{
				TargetObjectNullResponse();
				return false;
			}
			return true;
		}

		internal virtual void TargetObjectNullResponse()
		{
			logger.Warning("Removing UI id: {0} since the target object does not exist anymore!", m_trackedId);
			Hide();
		}

		internal virtual void OnUpdatePosition(global::Kampai.UI.PositionData positionData)
		{
			m_transform.position = positionData.WorldPositionInUI;
			m_transform.localPosition = VectorUtils.ZeroZ(m_transform.localPosition);
		}

		internal void LateUpdate()
		{
			if (!CanUpdate())
			{
				Hide();
				return;
			}
			global::Kampai.UI.PositionData positionData = positionService.GetPositionData(GetIndicatorPosition() + UIOffset);
			OnUpdatePosition(positionData);
			if (isForceHideEnabled)
			{
				Hide();
			}
			else
			{
				Show();
			}
		}
	}
}
