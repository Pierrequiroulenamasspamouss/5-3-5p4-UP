namespace Kampai.Game
{
	public interface FrolicCharacter : global::Kampai.Game.Character, global::Kampai.Game.NamedCharacter, global::Kampai.Game.Instance, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable, global::Kampai.Util.Prestigable
	{
		[global::Newtonsoft.Json.JsonIgnore]
		global::Kampai.Game.FloatLocation CurrentFrolicLocation { get; set; }

		new global::Kampai.Game.FrolicCharacterDefinition Definition { get; }
	}
	public abstract class FrolicCharacter<T> : global::Kampai.Game.NamedCharacter<T>, global::Kampai.Game.Character, global::Kampai.Game.FrolicCharacter, global::Kampai.Game.NamedCharacter, global::Kampai.Game.Instance, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable, global::Kampai.Util.Prestigable where T : global::Kampai.Game.FrolicCharacterDefinition
	{
		global::Kampai.Game.FrolicCharacterDefinition global::Kampai.Game.FrolicCharacter.Definition
		{
			get
			{
				return base.Definition;
			}
		}

		[global::Newtonsoft.Json.JsonIgnore]
		public global::Kampai.Game.FloatLocation CurrentFrolicLocation { get; set; }

		protected FrolicCharacter(T def)
			: base(def)
		{
		}
	}
}
