namespace Kampai.UI
{
	public class PositionService : global::Kampai.UI.IPositionService
	{
		private global::System.Collections.Generic.List<global::Kampai.UI.HudElementToAvoid> hudElementsToAvoid = new global::System.Collections.Generic.List<global::Kampai.UI.HudElementToAvoid>();

		private global::UnityEngine.Vector3[] viewportBoundaryCorners = new global::UnityEngine.Vector3[4];

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera MainCamera { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera UICamera { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public void AddHUDElementToAvoid(global::UnityEngine.GameObject toAppend, bool isCircleShape = false)
		{
			global::Kampai.UI.HudElementToAvoid item = new global::Kampai.UI.HudElementToAvoid(toAppend, isCircleShape);
			if (!hudElementsToAvoid.Contains(item))
			{
				hudElementsToAvoid.Add(item);
			}
		}

		public void RemoveHUDElementToAvoid(global::UnityEngine.GameObject toRemove)
		{
			global::System.Collections.Generic.IList<global::Kampai.UI.HudElementToAvoid> list = new global::System.Collections.Generic.List<global::Kampai.UI.HudElementToAvoid>();
			foreach (global::Kampai.UI.HudElementToAvoid item in hudElementsToAvoid)
			{
				if (item.GameObject == null || item.Contains(toRemove))
				{
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					hudElementsToAvoid.Remove(list[i]);
				}
			}
		}

		public global::Kampai.UI.PositionData GetPositionData(global::UnityEngine.Vector3 worldPosition)
		{
			global::UnityEngine.Vector3 vector = MainCamera.WorldToViewportPoint(worldPosition);
			global::UnityEngine.Vector3 worldPositionInUI = UICamera.ViewportToWorldPoint(vector);
			return new global::Kampai.UI.PositionData(worldPositionInUI, vector);
		}

		public global::UnityEngine.Vector2 GetUIAnchorRatioPosition(global::UnityEngine.Vector3 worldPosition)
		{
			global::UnityEngine.Vector3 vector = MainCamera.WorldToViewportPoint(worldPosition);
			return new global::UnityEngine.Vector2(vector.x, vector.y);
		}

		public global::UnityEngine.Vector2 GetUIAnchorRatioPosition(int buildingInstanceID)
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(buildingInstanceID);
			return GetUIAnchorRatioPosition(buildingObject.Center);
		}

		public global::Kampai.UI.SnappablePositionData GetSnappablePositionData(global::Kampai.UI.PositionData normalPositionData, global::Kampai.UI.ViewportBoundary boundary, bool avoidHudElements = false)
		{
			global::UnityEngine.Vector3 viewportPosition = normalPositionData.ViewportPosition;
			global::UnityEngine.Vector3 vector = ClampViewportPosition(viewportPosition, boundary, avoidHudElements);
			global::UnityEngine.Vector3 clampedWorldPositionInUI = UICamera.ViewportToWorldPoint(vector);
			return new global::Kampai.UI.SnappablePositionData(normalPositionData.WorldPositionInUI, clampedWorldPositionInUI, viewportPosition, vector);
		}

		private global::Kampai.UI.ViewportBoundary GetViewportBoundary(global::UnityEngine.RectTransform transform)
		{
			transform.GetWorldCorners(viewportBoundaryCorners);
			global::UnityEngine.Vector2 vector = UICamera.WorldToViewportPoint(viewportBoundaryCorners[0]);
			global::UnityEngine.Vector2 vector2 = UICamera.WorldToViewportPoint(viewportBoundaryCorners[2]);
			global::UnityEngine.Vector2 vector3 = new global::UnityEngine.Vector2(vector.x + (vector2.x - vector.x) / 2f, vector.y + (vector2.y - vector.y) / 2f);
			if (vector3.x <= 0.35f && vector3.y <= 0.35f)
			{
				return new global::Kampai.UI.ViewportBoundary(0f, 0f, vector2.y, vector2.x);
			}
			if (vector3.x <= 0.35f && vector3.y > 0.65f)
			{
				return new global::Kampai.UI.ViewportBoundary(vector.y, 0f, 1f, vector2.x);
			}
			if (vector3.x > 0.65f && vector3.y > 0.65f)
			{
				return new global::Kampai.UI.ViewportBoundary(vector.y, vector.x, 1f, 1f);
			}
			if (vector3.x > 0.65f && vector3.y <= 0.35f)
			{
				return new global::Kampai.UI.ViewportBoundary(0f, vector.x, vector2.y, 1f);
			}
			if (vector3.y > 0.65f)
			{
				return new global::Kampai.UI.ViewportBoundary(vector.y, vector.x, 1f, vector2.x);
			}
			if (vector3.y <= 0.35f)
			{
				return new global::Kampai.UI.ViewportBoundary(0f, vector.x, vector2.y, vector2.x);
			}
			if (vector3.x <= 0.35f)
			{
				return new global::Kampai.UI.ViewportBoundary(vector.y, 0f, vector2.y, vector2.x);
			}
			return new global::Kampai.UI.ViewportBoundary(vector.y, vector.x, vector2.y, 1f);
		}

		private global::UnityEngine.Vector3 ClampViewportPosition(global::UnityEngine.Vector3 viewportPosition, global::Kampai.UI.ViewportBoundary boundary, bool avoidHudElements)
		{
			float x = viewportPosition.x;
			float left = boundary.Left;
			float right = boundary.Right;
			viewportPosition.x = ((x < left) ? left : ((!(x > right)) ? x : right));
			float y = viewportPosition.y;
			float top = boundary.Top;
			float bottom = boundary.Bottom;
			viewportPosition.y = ((y < bottom) ? bottom : ((!(y > top)) ? y : top));
			if (avoidHudElements && hudElementsToAvoid != null)
			{
				global::System.Collections.Generic.List<global::Kampai.UI.HudElementToAvoid>.Enumerator enumerator = hudElementsToAvoid.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						global::UnityEngine.GameObject gameObject = enumerator.Current.GameObject;
						bool isCircleShape = enumerator.Current.IsCircleShape;
						if (!(gameObject != null) || !gameObject.activeInHierarchy)
						{
							continue;
						}
						if (!isCircleShape)
						{
							global::Kampai.UI.ViewportBoundary viewportBoundary = GetViewportBoundary(gameObject.transform as global::UnityEngine.RectTransform);
							if (viewportBoundary.Contains(viewportPosition))
							{
								viewportPosition = ClampViewportPositionForBoundary(x, y, viewportBoundary);
							}
						}
						else
						{
							viewportPosition = ClampWithinSphere(viewportPosition, gameObject);
						}
					}
				}
				finally
				{
					enumerator.Dispose();
				}
			}
			return viewportPosition;
		}

