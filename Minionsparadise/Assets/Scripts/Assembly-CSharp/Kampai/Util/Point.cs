namespace Kampai.Util
{
	public struct Point : global::System.IEquatable<global::Kampai.Util.Point>
	{
		public int x;

		public int y;

		public global::UnityEngine.Vector3 XYProjection
		{
			get
			{
				return new global::UnityEngine.Vector3(x, y, 0f);
			}
			set
			{
				x = global::UnityEngine.Mathf.RoundToInt(value.x);
				y = global::UnityEngine.Mathf.RoundToInt(value.y);
			}
		}

		public global::UnityEngine.Vector3 XZProjection
		{
			get
			{
				return new global::UnityEngine.Vector3(x, 0f, y);
			}
			set
			{
				x = global::UnityEngine.Mathf.RoundToInt(value.x);
				y = global::UnityEngine.Mathf.RoundToInt(value.z);
			}
		}

		public Point(int xVal, int yVal)
		{
			x = xVal;
			y = yVal;
		}

		public Point(float xVal, float yVal)
		{
			x = (int)xVal;
			y = (int)yVal;
		}

		public static global::Kampai.Util.Point FromVector3(global::UnityEngine.Vector3 pos)
		{
			return new global::Kampai.Util.Point(global::UnityEngine.Mathf.RoundToInt(pos.x), global::UnityEngine.Mathf.RoundToInt(pos.z));
		}

		public static float Distance(global::Kampai.Util.Point a, global::Kampai.Util.Point b)
		{
			int num = b.x - a.x;
			int num2 = b.y - a.y;
			return global::UnityEngine.Mathf.Sqrt(num * num + num2 * num2);
		}

		public static float DistanceSquared(global::Kampai.Util.Point a, global::Kampai.Util.Point b)
		{
			int num = b.x - a.x;
			int num2 = b.y - a.y;
			return num * num + num2 * num2;
		}

		public static int RoundedDistance(global::Kampai.Util.Point a, global::Kampai.Util.Point b)
		{
			int num = b.x - a.x;
			int num2 = b.y - a.y;
			return global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.Mathf.Sqrt(num * num + num2 * num2));
		}

		public bool Equals(global::Kampai.Util.Point other)
		{
			return other.x == x && other.y == y;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			global::Kampai.Util.Point point = (global::Kampai.Util.Point)obj;
			return point.x == x && point.y == y;
		}

		public override int GetHashCode()
		{
			return x ^ y;
		}

		public override string ToString()
		{
			return string.Format("({0}, {1})", x, y);
		}

		public static implicit operator global::UnityEngine.Vector2(global::Kampai.Util.Point p)
		{
			return new global::UnityEngine.Vector2(p.x, p.y);
		}

		public static bool operator ==(global::Kampai.Util.Point a, global::Kampai.Util.Point b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(global::Kampai.Util.Point a, global::Kampai.Util.Point b)
		{
			return !a.Equals(b);
		}
	}
}
