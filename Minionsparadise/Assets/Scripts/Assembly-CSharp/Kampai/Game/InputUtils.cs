namespace Kampai.Game
{
	public static class InputUtils
	{
		private sealed class TouchComparer : global::System.Collections.Generic.IComparer<global::UnityEngine.Touch>
		{
			int global::System.Collections.Generic.IComparer<global::UnityEngine.Touch>.Compare(global::UnityEngine.Touch x, global::UnityEngine.Touch y)
			{
				return x.fingerId - y.fingerId;
			}
		}

		private const int MAX_HANDLED_TOUCHES = 256;

		private static global::Kampai.Game.InputUtils.TouchComparer comparer = new global::Kampai.Game.InputUtils.TouchComparer();

		private static bool[] touchCorruptedBySamsung = new bool[256];

		private static global::UnityEngine.Touch[] touches = new global::UnityEngine.Touch[256];

		private static int _touchCount;

		private static int frameUpdated;

		public static int touchCount
		{
			get
			{
				if (frameUpdated < global::UnityEngine.Time.frameCount)
				{
					UpdateTouchStates();
				}
				return _touchCount;
			}
		}

		private static void UpdateTouchStates()
		{
			frameUpdated = global::UnityEngine.Time.frameCount;
			for (int i = 0; i < global::UnityEngine.Input.touchCount; i++)
			{
				global::UnityEngine.Touch touch = global::UnityEngine.Input.GetTouch(i);
				global::UnityEngine.TouchPhase phase = touch.phase;
				int fingerId = touch.fingerId;
				if (phase != global::UnityEngine.TouchPhase.Moved && phase != global::UnityEngine.TouchPhase.Stationary)
				{
					CorruptTouch(fingerId, false);
				}
			}
			int num = 0;
			if (global::UnityEngine.Input.touchCount > 0)
			{
				for (int j = 0; j < global::UnityEngine.Input.touchCount; j++)
				{
					global::UnityEngine.Touch touch2 = global::UnityEngine.Input.GetTouch(j);
					if (!IsTouchCorrupted(touch2.fingerId))
					{
						touches[num] = touch2;
						num++;
					}
				}
			}
			_touchCount = num;
			if (_touchCount > 0)
			{
				global::System.Array.Sort(touches, 0, _touchCount, comparer);
			}
		}

		private static bool IsTouchCorrupted(int fingerId)
		{
			return fingerId >= 256 || touchCorruptedBySamsung[fingerId];
		}

		private static void CorruptTouch(int fingerId, bool isCorrupted)
		{
			if (fingerId < 256)
			{
				touchCorruptedBySamsung[fingerId] = isCorrupted;
			}
		}

		public static global::UnityEngine.Touch GetTouch(int i)
		{
			if (frameUpdated < global::UnityEngine.Time.frameCount)
			{
				UpdateTouchStates();
			}
			return touches[i];
		}
	}
}
