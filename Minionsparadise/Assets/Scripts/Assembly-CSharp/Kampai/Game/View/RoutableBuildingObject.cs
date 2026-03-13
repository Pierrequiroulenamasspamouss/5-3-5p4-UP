namespace Kampai.Game.View
{
	public class RoutableBuildingObject : global::Kampai.Game.View.BuildingObject
	{
		protected global::UnityEngine.Transform[] routes;

		private bool routeToSlot;

		protected int stations;

		internal override void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::System.Collections.Generic.IDictionary<string, global::UnityEngine.RuntimeAnimatorController> controllers, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.Init(building, logger, controllers, definitionService);
			global::Kampai.Game.BuildingDefinition definition = building.Definition;
			if (definition == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.BV_ILLEGAL_ROUTABLE_DEFINITION, building.Definition.ID.ToString());
			}
			routeToSlot = definition.RouteToSlot;
			stations = definition.WorkStations;
			routes = new global::UnityEngine.Transform[stations];
			bool isRepaired = building.IsBuildingRepaired();
			for (int i = 0; i < stations; i++)
			{
				global::UnityEngine.GameObject route = GetRoute(i, isRepaired);
				if (route == null)
				{
					routes = null;
					break;
				}
				routes[i] = route.transform;
			}
		}

		public global::UnityEngine.Transform GetRouteTransform(int routeIndex)
		{
			return routes[routeIndex];
		}

		internal global::UnityEngine.Vector3 GetRoutePosition(int routeIndex, global::Kampai.Game.Building building, global::UnityEngine.Vector3 startingPosition)
		{
			if (routeToSlot && routeIndex >= 0 && routeIndex < routes.Length)
			{
				return routes[routeIndex].position;
			}
			global::Kampai.Util.Point closestBuildingSidewalk = BuildingUtil.GetClosestBuildingSidewalk(building.Location, startingPosition, definitionService.GetBuildingFootprint(building.Definition.FootprintID));
			return new global::UnityEngine.Vector3(closestBuildingSidewalk.x, 0f, closestBuildingSidewalk.y);
		}

		private global::UnityEngine.GameObject GetRoute(int index, bool isRepaired)
		{
			global::UnityEngine.GameObject gameObject = base.gameObject.FindChild("route" + index);
			if (gameObject == null && isRepaired)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.BV_NO_ROUTE, ID, "BV_NO_ROUTE: Building ID: {0}, Route Index: {1}", ID, index);
			}
			return gameObject;
		}

		internal int GetNumberOfStations()
		{
			return routes.Length;
		}

		internal virtual global::UnityEngine.Vector3 GetRouteRotation(int routeIndex)
		{
			if (routeIndex >= 0 && routeIndex < routes.Length)
			{
				return routes[routeIndex].rotation.eulerAngles;
			}
			return global::UnityEngine.Vector3.zero;
		}

		public virtual void MoveToRoutingPosition(global::Kampai.Game.View.CharacterObject characterObject, int routingIndex)
		{
			if (routingIndex < 0 || routingIndex >= routes.Length)
			{
				logger.Error("MoveToRoutingPosition: routingIndex {0} out of range (routes.Length={1})", routingIndex, routes.Length);
				return;
			}
			global::UnityEngine.Transform transform = routes[routingIndex].transform;
			global::UnityEngine.Transform transform2 = characterObject.gameObject.transform;
			global::UnityEngine.Vector3 vector = ((!(transform2.parent != null)) ? new global::UnityEngine.Vector3(0f, 0f, 0f) : transform2.parent.transform.position);
			global::UnityEngine.Vector3 vector2 = ((!(transform.parent != null)) ? new global::UnityEngine.Vector3(0f, 0f, 0f) : transform.parent.transform.position);
			transform2.localPosition = vector2 + transform.localPosition - vector;
			transform2.rotation = transform.rotation;
		}
	}
}
