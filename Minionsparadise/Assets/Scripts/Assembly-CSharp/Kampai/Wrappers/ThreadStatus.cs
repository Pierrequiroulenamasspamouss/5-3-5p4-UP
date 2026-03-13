namespace Kampai.Wrappers
{
	public enum ThreadStatus
	{
		LUA_OK = 0,
		LUA_YIELD = 1,
		LUA_ERRRUN = 2,
		LUA_ERRSYNTAX = 3,
		LUA_ERRMEM = 4,
		LUA_ERRGCMM = 5,
		LUA_ERRERR = 6
	}
}
