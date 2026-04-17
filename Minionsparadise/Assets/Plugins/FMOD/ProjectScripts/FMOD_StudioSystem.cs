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
				// global::UnityEngine.Debug.Log("[FMOD-DEBUG] Creating FMOD_StudioSystem GameObject...");
				global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("FMOD_StudioSystem");
				sInstance = gameObject.AddComponent<FMOD_StudioSystem>();
				// global::UnityEngine.Debug.Log("[FMOD-DEBUG] Attempting ForceLoadLowLevelBinary...");
				if (!global::FMOD.Studio.UnityUtil.ForceLoadLowLevelBinary())
				{
					global::FMOD.Studio.UnityUtil.LogError("Unable to load low level binary!");
					return sInstance;
				}
				// global::UnityEngine.Debug.Log("[FMOD-DEBUG] Calling Init()...");
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
		global::FMOD.Studio.EventInstance eventInstance = default(global::FMOD.Studio.EventInstance);
		if (string.IsNullOrEmpty(path))
		{
			global::FMOD.Studio.UnityUtil.LogError("Empty event path!");
			return eventInstance;
		}
		if (!system.isValid())
		{
			return eventInstance;
		}
		if (eventDescriptions.ContainsKey(path) && eventDescriptions[path].isValid())
		{
			ERRCHECK(eventDescriptions[path].createInstance(out eventInstance));
		}
		else
		{
			global::FMOD.GUID guid = default(global::FMOD.GUID);
			if (path.StartsWith("{"))
			{
				ERRCHECK(global::FMOD.Studio.Util.parseID(path, out guid));
			}
			else if (path.StartsWith("event:"))
			{
				ERRCHECK(system.lookupID(path, out guid));
			}
			else
			{
				global::FMOD.Studio.UnityUtil.LogError("Expected event path to start with 'event:/'");
			}
			global::FMOD.Studio.EventDescription _event = default(global::FMOD.Studio.EventDescription);
			ERRCHECK(system.getEventByID(guid, out _event));
			if (_event.isValid())
			{
				eventDescriptions[path] = _event;
				ERRCHECK(_event.createInstance(out eventInstance));
			}
		}
		if (!eventInstance.isValid())
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
		if (!eventInstance.isValid())
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
			// global::UnityEngine.Debug.Log("[FMOD-DEBUG] Init: Starting...");
			if (base.gameObject == null) {
				// global::UnityEngine.Debug.LogError("[FMOD-DEBUG] Init: base.gameObject is NULL!");
				return;
			}
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			// global::FMOD.Studio.UnityUtil.Log("FMOD_StudioSystem: System_Create");
			
			// global::UnityEngine.Debug.Log("[FMOD-DEBUG] Init: Calling System.create...");
			
			global::FMOD.RESULT result = global::FMOD.Studio.System.create(out this.system);
			if (result != global::FMOD.RESULT.OK || !this.system.isValid()) {
				// global::UnityEngine.Debug.LogError("[FMOD-DEBUG] Init: System.create FAILED with " + result.ToString());
				return;
			}
			
			global::FMOD.Studio.INITFLAGS iNITFLAGS = global::FMOD.Studio.INITFLAGS.NORMAL;
			global::FMOD.Studio.UnityUtil.Log("FMOD_StudioSystem: system.init");
			global::FMOD.RESULT rESULT = global::FMOD.RESULT.OK;
			
			// global::UnityEngine.Debug.Log("[FMOD-DEBUG] Init: Calling system.initialize...");
			rESULT = this.system.initialize(1024, iNITFLAGS, global::FMOD.INITFLAGS.NORMAL, global::System.IntPtr.Zero);
			if (rESULT == global::FMOD.RESULT.ERR_HEADER_MISMATCH)
			{
				global::FMOD.Studio.UnityUtil.LogError("Version mismatch between C# script and FMOD binary, restart Unity and reimport the integration package to resolve this issue.");
				return;
			}
			else if (rESULT != global::FMOD.RESULT.OK)
			{
				// global::UnityEngine.Debug.LogError("[FMOD-DEBUG] Init: system.initialize FAILED with " + rESULT.ToString());
				return;
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
				ERRCHECK(this.system.getCoreSystem(out system));
				rESULT = this.system.initialize(1024, iNITFLAGS, global::FMOD.INITFLAGS.NORMAL, global::System.IntPtr.Zero);
				ERRCHECK(rESULT);
			}
			global::UnityEngine.Debug.Log("[FMOD-DEBUG] Init: SUCCESS!");
			isInitialized = true;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		isPaused = pauseStatus;
		if (this.system.isValid())
		{
			global::FMOD.Studio.UnityUtil.Log("Pause state changed to: " + pauseStatus);
			global::FMOD.System system;
			ERRCHECK(this.system.getCoreSystem(out system));
			if (!system.hasHandle())
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
			system.clearHandle();
			isInitialized = false;
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
