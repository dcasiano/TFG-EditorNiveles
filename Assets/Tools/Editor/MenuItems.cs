using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MenuItems
{
    [MenuItem("Tools/Level Creator/New Level")]
    private static void NewLevel()
    {
        EditorUtils.NewLevel();
    }

    [MenuItem("Tools/Level Creator/Show Palette")]
    public static void ShowPalette()
    {
        PaletteWindow.ShowPalette();
    }
}
