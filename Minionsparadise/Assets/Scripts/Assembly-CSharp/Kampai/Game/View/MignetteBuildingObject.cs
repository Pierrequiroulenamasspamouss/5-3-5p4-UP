namespace Kampai.Game.View
{
	public class MignetteBuildingObject : global::Kampai.Game.View.TaskableBuildingObject
	{
		private global::System.Collections.Generic.List<global::Kampai.Game.BuildingAnimationDefinition> AnimationDefinitions;

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		internal override void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::System.Collections.Generic.IDictionary<string, global::UnityEngine.RuntimeAnimatorController> controllers, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.Init(building, logger, controllers, definitionService);
			global::Kampai.Game.AnimatingBuildingDefinition animatingBuildingDefinition = building.Definition as global::Kampai.Game.AnimatingBuildingDefinition;
			if (animatingBuildingDefinition != null && animatingBuildingDefinition.AnimationDefinitions != null)
			{
				AnimationDefinitions = (global::System.Collections.Generic.List<global::Kampai.Game.BuildingAnimationDefinition>)animatingBuildingDefinition.AnimationDefinitions;
			}
			else
			{
				logger.Fatal(global::Kampai.Util.FatalCode.BV_NO_DEFAULT_ANIMATION_CONTROLLER, animatingBuildingDefinition.ID.ToString());
			}
		}

		public global::Kampai.Game.View.TaskingMinionObject GetChildMinion(int index)
		{
			return childQueue[index];
		}

		public global::UnityEngine.Vector3 GetRouteLocation(int routeIndex)
		{
			if (routeIndex >= 0 && routeIndex < routes.Length)
			{
				return routes[routeIndex].position;
			}
			return global::UnityEngine.Vector3.zero;
		}

		public global::UnityEngine.Vector3 GetRouteForward(int routeIndex)
		{
			if (routeIndex >= 0 && routeIndex < routes.Length)
			{
				return routes[routeIndex].forward;
			}
			return global::UnityEngine.Vector3.forward;
		}

		public int GetMignetteMinionCount()
		{
			return GetActiveMinionCount();
		}

		public void LoadMignetteAnimationControllers(global::System.Collections.Generic.Dictionary<string, global::UnityEngine.RuntimeAnimatorController> animationControllers)
		{
			foreach (global::Kampai.Game.BuildingAnimationDefinition animationDefinition in AnimationDefinitions)
			{
				if (!buildingControllers.ContainsKey(animationDefinition.CostumeId) && !string.IsNullOrEmpty(animationDefinition.BuildingController))
				{
					global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(animationDefinition.BuildingController);
					if (runtimeAnimatorController == null)
					{
						logger.Fatal(global::Kampai.Util.FatalCode.BV_NO_DEFAULT_ANIMATION_CONTROLLER, animationDefinition.ID.ToString());
						break;
					}
					buildingControllers.Add(animationDefinition.CostumeId, runtimeAnimatorController);
					if (!animationControllers.ContainsKey(animationDefinition.BuildingController))
					{
						animationControllers.Add(animationDefinition.BuildingController, runtimeAnimatorController);
					}
				}
				if (!minionControllers.ContainsKey(animationDefinition.CostumeId) && !string.IsNullOrEmpty(animationDefinition.MinionController))
				{
					global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController2 = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(animationDefinition.MinionController);
					if (runtimeAnimatorController2 == null)
					{
						logger.Fatal(global::Kampai.Util.FatalCode.BV_NO_DEFAULT_ANIMATION_CONTROLLER, animationDefinition.ID.ToString());
						break;
					}
					minionControllers.Add(animationDefinition.CostumeId, runtimeAnimatorController2);
					if (!animationControllers.ContainsKey(animationDefinition.MinionController))
					{
						animationControllers.Add(animationDefinition.MinionController, runtimeAnimatorController2);
					}
				}
			}
		}

		public override void StartAnimating()
		{
		}

		protected override void SetupChild(int routingIndex, global::Kampai.Game.View.TaskingMinionObject taskingChild, global::UnityEngine.RuntimeAnimatorController controller = null)
		{
			taskingChild.RoutingIndex = routingIndex;
			global::Kampai.Game.View.MinionObject minion = taskingChild.Minion;
			if (controller != null)
			{
				minion.SetAnimController(controller);
			}
			int numberOfStations = GetNumberOfStations();
			if (routingIndex < numberOfStations)
			{
				minion.ApplyRootMotion(true);
				minion.EnableRenderers(false);
				AddMinionRigidBody(minion);
				MoveToRoutingPosition(minion, routingIndex);
			}
		}

		public override void UntrackChild(int minionId, global::Kampai.Game.TaskableBuilding building)
		{
			global::Kampai.Util.Point partyPoint = pathFinder.PartyPoint;
			global::UnityEngine.Vector3 position = new global::UnityEngine.Vector3(partyPoint.x, 0f, partyPoint.y);
			global::Kampai.Game.PurchasedLandExpansion byInstanceId = base.playerService.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
			if (!childAnimators.ContainsKey(minionId))
			{
				return;
			}
			global::Kampai.Game.View.TaskingMinionObject taskingMinionObject = childAnimators[minionId];
			global::Kampai.Game.View.MinionObject minion = taskingMinionObject.Minion;
			minion.SetRenderLayer(10);
			minion.ApplyRootMotion(false);
			minion.UnshelveActionQueue();
			bool flag = true;
			global::Kampai.Game.MignetteBuilding mignetteBuilding = building as global::Kampai.Game.MignetteBuilding;
			if (mignetteBuilding != null && mignetteBuilding.Definition.LandExpansionID != 0)
			{
				int landExpansionID = mignetteBuilding.Definition.LandExpansionID;
				if (!byInstanceId.HasPurchased(landExpansionID))
				{
					flag = false;
				}
			}
			if (!flag)
			{
				minion.transform.position = position;
				minion.ClearActionQueue();
			}
			else
			{
				minion.transform.position = ((building == null) ? base.gameObject.transform.position : GetRandomExit(building));
			}
			minion.transform.rotation = global::UnityEngine.Quaternion.identity;
			minion.EnableBlobShadow(true);
			minion.SetAnimatorCullingMode(global::UnityEngine.AnimatorCullingMode.CullUpdateTransforms);
			minion.EnqueueAction(new global::Kampai.Game.View.SetLayerAction(minion, 8, logger, 2));
			RemoveMinionRigidBody(minion);
			UnlinkChild(minionId);
		}

		public override bool CanFadeGFX()
		{
			return false;
		}

		public override bool CanFadeSFX()
		{
			return false;
		}

		private void AddMinionRigidBody(global::Kampai.Game.View.MinionObject minion)
		{
			global::UnityEngine.Rigidbody component = minion.gameObject.GetComponent<global::UnityEngine.Rigidbody>();
			if (component == null)
			{
				component = minion.gameObject.AddComponent<global::UnityEngine.Rigidbody>();
				component.useGravity = false;
				component.isKinematic = true;
			}
		}

		private void RemoveMinionRigidBody(global::Kampai.Game.View.MinionObject minion)
		{
			global::UnityEngine.Rigidbody component = minion.gameObject.GetComponent<global::UnityEngine.Rigidbody>();
			if (component != null)
			{
				global::UnityEngine.Object.Destroy(component);
			}
		}
	}
}
