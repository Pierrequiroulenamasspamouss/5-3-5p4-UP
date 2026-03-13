namespace Kampai.BuildingsSizeToolbox
{
	public class BuildingsSelectorListItemView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text Title;

		public global::UnityEngine.UI.Image Background;

		private global::Kampai.Game.BuildingDefinition buildingDefiniition;

		public global::strange.extensions.signal.impl.Signal<global::Kampai.Game.BuildingDefinition> ClickedSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.BuildingDefinition>();

		private bool modified;

		[Inject]
		public global::Kampai.BuildingsSizeToolbox.BuildingModifiedSignal buildingModifiedSignal { get; set; }

		[Inject]
		public global::Kampai.BuildingsSizeToolbox.BuildingsStateSavedSignal buildingsStateSavedSignal { get; set; }

		internal void Setup(global::Kampai.Game.BuildingDefinition def)
		{
			buildingDefiniition = def;
			Title.text = string.Format("{0}: {1}", def.ID, def.LocalizedKey);
			updateColor();
		}

		protected override void Start()
		{
			base.Start();
			buildingModifiedSignal.AddListener(delegate(global::Kampai.Game.BuildingDefinition d)
			{
				if (d == buildingDefiniition)
				{
					modified = true;
					updateColor();
				}
			});
			buildingsStateSavedSignal.AddListener(delegate
			{
				modified = false;
				updateColor();
			});
		}

		public void OnClick()
		{
			ClickedSignal.Dispatch(buildingDefiniition);
		}

		private void updateColor()
		{
			if (buildingDefiniition.UiScale < global::UnityEngine.Mathf.Epsilon || buildingDefiniition.UiPosition == global::UnityEngine.Vector3.zero)
			{
				Background.color = global::UnityEngine.Color.red;
			}
			else if (modified)
			{
				Background.color = global::UnityEngine.Color.cyan;
			}
			else
			{
				Background.color = global::UnityEngine.Color.white;
			}
		}
	}
}
