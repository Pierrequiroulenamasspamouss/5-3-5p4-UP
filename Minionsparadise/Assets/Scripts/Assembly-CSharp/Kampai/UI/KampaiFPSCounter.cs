namespace Kampai.UI
{
	internal sealed class KampaiFPSCounter : global::UnityEngine.MonoBehaviour
	{
		private int m_sampleInterval;

		private float[] filteringKernel;

		private int currentSample;

		private global::Kampai.Util.FastTextStreamWriter fpsStream;

		private int bufferSize = 30;

		private float min;

		private float max;

		public global::UnityEngine.UI.Text TextComponent { get; set; }

		internal int SampleInterval
		{
			get
			{
				return m_sampleInterval * bufferSize;
			}
			set
			{
				m_sampleInterval = ((value == 0) ? 1 : value);
			}
		}

		private float getFilteredFrameTime()
		{
			int num = global::System.Math.Min(bufferSize, currentSample);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				num2 += filteringKernel[i];
			}
			return num2 / (float)num;
		}

		private void Awake()
		{
#if !UNITY_WEBPLAYER
			fpsStream = new global::Kampai.Util.FastTextStreamWriter(new global::System.IO.FileStream(global::System.IO.Path.Combine(global::Kampai.Util.GameConstants.PERSISTENT_DATA_PATH, "fps.log.txt"), global::System.IO.FileMode.Create, global::System.IO.FileAccess.Write));
#endif
			bufferSize = ((global::UnityEngine.Application.targetFrameRate > 0) ? global::UnityEngine.Application.targetFrameRate : 30);
		}

		private void Update()
		{
			if ((filteringKernel == null && bufferSize > 0) || (filteringKernel != null && bufferSize != filteringKernel.Length))
			{
				filteringKernel = new float[bufferSize];
			}
			float num = 1f / global::UnityEngine.Time.smoothDeltaTime;
			int num2 = currentSample++ % bufferSize;
			int num3 = global::UnityEngine.Mathf.FloorToInt(getFilteredFrameTime());
			if (num > max)
			{
				max = num;
			}
			if (num < min)
			{
				min = num;
			}
			filteringKernel[num2] = num;
			TextComponent.text = string.Format("{0} {1}/{2}", num3, global::UnityEngine.Mathf.FloorToInt(min), global::UnityEngine.Mathf.FloorToInt(max));
			if (currentSample == SampleInterval)
			{
				currentSample = 0;
				min = (max = num);
			}
#if !UNITY_WEBPLAYER
			if (fpsStream != null)
			{
				fpsStream.WriteLine(num);
			}
#endif
		}

		public void OnDestroy()
		{
#if !UNITY_WEBPLAYER
			if (fpsStream != null)
			{
				fpsStream.Flush();
				fpsStream.Dispose();
			}
#endif
		}
	}
}
