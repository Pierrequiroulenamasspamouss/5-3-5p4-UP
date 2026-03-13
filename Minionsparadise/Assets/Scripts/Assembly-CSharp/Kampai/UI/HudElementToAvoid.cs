namespace Kampai.UI
{
	public class HudElementToAvoid : global::System.IEquatable<global::Kampai.UI.HudElementToAvoid>
	{
		public global::UnityEngine.GameObject GameObject { get; private set; }

		public bool IsCircleShape { get; private set; }

		public HudElementToAvoid(global::UnityEngine.GameObject gameObject, bool isCircleShape = false)
		{
			GameObject = gameObject;
			IsCircleShape = isCircleShape;
		}

		public bool Contains(global::UnityEngine.GameObject gameObject)
		{
			if (gameObject == null || GameObject == null)
			{
				return false;
			}
			return GameObject.GetInstanceID().Equals(gameObject.GetInstanceID());
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}
			global::Kampai.UI.HudElementToAvoid hudElementToAvoid = (global::Kampai.UI.HudElementToAvoid)obj;
			if (hudElementToAvoid == null)
			{
				return false;
			}
			return Equals(hudElementToAvoid);
		}

		public bool Equals(global::Kampai.UI.HudElementToAvoid other)
		{
			if (other == null)
			{
				return false;
			}
			return Contains(other.GameObject);
		}

		public override int GetHashCode()
		{
			return GameObject.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[HudElementToAvoid: gameObjectName: {0} gameObjectId: {1} isCircleShape: {2}]", GameObject.name, GameObject.GetInstanceID(), IsCircleShape);
		}
	}
}
