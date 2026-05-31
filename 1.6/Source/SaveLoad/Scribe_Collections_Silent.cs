using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Verse;

namespace Defaults.SaveLoad
{
    public static class Scribe_Collections_Silent
    {
        private static readonly Dictionary<object, List<string>> persistedNulls = new Dictionary<object, List<string>>();

        public static void Look<T>(ref HashSet<T> valueHashSet, string label) where T : Def, new()
        {
            List<T> list = null;
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                list = valueHashSet?.ToList();
            }
            Look(ref list, label);
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                valueHashSet = list?.ToHashSet();
            }
        }

        private static void DefineWorkingLists<K, V>(string label, LookMode nonDefLookMode, out List<K> keysWorkingList, out List<V> valuesWorkingList)
        {
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                if (nonDefLookMode == LookMode.Reference)
                {
                    Verse.Log.Error("You need to provide working lists for the keys and values in order to be able to load such dictionary. label=" + label);
                }
            }
            keysWorkingList = null;
            valuesWorkingList = null;
        }

        public static void LookKeysDef<K, V>(ref Dictionary<K, V> dict, string label, LookMode valueLookMode = LookMode.Undefined) where K : Def, new()
        {
            DefineWorkingLists(label, valueLookMode, out List<K> keysWorkingList, out List<V> valuesWorkingList);
            LookKeysDef(ref dict, label, valueLookMode, ref keysWorkingList, ref valuesWorkingList);
        }

        public static void LookValuesDef<K, V>(ref Dictionary<K, V> dict, string label, LookMode keyLookMode = LookMode.Undefined) where V : Def, new()
        {
            DefineWorkingLists(label, keyLookMode, out List<K> keysWorkingList, out List<V> valuesWorkingList);
            LookValuesDef(ref dict, label, keyLookMode, ref keysWorkingList, ref valuesWorkingList);
        }

        public static void LookBothDef<K, V>(ref Dictionary<K, V> dict, string label) where K : Def, new() where V : Def, new()
        {
            List<K> keysWorkingList = null;
            List<V> valuesWorkingList = null;
            LookBothDef(ref dict, label, ref keysWorkingList, ref valuesWorkingList);
        }

        private static void InitDict<K, V>(ref Dictionary<K, V> dict)
        {
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                XmlAttribute xmlAttribute = Scribe.loader.curXmlParent.Attributes["IsNull"];
                dict = xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase)
                    ? null
                    : new Dictionary<K, V>();
            }
        }

        private static void InitWorkingLists<K, V>(Dictionary<K, V> dict, ref List<K> keysWorkingList, ref List<V> valuesWorkingList)
        {
            if (Scribe.mode == LoadSaveMode.Saving || Scribe.mode == LoadSaveMode.LoadingVars)
            {
                keysWorkingList = new List<K>();
                valuesWorkingList = new List<V>();
                if (Scribe.mode == LoadSaveMode.Saving && dict != null)
                {
                    keysWorkingList = dict.Keys.ToList();
                    valuesWorkingList = dict.Values.ToList();
                }
            }
        }

        private static void UninitWorkingLists<K, V>(ref List<K> keysWorkingList, ref List<V> valuesWorkingList)
        {
            keysWorkingList?.Clear();
            keysWorkingList = null;
            valuesWorkingList?.Clear();
            valuesWorkingList = null;
        }

        private static void BuildDictionary<K, V>(Dictionary<K, V> dict, string label, LookMode nonDefLookMode, List<K> keysWorkingList, List<V> valuesWorkingList)
        {
            bool reference = nonDefLookMode == LookMode.Reference;
            if ((reference && Scribe.mode == LoadSaveMode.ResolvingCrossRefs) || (!reference && Scribe.mode == LoadSaveMode.LoadingVars))
            {
                typeof(Scribe_Collections).Method("BuildDictionary", generics: new[] { typeof(K), typeof(V) }).Invoke(null, new object[] { dict, keysWorkingList, valuesWorkingList, label, true });
            }
        }

        private static void RemoveNullEntries<T1, T2>(List<T1> defWorkingList, List<T2> otherWorkingList, bool bothDef = false)
        {
            List<int> nullIndeces = new List<int>();
            for (int i = 0; i < defWorkingList.Count; i++)
            {
                if (defWorkingList[i] == null || (bothDef && otherWorkingList[i] == null))
                {
                    nullIndeces.Add(i);
                }
            }
            nullIndeces.Reverse();
            foreach (int i in nullIndeces)
            {
                defWorkingList.RemoveAt(i);
                otherWorkingList.RemoveAt(i);
            }
        }

        private static bool TryWriteNull<K, V>(Dictionary<K, V> dict)
        {
            if (Scribe.mode == LoadSaveMode.Saving && dict == null)
            {
                Scribe.saver.WriteAttribute("IsNull", "True");
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void CleanupError<K, V>(ref Dictionary<K, V> dict)
        {
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                dict = null;
            }
        }

        public static void LookKeysDef<K, V>(ref Dictionary<K, V> dict, string label, LookMode valueLookMode, ref List<K> keysWorkingList, ref List<V> valuesWorkingList) where K : Def, new()
        {
            if (Scribe.EnterNode(label))
            {
                try
                {
                    if (!TryWriteNull(dict))
                    {
                        InitDict(ref dict);
                        InitWorkingLists(dict, ref keysWorkingList, ref valuesWorkingList);
                        if (Scribe.mode == LoadSaveMode.Saving || dict != null)
                        {
                            Look(ref keysWorkingList, "keys", false);
                            Scribe_Collections.Look(ref valuesWorkingList, "values", valueLookMode);
                            if (dict != null)
                            {
                                RemoveNullEntries(keysWorkingList, valuesWorkingList);
                            }
                        }
                        if (Scribe.mode == LoadSaveMode.Saving)
                        {
                            UninitWorkingLists(ref keysWorkingList, ref valuesWorkingList);
                        }
                        BuildDictionary(dict, label, valueLookMode, keysWorkingList, valuesWorkingList);
                        if (Scribe.mode == LoadSaveMode.PostLoadInit)
                        {
                            UninitWorkingLists(ref keysWorkingList, ref valuesWorkingList);
                        }
                    }
                }
                catch
                {
                    CleanupError(ref dict);
                }
                finally
                {
                    Scribe.ExitNode();
                }
            }
        }

        public static void LookValuesDef<K, V>(ref Dictionary<K, V> dict, string label, LookMode keyLookMode, ref List<K> keysWorkingList, ref List<V> valuesWorkingList) where V : Def, new()
        {
            if (Scribe.EnterNode(label))
            {
                try
                {
                    if (!TryWriteNull(dict))
                    {
                        InitDict(ref dict);
                        InitWorkingLists(dict, ref keysWorkingList, ref valuesWorkingList);
                        if (Scribe.mode == LoadSaveMode.Saving || dict != null)
                        {
                            Scribe_Collections.Look(ref keysWorkingList, "keys", keyLookMode);
                            Look(ref valuesWorkingList, "values", false);
                            if (dict != null)
                            {
                                RemoveNullEntries(valuesWorkingList, keysWorkingList);
                            }
                        }
                        if (Scribe.mode == LoadSaveMode.Saving)
                        {
                            UninitWorkingLists(ref keysWorkingList, ref valuesWorkingList);
                        }
                        BuildDictionary(dict, label, keyLookMode, keysWorkingList, valuesWorkingList);
                        if (Scribe.mode == LoadSaveMode.PostLoadInit)
                        {
                            UninitWorkingLists(ref keysWorkingList, ref valuesWorkingList);
                        }
                    }
                }
                catch
                {
                    CleanupError(ref dict);
                }
                finally
                {
                    Scribe.ExitNode();
                }
            }
        }

        public static void LookBothDef<K, V>(ref Dictionary<K, V> dict, string label, ref List<K> keysWorkingList, ref List<V> valuesWorkingList) where K : Def, new() where V : Def, new()
        {
            if (Scribe.EnterNode(label))
            {
                try
                {
                    if (!TryWriteNull(dict))
                    {
                        InitDict(ref dict);
                        InitWorkingLists(dict, ref keysWorkingList, ref valuesWorkingList);
                        if (Scribe.mode == LoadSaveMode.Saving || dict != null)
                        {
                            Look(ref keysWorkingList, "keys", false);
                            Look(ref valuesWorkingList, "values", false);
                            if (dict != null)
                            {
                                RemoveNullEntries(keysWorkingList, valuesWorkingList, true);
                            }
                        }
                        if (Scribe.mode == LoadSaveMode.Saving)
                        {
                            UninitWorkingLists(ref keysWorkingList, ref valuesWorkingList);
                        }
                        BuildDictionary(dict, label, LookMode.Def, keysWorkingList, valuesWorkingList);
                        if (Scribe.mode == LoadSaveMode.PostLoadInit)
                        {
                            UninitWorkingLists(ref keysWorkingList, ref valuesWorkingList);
                        }
                    }
                }
                catch
                {
                    CleanupError(ref dict);
                }
                finally
                {
                    Scribe.ExitNode();
                }
            }
        }

        public static void Look<T>(ref List<T> list, string label, object nullsKey = null, bool removeNulls = true) where T : Def, new()
        {
            if (Scribe.EnterNode(label))
            {
                try
                {
                    if (Scribe.mode == LoadSaveMode.Saving)
                    {
                        if (list != null)
                        {
                            foreach (T item in list)
                            {
                                Def def = item;
                                Scribe_Defs_Silent.Look(ref def, "li");
                            }
                            if (nullsKey == null)
                            {
                                nullsKey = list;
                            }
                            if (persistedNulls.ContainsKey(nullsKey))
                            {
                                foreach (string name in persistedNulls[nullsKey])
                                {
                                    string defName = name;
                                    Scribe_Values.Look(ref defName, "li");
                                }
                            }
                        }
                        else
                        {
                            Scribe.saver.WriteAttribute("IsNull", "True");
                        }
                    }
                    else if (Scribe.mode == LoadSaveMode.LoadingVars)
                    {
                        XmlNode curXmlParent = Scribe.loader.curXmlParent;
                        XmlAttribute xmlAttribute = curXmlParent.Attributes["IsNull"];
                        if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                        {
                            list = null;
                        }
                        else
                        {
                            list = new List<T>(curXmlParent.ChildNodes.Count);
                            if (nullsKey == null)
                            {
                                nullsKey = list;
                            }
                            foreach (XmlNode childNode in curXmlParent.ChildNodes)
                            {
                                string defName = ScribeExtractor.ValueFromNode(childNode, "null");
                                T def = DefDatabase<T>.GetNamedSilentFail(defName);
                                if (def != null || !removeNulls)
                                {
                                    list.Add(def);
                                }
                                if (def == null)
                                {
                                    if (!persistedNulls.ContainsKey(nullsKey))
                                    {
                                        persistedNulls[nullsKey] = new List<string>();
                                    }
                                    persistedNulls[nullsKey].Add(defName);
                                }
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
    }
}
