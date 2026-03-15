namespace Kampai.UI.View
{
	public class DisplayItemPopupCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int definitionID { get; set; }

		[Inject]
		public global::UnityEngine.RectTransform imageTransform { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIPopupType popupType { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[4];
			imageTransform.GetWorldCorners(array);
			global::UnityEngine.Vector3 position = default(global::UnityEngine.Vector3);
			float num = array[0].y;
			global::UnityEngine.Vector3[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				global::UnityEngine.Vector3 vector = array2[i];
				position += vector;
				num = global::UnityEngine.Mathf.Min(num, vector.y);
			}
			position /= 4f;
			if (popupType == global::Kampai.UI.View.UIPopupType.HELPTIP)
			{
				position.y = num;
			}
			global::UnityEngine.Vector3 vector2 = uiCamera.WorldToViewportPoint(position);
			global::Kampai.UI.View.IGUICommand iGUICommand = null;
			switch (popupType)
			{
			case global::Kampai.UI.View.UIPopupType.GENERIC:
			case global::Kampai.UI.View.UIPopupType.GENERICGOTO:
				iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "cmp_GenericTooltip");
				break;
			case global::Kampai.UI.View.UIPopupType.CRAFTING:
				iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "cmp_CraftingTooltip");
				break;
			case global::Kampai.UI.View.UIPopupType.MINIONPARTYHUD:
				iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "screen_PartyMeterFlyout");
				iGUICommand.skrimScreen = "GenericPopup";
				iGUICommand.darkSkrim = false;
				iGUICommand.genericPopupSkrim = true;
				break;
			case global::Kampai.UI.View.UIPopupType.MINIONPARTYCOUNTDOWN:
				iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "HUD_PartyMeterCountDownTimer");
				break;
			case global::Kampai.UI.View.UIPopupType.HELPTIP:
				iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "popup_HelpTip");
				break;
			}
			if (iGUICommand != null)
			{
				global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
				args.Add(vector2);
				args.Add(typeof(global::UnityEngine.RectTransform), imageTransform);
				if (popupType == global::Kampai.UI.View.UIPopupType.GENERIC || popupType == global::Kampai.UI.View.UIPopupType.GENERICGOTO || popupType == global::Kampai.UI.View.UIPopupType.CRAFTING)
				{
					global::Kampai.Game.ItemDefinition value = definitionService.Get<global::Kampai.Game.ItemDefinition>(definitionID);
					args.Add(typeof(global::Kampai.Game.ItemDefinition), value);
					args.Add(typeof(bool), popupType == global::Kampai.UI.View.UIPopupType.GENERICGOTO);
				}
				else if (popupType == global::Kampai.UI.View.UIPopupType.HELPTIP)
				{
					global::Kampai.Game.HelpTipDefinition value2 = definitionService.Get<global::Kampai.Game.HelpTipDefinition>(definitionID);
					args.Add(typeof(global::Kampai.Game.HelpTipDefinition), value2);
				}
				guiService.Execute(iGUICommand);
			}
		}
	}
}
