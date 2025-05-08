using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Scenary))]
public class ScenaryInspector : Editor
{
    Scenary thisTarget;
    GameObject itemSelected;
    string newLayerName;
    private void OnEnable()
    {
        thisTarget = (Scenary)target;
        

        // Subscribe to the event called when a new item is selected in the palette
        PaletteWindow.ItemSelectedEvent += new PaletteWindow.itemSelectedDelegate(UpdateCurrentPieceInstance);
    }
    private void OnDisable()
    {
        // Unsubscribe from the event called when a new item is selected in the palette
        PaletteWindow.ItemSelectedEvent -= new PaletteWindow.itemSelectedDelegate(UpdateCurrentPieceInstance);
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
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        DrawLayersGUI();
    }
    private void DrawLayersGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Layers", EditorStyles.boldLabel);
        
        for (int i = 0; i < thisTarget.layers.Count; i++)
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            Color defaultColor = GUI.backgroundColor;
            if (i == thisTarget.selectedLayerIndex)
            {
                buttonStyle.normal.textColor = Color.white;
                //buttonStyle.normal.background = MakeTex(1, 1, new Color(0.4f, 0.6f, 1.0f)); 
                GUI.backgroundColor = new Color(0.4f, 0.6f, 1.0f);
            }

            if (GUILayout.Button(thisTarget.layers[i], buttonStyle))
            {
                thisTarget.selectedLayerIndex = i;
                GUI.FocusControl(null);
            }
            GUI.backgroundColor = defaultColor;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Layer:");
        newLayerName = EditorGUILayout.TextField("Name", newLayerName);

        if (GUILayout.Button("Add Layer"))
        {
            if (!string.IsNullOrEmpty(newLayerName))
            {
                thisTarget.layers.Add(newLayerName);
                newLayerName = "";
            }
        }

        // Guardar los cambios
        if (GUI.changed)
        {
            EditorUtility.SetDirty(thisTarget);
        }

    }
    //private Texture2D MakeTex(int width, int height, Color col)
    //{
    //    Color[] pix = new Color[width * height];
    //    for (int i = 0; i < pix.Length; i++) pix[i] = col;

    //    Texture2D result = new Texture2D(width, height);
    //    result.SetPixels(pix);
    //    result.Apply();
    //    return result;
    //}
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
        int scenarySize = thisTarget.TotalColumns * thisTarget.TotalRows;

        GameObject newItem = PrefabUtility.InstantiatePrefab(itemSelected) as GameObject;
        newItem.transform.parent = thisTarget.transform;
        newItem.name = string.Format("[{0},{1}][{2}]", col, row, newItem.name);
        newItem.transform.position = thisTarget.GridToWorldCoordinates(col, row);

        EditorManager.PlaceNewItem(newItem, index, scenarySize);
    }

    private void Erase(int col, int row)
    {
        if (!thisTarget.IsInsideGridBounds(col, row)) return;

        int index = row * thisTarget.TotalColumns + col;
        EditorManager.DeleteItem(index);
    }

    private void UpdateCurrentPieceInstance(GameObject item)
    {
        itemSelected = item;
    }
}
