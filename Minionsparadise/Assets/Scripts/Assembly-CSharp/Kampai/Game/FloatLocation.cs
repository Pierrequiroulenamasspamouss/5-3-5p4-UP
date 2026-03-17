namespace Kampai.Game
{
	public class FloatLocation : global::System.IEquatable<global::Kampai.Game.FloatLocation>
	{
		public float x { get; set; }

		public float y { get; set; }

		[global::Newtonsoft.Json.JsonConstructor]
		public FloatLocation(float xPos = 0f, float yPos = 0f)
		{
			x = xPos;
			y = yPos;
		}

		public FloatLocation(global::UnityEngine.Vector3 pos)
		{
			x = pos.x;
			y = pos.z;
		}

		public static float Distance(global::Kampai.Game.Location l1, global::Kampai.Game.Location l2)
		{
			float num = l2.x - l1.x;
			float num2 = l2.y - l1.y;
			return global::UnityEngine.Mathf.Sqrt(num * num + num2 * num2);
		}

		public override bool Equals(object obj)
		{
			return obj is global::Kampai.Game.FloatLocation;
		}

		public bool Equals(global::Kampai.Game.FloatLocation obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != typeof(global::Kampai.Game.FloatLocation))
			{
				return false;
			}
			return global::System.Math.Abs(x - obj.x) < 0.001f && global::System.Math.Abs(y - obj.y) < 0.001f;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode();
		}

		public static explicit operator global::UnityEngine.Vector3(global::Kampai.Game.FloatLocation l)
		{
			return new global::UnityEngine.Vector3(l.x, 0f, l.y);
		}
	}
}
