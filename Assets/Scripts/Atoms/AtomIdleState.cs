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

    private void SwitchToMove(int layerIndex)
    {
        item.SwitchState(item.movingState);
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
    }
}
