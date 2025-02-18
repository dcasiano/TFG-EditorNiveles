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
    /// 1st index: row number | 2nd index: column number
    /// </summary>
    GameObject[,] itemsPlaced;

    private void OnEnable()
    {
        thisTarget = (Scenary)target;
        itemsPlaced = new GameObject[thisTarget.TotalRows, thisTarget.TotalColumns];

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
        Paint
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
        if (thisTarget.IsInsideGridBounds(gridPos)) Debug.LogFormat("GridPos {0},{1}", col, row);

        switch (currentState)
        {
            case EditorState.Paint:
                if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
                    Paint(col, row);
                break;
            default:
                break;
        }
    }
    private void Paint(int col, int row)
    {
        if (!thisTarget.IsInsideGridBounds(col, row) || itemSelected == null) return;

        if (itemsPlaced[row, col] != null)
        {
            DestroyImmediate(itemsPlaced[row, col]);
        }
        GameObject newItem = PrefabUtility.InstantiatePrefab(itemSelected) as GameObject;
        newItem.transform.parent = thisTarget.transform;
        newItem.name = string.Format("[{0},{1}][{2}]", col, row, newItem.name);
        newItem.transform.position = thisTarget.GridToWorldCoordinates(col, row);
        itemsPlaced[row, col] = newItem;
    }

    private void UpdateCurrentPieceInstance(GameObject item)
    {
        itemSelected = item;
    }
}
