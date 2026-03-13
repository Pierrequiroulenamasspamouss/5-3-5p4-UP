namespace Kampai.Util.Audio
{
	public static class GetAudioEmitter
	{
		public static CustomFMOD_StudioEventEmitter Get(global::UnityEngine.GameObject gameObject, string name = null)
		{
			CustomFMOD_StudioEventEmitter customFMOD_StudioEventEmitter = null;
			CustomFMOD_StudioEventEmitter[] components = gameObject.GetComponents<CustomFMOD_StudioEventEmitter>();
			int num = components.Length;
			if (num > 0)
			{
				if (string.IsNullOrEmpty(name))
				{
					customFMOD_StudioEventEmitter = components[0];
				}
				else
				{
					CustomFMOD_StudioEventEmitter[] array = components;
					foreach (CustomFMOD_StudioEventEmitter customFMOD_StudioEventEmitter2 in array)
					{
						if (customFMOD_StudioEventEmitter2.id.Equals(name))
						{
							customFMOD_StudioEventEmitter = customFMOD_StudioEventEmitter2;
							break;
						}
					}
				}
			}
			if (customFMOD_StudioEventEmitter == null)
			{
				customFMOD_StudioEventEmitter = gameObject.AddComponent<CustomFMOD_StudioEventEmitter>();
				customFMOD_StudioEventEmitter.startEventOnAwake = false;
				customFMOD_StudioEventEmitter.shiftPosition = true;
				customFMOD_StudioEventEmitter.staticSound = false;
				customFMOD_StudioEventEmitter.id = ((name != null) ? name : string.Empty);
			}
			return customFMOD_StudioEventEmitter;
		}
	}
}
