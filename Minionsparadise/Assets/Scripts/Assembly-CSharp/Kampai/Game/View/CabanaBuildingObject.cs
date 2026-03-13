namespace Kampai.Game.View
{
	public class CabanaBuildingObject : global::Kampai.Game.View.RoutableBuildingObject
	{
		private global::UnityEngine.Transform _routingPoint;

		private global::UnityEngine.Vector3 _offset = new global::UnityEngine.Vector3(1f, 2f, -6f);

		internal override void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::System.Collections.Generic.IDictionary<string, global::UnityEngine.RuntimeAnimatorController> controllers, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.Init(building, logger, controllers, definitionService);
			if (routes != null && routes.Length > 0)
			{
				_routingPoint = routes[0];
			}
			if (_routingPoint == null)
			{
				_routingPoint = base.transform;
			}
		}

		public global::UnityEngine.Transform GetRoutingPoint()
		{
			return _routingPoint;
		}

		protected override global::UnityEngine.Vector3 GetIndicatorPosition(bool centerY)
		{
			global::UnityEngine.Vector3 position = base.transform.position;
			return position + _offset;
		}
	}
}
