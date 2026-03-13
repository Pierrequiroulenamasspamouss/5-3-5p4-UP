namespace Kampai.Game
{
	internal sealed class LuaReturnValueContainer : global::Kampai.Game.ReturnValueContainer
	{
		private global::System.Collections.Generic.Dictionary<string, global::Kampai.Game.LuaReturnValueContainer> keyValues = new global::System.Collections.Generic.Dictionary<string, global::Kampai.Game.LuaReturnValueContainer>();

		private global::System.Collections.Generic.List<global::Kampai.Game.LuaReturnValueContainer> arrayIndices = new global::System.Collections.Generic.List<global::Kampai.Game.LuaReturnValueContainer>();

		public LuaReturnValueContainer(global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
		}

		public override void Reset()
		{
			base.Reset();
			keyValues.Clear();
		}

		public int PushToStack(global::Kampai.Wrappers.LuaState L)
		{
			switch (base.type)
			{
			case global::Kampai.Game.ReturnValueContainer.ValueType.Number:
				L.lua_pushnumber(numberValue);
				return 1;
			case global::Kampai.Game.ReturnValueContainer.ValueType.String:
				L.lua_pushstring(stringValue);
				return 1;
			case global::Kampai.Game.ReturnValueContainer.ValueType.Boolean:
				L.lua_pushboolean(boolValue);
				return 1;
			case global::Kampai.Game.ReturnValueContainer.ValueType.Nil:
				L.lua_pushnil();
				return 1;
			case global::Kampai.Game.ReturnValueContainer.ValueType.Dictionary:
				return PushDictionary(L);
			case global::Kampai.Game.ReturnValueContainer.ValueType.Array:
				return PushArray(L);
			case global::Kampai.Game.ReturnValueContainer.ValueType.Void:
				return 0;
			default:
				logger.Error("LuaReturnValueContainer: Don't know how to push {0} onto stack.", global::System.Enum.GetName(typeof(global::Kampai.Game.ReturnValueContainer.ValueType), base.type));
				return 0;
			}
		}

		public int PushArrayValuesToStack(global::Kampai.Wrappers.LuaState L)
		{
			int count = arrayIndices.Count;
			for (int i = 0; i < count; i++)
			{
				if (arrayIndices[i].PushToStack(L) == 0)
				{
					L.lua_pushnil();
				}
			}
			return count;
		}

		protected override global::Kampai.Game.ReturnValueContainer GetContainerForKey(string key)
		{
			global::Kampai.Game.LuaReturnValueContainer value;
			if (keyValues.TryGetValue(key, out value))
			{
				return value;
			}
			value = new global::Kampai.Game.LuaReturnValueContainer(logger);
			keyValues[key] = value;
			return value;
		}

		protected override global::Kampai.Game.ReturnValueContainer GetContainerForNextIndex()
		{
			global::Kampai.Game.LuaReturnValueContainer luaReturnValueContainer = new global::Kampai.Game.LuaReturnValueContainer(logger);
			arrayIndices.Add(luaReturnValueContainer);
			return luaReturnValueContainer;
		}

		protected override void ClearKeys()
		{
			keyValues.Clear();
		}

		protected override void ClearArray()
		{
			arrayIndices.Clear();
		}

		private int PushDictionary(global::Kampai.Wrappers.LuaState L)
		{
			L.lua_createtable(0, keyValues.Count);
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::Kampai.Game.LuaReturnValueContainer> keyValue in keyValues)
			{
				if (keyValue.Value.PushToStack(L) == 0)
				{
					L.lua_pushnil();
				}
				L.lua_setfield(-2, keyValue.Key);
			}
			return 1;
		}

		private int PushArray(global::Kampai.Wrappers.LuaState L)
		{
			int count = arrayIndices.Count;
			L.lua_createtable(count, 0);
			for (int i = 0; i < count; i++)
			{
				if (arrayIndices[i].PushToStack(L) == 0)
				{
					L.lua_pushnil();
				}
				L.lua_rawseti(-2, i + 1);
			}
			return 1;
		}
	}
}
