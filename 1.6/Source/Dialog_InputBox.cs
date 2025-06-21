using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace Defaults
{
    public abstract class Dialog_InputBox : Dialog_MessageBox
    {
        public static float HandleViewRect(Dialog_MessageBox instance) => instance is Dialog_InputBox dialog ? dialog.height + 10f : 0f;

        public static void HandleInputRect(Dialog_MessageBox instance, Rect viewRect)
        {
            if (instance is Dialog_InputBox dialog)
            {
                viewRect.yMin = viewRect.yMax - dialog.height;
                dialog.height = dialog.DoInput(viewRect);
            }
        }

        private float height;

        protected Dialog_InputBox(TaggedString text, string buttonAText = null, Action buttonAAction = null, string buttonBText = null, Action buttonBAction = null, string title = null, bool buttonADestructive = false, Action acceptAction = null, Action cancelAction = null, WindowLayer layer = WindowLayer.Dialog) : base(text, buttonAText, buttonAAction, buttonBText, buttonBAction, title, buttonADestructive, acceptAction, cancelAction, layer)
        {
        }

        public abstract float DoInput(Rect rect);
    }

    [HarmonyPatch(typeof(Dialog_MessageBox))]
    [HarmonyPatch(nameof(Dialog_MessageBox.DoWindowContents))]
    public static class Patch_Dialog_MessageBox_DoWindowContents
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionsList = instructions.ToList();
            int calcHeightIndex = instructionsList.FindIndex(i => i.opcode == OpCodes.Call && i.operand is MethodInfo info && info == typeof(Text).Method(nameof(Text.CalcHeight)));
            instructionsList.InsertRange(calcHeightIndex + 1, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, typeof(Dialog_InputBox).Method(nameof(Dialog_InputBox.HandleViewRect))),
                new CodeInstruction(OpCodes.Add)
            });
            int labelIndex = instructionsList.FindLastIndex(i => i.opcode == OpCodes.Call && i.operand is MethodInfo info && info == typeof(Widgets).Method(nameof(Widgets.Label), new[] { typeof(Rect), typeof(TaggedString) }));
            instructionsList.InsertRange(labelIndex + 1, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldloc_3),
                new CodeInstruction(OpCodes.Call, typeof(Dialog_InputBox).Method(nameof(Dialog_InputBox.HandleInputRect)))
            });
            return instructionsList;
        }
    }
}
