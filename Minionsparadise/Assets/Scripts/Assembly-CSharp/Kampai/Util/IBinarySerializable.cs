namespace Kampai.Util
{
	public interface IBinarySerializable
	{
		int TypeCode { get; }

		void Write(global::System.IO.BinaryWriter writer);

		void Read(global::System.IO.BinaryReader reader);
	}
}
