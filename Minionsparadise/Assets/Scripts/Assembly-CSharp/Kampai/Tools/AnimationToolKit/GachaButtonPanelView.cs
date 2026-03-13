namespace Kampai.Tools.AnimationToolKit
{
	public class GachaButtonPanelView : global::Kampai.Util.KampaiView
	{
		private const float itemHeight = 64f;

		private global::System.Collections.Generic.List<global::Kampai.Tools.AnimationToolKit.GachaButtonView> views;

		private global::UnityEngine.RectTransform scrollViewContent;

		private global::UnityEngine.GameObject gachaButtonPrefab;

		internal void Init(global::System.Collections.Generic.ICollection<global::Kampai.Game.MinionAnimationDefinition> mads, global::System.Collections.Generic.ICollection<global::Kampai.Game.GachaAnimationDefinition> gads)
		{
			views = new global::System.Collections.Generic.List<global::Kampai.Tools.AnimationToolKit.GachaButtonView>();
			gachaButtonPrefab = global::UnityEngine.Resources.Load("GachaButton") as global::UnityEngine.GameObject;
			scrollViewContent = base.gameObject.FindChild("Scroll_Box").GetComponent<global::UnityEngine.RectTransform>();
			global::System.Collections.Generic.List<global::Kampai.Game.AnimationDefinition> list = new global::System.Collections.Generic.List<global::Kampai.Game.AnimationDefinition>();
			foreach (global::Kampai.Game.GachaAnimationDefinition gad in gads)
			{
				list.Add(gad);
			}
			foreach (global::Kampai.Game.MinionAnimationDefinition mad in mads)
			{
				if (mad.arguments == null || !mad.arguments.ContainsKey("actor"))
				{
					list.Add(mad);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				global::Kampai.Tools.AnimationToolKit.GachaButtonView item = CreateView(list[i], i);
				views.Add(item);
			}
			scrollViewContent.offsetMin = new global::UnityEngine.Vector2(0f, 0f - (float)list.Count * 64f);
			scrollViewContent.offsetMax = global::UnityEngine.Vector2.zero;
		}

		public void SetButtonCallback(global::System.Action<global::Kampai.Game.AnimationDefinition> callback)
		{
			foreach (global::Kampai.Tools.AnimationToolKit.GachaButtonView view in views)
			{
				view.FireGachaSignal.AddListener(callback);
			}
		}

		private global::Kampai.Tools.AnimationToolKit.GachaButtonView CreateView(global::Kampai.Game.AnimationDefinition def, int index)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(gachaButtonPrefab);
			global::Kampai.Tools.AnimationToolKit.GachaButtonView component = gameObject.GetComponent<global::Kampai.Tools.AnimationToolKit.GachaButtonView>();
			component.SetGachaDefinition(def);
			string text = string.Empty;
			global::Kampai.Game.MinionAnimationDefinition minionAnimationDefinition = def as global::Kampai.Game.MinionAnimationDefinition;
			if (minionAnimationDefinition != null)
			{
				text = FormatMinionAnimationDefinition(minionAnimationDefinition);
			}
			global::Kampai.Game.GachaAnimationDefinition gachaAnimationDefinition = def as global::Kampai.Game.GachaAnimationDefinition;
			if (gachaAnimationDefinition != null)
			{
				text = FormatGachaAnimationDefinition(gachaAnimationDefinition);
			}
			global::UnityEngine.Transform transform = gameObject.transform;
			global::UnityEngine.UI.Text componentInChildren = gameObject.GetComponentInChildren<global::UnityEngine.UI.Text>();
			componentInChildren.text = text;
			transform.parent = scrollViewContent;
			UIUtils.ScaleFonts(gameObject);
			global::UnityEngine.RectTransform rectTransform = transform as global::UnityEngine.RectTransform;
			rectTransform.offsetMin = new global::UnityEngine.Vector2(0f, -64f * (float)(index + 1));
			rectTransform.offsetMax = new global::UnityEngine.Vector2(0f, -64f * (float)index);
			return component;
		}

		private string FormatMinionAnimationDefinition(global::Kampai.Game.MinionAnimationDefinition def)
		{
			string[] array = def.StateMachine.Split('/');
			string arg = array[array.Length - 1];
			string[] array2 = def.GetType().ToString().Split('.');
			arg = string.Format("{0}\ntype: {1}\n", arg, array2[array2.Length - 1]);
			if (def.arguments != null)
			{
				foreach (global::System.Collections.Generic.KeyValuePair<string, object> argument in def.arguments)
				{
					arg = string.Format("{0}\t{1}: {2}", arg, argument.Key, argument.Value);
				}
			}
			return arg;
		}

		private string FormatGachaAnimationDefinition(global::Kampai.Game.GachaAnimationDefinition def)
		{
			string arg = string.Format("GachaId: {0}\tAnimationId: {1}", def.ID, def.AnimationID);
			arg = ((def.Minions != 0) ? string.Format("{0}\ntype: Coordinated Gacha\tMinionCount: {1}", arg, def.Minions) : string.Format("{0}\ntype: Solo Gacha", arg));
			if (!string.IsNullOrEmpty(def.Prefab))
			{
				arg = string.Format("{0}\nprefab: {1}", arg, def.Prefab);
			}
			return arg;
		}
	}
}
