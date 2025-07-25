using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorNiveles
{
    public static class MenuItems
    {
        [MenuItem("Tools/Level Editor/New Level")]
        private static void NewLevel()
        {
            EditorUtils.NewLevel();
        }

        [MenuItem("Tools/Level Editor/Show Palette")]
        public static void ShowPalette()
        {
            PaletteWindow.ShowPalette();
        }
    }
}
