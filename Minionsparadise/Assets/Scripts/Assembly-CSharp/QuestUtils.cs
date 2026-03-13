public static class QuestUtils
{
	public static global::System.Collections.Generic.List<global::Kampai.Game.Quest> ResolveQuests(global::System.Collections.Generic.List<global::Kampai.Game.Quest> quests)
	{
		global::System.Collections.Generic.List<global::Kampai.Game.Quest> list = new global::System.Collections.Generic.List<global::Kampai.Game.Quest>();
		if (quests == null)
		{
			return list;
		}
		quests.Sort();
		int num = -1;
		int num2 = -1;
		foreach (global::Kampai.Game.Quest quest in quests)
		{
			if (quest == null)
			{
				continue;
			}
			global::Kampai.Game.QuestDefinition activeDefinition = quest.GetActiveDefinition();
			if (activeDefinition != null)
			{
				if (activeDefinition.SurfaceID != num)
				{
					list.Add(quest);
					num = activeDefinition.SurfaceID;
					num2 = activeDefinition.QuestPriority;
				}
				else if (activeDefinition.QuestPriority >= num2)
				{
					list.Add(quest);
					num2 = activeDefinition.QuestPriority;
				}
			}
		}
		return list;
	}
}
