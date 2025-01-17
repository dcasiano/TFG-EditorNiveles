using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteItem : MonoBehaviour
{
#if UNITY_EDITOR
    public enum Category
    {
        Default,
        Block,
        Pickable,
        Enemy
    }
    public Category category = Category.Default;
    public string itemName = "";
    public Object inspectedScript;
#endif

}
