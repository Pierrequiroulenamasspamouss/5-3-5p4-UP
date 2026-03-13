namespace Kampai.Game.View
{
	public class StageBuildingObject : global::Kampai.Game.View.AnimatingBuildingObject
	{
		private const string Base_Layer_StageIdle = "Base Layer.StageIdle";

		private const string BOOL_HIDE_MIC = "HideMic";

		private global::UnityEngine.Transform stageTransform;

		private global::Kampai.Game.View.StageBuildingSetting setting;

		internal override void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::System.Collections.Generic.IDictionary<string, global::UnityEngine.RuntimeAnimatorController> controllers, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.Init(building, logger, controllers, definitionService);
			if (routes != null && routes.Length > 0)
			{
				stageTransform = routes[0];
			}
			if (stageTransform == null)
			{
				stageTransform = base.transform;
			}
			global::Kampai.Game.BuildingState state = building.State;
			if (state != global::Kampai.Game.BuildingState.Inaccessible && state != global::Kampai.Game.BuildingState.Inactive && state != global::Kampai.Game.BuildingState.Broken)
			{
				setting = base.gameObject.GetComponent<global::Kampai.Game.View.StageBuildingSetting>();
				if (setting == null)
				{
					logger.Error("Stage state is {0}. StageBuildingSetting is null.", state);
				}
				else
				{
					UpdateStageState(state);
				}
			}
		}

		public global::UnityEngine.Transform GetStageTransform()
		{
			return stageTransform;
		}

		public void UpdateStageState(global::Kampai.Game.BuildingState stageState)
		{
			if (setting == null)
			{
				logger.Error("StageBuildingSetting is null");
				return;
			}
			switch (stageState)
			{
			case global::Kampai.Game.BuildingState.SocialAvailable:
				setting.SocialAvailableObject.SetActive(true);
				setting.SocialCompleteObject.SetActive(false);
				setting.LeftTorchVFX.Stop();
				setting.RightTorchVFX.Stop();
				break;
			case global::Kampai.Game.BuildingState.SocialComplete:
				setting.SocialAvailableObject.SetActive(true);
				setting.SocialCompleteObject.SetActive(true);
				setting.LeftTorchVFX.Play();
				setting.RightTorchVFX.Play();
				break;
			default:
				setting.SocialAvailableObject.SetActive(false);
				setting.SocialCompleteObject.SetActive(false);
				setting.LeftTorchVFX.Stop();
				setting.RightTorchVFX.Stop();
				break;
			}
		}

		public void SetSpinMic()
		{
			SetAnimTrigger("OnSpinMic");
		}

		public void SetHideMic(bool enable)
		{
			SetAnimBool("HideMic", enable);
		}

		public void PerformanceStarts()
		{
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorArgumentsAction(this, logger, "HideMic", false), true);
			EnqueueAction(new global::Kampai.Game.View.PlayMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.StageIdle"), logger));
		}
	}
}
