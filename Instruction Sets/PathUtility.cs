using Godot;
using System;

namespace Rusty.CutsceneEditor.InstructionSets
{
    public static class PathUtility
    {
        public static string GetPath(string relativePath)
        {
            string baseFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string absolutePath = "";
            if (OS.HasFeature("editor"))
                absolutePath = baseFolderPath + "..\\..\\..\\..\\" + relativePath;
            else
                absolutePath = baseFolderPath + relativePath;

            return absolutePath;
        }
    }
}