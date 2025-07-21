using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Categorie Data", menuName = "ScriptableObjects/CategorieData")]
public class CategoryData : ScriptableObject
{
    static readonly string scriptableObjectPath = "Packages/com.ucm.editorniveles/Editor/ScriptableObjects/";

    public List<ObjectData> objectData;

    public void Init(string name, int id)
    {
        this.name = name;
        objectData = new List<ObjectData>();
    }

    /* Checks if a specified object is already in the metadata list, and if not, 
     * parses that object to metadata and adds it to the list. */
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

    // Checks for a specified object within the metadata list and erases it
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
                Debug.Log("Removed object " + objectName + " at category " + this.name);
            }
            i++;
        }
    }

    // Iterates through a list of given objects to add them to the metadata list
    public void LoadMetadata(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            AddObject(obj);
        }
    }

    // Returns a list of ready-to-instantiate gameobjects based on current metadata
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

    // Instantiates the metadata container as a scriptable object of this class
    public static CategoryData CreateInstance(string name, int id, List<GameObject> o, int expectedNumberOfItems)
    {
        string completePath = scriptableObjectPath + name + ".asset";
        CategoryData scriptableObject =
            AssetDatabase.LoadAssetAtPath<CategoryData>(completePath);

        if (scriptableObject != null && scriptableObject.objectData.Count != expectedNumberOfItems)
        {
            scriptableObject = null;
            AssetDatabase.DeleteAsset(completePath);
        }

        if (scriptableObject == null)
        {
            var data = ScriptableObject.CreateInstance<CategoryData>();
            data.Init(name, id);
            data.LoadMetadata(o);
            AssetDatabase.CreateAsset(data, completePath);
            AssetDatabase.SaveAssets();
            return data;
        }

        else return scriptableObject;
    }
}

[System.Serializable]
public class ObjectData         // Metadata of the components that form your game objects. 
{                               // You may need to add or remove components to suit your specific game.
    [Tooltip("Name of Object")]
    public string name;
    [Tooltip("Prefab del object que instanciamos")]
    public GameObject[] prefab;
    [Tooltip("Object Rotation")]
    public Vector3 rotation;
}
