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
    static GameObject[][] placedItems;
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
        //scriptableObject.objects = (GameObject[][])placedItems.Clone();
        scriptableObject.objects = new List<GameObjectsGroup>();
        int numLayers = placedItems.Length;
        for (int i = 0; i < numLayers; i++)
        {
            GameObjectsGroup group = new GameObjectsGroup();
            group.gameObjects = new List<GameObject>(placedItems[i]);
            scriptableObject.objects.Add(group);
        }
        EditorUtility.SetDirty(scriptableObject);
        AssetDatabase.SaveAssets();
        Debug.Log("items guardados");
    }
    private static void LoadObjectsFromDisk()
    {
        if (scriptableObject == null || scriptableObject.objects == null) return;
        //placedItems = (GameObject[][])scriptableObject.objects.Clone();
        int numLayers = scriptableObject.objects.Count;
        placedItems = new GameObject[numLayers][];
        for(int i = 0; i < numLayers; i++)
        {
            List<GameObject> objectsInLayer = scriptableObject.objects[i].gameObjects;
            placedItems[i] = objectsInLayer.ToArray();
        }
        Debug.Log("items cargados");
    }
    /// <summary>
    /// This method must be called when we place a new item on the grid using this editor tool.
    /// </summary>
    /// <param name="newItem"> The item placed. </param>
    /// <param name="index"> Its index on the grid. </param>
    /// <param name="gridSize"> Size of the grid. </param>
    /// <param name="selectedLayer"> The layer where the new item is supposed to be placed. </param>
    /// <param name="numLayers"> The total number of layers. </param>
    public static void PlaceNewItem(GameObject newItem, int index, int gridSize, int selectedLayer, int numLayers)
    {
        if (placedItems == null)
        {
            placedItems = new GameObject[numLayers][];
            for (int i = 0; i < numLayers; i++) placedItems[i] = new GameObject[gridSize];
        }
        DeleteItem(index, selectedLayer);
        placedItems[selectedLayer][index] = newItem;
    }
    /// <summary>
    /// This method must be called when we erase an item from the grid using this editor tool.
    /// </summary>
    /// <param name="index"> The index on the grid of the item we want to erase. </param>
    /// <param name="selectedLayer"> The layer where the item we want to erase is. </param>
    public static void DeleteItem(int index, int selectedLayer)
    {
        if (placedItems == null)
        {
            Debug.Log("No se pudo borrar el objeto");
            return;
        }
        if (placedItems[selectedLayer][index] != null) 
        {
            Object.DestroyImmediate(placedItems[selectedLayer][index]);
            placedItems[selectedLayer][index] = null;
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
    /// <summary>
    /// This method must be called when we create a new layer using the editor.
    /// </summary>
    public static void OnNewLayerCreated()
    {
        int newNumLayers = placedItems.Length + 1;
        int gridSize = placedItems[0].Length;

        GameObject[][] newPlacedItems = new GameObject[newNumLayers][];
        for (int i = 0; i < newNumLayers - 1; i++)
        {
            newPlacedItems[i] = new GameObject[gridSize];
            System.Array.Copy(placedItems[i], newPlacedItems[i], gridSize);
        }
        newPlacedItems[newNumLayers - 1] = new GameObject[gridSize];
        
        placedItems = newPlacedItems;
    }
    /// <summary>
    /// This method must be called when we modify the depth of the selected layer.
    /// </summary>
    /// <param name="selectedLayer"> The index of the selected layer </param>
    /// <param name="depthModified"> The amount of Unity units we want to translate the layer. 
    /// The sign must be specified </param>
    public static void OnLayerDepthModified(int selectedLayer, float depthModified)
    {
        if (placedItems == null) return;
        if (placedItems[selectedLayer] == null)
        {
            Debug.Log("No existe la capa a desplazar");
            return;
        }
        int gridSize = placedItems[0].Length;
        for (int i = 0; i < gridSize; i++)
        {
            if (placedItems[selectedLayer][i] != null)
                placedItems[selectedLayer][i].transform.position += new Vector3(0, 0, depthModified);
        }
    }
}
