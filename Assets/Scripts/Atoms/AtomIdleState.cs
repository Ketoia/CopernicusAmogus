using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomIdleState : AtomBaseState
{
    private AtomStateManager item;
    public override void EnterState(AtomStateManager item)
    {
        this.item = item;
        item.SetPos(item.CurrentId);
        EventManager.AtomRotationEvent += SwitchToRotate;
        EventManager.AtomMoveEvent += SwitchToMove;
        EventManager.PlanetReturnEvent += PlanetReturn;
    }

    private void SwitchToRotate(int layerIndex, int hexIndexY, HexRotatingState hexRotatingState)
    {

        if (layerIndex == item.CurrentId.x)
        {
            //Debug.Log("rotate");
            item.hexRotatingState = hexRotatingState;
            item.PreviousCelestianBodyHexId = new Vector2Int(layerIndex, hexIndexY);
            item.SwitchState(item.rotationState);
        }
    }

    private void SwitchToMove(Vector2Int hexIndex, HexMovingState hexMovingState)
    {
        if (item.World.AllFlexibleIndex.Contains(item.CurrentId) && item.CurrentId.x == hexIndex.x)
        {
            item.HexMovingState = hexMovingState;
            item.PreviousCelestianBodyHexId = hexIndex; 
            item.SwitchState(item.movingState);
        }
    }

    private void PlanetReturn(Vector2Int prevIndex, Vector2Int hexIndex)
    {
        if (item.World.AllFlexibleIndex.Contains(item.CurrentId) && item.CurrentId.x == prevIndex.x)
        {
            item.PreviousCelestianBodyHexId = prevIndex;
            item.CurrentId = item.UpdateCurrentMoveId(hexIndex);
            item.SetPos(item.CurrentId);

        }
        EventManager.PlanetReturnEvent -= PlanetReturn;
    }
    public override void UpdateState(AtomStateManager item)
    {
        
  

    }
    public override void FixedUpdateState(AtomStateManager item)
    {

    }
 
    public override void OnCollisionEnter(AtomStateManager item, Collider other)
    {

    }
    public override void OnCollisionExit(AtomStateManager item, Collider other)
    {

    }
    public override void ExitState(AtomStateManager item)
    {
        EventManager.AtomRotationEvent -= SwitchToRotate;
        EventManager.AtomMoveEvent -= SwitchToMove;
    }
}
