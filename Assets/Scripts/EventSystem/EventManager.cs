using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int, int, HexRotatingState> AtomRotationEvent;

    public static event Action<int> AtomRotationEndEvent;

    public static event Action<Vector2Int> AtomMoveEvent;

    public static event Action<Vector2Int> AtomMoveEndEvent;



    public static void StartAtomRotationEvent(int layerIndex, int hexIndexY, HexRotatingState hexRotatingState)
    {
        AtomRotationEvent?.Invoke(layerIndex, hexIndexY, hexRotatingState);
    }

    public static void StartAtomRotationEndEvent(int hexIndexY)
    {
        AtomRotationEndEvent?.Invoke(hexIndexY);
    }

    public static void StartAtomMoveEvent(Vector2Int hexIndex)
    {
        AtomMoveEvent?.Invoke(hexIndex);
    }

    public static void StartAtomMoveEndEvent(Vector2Int hexIndex)
    {
        AtomMoveEndEvent?.Invoke(hexIndex);
    }
}
