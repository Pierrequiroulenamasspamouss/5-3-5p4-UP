using System;
using Newtonsoft.Json;
using UnityEngine;
using Kampai.Game.View;

namespace Kampai.Game
{
    public class BridgeBuilding : Building<BridgeBuildingDefinition>
    {
        public int BridgeId { get; set; }

        public int UnlockLevel { get; set; }

        public BridgeBuilding(BridgeBuildingDefinition def)
            : base(def)
        {
        }

        protected override bool DeserializeProperty(string propertyName, JsonReader reader, JsonConverters converters)
        {
            switch (propertyName)
            {
                case "BRIDGEID":
                    reader.Read();
                    int parsedBridgeId;
                    if (reader.Value != null && int.TryParse(reader.Value.ToString(), out parsedBridgeId))
                    {
                        BridgeId = parsedBridgeId;
                    }
                    else
                    {
                        Debug.LogWarning("Valeur invalide ignorée pour BridgeId : " + reader.Value);
                        BridgeId = 0;
                    }
                    break;

                case "UNLOCKLEVEL":
                    reader.Read();
                    int parsedUnlockLevel;
                    if (reader.Value != null && int.TryParse(reader.Value.ToString(), out parsedUnlockLevel))
                    {
                        UnlockLevel = parsedUnlockLevel;
                    }
                    else
                    {
                        Debug.LogWarning("Valeur invalide ignorée pour UnlockLevel : " + reader.Value);
                        UnlockLevel = 0;
                    }
                    break;

                default: 
                    return base.DeserializeProperty(propertyName, reader, converters);
            }
            
            // LA CORRECTION EST ICI : on renvoie 'true' si le case a été traité avec succès
            return true;
        }

        public override void Serialize(JsonWriter writer)
        {
            writer.WriteStartObject();
            SerializeProperties(writer);
            writer.WriteEndObject();
        }

        protected override void SerializeProperties(JsonWriter writer)
        {
            base.SerializeProperties(writer);
            writer.WritePropertyName("BridgeId");
            writer.WriteValue(BridgeId);
            writer.WritePropertyName("UnlockLevel");
            writer.WriteValue(UnlockLevel);
        }

        public override BuildingObject AddBuildingObject(GameObject gameObject)
        {
            return gameObject.AddComponent<BridgeBuildingObject>();
        }
    }
}