namespace Kampai.Util
{
	public static class Bresenham
	{
		public static global::System.Collections.Generic.IList<global::Kampai.Util.Point> Line(global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 dir, float dist)
		{
			global::UnityEngine.Vector3 xZProjection = start + dir * dist;
			return Line(new global::Kampai.Util.Point
			{
				XZProjection = start
			}, new global::Kampai.Util.Point
			{
				XZProjection = xZProjection
			});
		}

		public static global::System.Collections.Generic.IList<global::Kampai.Util.Point> Line(global::Kampai.Util.Point start, global::Kampai.Util.Point end)
		{
			global::System.Collections.Generic.List<global::Kampai.Util.Point> list = new global::System.Collections.Generic.List<global::Kampai.Util.Point>();
			bool flag = global::UnityEngine.Mathf.Abs(end.y - start.y) > global::UnityEngine.Mathf.Abs(end.x - start.x);
			if (flag)
			{
				Swap(ref start.x, ref start.y);
				Swap(ref end.x, ref end.y);
			}
			bool flag2 = false;
			if (start.x > end.x)
			{
				Swap(ref start.x, ref end.x);
				Swap(ref start.y, ref end.y);
				flag2 = true;
			}
			int num = end.x - start.x;
			int num2 = global::UnityEngine.Mathf.Abs(end.y - start.y);
			int num3 = num / 2;
			int num4 = ((start.y < end.y) ? 1 : (-1));
			int num5 = start.y;
			for (int i = start.x; i <= end.x; i++)
			{
				if (flag)
				{
					list.Add(new global::Kampai.Util.Point(num5, i));
				}
				else
				{
					list.Add(new global::Kampai.Util.Point(i, num5));
				}
				num3 -= num2;
				if (num3 < 0)
				{
					num5 += num4;
					num3 += num;
				}
			}
			if (flag2)
			{
				list.Reverse();
			}
			return list;
		}

		private static void Swap(ref int lhs, ref int rhs)
		{
			int num = lhs;
			lhs = rhs;
			rhs = num;
		}
	}
}
