namespace Kampai.UI
{
	internal sealed class AddFPSCounterCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::Kampai.Main.MainElement.UI_GLASSCANVAS)]
		public global::UnityEngine.GameObject GlassCanvas { get; set; }

		[Inject]
		public bool Show { get; set; }

		[Inject]
		public int SampleSize { get; set; }

		public override void Execute()
		{
			if (Show)
			{
				ShowCounter();
			}
			else
			{
				HideCounter();
			}
		}

		private void HideCounter()
		{
			global::Kampai.UI.KampaiFPSCounter componentInChildren = GlassCanvas.gameObject.GetComponentInChildren<global::Kampai.UI.KampaiFPSCounter>();
			if (!(componentInChildren == null))
			{
				global::UnityEngine.Object.Destroy(componentInChildren.gameObject);
			}
		}

		private void ShowCounter()
		{
			global::Kampai.UI.KampaiFPSCounter componentInChildren = GlassCanvas.gameObject.GetComponentInChildren<global::Kampai.UI.KampaiFPSCounter>();
			if (componentInChildren != null)
			{
				if (componentInChildren.SampleInterval == SampleSize)
				{
					return;
				}
				global::UnityEngine.Object.DestroyImmediate(componentInChildren.gameObject);
			}
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("FPSCounter", typeof(global::UnityEngine.RectTransform));
			gameObject.transform.SetParent(GlassCanvas.transform, false);
			global::UnityEngine.UI.Text text = gameObject.AddComponent<global::UnityEngine.UI.Text>();
			text.rectTransform.sizeDelta = global::UnityEngine.Vector2.zero;
			text.rectTransform.anchorMin = new global::UnityEngine.Vector2(0.4f, 0.002f);
			text.rectTransform.anchorMax = new global::UnityEngine.Vector2(0.8f, 0.1f);
			text.rectTransform.anchoredPosition = new global::UnityEngine.Vector2(0.5f, 0.5f);
			text.text = "30";
			text.font = global::UnityEngine.Resources.FindObjectsOfTypeAll<global::UnityEngine.Font>()[0];
			text.fontSize = 40;
			text.color = global::UnityEngine.Color.black;
			text.alignment = global::UnityEngine.TextAnchor.LowerCenter;
			componentInChildren = gameObject.AddComponent<global::Kampai.UI.KampaiFPSCounter>();
			componentInChildren.TextComponent = text;
			if (SampleSize > 0)
			{
				componentInChildren.SampleInterval = SampleSize;
			}
		}
	}
}
