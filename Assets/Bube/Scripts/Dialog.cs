using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    public List<DialogProperty> dialogProperties;

}

[System.Serializable]
public struct DialogProperty
{
    public string simpleDialog;
    public int actorIndex;
}
