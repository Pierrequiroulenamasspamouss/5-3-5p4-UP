namespace Kampai.Util
{
	public struct AABox2D
	{
		private const int INSIDE = 0;

		private const int LEFT = 1;

		private const int RIGHT = 2;

		private const int BOTTOM = 4;

		private const int TOP = 8;

		public global::UnityEngine.Vector2 center;

		public float width;

		public float height;

		private float xMin;

		private float xMax;

		private float yMin;

		private float yMax;

		public AABox2D(global::UnityEngine.Vector2 center, float size)
			: this(center.x, center.y, size, size)
		{
		}

		public AABox2D(global::UnityEngine.Vector2 center, float width, float height)
			: this(center.x, center.y, width, height)
		{
		}

		public AABox2D(float cX, float cY, float width, float height)
		{
			center.x = cX;
			center.y = cY;
			this.width = width;
			this.height = height;
			float num = width * 0.5f;
			float num2 = height * 0.5f;
			xMin = center.x - num;
			xMax = center.x + num;
			yMin = center.y - num2;
			yMax = center.y + num2;
		}

		public AABox2D(global::Kampai.Util.AABox2D other)
		{
			center = other.center;
			width = other.width;
			height = other.height;
			xMin = other.xMin;
			xMax = other.xMax;
			yMin = other.yMin;
			yMax = other.yMax;
		}

		public bool RaycastXZ(global::UnityEngine.Ray ray, out global::UnityEngine.Vector3 hit, float dist)
		{
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(ray.origin.x, ray.origin.z);
			global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(ray.direction.x, ray.direction.z);
			global::UnityEngine.Vector2 final = vector + vector2 * dist;
			global::UnityEngine.Vector2 hit2;
			bool result = Intersect(vector, final, out hit2);
			hit = new global::UnityEngine.Vector3(hit2.x, 0f, hit2.y);
			return result;
		}

		public bool Raycast(global::UnityEngine.Ray2D ray, out global::UnityEngine.Vector2 hit, float dist)
		{
			global::UnityEngine.Vector2 origin = ray.origin;
			global::UnityEngine.Vector2 final = origin + ray.direction * dist;
			return Intersect(origin, final, out hit);
		}

		public bool Intersect(global::UnityEngine.Vector2 origin, global::UnityEngine.Vector2 final, out global::UnityEngine.Vector2 hit)
		{
			global::UnityEngine.Vector2 p = origin;
			global::UnityEngine.Vector2 p2 = final;
			hit = global::UnityEngine.Vector2.zero;
			if (ComputeOutCode(origin) == 0 && ComputeOutCode(final) == 0)
			{
				return false;
			}
			if (Clip(ref p, ref p2))
			{
				if (origin != p)
				{
					hit = p;
					return true;
				}
				hit = p2;
				return true;
			}
			return false;
		}

		public bool Overlaps(global::Kampai.Util.AABox2D other)
		{
			global::UnityEngine.Vector2 vector = (other.center - center) * 2f;
			return global::UnityEngine.Mathf.Abs(vector.x) <= width + other.width && global::UnityEngine.Mathf.Abs(vector.y) <= height + other.height;
		}

		public bool Contains(global::UnityEngine.Vector2 pos)
		{
			return pos.x >= xMin && pos.x <= xMax && pos.y >= yMin && pos.y <= yMax;
		}

		public void Split(float v, int axis, out global::Kampai.Util.AABox2D left, out global::Kampai.Util.AABox2D right)
		{
			left = this;
			right = this;
			if (axis == 0)
			{
				left.xMax = v;
				right.xMin = v;
			}
			else
			{
				left.yMax = v;
				right.yMin = v;
			}
			left.Recompute();
			right.Recompute();
		}

		private void Recompute()
		{
			width = xMax - xMin;
			height = yMax - yMin;
			center.x = xMin + width * 0.5f;
			center.y = yMin + height * 0.5f;
		}

		public bool Clip(ref global::UnityEngine.Vector2 p0, ref global::UnityEngine.Vector2 p1)
		{
			int num = ComputeOutCode(p0);
			int num2 = ComputeOutCode(p1);
			bool result = false;
			while (true)
			{
				if ((num | num2) == 0)
				{
					result = true;
					break;
				}
				if ((num & num2) != 0)
				{
					break;
				}
				float x = 0f;
				float y = 0f;
				int num3 = ((num == 0) ? num2 : num);
				if ((num3 & 8) != 0)
				{
					x = p0.x + (p1.x - p0.x) * (yMax - p0.y) / (p1.y - p0.y);
					y = yMax;
				}
				else if ((num3 & 4) != 0)
				{
					x = p0.x + (p1.x - p0.x) * (yMin - p0.y) / (p1.y - p0.y);
					y = yMin;
				}
				else if ((num3 & 2) != 0)
				{
					y = p0.y + (p1.y - p0.y) * (xMax - p0.x) / (p1.x - p0.x);
					x = xMax;
				}
				else if ((num3 & 1) != 0)
				{
					y = p0.y + (p1.y - p0.y) * (xMin - p0.x) / (p1.x - p0.x);
					x = xMin;
				}
				if (num3 == num)
				{
					p0.x = x;
					p0.y = y;
					num = ComputeOutCode(p0);
				}
				else
				{
					p1.x = x;
					p1.y = y;
					num2 = ComputeOutCode(p1);
				}
			}
			return result;
		}

		private int ComputeOutCode(global::UnityEngine.Vector2 p)
		{
			int num = 0;
			if (p.x < xMin)
			{
				num |= 1;
			}
			else if (p.x > xMax)
			{
				num |= 2;
			}
			if (p.y < yMin)
			{
				num |= 4;
			}
			else if (p.y > yMax)
			{
				num |= 8;
			}
			return num;
		}
	}
}
