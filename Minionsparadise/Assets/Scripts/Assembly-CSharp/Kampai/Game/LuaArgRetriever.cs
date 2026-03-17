namespace Kampai.Game
{
	internal sealed class LuaArgRetriever : global::Kampai.Game.IArgRetriever
	{
		private global::Kampai.Wrappers.LuaState L;

		public int Length { get; private set; }

		public void Setup(global::Kampai.Wrappers.LuaState l)
		{
			L = l;
			Length = l.lua_gettop();
		}

		public int GetInt(int index)
		{
			return L.lua_tointegerx(index, global::System.IntPtr.Zero);
		}

		public float GetFloat(int index)
		{
			return (float)L.lua_tonumberx(index, global::System.IntPtr.Zero);
		}

		public string GetString(int index)
		{
			return L.lua_tostring(index);
		}

		public bool GetBoolean(int index)
		{
			return L.lua_toboolean(index);
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
