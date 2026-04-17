using Discord.Sdk;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DiscordController : MonoBehaviour
{
    private const ulong applicationId = 1493210561616543777UL;

    [SerializeField] private float presenceUpdateInterval = 5f;

    private Discord.Sdk.Client client;
    private bool initialized;

    private static ulong gameStartTimeMs;
    private static MethodInfo runCallbacksStatic;

    private float nextUpdateUnscaled;

    public static DiscordController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (!initialized || client == null)
            return;

        TryRunCallbacksIfExposed();

        if (Time.unscaledTime >= nextUpdateUnscaled)
        {
            nextUpdateUnscaled = Time.unscaledTime + Mathf.Max(1f, presenceUpdateInterval);
            RefreshPresence();
        }
    }

    private void OnDestroy()
    {
        Shutdown();

        if (Instance == this)
            Instance = null;
    }

    private void OnApplicationQuit()
    {
        Shutdown();
    }

    private void Initialize()
    {
        if (initialized || client != null)
            return;

        if (!IsDiscordRunning())
            return;

        try
        {
            client = new Discord.Sdk.Client();
            client.AddLogCallback(OnDiscordLog, LoggingSeverity.Error);
            client.SetStatusChangedCallback(OnStatusChanged);
            client.SetApplicationId(applicationId);

            try
            {
                int pid = System.Diagnostics.Process.GetCurrentProcess().Id;
                client.SetGameWindowPid(pid);
            }
            catch { }

            if (gameStartTimeMs == 0UL)
                gameStartTimeMs = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            CacheOptionalRunCallbacks();

            nextUpdateUnscaled = Time.unscaledTime + Mathf.Max(1f, presenceUpdateInterval);
            initialized = true;

            RefreshPresence();
        }
        catch
        {
            client = null;
            initialized = false;
        }
    }

    private void Shutdown()
    {
        if (client != null)
        {
            try { client.ClearRichPresence(); } catch { }
            TryRunCallbacksIfExposed();
            try { client.Disconnect(); } catch { }
            TryRunCallbacksIfExposed();
            try { client.Dispose(); } catch { }
            client = null;
        }

        initialized = false;
    }

    private void RefreshPresence()
    {
        if (client == null || !initialized)
            return;

        UpdatePresence(BuildStateText(), "default_icon", "In Game", gameStartTimeMs);
    }

    private string BuildStateText()
    {
        return "Unity " + Application.unityVersion;
    }

    private void UpdatePresence(string state, string imageKey, string details, ulong startTimestampMs)
    {
        if (client == null)
            return;

        if (startTimestampMs == 0UL)
        {
            startTimestampMs = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            gameStartTimeMs = startTimestampMs;
        }

        Activity activity = new Activity();
        activity.SetType(ActivityTypes.Playing);
        activity.SetState(state);
        activity.SetDetails(details);

        ActivityAssets assets = new ActivityAssets();
        assets.SetLargeImage(imageKey);
        assets.SetLargeText("Unity Game");
        activity.SetAssets(assets);

        ActivityTimestamps timestamps = new ActivityTimestamps();
        timestamps.SetStart(startTimestampMs);
        activity.SetTimestamps(timestamps);

        client.UpdateRichPresence(activity, OnUpdateRichPresence);
    }

    private void OnUpdateRichPresence(ClientResult result)
    {
        if (!result.Successful())
            Debug.LogError("[DiscordController] Failed to update rich presence: " + result.Error());
    }

    private void OnDiscordLog(string message, LoggingSeverity severity) { }

    private void OnStatusChanged(Discord.Sdk.Client.Status status, Discord.Sdk.Client.Error error, int errorCode)
    {
        if (error != Discord.Sdk.Client.Error.None)
            Debug.LogError("[DiscordController] Error: " + error + " (" + errorCode + ")");
    }

    private bool IsDiscordRunning()
    {
        try
        {
            return System.Diagnostics.Process.GetProcessesByName("Discord").Any();
        }
        catch
        {
            return false;
        }
    }

    private static void CacheOptionalRunCallbacks()
    {
        if (runCallbacksStatic != null)
            return;

        string[] candidateTypeNames =
        {
            "discordpp",
            "discordpp.discordpp",
            "Discord.Sdk.discordpp",
            "Discord.Sdk.NativeMethods"
        };

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var tn in candidateTypeNames)
            {
                try
                {
                    var t = asm.GetType(tn, false);
                    if (t == null) continue;

                    var mi = t.GetMethod("RunCallbacks",
                        BindingFlags.Public | BindingFlags.Static,
                        null, Type.EmptyTypes, null);

                    if (mi != null)
                    {
                        runCallbacksStatic = mi;
                        return;
                    }
                }
                catch { }
            }
        }
    }

    private static void TryRunCallbacksIfExposed()
    {
        if (runCallbacksStatic == null)
            return;

        try
        {
            runCallbacksStatic.Invoke(null, null);
        }
        catch { }
    }
}