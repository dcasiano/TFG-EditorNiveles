using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace EditorNiveles
{
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
        static readonly string scriptableObjectPath = "Packages/com.ucm.editorniveles/Editor/ScriptableObjects/";
        static EditorManager()
        {
            LoadTemporaryScriptableObject();

            // Subscribe to the event called when we enter or exit Edit Mode in Unity
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            // Subscribe to the event called when we open a scene
            EditorSceneManager.sceneOpened += OnSceneOpened;

            EditorSceneManager.sceneSaved += OnSceneSaved;
        }


        // Callback method to handle play mode state changes
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            // Check if the state is EnteredPlayMode
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                SaveObjectsToDisk();
            }

            else if (state == PlayModeStateChange.EnteredEditMode)
            {
                LoadObjectsFromDisk();
            }
        }
        private static void LoadTemporaryScriptableObject()
        {
            string completePath = scriptableObjectPath + "LevelObjects.asset";
            scriptableObject = AssetDatabase.LoadAssetAtPath<LevelObjectScriptable>(completePath);
            if (scriptableObject == null) CreateTemporaryScriptableObject();
        }
        private static void CreateTemporaryScriptableObject()
        {
            string completePath = scriptableObjectPath + "LevelObjects.asset";
            scriptableObject = ScriptableObject.CreateInstance<LevelObjectScriptable>();
            AssetDatabase.CreateAsset(scriptableObject, completePath);
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
        }
        private static void LoadObjectsFromDisk()
        {
            if (scriptableObject == null || scriptableObject.objects == null) return;
            
            int numLayers = scriptableObject.objects.Count;
            placedItems = new GameObject[numLayers][];
            for (int i = 0; i < numLayers; i++)
            {
                List<GameObject> objectsInLayer = scriptableObject.objects[i].gameObjects;
                placedItems[i] = objectsInLayer.ToArray();
            }
        }

        private static void LoadObjectsOnLoad() 
        {
            Scenary scenary = GameObject.FindFirstObjectByType<Scenary>();

            if (scenary != null)
            {
                GameObject s = scenary.gameObject;
                Debug.Log(s.name);

                int numLayers = scenary.layers.Count;
                int gridSize = scenary.TotalColumns * scenary.TotalRows;
                Debug.Log(numLayers);
                placedItems = new GameObject[numLayers][];
                for (int i = 0; i < numLayers; i++) placedItems[i] = new GameObject[gridSize];

                foreach (Transform t in s.GetComponentInChildren<Transform>())
                {
                    int layer = 0;
                    int.TryParse(t.name.Replace("Layer", ""), out layer);

                    foreach (Transform child in t.GetComponentsInChildren<Transform>())
                    {
                        string name = child.name;

                        // Match pattern L0-[x,y][Cube]
                        Match match = Regex.Match(name, @"\[(\d+),(\d+)\]");

                        if (match.Success)
                        {
                            int col = int.Parse(match.Groups[1].Value);
                            int row = int.Parse(match.Groups[2].Value);

                            int index = row * s.GetComponent<Scenary>().TotalColumns + col;
                            placedItems[layer][index] = child.gameObject;
                        }
                    }
                }
            }
        }

        private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            EditorApplication.update += WaitForSceneObjects;
        }

        [InitializeOnLoadMethod]
        private static void OnEditorOpened() 
        {
            EditorApplication.update += WaitForSceneObjects;
        }

        private static void WaitForSceneObjects()
        {
            var scenary = GameObject.FindFirstObjectByType<Scenary>();
            if (scenary != null)
            {
                EditorApplication.update -= WaitForSceneObjects;
                LoadObjectsOnLoad();
            }
        }

        private static void OnSceneSaved(UnityEngine.SceneManagement.Scene scene)
        {
            SaveObjectsToDisk();
            ScriptableObject persistantSO = ScriptableObject.Instantiate<LevelObjectScriptable>(scriptableObject);
            string completePath = scriptableObjectPath + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "LevelObjects.asset";
            AssetDatabase.CreateAsset(persistantSO, completePath);
            AssetDatabase.SaveAssets();
        }
        /// <summary>
        /// Returns the array that contains the items placed on the grid.
        /// </summary>
        /// <returns></returns>
        public static GameObject[][] GetPlacedItems()
        {
            return placedItems;
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
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
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
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
        private static void ClearObjects()
        {
            placedItems = null;
            string completePath = scriptableObjectPath + "LevelObjects.asset";
            AssetDatabase.DeleteAsset(completePath);
        }
        /// <summary>
        /// This method must be called when we create a new level, so
        /// the references to previous objects are removed.
        /// </summary>
        public static void OnNewLevel()
        {
            ClearObjects();
            CreateTemporaryScriptableObject();
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
        /// <summary>
        /// This method must be called when a layer is removed
        /// </summary>
        /// <param name="selectedLayer"> The index of the layer we want to remove </param>
        public static void OnLayerRemoved(int selectedLayer)
        {
            if (placedItems == null) return;
            if (placedItems[selectedLayer] == null)
            {
                Debug.Log("No existe la capa a eliminar");
                return;
            }
            int gridSize = placedItems[0].Length;
            int newNumLayers = placedItems.Length - 1;

            GameObject[][] newPlacedItems = new GameObject[newNumLayers][];
            for (int i = 0; i < newNumLayers; i++)
            {
                if (i == selectedLayer) continue;
                newPlacedItems[i] = new GameObject[gridSize];
                System.Array.Copy(placedItems[i], newPlacedItems[i], gridSize);
            }

            // Delete every objetc on the layer
            for (int i = 0; i < gridSize; i++)
            {
                if (placedItems[selectedLayer][i] != null)
                {
                    Object.DestroyImmediate(placedItems[selectedLayer][i]);
                    placedItems[selectedLayer][i] = null;
                }
            }

            Scenary scenary = GameObject.FindFirstObjectByType<Scenary>();
            GameObject scenGO = scenary.gameObject;
            if (scenGO.transform.Find("Layer" + selectedLayer) != null)
            {
                Object.DestroyImmediate(scenGO.transform.Find("Layer" + selectedLayer).gameObject);
            }

            placedItems = newPlacedItems;

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            
        }
    }
}

