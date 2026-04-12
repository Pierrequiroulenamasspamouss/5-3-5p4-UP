using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Kampai.Util;
using UnityEngine;

namespace Kampai.Game
{
    public abstract class AnimatingBuildingDefinition : RepairableBuildingDefinition
    {
        public int GagFrequency;

        public override int TypeCode
        {
            get { return 1030; }
        }

        public IList<BuildingAnimationDefinition> AnimationDefinitions { get; set; }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            BinarySerializationUtil.WriteList(writer, AnimationDefinitions);
            writer.Write(GagFrequency);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            AnimationDefinitions = BinarySerializationUtil.ReadList(reader, AnimationDefinitions);
            GagFrequency = reader.ReadInt32();
        }

        protected override bool DeserializeProperty(string propertyName, JsonReader reader, JsonConverters converters)
        {
            switch (propertyName)
            {
                // C'est ici que le décompilateur avait mis un "default" absurde !
                case "GAGFREQUENCY":
                    reader.Read();
                    int parsedFreq;
                    // Sécurité anti-crash si le JSON contient une mauvaise valeur
                    if (reader.Value != null && int.TryParse(reader.Value.ToString(), out parsedFreq))
                    {
                        GagFrequency = parsedFreq;
                    }
                    else
                    {
                        Debug.LogWarning("Valeur invalide ignorée pour GagFrequency : " + reader.Value);
                        GagFrequency = 0;
                    }
                    break;

                case "ANIMATIONDEFINITIONS":
                    reader.Read();
                    AnimationDefinitions = ReaderUtil.PopulateList(reader, converters, AnimationDefinitions);
                    break;

                // Le VRAI comportement par défaut : on passe à la classe parente
                default:
                    return base.DeserializeProperty(propertyName, reader, converters);
            }
            return true;
        }

        public virtual IList<string> AnimationControllerKeys()
        {
            IList<string> list = new List<string>();
            if (AnimationDefinitions != null && AnimationDefinitions.Count > 0)
            {
                foreach (BuildingAnimationDefinition animationDefinition in AnimationDefinitions)
                {
                    if (!string.IsNullOrEmpty(animationDefinition.BuildingController))
                    {
                        list.Add(animationDefinition.BuildingController);
                    }
                    if (!string.IsNullOrEmpty(animationDefinition.MinionController))
                    {
                        list.Add(animationDefinition.MinionController);
                    }
                }
            }
            return list;
        }
    }
}