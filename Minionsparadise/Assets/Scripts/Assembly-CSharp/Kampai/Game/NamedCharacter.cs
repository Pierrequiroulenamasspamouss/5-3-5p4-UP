namespace Kampai.Game
{
	public interface NamedCharacter : global::Kampai.Game.Character, global::Kampai.Game.Instance, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable, global::Kampai.Util.Prestigable
	{
		new global::Kampai.Game.NamedCharacterDefinition Definition { get; }

		[global::Newtonsoft.Json.JsonIgnore]
		bool Created { get; set; }

		global::Kampai.Game.View.NamedCharacterObject Setup(global::UnityEngine.GameObject go);
	}
	public abstract class NamedCharacter<T> : global::Kampai.Game.Character<T>, global::Kampai.Game.Character, global::Kampai.Game.NamedCharacter, global::Kampai.Game.Instance, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable, global::Kampai.Util.Prestigable where T : global::Kampai.Game.NamedCharacterDefinition
	{
		global::Kampai.Game.NamedCharacterDefinition global::Kampai.Game.NamedCharacter.Definition
		{
			get
			{
				return base.Definition;
			}
		}

		[global::Newtonsoft.Json.JsonIgnore]
		public bool Created { get; set; }

		protected NamedCharacter(T def)
			: base(def)
		{
		}

		public abstract global::Kampai.Game.View.NamedCharacterObject Setup(global::UnityEngine.GameObject go);
	}
}
