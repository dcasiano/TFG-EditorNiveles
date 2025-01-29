using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Scenary))]
public class EditorManager : Editor
{
    Scenary thisTarget;

    private void OnEnable()
    {
        thisTarget = (Scenary)target;
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
        Debug.Log("sadasda");
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
        Debug.LogFormat("MousePos: {0}", worldPos);
        Vector3 gridPos = thisTarget.WorldToGridCoordinates(worldPos);
        int col = (int) gridPos.x;
        int row = (int) gridPos.y;
        Debug.LogFormat("GridPos {0},{1}", col, row);
    }

}
