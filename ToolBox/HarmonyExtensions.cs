using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;

namespace ToolBox.Extensions {
    public static class HarmonyExtensions
    {
        public static HarmonyInstance CreateInstance(string guid)
        {
            return HarmonyInstance.Create(guid);
        }

        public static void PatchAllSafe(this HarmonyInstance self)
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                try
                {
                    List<HarmonyMethod> harmonyMethods = type.GetHarmonyMethods();
                    if (harmonyMethods == null || harmonyMethods.Count <= 0)
                        continue;
                    HarmonyMethod attributes = HarmonyMethod.Merge(harmonyMethods);
                    new PatchProcessor(self, type, attributes).Patch();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(self.Id + ": Exception occured when patching: " + e);
                }
            }
        }

        public class Replacement
        {
            public CodeInstruction[] pattern = null;
            public CodeInstruction[] replacer = null;
        }

        public static IEnumerable<CodeInstruction> ReplaceCodePattern(IEnumerable<CodeInstruction> instructions, IList<Replacement> replacements)
        {
            List<CodeInstruction> codeInstructions = instructions.ToList();
            foreach (Replacement replacement in replacements)
            {
                for (int i = 0; i < codeInstructions.Count; i++)
                {
                    int j = 0;
                    while (j < replacement.pattern.Length && i + j < codeInstructions.Count &&
                           CompareCodeInstructions(codeInstructions[i + j], replacement.pattern[j]))
                        ++j;
                    if (j == replacement.pattern.Length)
                    {
                        for (int k = 0; k < replacement.replacer.Length; k++)
                        {
                            int finalIndex = i + k;
                            codeInstructions[finalIndex] = new CodeInstruction(replacement.replacer[k]) { labels = new List<Label>(codeInstructions[finalIndex].labels) };
                        }
                        i += replacement.replacer.Length;
                    }
                }
            }
            return codeInstructions;
        }

        private static bool CompareCodeInstructions(CodeInstruction first, CodeInstruction second)
        {
            return first.opcode == second.opcode && first.operand == second.operand;
        }
    }
}