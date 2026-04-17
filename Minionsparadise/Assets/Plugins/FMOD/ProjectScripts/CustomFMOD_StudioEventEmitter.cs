public class CustomFMOD_StudioEventEmitter : global::UnityEngine.MonoBehaviour
{
	[global::System.Serializable]
	public class Parameter
	{
		public string name;

		public float value;
	}

	public FMODAsset asset;

	public string path = string.Empty;

	public bool startEventOnAwake = true;

	public string id;

	public bool staticSound = true;

	private global::System.Collections.Queue queue = new global::System.Collections.Queue();

	public global::FMOD.Studio.EventInstance evt;

	private bool hasStarted;

	public bool shiftPosition;

	private global::UnityEngine.AnimationCurve fadeCurve;

	private float fadeElapsed;

	private float fadeDuration;

	private bool isFading;

	private float currentVolume = 1f;

	private global::UnityEngine.Rigidbody cachedRigidBody;

	private global::System.Collections.Generic.Dictionary<string, float> eventParameters;

	private static bool isShuttingDown;

	public bool HasValidEventInstance()
	{
		return evt.isValid();
	}

	public void ReleaseEventInstance()
	{
		if (evt.isValid())
		{
			ERRCHECK(evt.release());
		}
		evt.clearHandle();
	}

	public void Play()
	{
		if (HasValidEventInstance())
		{
			UpdateEventParameters();
			ERRCHECK(evt.start());
		}
		else
		{
			global::FMOD.Studio.UnityUtil.Log("Tried to play event without a valid instance: " + path);
		}
	}

	public void Pause()
	{
		if (HasValidEventInstance())
		{
			ERRCHECK(evt.setPaused(true));
		}
	}

	public void Resume()
	{
		if (HasValidEventInstance())
		{
			ERRCHECK(evt.setPaused(false));
		}
	}

	public void QueueClip(string clip)
	{
		queue.Enqueue(clip);
	}

	public void SetTimelinePosition(int time)
	{
		if (HasValidEventInstance())
		{
			ERRCHECK(evt.setTimelinePosition(time));
		}
	}

	public void Fade(float startVol, float endVol, float duration)
	{
		if (!isFading)
		{
			fadeCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe(0f, startVol), new global::UnityEngine.Keyframe(duration, endVol));
		}
		else
		{
			float volume = 0f;
			if (HasValidEventInstance())
			{
				evt.getVolume(out volume);
			}
			currentVolume = volume;
			fadeCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe(0f, volume), new global::UnityEngine.Keyframe(duration, endVol));
		}
		fadeElapsed = 0f;
		fadeDuration = duration;
		isFading = true;
	}

	public void Stop()
	{
		if (HasValidEventInstance())
		{
			ERRCHECK(evt.stop(global::FMOD.Studio.STOP_MODE.IMMEDIATE));
		}
	}

	public float getParameter(string name)
	{
		float value = 0f;
		if (HasValidEventInstance())
		{
			ERRCHECK(evt.getParameterByName(name, out value));
		}
		return value;
	}

	public global::FMOD.Studio.PLAYBACK_STATE getPlaybackState()
	{
		if (!HasValidEventInstance())
		{
			return global::FMOD.Studio.PLAYBACK_STATE.STOPPED;
		}
		global::FMOD.Studio.PLAYBACK_STATE state = global::FMOD.Studio.PLAYBACK_STATE.STOPPED;
		if (ERRCHECK(evt.getPlaybackState(out state)) == global::FMOD.RESULT.OK)
		{
			return state;
		}
		return global::FMOD.Studio.PLAYBACK_STATE.STOPPED;
	}

	public void SetEventParameters(global::System.Collections.Generic.Dictionary<string, float> eventParameters)
	{
		this.eventParameters = eventParameters;
	}

	private void Start()
	{
		if (asset != null || !string.IsNullOrEmpty(path))
		{
			CacheEventInstance();
		}
		cachedRigidBody = GetComponent<global::UnityEngine.Rigidbody>();
		if (startEventOnAwake)
		{
			StartEvent();
		}
	}

	private void CacheEventInstance()
	{
		if (!HasValidEventInstance())
		{
			if (asset != null)
			{
				evt = FMOD_StudioSystem.instance.GetEvent(asset.id);
			}
			else if (!string.IsNullOrEmpty(path))
			{
				evt = FMOD_StudioSystem.instance.GetEvent(path);
			}
			else
			{
				global::FMOD.Studio.UnityUtil.LogError("No asset or path specified for Event Emitter");
			}
		}
	}

	public void UpdateEventParameters()
	{
		if (!HasValidEventInstance() || eventParameters == null)
		{
			return;
		}
		foreach (global::System.Collections.Generic.KeyValuePair<string, float> eventParameter in eventParameters)
		{
			if (float.IsNaN(eventParameter.Value))
			{
				global::FMOD.Studio.UnityUtil.LogError("NAN passed in as float value for param:" + eventParameter.Key);
			}
			else
			{
				ERRCHECK(evt.setParameterByName(eventParameter.Key, eventParameter.Value));
			}
		}
	}

	private void OnApplicationQuit()
	{
		isShuttingDown = true;
	}

	private void OnDestroy()
	{
		if (isShuttingDown)
		{
			return;
		}
		global::FMOD.Studio.UnityUtil.Log("Destroy called");
		if (HasValidEventInstance())
		{
			if (getPlaybackState() != global::FMOD.Studio.PLAYBACK_STATE.STOPPED)
			{
				global::FMOD.Studio.UnityUtil.Log("Release evt: " + path);
				ERRCHECK(evt.stop(global::FMOD.Studio.STOP_MODE.IMMEDIATE));
			}
			ReleaseEventInstance();
		}
	}

	public void StartEvent()
	{
		if (!HasValidEventInstance())
		{
			CacheEventInstance();
		}
		if (HasValidEventInstance())
		{
			Update3DAttributes();
			UpdateEventParameters();
			ERRCHECK(evt.start());
			evt.setVolume(currentVolume);
		}
		else
		{
			global::FMOD.Studio.UnityUtil.LogError("Event retrieval failed: " + path);
		}
		hasStarted = true;
	}

	public bool HasFinished()
	{
		if (!hasStarted)
		{
			return false;
		}
		if (!HasValidEventInstance())
		{
			return true;
		}
		return getPlaybackState() == global::FMOD.Studio.PLAYBACK_STATE.STOPPED;
	}

	private void Update()
	{
		if (HasValidEventInstance())
		{
			if (!staticSound)
			{
				Update3DAttributes();
			}
			if (isFading)
			{
				fadeElapsed += global::UnityEngine.Time.deltaTime;
				currentVolume = fadeCurve.Evaluate(fadeElapsed);
				evt.setVolume(currentVolume);
				if (fadeElapsed >= fadeDuration)
				{
					isFading = false;
				}
			}
			if (queue.Count > 0 && HasFinished())
			{
				string text = (string)queue.Dequeue();
				path = text;
				ReleaseEventInstance();
				StartEvent();
			}
		}
		else
		{
			evt.clearHandle();
		}
	}

	private void Update3DAttributes()
	{
		if (HasValidEventInstance())
		{
			global::FMOD.ATTRIBUTES_3D attributes = ((!shiftPosition) ? global::FMOD.Studio.UnityUtil.to3DAttributes(base.gameObject, cachedRigidBody) : global::FMOD.Studio.UnityUtil.to3DAttributes(base.gameObject, cachedRigidBody));
			ERRCHECK(evt.set3DAttributes(attributes));
		}
	}

	private global::FMOD.RESULT ERRCHECK(global::FMOD.RESULT result)
	{
		global::FMOD.Studio.UnityUtil.ERRCHECK(result);
		return result;
	}
}
