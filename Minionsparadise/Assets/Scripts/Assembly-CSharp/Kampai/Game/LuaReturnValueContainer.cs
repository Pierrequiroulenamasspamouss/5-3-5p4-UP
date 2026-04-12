using MoonSharp.Interpreter;
using System.Collections.Generic;

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

		public DynValue ToDynValue()
		{
			switch (base.type)
			{
			case global::Kampai.Game.ReturnValueContainer.ValueType.Number:
				return DynValue.NewNumber(numberValue);
			case global::Kampai.Game.ReturnValueContainer.ValueType.String:
				return DynValue.NewString(stringValue);
			case global::Kampai.Game.ReturnValueContainer.ValueType.Boolean:
				return DynValue.NewBoolean(boolValue);
			case global::Kampai.Game.ReturnValueContainer.ValueType.Nil:
				return DynValue.Nil;
			case global::Kampai.Game.ReturnValueContainer.ValueType.Dictionary:
				return ToDictionaryDynValue();
			case global::Kampai.Game.ReturnValueContainer.ValueType.Array:
				return ToArrayDynValue();
			case global::Kampai.Game.ReturnValueContainer.ValueType.Void:
				return DynValue.Void;
			default:
				logger.Error("LuaReturnValueContainer: Don't know how to convert {0} to DynValue.", global::System.Enum.GetName(typeof(global::Kampai.Game.ReturnValueContainer.ValueType), base.type));
				return DynValue.Void;
			}
		}

		public DynValue[] ToDynValueArray()
		{
			int count = arrayIndices.Count;
			DynValue[] result = new DynValue[count];
			for (int i = 0; i < count; i++)
			{
				DynValue dv = arrayIndices[i].ToDynValue();
				result[i] = dv.IsVoid() ? DynValue.Nil : dv;
			}
			return result;
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

		private DynValue ToDictionaryDynValue()
		{
			Table table = new Table(null);
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::Kampai.Game.LuaReturnValueContainer> keyValue in keyValues)
			{
				DynValue dv = keyValue.Value.ToDynValue();
				table.Set(keyValue.Key, dv.IsVoid() ? DynValue.Nil : dv);
			}
			return DynValue.NewTable(table);
		}

		private DynValue ToArrayDynValue()
		{
			int count = arrayIndices.Count;
			Table table = new Table(null);
			for (int i = 0; i < count; i++)
			{
				DynValue dv = arrayIndices[i].ToDynValue();
				table.Set(i + 1, dv.IsVoid() ? DynValue.Nil : dv);
			}
			return DynValue.NewTable(table);
		}
	}
}
