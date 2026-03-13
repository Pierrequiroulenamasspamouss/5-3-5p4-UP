public struct GoSmoothedQuaternion
{
	public GoSmoothingType smoothingType;

	public float duration;

	private global::UnityEngine.Quaternion _currentValue;

	private global::UnityEngine.Quaternion _target;

	private global::UnityEngine.Quaternion _start;

	private float _startTime;

	public global::UnityEngine.Quaternion smoothValue
	{
		get
		{
			float t = (global::UnityEngine.Time.time - _startTime) / duration;
			switch (smoothingType)
			{
			case GoSmoothingType.Lerp:
				_currentValue = global::UnityEngine.Quaternion.Lerp(_start, _target, t);
				break;
			case GoSmoothingType.Slerp:
				_currentValue = global::UnityEngine.Quaternion.Slerp(_start, _target, t);
				break;
			}
			return _currentValue;
		}
		set
		{
			_start = smoothValue;
			_startTime = global::UnityEngine.Time.time;
			_target = value;
		}
	}

	public float x
	{
		get
		{
			return _currentValue.x;
		}
		set
		{
			smoothValue = new global::UnityEngine.Quaternion(value, _target.y, _target.z, _target.w);
		}
	}

	public float y
	{
		get
		{
			return _currentValue.y;
		}
		set
		{
			smoothValue = new global::UnityEngine.Quaternion(_target.x, value, _target.y, _target.w);
		}
	}

	public float z
	{
		get
		{
			return _currentValue.z;
		}
		set
		{
			smoothValue = new global::UnityEngine.Quaternion(_target.x, _target.y, value, _target.w);
		}
	}

	public float w
	{
		get
		{
			return _currentValue.w;
		}
		set
		{
			smoothValue = new global::UnityEngine.Quaternion(_target.x, _target.y, _target.z, value);
		}
	}

	public GoSmoothedQuaternion(global::UnityEngine.Quaternion quat)
	{
		_currentValue = quat;
		_start = quat;
		_target = quat;
		_startTime = global::UnityEngine.Time.time;
		duration = 0.2f;
		smoothingType = GoSmoothingType.Lerp;
	}

	public static implicit operator GoSmoothedQuaternion(global::UnityEngine.Quaternion q)
	{
		return new GoSmoothedQuaternion(q);
	}
}
