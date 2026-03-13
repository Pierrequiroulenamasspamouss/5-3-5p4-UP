namespace Kampai.Util
{
	internal sealed class FastTextStreamWriter : global::System.IO.StreamWriter
	{
		private global::Kampai.Util.FastDoubleToString doubleConverter = new global::Kampai.Util.FastDoubleToString();

		private global::Kampai.Util.FastIntToString intConverter = new global::Kampai.Util.FastIntToString();

		public FastTextStreamWriter(global::System.IO.Stream stream)
			: base(stream)
		{
		}

		public FastTextStreamWriter(global::System.IO.Stream stream, global::System.Text.Encoding encoding)
			: base(stream, encoding)
		{
		}

		public FastTextStreamWriter(global::System.IO.Stream stream, global::System.Text.Encoding encoding, int bufferSize)
			: base(stream, encoding, bufferSize)
		{
		}

		public override void Write(double value)
		{
			char[] buffer;
			int len;
			doubleConverter.ToCharArray(value, out buffer, out len);
			Write(buffer, 0, len);
		}

		public override void Write(int value)
		{
			char[] buffer;
			int len;
			intConverter.ToCharArray(value, out buffer, out len);
			Write(buffer, 0, len);
		}

		public override void Write(long value)
		{
			char[] buffer;
			int len;
			intConverter.ToCharArray(value, out buffer, out len);
			Write(buffer, 0, len);
		}

		public override void Write(float value)
		{
			char[] buffer;
			int len;
			doubleConverter.ToCharArray(value, out buffer, out len);
			Write(buffer, 0, len);
		}

		public override void Write(uint value)
		{
			char[] buffer;
			int len;
			intConverter.ToCharArray(value, out buffer, out len);
			Write(buffer, 0, len);
		}

		public override void Write(ulong value)
		{
			char[] buffer;
			int len;
			intConverter.ToCharArray(value, out buffer, out len);
			Write(buffer, 0, len);
		}
	}
}
