namespace Kampai.UI.View
{
	public class FloatingTextPanelView : global::Kampai.Util.KampaiView
	{
		private global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.FloatingTextView> trackedFloatingTexts;

		private global::Kampai.Util.IKampaiLogger logger;

		internal void Init(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
			trackedFloatingTexts = new global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.FloatingTextView>();
		}

		internal void Cleanup()
		{
			if (trackedFloatingTexts == null)
			{
				return;
			}
			foreach (global::Kampai.UI.View.FloatingTextView value in trackedFloatingTexts.Values)
			{
				if (value != null)
				{
					global::UnityEngine.Object.Destroy(value.gameObject);
				}
			}
			trackedFloatingTexts.Clear();
		}

		internal void CreateFloatingText(global::Kampai.UI.View.FloatingTextSettings settings)
		{
			int trackedId = settings.TrackedId;
			logger.Info("Creating Text WayFinder with id: {0}", trackedId);
			global::Kampai.UI.View.FloatingTextView floatingTextView = null;
			if ((floatingTextView = GetFloatingText(trackedId)) != null)
			{
				logger.Info("Text WayFinder with id: {0} already exists, ignoring", trackedId);
				return;
			}
			floatingTextView = global::Kampai.UI.View.WorldToGlassUIBuilder.Build<global::Kampai.UI.View.FloatingTextView>("cmp_FloatingText", base.transform, settings, logger);
			if (settings.heightOverrideActive)
			{
				floatingTextView.SetHeight(settings.height);
			}
			trackedFloatingTexts.Add(trackedId, floatingTextView);
		}

		internal void RemoveFloatingText(int trackedId)
		{
			logger.Info("Removing Text WayFinder with id: {0}", trackedId);
			if (ContainsFloatingText(trackedId))
			{
				global::Kampai.UI.View.FloatingTextView floatingTextView = trackedFloatingTexts[trackedId];
				trackedFloatingTexts.Remove(trackedId);
				global::UnityEngine.Object.Destroy(floatingTextView.gameObject);
			}
			else
			{
				logger.Warning("Text WayFinder with id: {0} will not be removed since it doesn't exist!", trackedId);
			}
		}

		internal void ToggleAllFloatingText(bool show)
		{
			if (trackedFloatingTexts == null || trackedFloatingTexts.Count == 0)
			{
				return;
			}
			foreach (global::Kampai.UI.View.FloatingTextView value in trackedFloatingTexts.Values)
			{
				value.SetForceHide(!show);
			}
		}

		private global::Kampai.UI.View.FloatingTextView GetFloatingText(int trackedId)
		{
			if (ContainsFloatingText(trackedId))
			{
				return trackedFloatingTexts[trackedId];
			}
			return null;
		}

		private bool ContainsFloatingText(int trackedId)
		{
			if (trackedFloatingTexts != null && trackedFloatingTexts.ContainsKey(trackedId))
			{
				return true;
			}
			return false;
		}
	}
}
