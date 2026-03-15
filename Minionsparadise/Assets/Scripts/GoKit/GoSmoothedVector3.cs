public struct GoSmoothedVector3
{
	public GoSmoothingType smoothingType;

	public float duration;

	private global::UnityEngine.Vector3 _currentValue;

	private global::UnityEngine.Vector3 _target;

	private global::UnityEngine.Vector3 _start;

	private float _startTime;

	public global::UnityEngine.Vector3 smoothValue
	{
		get
		{
			float t = (global::UnityEngine.Time.time - _startTime) / duration;
			switch (smoothingType)
			{
			case GoSmoothingType.Lerp:
				_currentValue = global::UnityEngine.Vector3.Lerp(_start, _target, t);
				break;
			case GoSmoothingType.Slerp:
				_currentValue = global::UnityEngine.Vector3.Slerp(_start, _target, t);
				break;
			}
			return _currentValue;
		}
		private set
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
			smoothValue = new global::UnityEngine.Vector3(value, _target.y, _target.z);
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
			smoothValue = new global::UnityEngine.Vector3(_target.x, value, _target.y);
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
			smoothValue = new global::UnityEngine.Vector3(_target.x, _target.y, value);
		}
	}

	public GoSmoothedVector3(global::UnityEngine.Vector3 vector)
	{
		_currentValue = vector;
		_start = vector;
		_target = vector;
		_startTime = global::UnityEngine.Time.time;
		duration = 0.2f;
		smoothingType = GoSmoothingType.Lerp;
	}

	public static implicit operator GoSmoothedVector3(global::UnityEngine.Vector3 v)
	{
		return new GoSmoothedVector3(v);
	}
}
