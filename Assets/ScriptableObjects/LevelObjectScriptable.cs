using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelObjects", menuName = "ScriptableObjects/ObjectScriptable", order = 2)]
public class LevelObjectScriptable : ScriptableObject
{
    public GameObject[] objects;
}

