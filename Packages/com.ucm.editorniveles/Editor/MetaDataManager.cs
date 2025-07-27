using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

namespace EditorNiveles
{
    [InitializeOnLoad]

    public class MetaDataManager : AssetModificationProcessor
    {
        private static readonly string prefabPath = "Packages/com.ucm.editorniveles/Prefabs/LevelObjects";
        private static readonly string scriptableObjectPath = "Packages/com.ucm.editorniveles/Editor/ScriptableObjects";

        private static Dictionary<string, List<GameObject>> categories;
        private static List<string> categoryLabels;
        private static Dictionary<string, CategoryData> categoriesData;

        private static List<string> assetsToLoad;
        private static List<string> assetsToDelete;

        public delegate void metadataChangedDelegate();
        public static event metadataChangedDelegate metadataChangedEvent;

        static MetaDataManager()
        {
            EditorApplication.update += Update;
            InitializeCategories();
            assetsToLoad = new List<string>();
            assetsToDelete = new List<string>();
        }

        static void Update()
        {
            if (assetsToLoad.Count > 0) LoadAssets();
            if (assetsToDelete.Count > 0) DeleteAssets();
        }

        // Creates our metadata containers based on the existing prefab folders 
        public static void InitializeCategories()
        {
            categories = EditorUtils.GetFolders(prefabPath);
            categoryLabels = categories.Keys.ToList();
            categoriesData = new Dictionary<string, CategoryData>();

            foreach (string category in categories.Keys)
            {
                CategoryData d = CategoryData.CreateInstance(category, 0, categories[category], categories[category].Count);
                categoriesData.Add(category, d);
            }
        }

        /* Callback called when deleting assets. We check if the deleted asset is a metadata container or
         * part of one, and if so, mark it for deletion */
        public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
        {
            string folderPath = Path.GetDirectoryName(path).Replace("\\", "/");
            if (folderPath == prefabPath && AssetDatabase.IsValidFolder(path))
            {
                assetsToDelete.Add(path);
                return AssetDeleteResult.DidNotDelete;
            }

            else
            {
                int i = 0;
                bool found = false;
                while (i < categoryLabels.Count && !found)
                {
                    if (prefabPath + "/" + categoryLabels[i] == folderPath)
                    {
                        assetsToDelete.Add(path);
                        found = true;
                    }

                    i++;
                }
            }


            return AssetDeleteResult.DidNotDelete;
        }

        /* Callback called when creating assets. We check if the created asset is a viable prefab folder, in which
         * case we create a new metadata container, or if it is a prefab stored in one of these folders, marking it
         * as an object to load in its container */

        public static void OnWillCreateAsset(string path)
        {
            string folderPath = Path.GetDirectoryName(path).Replace("\\", "/");
            string assetName = Path.GetFileNameWithoutExtension(path);

            if (path.EndsWith(".meta"))
            {
                path = path.Replace(".meta", "");

                if (folderPath == prefabPath && AssetDatabase.IsValidFolder(path))
                {
                    if (!categoryLabels.Contains(assetName))
                    {
                        categoryLabels.Add(assetName);
                        categories.Add(assetName, new List<GameObject>());
                        CategoryData d = CategoryData.CreateInstance(assetName, 0, categories[assetName], categories[assetName].Count);
                        categoriesData.Add(assetName, d);
                        if (metadataChangedEvent != null) metadataChangedEvent();
                    }
                }
            }

            else
            {
                int i = 0;
                bool found = false;
                while (i < categoryLabels.Count && !found)
                {
                    if (prefabPath + "/" + categoryLabels[i] == folderPath)
                    {
                        assetsToLoad.Add(path);
                        found = true;
                    }

                    i++;
                }
            }
        }


        private static void LoadAssets()
        {
            while (assetsToLoad.Count > 0)
            {
                string a = assetsToLoad[0];

                GameObject asset = AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject;
                if (asset != null )UpdateMetadata(a, asset);
                if (metadataChangedEvent != null) metadataChangedEvent();

                assetsToLoad.RemoveAt(0);
            }
        }

        private static void DeleteAssets()
        {
            while (assetsToDelete.Count > 0)
            {
                string a = assetsToDelete[0];

                if (Path.GetExtension(a) == "")
                {
                    bool deleted = AssetDatabase.DeleteAsset(scriptableObjectPath + "/" +
                        Path.GetFileNameWithoutExtension(a) + ".asset");

                    if (deleted)
                    {
                        string folderName = Path.GetFileNameWithoutExtension(a);
                        categories.Remove(folderName);
                        categoriesData.Remove(folderName);
                        categoryLabels.Remove(folderName);
                    }
                }

                else
                {
                    string assetName = Path.GetFileNameWithoutExtension(a);
                    string category = Path.GetDirectoryName(a).Replace("\\", "/").Replace(prefabPath + "/", "");
                    CategoryData categoryData = categoriesData[category];

                    if (categoryData != null)
                    {
                        categoryData.RemoveObject(assetName);
                    }
                }

                if (metadataChangedEvent != null) metadataChangedEvent();
                assetsToDelete.RemoveAt(0);
            }
        }


        private static void UpdateMetadata(string assetPath, GameObject prefab)
        {
            string category = Path.GetDirectoryName(assetPath).Replace("\\", "/").Replace(prefabPath + "/", "");

            CategoryData categoryData = categoriesData[category];
            categoryData.AddObject(prefab);
            categories[category].Add(prefab);
        }

        public static List<GameObject> RetrieveCategory(string categoryName)
        {
            CategoryData categoryData = categoriesData[categoryName];
            if (categoryData != null)
            {
                return categoryData.LoadObjects();
            }

            else return null;
        }

        public static List<GameObject> GetCategory(string categoryName)
        {
            return categories[categoryName];
        }

        public static Dictionary<string, List<GameObject>> GetCategories()
        {
            categories = new Dictionary<string, List<GameObject>>();
            foreach (string c in categoryLabels) categories.Add(c, RetrieveCategory(c));

            return categories;
        }
    }
}