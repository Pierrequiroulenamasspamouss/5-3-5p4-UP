namespace Kampai.Game
{
	public static class AudioCommandHelper
	{
		public static void playSound(global::UnityEngine.AudioSource source, string audioClip, bool oneShot)
		{
			if ((source.clip == null || source.clip.name != audioClip) && !oneShot)
			{
				source.clip = global::UnityEngine.Resources.Load(audioClip) as global::UnityEngine.AudioClip;
				source.Play();
			}
			else if (oneShot)
			{
				global::UnityEngine.AudioClip clip = global::UnityEngine.Resources.Load(audioClip) as global::UnityEngine.AudioClip;
				source.PlayOneShot(clip);
			}
		}
	}
}
