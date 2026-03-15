namespace Kampai.UI.View
{
	public class ResourceIconPanelView : global::Kampai.Util.KampaiView
	{
		private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView>> trackedResourceIcons;

		private global::Kampai.Util.IKampaiLogger logger;

		private bool isForceHideEnabled;

		internal void Init(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
			trackedResourceIcons = new global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView>>();
		}

		internal void Cleanup()
		{
			if (trackedResourceIcons == null)
			{
				return;
			}
			foreach (global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView> value in trackedResourceIcons.Values)
			{
				if (value == null)
				{
					continue;
				}
				foreach (global::Kampai.UI.View.ResourceIconView value2 in value.Values)
				{
					if (value2 != null)
					{
						global::UnityEngine.Object.Destroy(value2.gameObject);
					}
				}
			}
			trackedResourceIcons.Clear();
		}

		internal void CreateResourceIcon(global::Kampai.UI.View.ResourceIconSettings resourceIconSettings)
		{
			int trackedId = resourceIconSettings.TrackedId;
			int itemDefId = resourceIconSettings.ItemDefId;
			logger.Info("Creating Resource Icon with id: {0} and itemDefId: {1}", trackedId, itemDefId);
			global::Kampai.UI.View.ResourceIconView resourceIconView = null;
			if ((resourceIconView = GetResourceIcon(trackedId, itemDefId)) != null)
			{
				if (base.gameObject.activeInHierarchy)
				{
					resourceIconView.UpdateIconCount(resourceIconSettings.Count);
					logger.Info("Resource Icon with id: {0} and itemDefId: {1} already exists, ignoring create but updating count", trackedId, itemDefId);
				}
				else
				{
					global::Kampai.UI.View.WorldToGlassUIModal component = resourceIconView.GetComponent<global::Kampai.UI.View.WorldToGlassUIModal>();
					component.Settings = resourceIconSettings;
				}
				return;
			}
			resourceIconView = global::Kampai.UI.View.WorldToGlassUIBuilder.Build<global::Kampai.UI.View.ResourceIconView>("cmp_HarvestIcon", base.transform, resourceIconSettings, logger);
			global::Kampai.UI.View.WorldToGlassUIModal component2 = resourceIconView.GetComponent<global::Kampai.UI.View.WorldToGlassUIModal>();
			logger.Info("Got Resource Icon Modal " + component2.name);
			if (ContainsResourceIcon(trackedId))
			{
				trackedResourceIcons[trackedId].Add(itemDefId, resourceIconView);
				global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView>.ValueCollection values = trackedResourceIcons[trackedId].Values;
				UpdateIconIndexes(values);
			}
			else
			{
				global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView> dictionary = new global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView>();
				dictionary.Add(itemDefId, resourceIconView);
				trackedResourceIcons.Add(trackedId, dictionary);
			}
			resourceIconView.SetForceHide(isForceHideEnabled);
		}

		private void UpdateIconIndexes(global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView>.ValueCollection resourceIconViews)
		{
			int count = resourceIconViews.Count;
			float num = -(count / 2);
			if (count % 2 == 0)
			{
				num += 0.4f;
			}
			int num2 = 0;
			global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView>.ValueCollection.Enumerator enumerator = resourceIconViews.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::Kampai.UI.View.ResourceIconView current = enumerator.Current;
					current.UpdateIconIndex(num + (float)num2);
					num2++;
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		private bool ContainsResourceIcon(int trackedId)
		{
			if (trackedResourceIcons != null && trackedResourceIcons.ContainsKey(trackedId))
			{
				return true;
			}
			return false;
		}

		private bool ContainsResourceIcon(int trackedId, int itemDefId)
		{
			if (ContainsResourceIcon(trackedId))
			{
				global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView> dictionary = trackedResourceIcons[trackedId];
				if (dictionary != null && dictionary.ContainsKey(itemDefId))
				{
					return true;
				}
			}
			return false;
		}

		internal void UpdateResourceIconCount(int trackedId, int itemId, int count)
		{
			global::Kampai.UI.View.ResourceIconView resourceIcon = GetResourceIcon(trackedId, itemId);
			if (resourceIcon != null)
			{
				resourceIcon.UpdateIconCount(count);
				return;
			}
			logger.Warning("Could not find resource icon with id: {0} and itemId: {1} ", trackedId, itemId);
		}

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView> GetResourceIcon(int trackedId)
		{
			if (ContainsResourceIcon(trackedId))
			{
				return trackedResourceIcons[trackedId];
			}
			return null;
		}

		private global::Kampai.UI.View.ResourceIconView GetResourceIcon(int trackedId, int itemDefId)
		{
			global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView> resourceIcon = GetResourceIcon(trackedId);
			if (resourceIcon != null && resourceIcon.ContainsKey(itemDefId))
			{
				return resourceIcon[itemDefId];
			}
			return null;
		}

		internal void RemoveResourceIcon(int trackedId)
		{
			logger.Info("Removing Resource Icon with id: {0}", trackedId);
			if (ContainsResourceIcon(trackedId))
			{
				global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView> dictionary = trackedResourceIcons[trackedId];
				{
					foreach (int key in dictionary.Keys)
					{
						RemoveResourceIcon(trackedId, key);
					}
					return;
				}
			}
			logger.Warning("Resource Icon with id: {0} will not be removed since it doesn't exist!", trackedId);
		}

		internal void RemoveResourceIcon(int trackedId, int itemDefId)
		{
			logger.Info("Removing Resource Icon with id: {0} and itemDefId: {1}", trackedId, itemDefId);
			if (ContainsResourceIcon(trackedId, itemDefId))
			{
				global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView> dictionary = trackedResourceIcons[trackedId];
				global::Kampai.UI.View.ResourceIconView resourceIconView = dictionary[itemDefId];
				resourceIconView.Close();
				dictionary.Remove(itemDefId);
				if (dictionary.Count == 0)
				{
					trackedResourceIcons.Remove(trackedId);
				}
				else
				{
					UpdateIconIndexes(dictionary.Values);
				}
			}
			else
			{
				logger.Warning("Ignoring remove Resource Icon with id: {0} and itemDefId: {1} since it doesn't exist", trackedId, itemDefId);
			}
		}

		private void SetForceHideForAllResourceIcons()
		{
			foreach (global::System.Collections.Generic.Dictionary<int, global::Kampai.UI.View.ResourceIconView> value in trackedResourceIcons.Values)
			{
				foreach (global::Kampai.UI.View.ResourceIconView value2 in value.Values)
				{
					value2.SetForceHide(isForceHideEnabled);
				}
			}
		}

		internal void HideAllResourceIcons()
		{
			isForceHideEnabled = true;
			SetForceHideForAllResourceIcons();
		}

		internal void ShowAllResourceIcons()
		{
			isForceHideEnabled = false;
			SetForceHideForAllResourceIcons();
		}
	}
}