		private global::UnityEngine.Vector3 ClampWithinSphere(global::UnityEngine.Vector3 viewportPosition, global::UnityEngine.GameObject go)
		{
			global::UnityEngine.RectTransform rectTransform = go.transform as global::UnityEngine.RectTransform;
			if (rectTransform != null)
			{
				return ClampWithinSphere2D(viewportPosition, rectTransform);
			}
			return ClampWithinSphere3D(viewportPosition, go.GetComponent<global::UnityEngine.Renderer>());
		}

		private global::UnityEngine.Vector3 ClampWithinSphere2D(global::UnityEngine.Vector3 viewportPosition, global::UnityEngine.RectTransform rectTransform)
		{
			global::UnityEngine.Vector3 result = viewportPosition;
			global::UnityEngine.Vector3 vector = UICamera.ViewportToWorldPoint(viewportPosition);
			rectTransform.GetWorldCorners(viewportBoundaryCorners);
			float num = (viewportBoundaryCorners[2].y - viewportBoundaryCorners[0].y) / 2f;
			global::UnityEngine.Vector3 position = rectTransform.position;
			global::UnityEngine.Vector3 vector2 = vector - position;
			vector2.z = 0f;
			if (vector2.sqrMagnitude <= num * num)
			{
				global::UnityEngine.Vector3 position2 = position + vector2.normalized * num;
				result = UICamera.WorldToViewportPoint(position2);
			}
			return result;
		}

		private global::UnityEngine.Vector3 ClampWithinSphere3D(global::UnityEngine.Vector3 viewportPosition, global::UnityEngine.Renderer renderer)
		{
			global::UnityEngine.Vector3 result = viewportPosition;
			global::UnityEngine.Vector3 vector = UICamera.ViewportToWorldPoint(viewportPosition);
			global::UnityEngine.Vector3 center = renderer.bounds.center;
			center.z = vector.z;
			float num = global::UnityEngine.Mathf.Abs(renderer.bounds.max.y - renderer.bounds.min.y) / 2f;
			global::UnityEngine.Vector3 vector2 = vector - center;
			if (vector2.sqrMagnitude <= num * num)
			{
				global::UnityEngine.Vector3 position = center + vector2.normalized * num;
				result = UICamera.WorldToViewportPoint(position);
			}
			return result;
		}

		private global::UnityEngine.Vector2 ClampViewportPositionForBoundary(float originalX, float originalY, global::Kampai.UI.ViewportBoundary boundary)
		{
			bool flag = global::UnityEngine.Mathf.Approximately(boundary.Bottom, 0f);
			bool flag2 = global::UnityEngine.Mathf.Approximately(boundary.Left, 0f);
			bool flag3 = global::UnityEngine.Mathf.Approximately(boundary.Right, 1f);
			bool flag4 = global::UnityEngine.Mathf.Approximately(boundary.Top, 1f);
			float x = global::UnityEngine.Mathf.Clamp(originalX, boundary.Left, boundary.Right);
			float y = global::UnityEngine.Mathf.Clamp(originalY, boundary.Bottom, boundary.Top);
			if (flag2)
			{
				if ((flag || flag4) && originalX <= boundary.Right)
				{
					return new global::UnityEngine.Vector2(x, (!flag) ? boundary.Bottom : boundary.Top);
				}
				return new global::UnityEngine.Vector2(boundary.Right, y);
			}
			if (flag3)
			{
				if ((flag || flag4) && originalX >= boundary.Left)
				{
					return new global::UnityEngine.Vector2(x, (!flag) ? boundary.Bottom : boundary.Top);
				}
				return new global::UnityEngine.Vector2(boundary.Left, y);
			}
			return new global::UnityEngine.Vector2(x, (!flag) ? boundary.Bottom : boundary.Top);
		}
	}
}
