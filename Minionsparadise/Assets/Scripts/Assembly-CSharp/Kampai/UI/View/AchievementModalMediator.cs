using System.Collections.Generic;

namespace Kampai.UI.View
{
	public class AchievementModalMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.AchievementModalView view { get; set; }

		[Inject]
		public global::Kampai.Game.IAchievementService achievementService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();

			view.Init();
			view.Open();
			
			// Handle Close button (standard for PopupMenuView)
			if (view.OnMenuClose != null)
			{
				view.OnMenuClose.AddListener(OnMenuClose);
			}

			global::System.Collections.Generic.List<global::Kampai.Game.AchievementDefinition> definitions = definitionService.GetAll<global::Kampai.Game.AchievementDefinition>();
			List<global::Kampai.Game.Achievement> achievements = new List<global::Kampai.Game.Achievement>();
			
			if (definitions != null)
			{
				foreach (global::Kampai.Game.AchievementDefinition def in definitions)
				{
					// Use def.ID (the AchievementDefinition ID) instead of def.DefinitionID (the tracked ID)
					global::Kampai.Game.Achievement ach = achievementService.GetAchievementByDefinitionID(def.ID);
					if (ach != null)
					{
						achievements.Add(ach);
					}
				}
			}

			view.CreateAchievements(achievements, localizationService);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			view.OnMenuClose.RemoveListener(OnMenuClose);
		}

		private void OnMenuClose()
		{
			// Properly unload the instance
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_QuestPanel");
			showHUDSignal.Dispatch(true); // Restore HUD
		}
	}
}
