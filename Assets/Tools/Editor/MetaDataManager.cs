using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

[InitializeOnLoad]

public class MetaDataManager : AssetModificationProcessor
{
    private static readonly string prefabPath = "Assets/Prefabs/LevelObjects";
    private static readonly string scriptableObjectPath = "Assets/ScriptableObjects";

    private static Dictionary<string, List<GameObject>> categories;
    private static List<string> categoryLabels;
    private static List<CategoryData> categorieData;

    private static List<string> assetsToLoad;
    private static List<string> assetsToDelete;

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

    private static void InitializeCategories()
    {
        categories = EditorUtils.GetFolders(prefabPath);
        categoryLabels = categories.Keys.ToListPooled();

        foreach (string category in categories.Keys)
        {
            CategoryData.CreateInstance(category, 0, categories[category]);
        }
    }

    public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
    {
        string folderPath = Path.GetDirectoryName(path).Replace("\\", "/");

        Debug.Log(path);
        Debug.Log(AssetDatabase.IsValidFolder(path));

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
                    CategoryData.CreateInstance(assetName, 0, categories[assetName]);
                    Debug.Log("Added category: " + assetName);
                }
            }
        }

        else
        {
            int i = 0;
            bool found = false;
            while(i < categoryLabels.Count && !found) 
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
            if (asset == null) Debug.Log("Asset is null");
            UpdateMetadata(a, asset);

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

                Debug.Log("Could delete?: " + deleted);

                if (deleted)
                {
                    string folderName = Path.GetFileNameWithoutExtension(a);
                    categoryLabels.Remove(a);
                    categories.Remove(a);
                    Debug.Log("Deleted folder at: " + a);
                }
            }

            else 
            {
                Debug.Log(a);

                string assetName = Path.GetFileNameWithoutExtension(a);
                string category = Path.GetDirectoryName(a).Replace("\\", "/").Replace(prefabPath + "/", "");
                CategoryData categoryData = AssetDatabase.LoadAssetAtPath<CategoryData>(scriptableObjectPath + "/" + category + ".asset");

                if (categoryData != null)
                {
                    Debug.Log(categoryData.name);
                    categoryData.RemoveObject(assetName);
                }
            }

            assetsToDelete.RemoveAt(0);
        }
    }

    private static void UpdateMetadata(string assetPath, GameObject prefab) 
    {
        string assetName = Path.GetFileNameWithoutExtension(assetPath);
        string category = Path.GetDirectoryName(assetPath).Replace("\\", "/").Replace(prefabPath + "/", "");

        CategoryData categoryData = AssetDatabase.LoadAssetAtPath<CategoryData>(scriptableObjectPath + "/" + category + ".asset");
        categoryData.AddObject(prefab);
    }

    public static List<GameObject> RetrieveCategory(string categoryName) 
    {
        CategoryData categoryData = AssetDatabase.LoadAssetAtPath<CategoryData>(scriptableObjectPath + "/" + categoryName + ".asset");
        if (categoryData != null)
        {
            return categoryData.LoadObjects();
        }

        else return null;
    }

    public static Dictionary<string, List<GameObject>> getCategories() 
    {
        categories = new Dictionary<string, List<GameObject>>();
        foreach (string c in categoryLabels) categories.Add(c, RetrieveCategory(c));

        return categories;
    }
}
