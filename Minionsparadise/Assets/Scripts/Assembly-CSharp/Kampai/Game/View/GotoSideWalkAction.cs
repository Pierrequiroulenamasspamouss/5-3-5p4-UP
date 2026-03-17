namespace Kampai.Game.View
{
	internal sealed class GotoSideWalkAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.CharacterObject minionObject;

		private global::Kampai.Game.Building building;

		private global::UnityEngine.Vector3 lastPosition;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.CharacterObject> relocateSignal;

		private int pathIndex;

		private GoTween tween;

		public GotoSideWalkAction(global::Kampai.Game.View.CharacterObject minionObj, global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IDefinitionService definitionService, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.CharacterObject> relocateSignal, int preferredPathIndex = 0)
			: base(logger)
		{
			minionObject = minionObj;
			this.building = building;
			this.definitionService = definitionService;
			this.relocateSignal = relocateSignal;
			pathIndex = preferredPathIndex;
		}

		public override void Execute()
		{
			string buildingFootprint = definitionService.GetBuildingFootprint(building.Definition.FootprintID);
			global::Kampai.Util.Point point;
			if (BuildingUtil.HasSidewalk(buildingFootprint))
			{
				if (pathIndex == 0)
				{
					point = BuildingUtil.GetClosestBuildingSidewalk(building.Location, minionObject.transform.position, buildingFootprint);
				}
				else
				{
					global::System.Collections.Generic.List<global::Kampai.Util.Point> buildingSideWalkList = BuildingUtil.GetBuildingSideWalkList(building.Location, buildingFootprint);
					point = buildingSideWalkList[global::System.Math.Min(pathIndex, buildingSideWalkList.Count - 1)];
				}
			}
			else
			{
				if (relocateSignal != null && !(building is global::Kampai.Game.DebrisBuilding))
				{
					relocateSignal.Dispatch(minionObject);
					base.Done = true;
					return;
				}
				point = new global::Kampai.Util.Point
				{
					XZProjection = minionObject.transform.position
				};
			}
			global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(point.x, 0f, point.y);
			global::UnityEngine.Vector3 vector2 = VectorUtils.ZeroY(vector - minionObject.transform.position);
			if (vector2 != global::UnityEngine.Vector3.zero)
			{
				minionObject.transform.rotation = global::UnityEngine.Quaternion.LookRotation(vector2);
			}
			lastPosition = minionObject.transform.position;
			minionObject.SetAnimBool("isMoving", true);
			tween = Go.to(minionObject.transform, 1f, new GoTweenConfig().setEaseType(GoEaseType.Linear).position(vector).onComplete(delegate(AbstractGoTween thisTween)
			{
				thisTween.destroy();
				minionObject.SetAnimBool("isMoving", false);
				base.Done = true;
			}));
		}

		public override void Abort()
		{
			if (tween != null)
			{
				tween.complete();
			}
			base.Abort();
		}

		public override void LateUpdate()
		{
			global::UnityEngine.Vector3 position = minionObject.transform.position;
			float num = global::UnityEngine.Vector3.Distance(lastPosition, position);
			minionObject.SetAnimFloat("speed", global::UnityEngine.Mathf.Clamp(num / global::UnityEngine.Time.deltaTime, 0f, 2f));
			lastPosition = position;
		}
	}
}
