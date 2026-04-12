namespace Kampai.Util
{
	internal sealed class FastJSONWriter : global::Newtonsoft.Json.JsonTextWriter
	{
		private global::System.IO.TextWriter writer;

		public FastJSONWriter(global::System.IO.TextWriter writer)
			: base(writer)
		{
			this.writer = writer;
		}

		public override void WriteValue(int value)
		{
			AutoComplete(global::Newtonsoft.Json.JsonToken.Integer);
			writer.Write(value);
		}

		public override void WriteValue(uint value)
		{
			AutoComplete(global::Newtonsoft.Json.JsonToken.Integer);
			writer.Write(value);
		}

		public override void WriteValue(long value)
		{
			AutoComplete(global::Newtonsoft.Json.JsonToken.Integer);
			writer.Write(value);
		}

		public override void WriteValue(ulong value)
		{
			AutoComplete(global::Newtonsoft.Json.JsonToken.Integer);
			writer.Write(value);
		}

		public override void WriteValue(float value)
		{
			AutoComplete(global::Newtonsoft.Json.JsonToken.Float);
			writer.Write(value);
		}

		public override void WriteValue(double value)
		{
			AutoComplete(global::Newtonsoft.Json.JsonToken.Float);
			writer.Write(value);
		}

		public override void WriteValue(short value)
		{
			AutoComplete(global::Newtonsoft.Json.JsonToken.Integer);
			writer.Write(value);
		}

		public override void WriteValue(ushort value)
		{
			AutoComplete(global::Newtonsoft.Json.JsonToken.Integer);
			writer.Write(value);
		}

		public override void WriteValue(byte value)
		{
			AutoComplete(global::Newtonsoft.Json.JsonToken.Integer);
			writer.Write(value);
		}

		public override void WriteValue(sbyte value)
		{
			AutoComplete(global::Newtonsoft.Json.JsonToken.Integer);
			writer.Write(value);
		}
	}
}
