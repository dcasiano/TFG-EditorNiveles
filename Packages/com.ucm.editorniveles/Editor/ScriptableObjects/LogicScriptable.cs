using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Logic", menuName = "ScriptableObjects/LogicScriptable", order = 1)]
[Serializable]
public class LogicScriptable : ScriptableObject
{
    public string prefabName;

    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;
}
