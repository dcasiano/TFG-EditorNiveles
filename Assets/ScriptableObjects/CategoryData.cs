using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Categorie Data", menuName = "ScriptableObjects/CategorieData")]
public class CategoryData : ScriptableObject
{
    static readonly string scriptableObjectPath = "Assets/ScriptableObjects/";

    public List<ObjectData> objectData;

    public void Init(string name, int id)
    {
        this.name = name;
        objectData = new List<ObjectData>();
    }

    public void LoadObjects(List<GameObject> objects) 
    {
        foreach (GameObject obj in objects) 
        {
            AddObject(obj);
        }
    }

    public void AddObject(GameObject o)
    {
        int i = 0;
        bool found = false;

        while (i < objectData.Count && !found) 
        {
            if (objectData[i].name == o.name) found = true;
            i++;
        }

        if (!found)
        {
            ObjectData data = new ObjectData();
            data.name = o.name;
            data.prefab = new GameObject[1];
            data.prefab[0] = o;
            objectData.Add(data);
        }
    }

    public void RemoveObject(string objectName)
    {
        int i = 0;
        bool found = false;

        while (i < objectData.Count && !found)
        {
            if (objectData[i].name == objectName)
            {
                found = true;
                objectData.RemoveAt(i);
                Debug.Log("Removed object " + objectName +  " at category " + this.name);
            }
            i++;
        }
    }

    public List<GameObject> LoadObjects()
    {
        List<GameObject> l = new List<GameObject>();

        foreach (ObjectData o in objectData) 
        {
            GameObject gameObject = o.prefab[Random.Range(0, o.prefab.Length)];

            gameObject.name = o.name;
            gameObject.transform.rotation = Quaternion.Euler(o.rotation);

            l.Add(gameObject);
        }

        return l;
    }

    public static CategoryData CreateInstance(string name, int id, List<GameObject> o)
    {
        CategoryData scriptableObject = 
            AssetDatabase.LoadAssetAtPath<CategoryData>(scriptableObjectPath + name + ".asset");

        if (scriptableObject == null)
        {
            var data = ScriptableObject.CreateInstance<CategoryData>();
            data.Init(name, id);
            data.LoadObjects(o);
            AssetDatabase.CreateAsset(data, scriptableObjectPath + name + ".asset");
            AssetDatabase.SaveAssets();
            return data;
        }

        else return scriptableObject;
    }
}

//public class 

[System.Serializable]
public class ObjectData
{
    [Tooltip("Name of Object")]
    public string name;
    [Tooltip("Prefab del object que instanciamos")]
    public GameObject[] prefab;
    [Tooltip("Object Rotation")]
    public Vector3 rotation;
}
