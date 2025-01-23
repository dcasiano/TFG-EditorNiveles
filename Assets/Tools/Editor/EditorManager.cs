using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Scenary))]
public class EditorManager : Editor
{
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
                Tools.current = Tool.Move;
                break;
        }
        SceneView.currentDrawingSceneView.in2DMode = true;
    }
}
