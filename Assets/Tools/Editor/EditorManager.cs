using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Scenary))]
public class EditorManager : Editor
{
    Scenary thisTarget;
    GameObject itemSelected;

    /// <summary>
    /// Array which contains the instantiated items on the grid using the editor.
    /// </summary>
    [SerializeField] GameObject[] itemsPlaced;
    string scriptableobjectsPath = "Assets/ScriptableObjects";
    LevelObjectScriptable scriptableObject;

    //static EditorManager()
    //{
    //    Debug.Log("mmmmmmm");
    //    EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    //}
    //~EditorManager()
    //{
    //    EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    //}

    private void OnEnable()
    {
        thisTarget = (Scenary)target;
        //Debug.Log("Length itemsPlaced: "+itemsPlaced.Length);
        itemsPlaced = new GameObject[thisTarget.TotalColumns * thisTarget.TotalRows];

        scriptableObject = AssetDatabase.LoadAssetAtPath<LevelObjectScriptable>(scriptableobjectsPath + "/LevelObjects.asset");
        if (scriptableObject == null)
        {
            scriptableObject = CreateInstance<LevelObjectScriptable>();
            AssetDatabase.CreateAsset(scriptableObject, scriptableobjectsPath + "/LevelObjects.asset");
            AssetDatabase.SaveAssets();
            SaveObjects();
        }
        else LoadObjects();

        int aux = 0;
        for (int i = 0; i < itemsPlaced.Length; i++)
        {
            if (itemsPlaced[i] != null)
            {
                aux = 1;
                break;
            }
        }
        //if (aux == 1) Debug.Log("yyyyyy");
        //else Debug.Log("XXXXX");

        // Subscribe to the event called when a new item is selected in the palette
        PaletteWindow.ItemSelectedEvent += new PaletteWindow.itemSelectedDelegate(UpdateCurrentPieceInstance);
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    private void OnDisable()
    {
        //SaveObjects();

        // Unsubscribe from the event called when a new item is selected in the palette
        PaletteWindow.ItemSelectedEvent -= new PaletteWindow.itemSelectedDelegate(UpdateCurrentPieceInstance);
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        Debug.Log("Disable");
    }
    public enum EditorState
    {
        View, 
        Paint,
        Erase
    }
    private EditorState currentState;
    // Start is called before the first frame update
    void Start()
    {
        currentState = EditorState.View;
        itemSelected = null;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnSceneGUI()
    {
        DrawStateButtons();
        StateHandler();
        EventHandler();
    }
    private void DrawStateButtons()
    {
        List<string> stateLabels = new List<string>();

        foreach (EditorState state in Enum.GetValues(typeof(EditorState)))
        {
            stateLabels.Add(state.ToString());
        }

        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(80f, 10f, 360, 40f));
        currentState = (EditorState)GUILayout.Toolbar((int)currentState,
        stateLabels.ToArray(), GUILayout.ExpandHeight(true));
        GUILayout.EndArea();
        Handles.EndGUI();
    }
    private void StateHandler()
    {
        switch (currentState)
        {
            case EditorState.View:
                Tools.current = Tool.View;
                break;
            case EditorState.Paint:
            case EditorState.Erase:
                Tools.current = Tool.None;
                break;
            default:
                break;
        }
        SceneView.currentDrawingSceneView.in2DMode = true;
    }

    private void EventHandler() 
    {
        HandleUtility.AddDefaultControl(
        GUIUtility.GetControlID(FocusType.Passive));

        Camera camera = SceneView.currentDrawingSceneView.camera;
        Vector3 mousePosition = new Vector3(Event.current.mousePosition.x, Event.current.mousePosition.y, camera.nearClipPlane);
        mousePosition.y = Screen.height - mousePosition.y - 36.0f;
        //Debug.LogFormat("MousePos: {0}", mousePosition);
        Vector3 worldPos = camera.ScreenToWorldPoint(mousePosition);
        //Debug.LogFormat("MousePos: {0}", worldPos);
        Vector3 gridPos = thisTarget.WorldToGridCoordinates(worldPos);
        int col = (int) gridPos.x;
        int row = (int) gridPos.y;
        //if (thisTarget.IsInsideGridBounds(gridPos)) Debug.LogFormat("GridPos {0},{1}", col, row);

        switch (currentState)
        {
            case EditorState.Paint:
                if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
                    Paint(col, row);
                break;
            case EditorState.Erase:
                if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
                    Erase(col, row);
                break;
            default:
                break;
        }
    }
    private void Paint(int col, int row)
    {
        if (!thisTarget.IsInsideGridBounds(col, row) || itemSelected == null) return;

        int index = row * thisTarget.TotalColumns + col;

        if (itemsPlaced[index] != null) 
        {
            DestroyImmediate(itemsPlaced[index]);
        }

        GameObject newItem = PrefabUtility.InstantiatePrefab(itemSelected) as GameObject;
        newItem.transform.parent = thisTarget.transform;
        newItem.name = string.Format("[{0},{1}][{2}]", col, row, newItem.name);
        newItem.transform.position = thisTarget.GridToWorldCoordinates(col, row);

        itemsPlaced[index] = newItem;

        //for(int i = 0; i < itemsPlaced.Length; i++) 
        //{
        //    Debug.Log(itemsPlaced[i] + "\n");
        //}
    }

    private void Erase(int col, int row)
    {
        if (!thisTarget.IsInsideGridBounds(col, row) || itemSelected == null) return;

        int index = row * thisTarget.TotalColumns + col;

        if (itemsPlaced[index] != null)
        {
            DestroyImmediate(itemsPlaced[index]);
        }
    }

    private void UpdateCurrentPieceInstance(GameObject item)
    {
        itemSelected = item;
    }

    private void SaveObjects() 
    {
        //oS.objects = itemsPlaced;
        if (scriptableObject == null)
        {
            Debug.Log("No se ha podido guardar");
            return;
        }
        //mySO.objects = new GameObject[itemsPlaced.Length];
        //for(int i = 0; i < itemsPlaced.Length; i++)
        //{
        //    mySO.objects[i] = (GameObject)EditorGUILayout.ObjectField(itemsPlaced[i], typeof(GameObject), true);
        //}
        scriptableObject.objects = (GameObject[])itemsPlaced.Clone();
        scriptableObject.saved = 1;
        EditorUtility.SetDirty(scriptableObject);
        AssetDatabase.SaveAssets();
        Debug.Log("items guardados");
    }

    private void LoadObjects()
    {
        if (scriptableObject == null) return;
        //itemsPlaced = mySO.objects;
        itemsPlaced = (GameObject[])scriptableObject.objects.Clone();
        scriptableObject.saved = 0;
        Debug.Log("items cargados");
    }

    //static void PlayModeChecker()
    //{
    //    // Subscribe to the play mode state change event
    //    EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    //}

    // Callback method to handle play mode state changes
    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // Check if the state is EnteredPlayMode
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            Debug.Log("Saliendo Edit Mode");
            SaveObjects();
        }

        else if (state == PlayModeStateChange.EnteredEditMode) 
        {
            Debug.Log("Entrado Edit Mode");
            LoadObjects();
        }
    }
}
