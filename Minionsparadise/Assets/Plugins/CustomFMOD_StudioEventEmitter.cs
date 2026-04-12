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

	public void Play()
	{
		if (evt != null)
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
		if (evt != null)
		{
			ERRCHECK(evt.setPaused(true));
		}
	}

	public void Resume()
	{
		if (evt != null)
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
		if (evt != null)
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
			if (evt != null)
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
		if (evt != null)
		{
			ERRCHECK(evt.stop(global::FMOD.Studio.STOP_MODE.IMMEDIATE));
		}
	}

	public global::FMOD.Studio.ParameterInstance getParameter(string name)
	{
		global::FMOD.Studio.ParameterInstance instance = null;
		ERRCHECK(evt.getParameter(name, out instance));
		return instance;
	}

	public global::FMOD.Studio.PLAYBACK_STATE getPlaybackState()
	{
		if (evt == null || !evt.isValid())
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
		if (evt == null)
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
		if (!(evt != null) || !evt.isValid() || eventParameters == null)
		{
			return;
		}
		foreach (global::System.Collections.Generic.KeyValuePair<string, float> eventParameter in eventParameters)
		{
			global::FMOD.Studio.ParameterInstance instance;
			if (float.IsNaN(eventParameter.Value))
			{
				global::FMOD.Studio.UnityUtil.LogError("NAN passed in as float value for param:" + eventParameter.Key);
			}
			else if (ERRCHECK(evt.getParameter(eventParameter.Key, out instance)) == global::FMOD.RESULT.OK && instance != null)
			{
				ERRCHECK(instance.setValue(eventParameter.Value));
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
		if (evt != null && evt.isValid())
		{
			if (getPlaybackState() != global::FMOD.Studio.PLAYBACK_STATE.STOPPED)
			{
				global::FMOD.Studio.UnityUtil.Log("Release evt: " + path);
				ERRCHECK(evt.stop(global::FMOD.Studio.STOP_MODE.IMMEDIATE));
			}
			ERRCHECK(evt.release());
			evt = null;
		}
	}

	public void StartEvent()
	{
		if (evt == null || !evt.isValid())
		{
			CacheEventInstance();
		}
		if (evt != null && evt.isValid())
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
		if (evt == null || !evt.isValid())
		{
			return true;
		}
		return getPlaybackState() == global::FMOD.Studio.PLAYBACK_STATE.STOPPED;
	}

	private void Update()
	{
		if (evt != null && evt.isValid())
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
				evt.release();
				evt = null;
				StartEvent();
			}
		}
		else
		{
			if (evt != null)
			{
				ERRCHECK(evt.release());
			}
			evt = null;
		}
	}

	private void Update3DAttributes()
	{
		if (evt != null && evt.isValid())
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
