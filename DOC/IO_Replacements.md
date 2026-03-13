# Restricted API Replacements & Wrappers

This document lists all `System.IO`, `SafeHandle`, and platform-specific API calls that have been wrapped or replaced to ensure compatibility with the restricted Unity WebPlayer environment.

## SafeHandle Fixes

| Original Code | Replacement / Fix | Files Affected |
|---------------|-------------------|----------------|
| `base.IsClosed` | Removed (check `handle == global::System.IntPtr.Zero`) | 12 `NimbleBridge_*.cs` files |
| `SetHandleAsInvalid()` | `SetHandle(global::System.IntPtr.Zero)` | `NimbleBridge_NotificationListener.cs` |

## System.IO Wrappers (`#if !UNITY_WEBPLAYER`)

| API Call | Wrapper Logic | Impact |
|----------|---------------|--------|
| `File.Exists(path)` | Returns `false` on WebPlayer | Prevents I/O errors; code handles missing files gracefully. |
| `File.Delete(path)` | Skipped on WebPlayer | Log/temp file cleanup ignored. |
| `File.ReadAllBytes(path)` | Skipped on WebPlayer (returns `null`) | Asset loading via disk skipped. |
| `Directory.CreateDirectory(path)` | Skipped on WebPlayer | Folder creation ignored. |
| `FileInfo.Length` | Returns `0L` on WebPlayer | File size checks simplified. |
| `DirectoryInfo.GetFiles()` | Skipped on WebPlayer | Directory enumeration removed from Debug Console. |

## Platform-Specific Wrappers (`#if !UNITY_WEBPLAYER && UNITY_ANDROID`)

| API Call | Wrapper Logic | Impact |
|----------|---------------|--------|
| `AndroidJavaClass` | Skipped on WebPlayer | Prevents compilation errors for Android-only APIs. |
| `Process.StartTime` | Uses `DateTime.Now` on WebPlayer | Fallback for restricted diagnostics. |

## Files Modified (Summary)

- **NimbleBridge Handles**: 12 files updated for `SafeHandle` compatibility.
- **Kampai Utils**: `CrossPlatformFile.cs`, `Native.cs`, `DebugConsoleController.cs`, `DownloadUtil.cs`.
- **Kampai Services**: `ManifestService.cs`, `AssetBundlesService.cs`, `DefinitionService.cs`, `TimeService.cs`.
- **External Plugins**: `HockeyAppAndroid.cs`, `HockeyAppIOS.cs`, `DefaultRequest.cs`, `NimbleRequest.cs`.
- **Swrve SDK**: `CrossPlatformFile.cs` usage verified (Safe).
