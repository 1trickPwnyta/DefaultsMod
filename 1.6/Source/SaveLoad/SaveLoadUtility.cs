using System;
using System.Xml;
using Verse;

namespace Defaults.SaveLoad
{
    public static class SaveLoadUtility
    {
        public static void Scribe_Custom<T>(ref T value, string label, Action<T> scribeFields) where T : class, IExposable
        {
            if (Scribe.EnterNode(label))
            {
                try
                {
                    if (Scribe.mode == LoadSaveMode.Saving)
                    {
                        if (value != null)
                        {
                            scribeFields(value);
                        }
                        else
                        {
                            Scribe.saver.WriteAttribute("IsNull", "True");
                        }
                    }
                    else if (Scribe.mode == LoadSaveMode.LoadingVars)
                    {
                        XmlAttribute xmlAttribute = Scribe.loader.curXmlParent.Attributes["IsNull"];
                        if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                        {
                            value = null;
                        }
                        else
                        {
                            value = Activator.CreateInstance<T>();
                            scribeFields(value);
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
