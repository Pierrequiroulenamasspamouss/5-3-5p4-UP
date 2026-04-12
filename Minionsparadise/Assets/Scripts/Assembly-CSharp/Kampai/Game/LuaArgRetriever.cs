using MoonSharp.Interpreter;

namespace Kampai.Game
{
	internal sealed class LuaArgRetriever : global::Kampai.Game.IArgRetriever
	{
		private CallbackArguments args;
		private string _methodName;

		public int Length { get; private set; }

		public void Setup(CallbackArguments arguments, string methodName)
		{
			args = arguments;
			Length = arguments.Count;
			_methodName = methodName;
		}

		// MoonSharp arguments are 0-indexed, but the old Lua C API arguments were 1-indexed (or 2-indexed dependig on closures).
		// Wait, in old native LuaScriptRunner, arguments were usually retrieved starting from 1 or 2 upvalue.
		// Actually, in LuaScriptRunner.cs `InvokeMethodFromLua(L)` it did `argRetriever.Setup(L)` and old `IArgRetriever.GetInt(index)` just called `L.lua_tointegerx(index)`.
		// Usually in Lua C API `lua_gettop` gives the count, and args start at index 1.
		// So `args.AsType(index - 1, DataType.Number)` is correct for 1-based indices.
		public int GetInt(int index)
		{
			if (index > Length || args[index - 1].IsNil()) return 0;
			return (int)args.AsType(index - 1, _methodName, DataType.Number).Number;
		}

		public float GetFloat(int index)
		{
			if (index > Length || args[index - 1].IsNil()) return 0f;
			return (float)args.AsType(index - 1, _methodName, DataType.Number).Number;
		}

		public string GetString(int index)
		{
			if (index > Length || args[index - 1].IsNil()) return string.Empty;
			return args.AsType(index - 1, _methodName, DataType.String).String;
		}

		public bool GetBoolean(int index)
		{
			if (index > Length || args[index - 1].IsNil()) return false;
			return args.AsType(index - 1, _methodName, DataType.Boolean).Boolean;
		}

		public object Get(int index, global::System.Type type)
		{
			if (type == typeof(int))
			{
				return GetInt(index);
			}
			if (type == typeof(float))
			{
				return GetFloat(index);
			}
			if (type == typeof(bool))
			{
				return GetBoolean(index);
			}
			if (type == typeof(string))
			{
				return GetString(index);
			}
			return null;
		}
	}
}
