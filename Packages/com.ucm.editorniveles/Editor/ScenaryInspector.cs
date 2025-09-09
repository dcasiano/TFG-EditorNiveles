using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace EditorNiveles
{
    [CustomEditor(typeof(Scenary))]
    public class ScenaryInspector : Editor
    {
        Scenary thisTarget;
        GameObject itemSelected;
        string currentCategory;
        string newLayerName;
        private void OnEnable()
        {
            thisTarget = (Scenary)target;

            // Subscribe to the event called when a new item is selected in the palette
            PaletteWindow.ItemSelectedEvent += new PaletteWindow.itemSelectedDelegate(UpdateCurrentPieceInstance);

            if (!EditorWindow.HasOpenInstances<PaletteWindow>())
            {
                MenuItems.ShowPalette();
            }
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
        
        void Start()
        {
            currentState = EditorState.View;
            itemSelected = null;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawLayersGUI();

            // Place items on their new position when tile resized
            if (thisTarget.TileSize != thisTarget.PreviousTileSize)
            {
                GameObject[][] placedItems = EditorManager.GetPlacedItems();
                if (placedItems != null)
                {
                    int numLayers = placedItems.Length;
                    int gridSize = placedItems[0].Length;

                    for (int i = 0; i < numLayers; i++)
                    {
                        for (int j = 0; j < gridSize; j++)
                        {
                            if (placedItems[i][j] != null)
                            {
                                int row = j / thisTarget.TotalColumns;
                                int col = j % thisTarget.TotalColumns;
                                placedItems[i][j].transform.position = thisTarget.GridToWorldCoordinates(col, row, i);
                            }

                        }
                    }
                }
                thisTarget.PreviousTileSize = thisTarget.TileSize;
            }
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
            EditorGUILayout.LabelField("Change Layer Depth");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                thisTarget.layersDepth[thisTarget.selectedLayerIndex] -= thisTarget.GetLayersDepthValue();
                EditorManager.OnLayerDepthModified(thisTarget.selectedLayerIndex, -thisTarget.GetLayersDepthValue());
            }

            EditorGUILayout.LabelField(thisTarget.layers[thisTarget.selectedLayerIndex], GUILayout.ExpandWidth(true));
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                thisTarget.layersDepth[thisTarget.selectedLayerIndex] += thisTarget.GetLayersDepthValue();
                EditorManager.OnLayerDepthModified(thisTarget.selectedLayerIndex, thisTarget.GetLayersDepthValue());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Add New Layer");
            newLayerName = EditorGUILayout.TextField("Name", newLayerName);

            if (GUILayout.Button("Add Layer"))
            {
                if (!string.IsNullOrEmpty(newLayerName))
                {
                    thisTarget.layers.Add(newLayerName);
                    thisTarget.layersDepth.Add(0.0f);
                    EditorManager.OnNewLayerCreated();
                    newLayerName = "";
                }
            }

            EditorGUILayout.Space();
            string removeButtonText = "Remove " + thisTarget.layers[thisTarget.selectedLayerIndex] + " Layer";
            Color defaultColor2 = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button(removeButtonText))
            {
                thisTarget.layers.RemoveAt(thisTarget.selectedLayerIndex);
                thisTarget.layersDepth.RemoveAt(thisTarget.selectedLayerIndex);
                EditorManager.OnLayerRemoved(thisTarget.selectedLayerIndex);
                
                thisTarget.selectedLayerIndex--;
                if (thisTarget.selectedLayerIndex < 0) thisTarget.selectedLayerIndex = 0;
            }
            GUI.backgroundColor = defaultColor2;

            // Guardar los cambios
            if (GUI.changed)
            {
                EditorUtility.SetDirty(thisTarget);
            }

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
            //SceneView.currentDrawingSceneView.in2DMode = true;

        }

        private void EventHandler()
        {
            Event e = Event.current;

            HandleUtility.AddDefaultControl(
            GUIUtility.GetControlID(FocusType.Passive));

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            Plane gridPlane = new Plane(Vector3.forward, Vector3.zero);

            if (gridPlane.Raycast(ray, out float enter))
            {
                Vector3 worldPoint = ray.GetPoint(enter);
                Vector3 gridPos = thisTarget.WorldToGridCoordinates(worldPoint);

                int col = Mathf.FloorToInt(gridPos.x);
                int row = Mathf.FloorToInt(gridPos.y);

                switch (currentState)
                {
                    case EditorState.Paint:
                        if (e.button == 0 && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag))
                            Paint(col, row);
                        break;
                    case EditorState.Erase:
                        if (e.button == 0 && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag))
                            Erase(col, row);
                        break;
                    default:
                        break;
                }
            }
        }
        private void Paint(int col, int row)
        {
            if (!thisTarget.IsInsideGridBounds(col, row) || itemSelected == null) return;

            int index = row * thisTarget.TotalColumns + col;
            int scenarySize = thisTarget.TotalColumns * thisTarget.TotalRows;
            int selectedLayer = thisTarget.selectedLayerIndex;

            if (thisTarget.transform.Find("Layer" + selectedLayer) == null)
            {
                GameObject layerObj = new GameObject("Layer" + selectedLayer);
                layerObj.transform.SetParent(thisTarget.transform);
                layerObj.transform.localPosition = Vector3.zero;
            }
            GameObject newItem = PrefabUtility.InstantiatePrefab(MetaDataManager.GetObjectVariant(currentCategory, itemSelected.name)) as GameObject;
            Transform layerParent = thisTarget.transform.Find("Layer" + selectedLayer);
            newItem.transform.SetParent(layerParent);
            newItem.name = string.Format("L{0}-[{1},{2}][{3}]", selectedLayer, col, row, newItem.name);
            newItem.transform.position = thisTarget.GridToWorldCoordinates(col, row, selectedLayer);

            EditorManager.PlaceNewItem(newItem, index, scenarySize, thisTarget.selectedLayerIndex, thisTarget.layers.Count);
        }

        private void Erase(int col, int row)
        {
            if (!thisTarget.IsInsideGridBounds(col, row)) return;

            int index = row * thisTarget.TotalColumns + col;
            EditorManager.DeleteItem(index, thisTarget.selectedLayerIndex);
        }

        private void UpdateCurrentPieceInstance(GameObject item, string category)
        {
            itemSelected = item;
            currentCategory = category;
        }
    }
}
