using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    public List<string> Dialogs;

    public Sprite firstActor;
    public Sprite SecondActor;
}
