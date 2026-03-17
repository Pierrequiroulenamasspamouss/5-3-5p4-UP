using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Kampai.Util;

namespace Kampai.Game
{
    public class StorageBuildingDefinition : AnimatingBuildingDefinition
    {
        public override int TypeCode
        {
            get { return 1063; }
        }

        public int Capacity { get; set; }

        public IList<StorageUpgradeDefinition> StorageUpgrades { get; set; }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(Capacity);
            BinarySerializationUtil.WriteList(writer, BinarySerializationUtil.WriteStorageUpgradeDefinition, StorageUpgrades);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            Capacity = reader.ReadInt32();
            StorageUpgrades = BinarySerializationUtil.ReadList(reader, BinarySerializationUtil.ReadStorageUpgradeDefinition, StorageUpgrades);
        }

        protected override bool DeserializeProperty(string propertyName, JsonReader reader, JsonConverters converters)
        {
            switch (propertyName)
            {
                case "STORAGEUPGRADES":
                    reader.Read();
                    StorageUpgrades = ReaderUtil.PopulateList(reader, converters, ReaderUtil.ReadStorageUpgradeDefinition, StorageUpgrades);
                    break;

                case "CAPACITY":
                    reader.Read();
                    int parsedCapacity;
                    if (reader.Value != null && int.TryParse(reader.Value.ToString(), out parsedCapacity))
                    {
                        Capacity = parsedCapacity;
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning("Valeur invalide ignorée pour Capacity : " + reader.Value);
                        Capacity = 0;
                    }
                    break;

                default:
                    return base.DeserializeProperty(propertyName, reader, converters);
            }
            return true;
        }

        public override Building BuildBuilding()
        {
            return new StorageBuilding(this);
        }
    }
}