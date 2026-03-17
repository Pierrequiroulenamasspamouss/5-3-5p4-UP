public static class BuildingUtil
{
	public static int GetFootprintWidth(string footprint)
	{
		int num = 0;
		foreach (char c in footprint)
		{
			if (c != '|')
			{
				num++;
				continue;
			}
			break;
		}
		return num;
	}

	public static int GetFootprintDepth(string footprint)
	{
		int num = 1;
		foreach (char c in footprint)
		{
			if (c == '|')
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHarvestTimeForTaskableBuilding(global::Kampai.Game.TaskableBuilding building, global::Kampai.Game.IDefinitionService definitionService)
	{
		int result = 0;
		global::Kampai.Game.ResourceBuilding resourceBuilding = building as global::Kampai.Game.ResourceBuilding;
		if (resourceBuilding != null)
		{
			int itemId = resourceBuilding.Definition.ItemId;
			global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(itemId);
			result = (int)ingredientsItemDefinition.TimeToHarvest;
		}
		return result;
	}

	public static global::UnityEngine.Vector3 UIToWorldCoords(global::UnityEngine.Camera camera, global::UnityEngine.Vector2 uiPosition)
	{
		global::UnityEngine.Vector3 inNormal = new global::UnityEngine.Vector3(0f, 1f, 0f);
		global::UnityEngine.Vector3 inPoint = new global::UnityEngine.Vector3(0f, 0f, 0f);
		global::UnityEngine.Plane plane = new global::UnityEngine.Plane(inNormal, inPoint);
		global::UnityEngine.Vector3 position = new global::UnityEngine.Vector3(uiPosition.x, uiPosition.y, 0f);
		global::UnityEngine.Ray ray = camera.ScreenPointToRay(position);
		float enter;
		plane.Raycast(ray, out enter);
		global::UnityEngine.Vector3 point = ray.GetPoint(enter);
		return new global::UnityEngine.Vector3(global::UnityEngine.Mathf.RoundToInt(point.x), 0f, global::UnityEngine.Mathf.RoundToInt(point.z));
	}

	public static global::UnityEngine.Vector3 WorldToUICoords(global::UnityEngine.Camera camera, global::UnityEngine.Vector3 worldPosition)
	{
		return camera.WorldToScreenPoint(worldPosition);
	}

	public static global::System.Collections.Generic.List<global::Kampai.Util.Point> GetBuildingSideWalkList(global::Kampai.Game.Location buildingLocation, string footprint)
	{
		global::Kampai.Util.Point point = new global::Kampai.Util.Point(buildingLocation.x, buildingLocation.y);
		global::System.Collections.Generic.List<global::Kampai.Util.Point> list = new global::System.Collections.Generic.List<global::Kampai.Util.Point>();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < footprint.Length; i++)
		{
			switch (footprint[i])
			{
			case '.':
			{
				global::Kampai.Util.Point item = new global::Kampai.Util.Point(point.x + num, point.y + num2);
				list.Add(item);
				break;
			}
			case '|':
				num = 0;
				num2--;
				break;
			}
			num++;
		}
		return list;
	}

	public static global::Kampai.Util.Point GetClosestBuildingSidewalk(global::Kampai.Game.Location buildingLocation, global::UnityEngine.Vector3 position, string footprint)
	{
		global::Kampai.Util.Point point = new global::Kampai.Util.Point(buildingLocation.x, buildingLocation.y);
		global::Kampai.Util.Point a = new global::Kampai.Util.Point
		{
			XZProjection = position
		};
		int num = 0;
		int num2 = 0;
		float num3 = float.MaxValue;
		global::Kampai.Util.Point result = default(global::Kampai.Util.Point);
		bool flag = false;
		for (int i = 0; i < footprint.Length; i++)
		{
			switch (footprint[i])
			{
			case '.':
			{
				global::Kampai.Util.Point point2 = new global::Kampai.Util.Point(point.x + num, point.y + num2);
				float num4 = global::Kampai.Util.Point.Distance(a, point2);
				if (num4 < num3)
				{
					num3 = num4;
					result = point2;
					flag = true;
				}
				break;
			}
			case '|':
				num = 0;
				num2--;
				break;
			case 'x':
				if (!flag)
				{
					global::Kampai.Util.Point point2 = new global::Kampai.Util.Point(point.x + num, point.y + num2);
					float num4 = global::Kampai.Util.Point.Distance(a, point2);
					if (num4 < num3)
					{
						num3 = num4;
						result = point2;
					}
				}
				break;
			}
			num++;
		}
		return result;
	}

	public static bool HasSidewalk(string footprint)
	{
		foreach (char c in footprint)
		{
			if (c == '.')
			{
				return true;
			}
		}
		return false;
	}
}
