namespace Kampai.Game.View
{
	public class CraftableBuildingObject : global::Kampai.Game.View.AnimatingBuildingObject, global::Kampai.Game.View.IRequiresBuildingScaffolding
	{
		private readonly global::System.Collections.Generic.List<global::UnityEngine.Material> craftingMaterials = new global::System.Collections.Generic.List<global::UnityEngine.Material>();

		private readonly global::System.Collections.Generic.List<GoTween> tweenList = new global::System.Collections.Generic.List<GoTween>();

		private bool isHighlighted;

		public float highlightedDuration = 0.25f;

		private void Start()
		{
			for (int i = 0; i < base.objectRenderers.Length; i++)
			{
				global::UnityEngine.Renderer renderer = base.objectRenderers[i];
				for (int j = 0; j < renderer.materials.Length; j++)
				{
					global::UnityEngine.Material material = renderer.materials[j];
					if ((!material.HasProperty(global::Kampai.Util.GameConstants.ShaderProperties.Blend.DstBlend) || (int)material.GetFloat(global::Kampai.Util.GameConstants.ShaderProperties.Blend.DstBlend) != 1) && !material.name.Contains("Platform") && !material.name.Contains("Shadow") && material.HasProperty(global::Kampai.Util.GameConstants.ShaderProperties.Procedural.BlendedColor))
					{
						craftingMaterials.Add(material);
					}
				}
			}
		}

		internal void SetWorking()
		{
			if (!IsInAnimatorState(GetHashAnimationState("Base Layer.Loop")))
			{
				if (IsInAnimatorState(GetHashAnimationState("Base Layer.Wait")))
				{
					EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, OnlyStateEnabled("OnStop"), logger));
				}
				EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, "OnLoop", logger));
			}
		}

		internal void SetWait()
		{
			if (IsInAnimatorState(GetHashAnimationState("Base Layer.Loop")))
			{
				EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, OnlyStateEnabled("OnWait"), logger));
			}
		}

		internal void SetIdle()
		{
			if (IsInAnimatorState(GetHashAnimationState("Base Layer.Loop")) || IsInAnimatorState(GetHashAnimationState("Base Layer.Wait")))
			{
				EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, OnlyStateEnabled("OnStop"), logger));
			}
		}

		public void EnableHighLightBuilding(bool canCraftRecipe)
		{
			if (!isHighlighted)
			{
				ClearMaterialTweens();
				for (int i = 0; i < craftingMaterials.Count; i++)
				{
					global::UnityEngine.Material self = craftingMaterials[i];
					tweenList.Add(self.colorTo(highlightedDuration, (!canCraftRecipe) ? global::Kampai.Util.GameConstants.Building.INVALID_CRAFTING_RECIPE_DROP : global::Kampai.Util.GameConstants.Building.VALID_CRAFTING_RECIPE_DROP, "_BlendedColor"));
				}
				isHighlighted = true;
			}
		}

		public void DisableHighLightBuilding()
		{
			if (isHighlighted)
			{
				ClearMaterialTweens();
				for (int i = 0; i < craftingMaterials.Count; i++)
				{
					global::UnityEngine.Material self = craftingMaterials[i];
					tweenList.Add(self.colorTo(highlightedDuration, global::UnityEngine.Color.clear, "_BlendedColor"));
					isHighlighted = false;
				}
			}
		}

		private void ClearMaterialTweens()
		{
			for (int i = 0; i < tweenList.Count; i++)
			{
				tweenList[i].destroy();
			}
			tweenList.Clear();
		}

		public override void SetState(global::Kampai.Game.BuildingState newState)
		{
			base.SetState(newState);
			if (buildingState != newState)
			{
				buildingState = newState;
				switch (newState)
				{
				case global::Kampai.Game.BuildingState.Working:
				case global::Kampai.Game.BuildingState.HarvestableAndWorking:
					SetWorking();
					break;
				case global::Kampai.Game.BuildingState.Inactive:
				case global::Kampai.Game.BuildingState.Idle:
					SetIdle();
					break;
				case global::Kampai.Game.BuildingState.Harvestable:
					SetWait();
					break;
				}
			}
		}
	}
}
