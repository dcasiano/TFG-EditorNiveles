using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelObjects", menuName = "ScriptableObjects/ObjectScriptable", order = 2)]

[Serializable]
public class LevelObjectScriptable : ScriptableObject
{
    public GameObject[] objects;
    public int saved = 0;
}

