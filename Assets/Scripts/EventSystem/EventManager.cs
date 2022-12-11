using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int, int, HexRotatingState> AtomRotationEvent;

    public static event Action<int> AtomRotationEndEvent;

    public static event Action<Vector2Int, HexMovingState> AtomMoveEvent;

    public static event Action<Vector2Int> AtomMoveEndEvent;

    public static event Action CalculateMisionProgressEvent;

    public static event Action<Vector2Int, Vector2Int> PlanetReturnEvent;

    public static void StartAtomRotationEvent(int layerIndex, int hexIndexY, HexRotatingState hexRotatingState)
    {
        AtomRotationEvent?.Invoke(layerIndex, hexIndexY, hexRotatingState);
    }

    public static void StartAtomRotationEndEvent(int hexIndexY)
    {
        AtomRotationEndEvent?.Invoke(hexIndexY);
    }

    public static void StartAtomMoveEvent(Vector2Int hexIndex, HexMovingState hexMovingState)
    {
        AtomMoveEvent?.Invoke(hexIndex, hexMovingState);
    }

    public static void StartAtomMoveEndEvent(Vector2Int hexIndex)
    {
        AtomMoveEndEvent?.Invoke(hexIndex);
    }

    public static void StartCalculateMisionProgressEvent()
    {
        CalculateMisionProgressEvent?.Invoke();
    }

    public static void StartPlanetReturnEvent(Vector2Int prevIndex, Vector2Int hexIndex)
    {
        PlanetReturnEvent?.Invoke(prevIndex,hexIndex);
    }
}
