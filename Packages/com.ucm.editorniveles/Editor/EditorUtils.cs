using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorNiveles
{
    public static class EditorUtils
    {
        // Creates a new scene
        public static void NewScene()
        {
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
            EditorApplication.NewScene();
        }
        // Remove all the elements of the scene
        public static void CleanScene()
        {
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
            {
                GameObject.DestroyImmediate(go);
            }
        }
        // Creates a new scene capable to be used as a level
        public static void NewLevel()
        {
            NewScene();
            CleanScene();
            EditorManager.OnNewLevel();

            GameObject mainCamera = new GameObject("Main Camera");
            mainCamera.transform.position = new Vector3(0, 1, -10);
            mainCamera.AddComponent<Camera>();

            GameObject dirLight = new GameObject("Directional Light");
            dirLight.transform.position = new Vector3(0, 3, 0);
            dirLight.AddComponent<Light>();
            dirLight.GetComponent<Light>().type = LightType.Directional;

            GameObject scenaryGO = new GameObject("Scenary");
            scenaryGO.transform.position = Vector3.zero;
            scenaryGO.AddComponent<Scenary>();
        }
        // Finds all prefabs in the specified path which have the script corresponding to the generic type attached
        public static List<T> GetAssetsWithScript<T>(string path) where T : MonoBehaviour
        {
            T tmp;
            string assetPath;
            GameObject asset;
            List<T> assetList = new List<T>();
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { path });
            for (int i = 0; i < guids.Length; i++)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
                tmp = asset.GetComponent<T>();
                if (tmp != null) assetList.Add(tmp);
            }
            return assetList;
        }

        // Finds all prefabs in the specified path
        public static List<GameObject> GetAssets(string path)
        {
            string assetPath;
            GameObject asset;
            List<GameObject> assetList = new List<GameObject>();
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { path });
            for (int i = 0; i < guids.Length; i++)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
                if (asset != null) assetList.Add(asset);
            }
            return assetList;
        }

        public static Dictionary<string, List<GameObject>> GetFolders(string path)
        {
            Dictionary<string, List<GameObject>> folderAssets = new Dictionary<string, List<GameObject>>();
            string[] folders = AssetDatabase.GetSubFolders(path);

            foreach (string folder in folders)
            {
                folderAssets.Add(folder.Replace(path + "/", ""), GetAssets(folder));
            }

            return folderAssets;
        }

        public static int CheckFolders(string path)
        {
            return AssetDatabase.GetSubFolders(path).Length;
        }

        public static string[] GetFolderNames(string path)
        {
            string[] folders = AssetDatabase.GetSubFolders(path);
            for (int i = 0; i < folders.Length; i++)
            {
                folders[i] = folders[i].Replace(path + "/", "");
            }
            return folders;
        }
    }
}