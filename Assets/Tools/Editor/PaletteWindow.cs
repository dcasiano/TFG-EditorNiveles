using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PaletteWindow : EditorWindow
{
    public static PaletteWindow instance;
    private string path = "Assets/Prefabs/LevelObjects";

    // TODO: agrupar los items en categorias. En el editor se crearan tantas 
    // carpetas como categorias de items haya y se guardara cada item en su 
    // carpeta correspondiente. Maybe agruparlos aqui con un diccionario de listas??

    private List<GameObject> items;
    private Dictionary<string, Texture2D> previews;
    private Vector2 scrollPosition;
    private const float buttonWidth = 80;
    private const float buttonHeight = 90;

    public delegate void itemSelectedDelegate(GameObject item);
    public static event itemSelectedDelegate ItemSelectedEvent;

    public static void ShowPalette()
    {
        instance = (PaletteWindow)GetWindow
       (typeof(PaletteWindow));
        instance.titleContent = new GUIContent("Palette");
    }
    private void OnEnable()
    {
        items = EditorUtils.GetAssets(path);
        previews = new Dictionary<string, Texture2D>();
    }
    private void Update()
    {
        if (previews.Count != items.Count)
        {
            GeneratePreviews();
        }
    }
    private void OnGUI()
    {
        DrawScroll();
    }
    private GUIContent[] GetGUIContentsFromItems()
    {
        List<GUIContent> guiContents = new List<GUIContent>();
        if (previews.Count == items.Count)
        {
            int totalItems = items.Count;
            for (int i = 0; i < totalItems; i++)
            {
                GUIContent guiContent = new GUIContent();
                guiContent.text = items[i].name;
                guiContent.image = previews[items[i].name];
                guiContents.Add(guiContent);
            }
        }
        return guiContents.ToArray();
    }
    private GUIStyle GetGUIStyle()
    {
        GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
        guiStyle.alignment = TextAnchor.LowerCenter;
        guiStyle.imagePosition = ImagePosition.ImageAbove;
        guiStyle.fixedWidth = buttonWidth;
        guiStyle.fixedHeight = buttonHeight;
        return guiStyle;
    }
    private void GetSelectedItem(int index)
    {
        if (index != -1)
        {
            GameObject selectedItem = items[index];
            Debug.Log("Selected Item is: " + selectedItem.name);
            if (ItemSelectedEvent != null) ItemSelectedEvent(selectedItem);
        }
    }
    private void DrawScroll()
    {
        int rowCapacity = Mathf.FloorToInt(position.width / (buttonWidth));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        int selectionGridIndex = -1;
        selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex,
        GetGUIContentsFromItems(), rowCapacity, GetGUIStyle());
        GetSelectedItem(selectionGridIndex);
        GUILayout.EndScrollView();
    }
    private void GeneratePreviews()
    {
        foreach (GameObject item in items)
        {
            if (!previews.ContainsKey(item.name))
            {
                Texture2D preview = AssetPreview.GetAssetPreview(item);
                if (preview != null)
                {
                    previews.Add(item.name, preview);
                }
            }
        }
    }
}


