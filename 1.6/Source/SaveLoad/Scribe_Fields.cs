using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Verse;

namespace Defaults.SaveLoad
{
    public static class Scribe_Fields
    {
        private const string PREFERRED_DEF_ATTRIBUTE_NAME = "defaults.preferredDef";
        private static readonly Dictionary<(object, string), (string, Def)> preferredDefs = new Dictionary<(object, string), (string, Def)>();

        public static void Look_Value<T>(object obj, string fieldName, string label = null, T defaultValue = default)
        {
            FieldInfo field = obj.GetType().Field(fieldName);
            T value = (T)field.GetValue(obj);
            Scribe_Values.Look(ref value, label ?? fieldName, defaultValue);
            field.SetValue(obj, value);
        }

        public static void Look_Def<T>(object obj, string fieldName, string label = null) where T : Def, new()
        {
            if (label == null)
            {
                label = fieldName;
            }
            FieldInfo field = obj.GetType().Field(fieldName);
            T value = field.GetValue(obj) as T;
            Scribe_Defs_Silent.Look(ref value, label);
            field.SetValue(obj, value);

            if (Scribe.EnterNode(label))
            {
                try
                {
                    if (Scribe.mode == LoadSaveMode.Saving && preferredDefs.ContainsKey((obj, fieldName)))
                    {
                        if (preferredDefs[(obj, fieldName)].Item2 == value)
                        {
                            Scribe.saver.WriteAttribute(PREFERRED_DEF_ATTRIBUTE_NAME, preferredDefs[(obj, fieldName)].Item1);
                        }
                    }
                    if (Scribe.mode == LoadSaveMode.LoadingVars)
                    {
                        XmlAttribute preferredDefAttribute = Scribe.loader.curXmlParent.Attributes[PREFERRED_DEF_ATTRIBUTE_NAME];
                        if (preferredDefAttribute != null)
                        {
                            T def = DefDatabase<T>.GetNamedSilentFail(preferredDefAttribute.Value);
                            if (def != null)
                            {
                                field.SetValue(obj, def);
                            }
                        }
                    }
                }
                finally
                {
                    Scribe.ExitNode();
                }
            }
        }

        public static void Look_HashSet<T>(object obj, string fieldName, string label = null) where T : Def, new()
        {
            FieldInfo field = obj.GetType().Field(fieldName);
            HashSet<T> value = field.GetValue(obj) as HashSet<T>;
            Scribe_Collections_Silent.Look(ref value, label ?? fieldName, (obj, fieldName));
            field.SetValue(obj, value);
        }

        public static void Look_List<T>(object obj, string fieldName, string label = null) where T : Def, new()
        {
            FieldInfo field = obj.GetType().Field(fieldName);
            List<T> value = field.GetValue(obj) as List<T>;
            Scribe_Collections_Silent.Look(ref value, label ?? fieldName, (obj, fieldName));
            field.SetValue(obj, value);
        }
    }
}
