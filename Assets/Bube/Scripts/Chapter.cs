using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "chapter", menuName = "chapter", order = 1)]
public class Chapter : ScriptableObject
{
    public List<Dialog> Dialogs;

}

