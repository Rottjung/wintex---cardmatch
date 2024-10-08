using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;

namespace RotaryHeart.Lib.SerializableDictionary
{
    [InitializeOnLoad]
    public class Definer
    {
        static Definer()
        {
            List<string> defines = new List<string>(1)
            {
                "RHSD"
            };
            ApplyDefines(defines);
        }

        public static void ApplyDefines(List<string> defines)
        {
            if (defines == null || defines.Count == 0)
                return;

            var target = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(target);
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(group);
            string availableDefines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
            List<string> definesSplit = new List<string>(availableDefines.Split(';'));

            foreach (var define in defines)
                if (!definesSplit.Contains(define))
                    definesSplit.Add(define);

            _ApplyDefine(string.Join(";", definesSplit.ToArray()));
        }

        public static void RemoveDefines(List<string> defines)
        {
            if (defines == null || defines.Count == 0)
                return;

            var target = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(target);
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(group);
            string availableDefines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
            List<string> definesSplit = new List<string>(availableDefines.Split(';'));

            foreach (var define in defines)
                definesSplit.Remove(define);

            _ApplyDefine(string.Join(";", definesSplit.ToArray()));
        }

        private static void _ApplyDefine(string define)
        {
            if (string.IsNullOrEmpty(define))
                return;

            var target = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(target);
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(group);
            PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
        }
    }
}