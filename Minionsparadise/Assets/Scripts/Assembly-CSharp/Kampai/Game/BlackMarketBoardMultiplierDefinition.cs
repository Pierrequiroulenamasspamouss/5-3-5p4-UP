using System;
using System.IO;
using Newtonsoft.Json;

namespace Kampai.Game
{
    public class BlackMarketBoardMultiplierDefinition : Definition
    {
        public override int TypeCode
        {
            get
            {
                return 1038;
            }
        }

        public int Level { get; set; }

        public float Multiplier { get; set; }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(Level);
            writer.Write(Multiplier);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            Level = reader.ReadInt32();
            Multiplier = reader.ReadSingle();
        }

        protected override bool DeserializeProperty(string propertyName, JsonReader reader, JsonConverters converters)
        {
            switch (propertyName)
            {
                case "MULTIPLIER":
                    reader.Read();
                    float parsedMultiplier;
                    if (reader.Value != null && float.TryParse(reader.Value.ToString(), out parsedMultiplier))
                    {
                        Multiplier = parsedMultiplier;
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning("Valeur invalide ignorée pour Multiplier : " + reader.Value);
                        Multiplier = 0f;
                    }
                    break;

                case "LEVEL":
                    reader.Read();
                    int parsedLevel;
                    if (reader.Value != null && int.TryParse(reader.Value.ToString(), out parsedLevel))
                    {
                        Level = parsedLevel;
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning("Valeur invalide ignorée pour Level : " + reader.Value);
                        Level = 0;
                    }
                    break;

                default:
                    return base.DeserializeProperty(propertyName, reader, converters);
            }
            return true;
        }
    }
}