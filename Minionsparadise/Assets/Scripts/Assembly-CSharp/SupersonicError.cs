public class SupersonicError
{
	private string description;

	private int code;

	public SupersonicError(int errCode, string errDescription)
	{
		code = errCode;
		description = errDescription;
	}

	public int getErrorCode()
	{
		return code;
	}

	public string getDescription()
	{
		return description;
	}

	public int getCode()
	{
		return code;
	}

	public override string ToString()
	{
		return code + " : " + description;
	}
}
