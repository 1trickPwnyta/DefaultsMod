using HarmonyLib;
using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.Policies
{
    public struct PolicyParms
    {
        public string NameKey;
        public IList VanillaPolicies;
        public IList DefaultPolicies;
    }

    public static class PolicyUtility
    {
        private static readonly Dictionary<Type, PolicyParms> policyParms = new Dictionary<Type, PolicyParms>()
        {
            {
                typeof(ApparelPolicy), new PolicyParms()
                {
                    NameKey = "ApparelPolicy",
                    VanillaPolicies = VanillaPolicyStore.VanillaApparelPolicies,
                    DefaultPolicies = DefaultsSettings.DefaultApparelPolicies
                }
            },
            {
                typeof(FoodPolicy), new PolicyParms()
                {
                    NameKey = "FoodPolicy",
                    VanillaPolicies = VanillaPolicyStore.VanillaFoodPolicies,
                    DefaultPolicies = DefaultsSettings.DefaultFoodPolicies
                }
            },
            {
                typeof(DrugPolicy), new PolicyParms()
                {
                    NameKey = "DrugPolicy",
                    VanillaPolicies = VanillaPolicyStore.VanillaDrugPolicies,
                    DefaultPolicies = DefaultsSettings.DefaultDrugPolicies
                }
            },
            {
                typeof(ReadingPolicy), new PolicyParms()
                {
                    NameKey = "ReadingPolicy",
                    VanillaPolicies = VanillaPolicyStore.VanillaReadingPolicies,
                    DefaultPolicies = DefaultsSettings.DefaultReadingPolicies
                }
            }
        };

        public static bool IsLocked(this Policy policy) => !DefaultsSettings.UnlockedPolicies.Contains(policy);

        public static void SetLocked(this Policy policy, bool value)
        {
            if (policy.IsLockable())
            {
                if (value)
                {
                    DefaultsSettings.UnlockedPolicies.Remove(policy);
                }
                else
                {
                    DefaultsSettings.UnlockedPolicies.Add(policy);
                }
            }
        }

        public static bool IsLockable(this Policy policy) => policy is ApparelPolicy || policy is FoodPolicy || policy is ReadingPolicy;

        public static T NewPolicy<T>(Dialog_ManagePolicies<T> dialog, string name = null) where T : Policy => (T)NewPolicy(typeof(T), dialog, name);

        public static Policy NewPolicy(Type type, Window dialog, string name = null)
        {
            IList defaultPolicies = (IList)dialog.GetType().Method("GetPolicies").Invoke(dialog, new object[] { });
            name = GetAvailableNameForPolicy(name, policyParms[type].NameKey, defaultPolicies);
            Policy policy = (Policy)dialog.GetType().Method("CreateNewPolicy").Invoke(dialog, new object[] { });
            policy.label = name;
            return policy;
        }

        public static T NewDefaultPolicy<T>(string name = null) where T : Policy
        {
            List<T> defaultPolicies = GetDefaultPolicies<T>();
            name = GetAvailableNameForPolicy(name, policyParms[typeof(T)].NameKey, defaultPolicies);
            T policy = (T)Activator.CreateInstance(typeof(T), new object[] { 0, name });
            defaultPolicies.Add(policy);
            return policy;
        }

        public static Policy NewDefaultPolicy(Type type, string name = null)
        {
            IList defaultPolicies = policyParms[type].DefaultPolicies;
            name = GetAvailableNameForPolicy(name, policyParms[type].NameKey, defaultPolicies);
            Policy policy = (Policy)Activator.CreateInstance(type, new object[] { 0, name });
            defaultPolicies.Add(policy);
            return policy;
        }

        public static List<T> GetVanillaPolicies<T>() => GetVanillaPolicies(typeof(T)) as List<T>;

        public static IList GetVanillaPolicies(Type type) => policyParms[type].VanillaPolicies;

        public static List<T> GetDefaultPolicies<T>() => GetDefaultPolicies(typeof(T)) as List<T>;

        public static IList GetDefaultPolicies(Type type) => policyParms[type].DefaultPolicies;

        private static string GetAvailableNameForPolicy(string preferredName, string nameKey, IList existingPolicies)
        {
            string name = preferredName;
            int i = existingPolicies.Count + 1;
            while (name == null || existingPolicies.Cast<Policy>().Any(p => p.label == name))
            {
                name = nameKey.Translate() + " " + i++;
            }
            return name;
        }

        public static bool IsGamePolicyDialog(this Window window) =>
            window.GetType() == typeof(Dialog_ManageApparelPolicies)
            || window.GetType() == typeof(Dialog_ManageFoodPolicies)
            || window.GetType() == typeof(Dialog_ManageDrugPolicies)
            || window.GetType() == typeof(Dialog_ManageReadingPolicies);

        public static float GetNewPolicyButtonPaddingTop(Window window)
        {
            Type type = window.GetType();
            return new[]
            {
                typeof(Dialog_ManageApparelPolicies),
                typeof(Dialog_ManageFoodPolicies),
                typeof(Dialog_ManageDrugPolicies),
                typeof(Dialog_ManageReadingPolicies)
            }.Contains(type) ? 10f + Window.CloseButSize.y : 10f;
        }
    }
}
