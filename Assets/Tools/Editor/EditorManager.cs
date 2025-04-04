using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class EditorManager
{
    /// <summary>
    /// Reference to the scriptable object used to store data of
    /// the items placed using the editor tool.
    /// </summary>
    static LevelObjectScriptable scriptableObject;
    /// <summary>
    /// Array which contains the instantiated items on the grid using the editor.
    /// </summary>
    static GameObject[] placedItems;
    static readonly string scriptableObjectPath = "Assets/ScriptableObjects/LevelObjects.asset";
    static EditorManager()
    {
        LoadScriptableObject();

        // Subscribe to the event called when we enter or exit Edit Mode in Unity
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    // Callback method to handle play mode state changes
    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // Check if the state is EnteredPlayMode
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            Debug.Log("Saliendo Edit Mode");
            SaveObjectsToDisk();
        }

        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            Debug.Log("Entrado Edit Mode");
            LoadObjectsFromDisk();
        }
    }
    private static void LoadScriptableObject()
    {
        scriptableObject = AssetDatabase.LoadAssetAtPath<LevelObjectScriptable>(scriptableObjectPath);
        if (scriptableObject == null) CreateScriptableObject();
    }
    private static void CreateScriptableObject()
    {
        scriptableObject = ScriptableObject.CreateInstance<LevelObjectScriptable>();
        AssetDatabase.CreateAsset(scriptableObject, scriptableObjectPath);
        AssetDatabase.SaveAssets();
    }
    private static void SaveObjectsToDisk()
    {
        if (scriptableObject == null)
        {
            Debug.Log("No se ha podido guardar");
            return;
        }
        if (placedItems == null)
        {
            Debug.Log("No se puede guardar un array de objetos vacio");
            return;
        }
        scriptableObject.objects = (GameObject[])placedItems.Clone();
        EditorUtility.SetDirty(scriptableObject);
        AssetDatabase.SaveAssets();
        Debug.Log("items guardados");
    }
    private static void LoadObjectsFromDisk()
    {
        if (scriptableObject == null) return;
        placedItems = (GameObject[])scriptableObject.objects.Clone();
        Debug.Log("items cargados");
    }
    /// <summary>
    /// This method must be called when we place a new item on the grid using this editor tool.
    /// </summary>
    /// <param name="newItem"> The item placed. </param>
    /// <param name="index"> Its index on the grid. </param>
    /// <param name="gridSize"> Size of the grid. </param>
    public static void PlaceNewItem(GameObject newItem, int index, int gridSize)
    {
        if (placedItems == null) placedItems = new GameObject[gridSize];
        DeleteItem(index);
        placedItems[index] = newItem;
    }
    /// <summary>
    /// This method must be called when we erase an item from the grid using this editor tool.
    /// </summary>
    /// <param name="index"> The index on the grid of the item we want to erase. </param>
    public static void DeleteItem(int index)
    {
        if (placedItems == null)
        {
            Debug.Log("No se pudo borrar el objeto");
            return;
        }
        if (placedItems[index] != null) 
        {
            Object.DestroyImmediate(placedItems[index]);
            placedItems[index] = null;
        }
    }
    private static void ClearObjects()
    {
        placedItems = null;
        AssetDatabase.DeleteAsset(scriptableObjectPath);
    }
    /// <summary>
    /// This method must be called when we create a new level, so
    /// the references to previous objects are removed.
    /// </summary>
    public static void OnNewLevel()
    {
        ClearObjects();
        CreateScriptableObject();
    }
}
