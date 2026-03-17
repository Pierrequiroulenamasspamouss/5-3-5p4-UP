public class FMOD_StudioSystem : global::UnityEngine.MonoBehaviour
{
	private global::FMOD.Studio.System system;

	private global::System.Collections.Generic.Dictionary<string, global::FMOD.Studio.EventDescription> eventDescriptions = new global::System.Collections.Generic.Dictionary<string, global::FMOD.Studio.EventDescription>();

	private bool isInitialized;

	private bool isPaused;

	private static FMOD_StudioSystem sInstance;

	public static FMOD_StudioSystem instance
	{
		get
		{
			if (sInstance == null)
			{
				global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("FMOD_StudioSystem");
				sInstance = gameObject.AddComponent<FMOD_StudioSystem>();
				if (!global::FMOD.Studio.UnityUtil.ForceLoadLowLevelBinary())
				{
					global::FMOD.Studio.UnityUtil.LogError("Unable to load low level binary!");
					return sInstance;
				}
				sInstance.Init();
			}
			return sInstance;
		}
	}

	public global::FMOD.Studio.System System
	{
		get
		{
			return system;
		}
	}

	public global::FMOD.Studio.EventInstance GetEvent(FMODAsset asset)
	{
		return GetEvent(asset.id);
	}

	public global::FMOD.Studio.EventInstance GetEvent(string path)
	{
		global::FMOD.Studio.EventInstance eventInstance = null;
		if (string.IsNullOrEmpty(path))
		{
			global::FMOD.Studio.UnityUtil.LogError("Empty event path!");
			return null;
		}
		if (eventDescriptions.ContainsKey(path) && eventDescriptions[path].isValid())
		{
			ERRCHECK(eventDescriptions[path].createInstance(out eventInstance));
		}
		else
		{
			global::System.Guid guid = default(global::System.Guid);
			if (path.StartsWith("{"))
			{
				ERRCHECK(global::FMOD.Studio.Util.ParseID(path, out guid));
			}
			else if (path.StartsWith("event:"))
			{
				ERRCHECK(system.lookupID(path, out guid));
			}
			else
			{
				global::FMOD.Studio.UnityUtil.LogError("Expected event path to start with 'event:/'");
			}
			global::FMOD.Studio.EventDescription _event = null;
			ERRCHECK(system.getEventByID(guid, out _event));
			if (_event != null && _event.isValid())
			{
				eventDescriptions[path] = _event;
				ERRCHECK(_event.createInstance(out eventInstance));
			}
		}
		if (eventInstance == null)
		{
			global::FMOD.Studio.UnityUtil.Log("GetEvent FAILED: \"" + path + "\"");
		}
		return eventInstance;
	}

	public void PlayOneShot(FMODAsset asset, global::UnityEngine.Vector3 position)
	{
		PlayOneShot(asset.id, position);
	}

	public void PlayOneShot(string path, global::UnityEngine.Vector3 position)
	{
		PlayOneShot(path, position, 1f);
	}

	public void PlayOneShot(string path, global::UnityEngine.Vector3 position, float volume)
	{
		global::FMOD.Studio.EventInstance eventInstance = GetEvent(path);
		if (eventInstance == null)
		{
			global::FMOD.Studio.UnityUtil.LogWarning("PlayOneShot couldn't find event: \"" + path + "\"");
			return;
		}
		global::FMOD.ATTRIBUTES_3D attributes = global::FMOD.Studio.UnityUtil.to3DAttributes(position);
		ERRCHECK(eventInstance.set3DAttributes(attributes));
		ERRCHECK(eventInstance.setVolume(volume));
		ERRCHECK(eventInstance.start());
		ERRCHECK(eventInstance.release());
	}

	private void Init()
	{
		global::FMOD.Studio.UnityUtil.Log("FMOD_StudioSystem: Initialize");
		if (!isInitialized)
		{
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			global::FMOD.Studio.UnityUtil.Log("FMOD_StudioSystem: System_Create");
			ERRCHECK(global::FMOD.Studio.System.create(out this.system));
			global::FMOD.Studio.INITFLAGS iNITFLAGS = global::FMOD.Studio.INITFLAGS.NORMAL;
			global::FMOD.Studio.UnityUtil.Log("FMOD_StudioSystem: system.init");
			global::FMOD.RESULT rESULT = global::FMOD.RESULT.OK;
			rESULT = this.system.initialize(1024, iNITFLAGS, global::FMOD.INITFLAGS.NORMAL, global::System.IntPtr.Zero);
			if (rESULT == global::FMOD.RESULT.ERR_HEADER_MISMATCH)
			{
				global::FMOD.Studio.UnityUtil.LogError("Version mismatch between C# script and FMOD binary, restart Unity and reimport the integration package to resolve this issue.");
			}
			else
			{
				ERRCHECK(rESULT);
			}
			ERRCHECK(this.system.flushCommands());
			rESULT = this.system.update();
			if (rESULT == global::FMOD.RESULT.ERR_NET_SOCKET_ERROR)
			{
				global::FMOD.Studio.UnityUtil.LogWarning("LiveUpdate disabled: socket in already in use");
				iNITFLAGS = (global::FMOD.Studio.INITFLAGS)((uint)iNITFLAGS & 0xFFFFFFFEu);
				ERRCHECK(this.system.release());
				ERRCHECK(global::FMOD.Studio.System.create(out this.system));
				global::FMOD.System system;
				ERRCHECK(this.system.getLowLevelSystem(out system));
				rESULT = this.system.initialize(1024, iNITFLAGS, global::FMOD.INITFLAGS.NORMAL, global::System.IntPtr.Zero);
				ERRCHECK(rESULT);
			}
			isInitialized = true;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		isPaused = pauseStatus;
		if (this.system != null && this.system.isValid())
		{
			global::FMOD.Studio.UnityUtil.Log("Pause state changed to: " + pauseStatus);
			global::FMOD.System system;
			ERRCHECK(this.system.getLowLevelSystem(out system));
			if (system == null)
			{
				global::FMOD.Studio.UnityUtil.LogError("Tried to suspend mixer, but no low level system found");
			}
			else if (pauseStatus)
			{
				ERRCHECK(system.mixerSuspend());
			}
			else
			{
				ERRCHECK(system.mixerResume());
			}
		}
	}

	public bool IsPaused()
	{
		return isPaused;
	}

	private void Update()
	{
		if (isInitialized)
		{
			ERRCHECK(system.update());
		}
	}

	private void OnDisable()
	{
		if (isInitialized)
		{
			global::FMOD.Studio.UnityUtil.Log("__ SHUT DOWN FMOD SYSTEM __");
			ERRCHECK(system.release());
			if (this == sInstance)
			{
				sInstance = null;
			}
		}
	}

	private static bool ERRCHECK(global::FMOD.RESULT result)
	{
		return global::FMOD.Studio.UnityUtil.ERRCHECK(result);
	}
}
