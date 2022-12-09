using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexIdleState : HexBaseState
{
    private HexStateManager item;
    private bool canBeMoving;
    private Collider collider;
    public override void EnterState(HexStateManager item)
    {
        this.item = item;
        item.SetPos(item.LastId);
        collider = item.transform.GetComponent<Collider>();
        
    }
    public override void UpdateState(HexStateManager item)
    {
        //Debug.Log(item.World.MousePosition);
        //Debug.Log(mousePos);
        if (Input.GetMouseButtonDown(0) && collider.bounds.Contains(item.World.MousePosition))
        {
            //start EventMoving
            //Debug.Log("zmien state");
            item.LastMousePos = item.World.MousePosition;
            item.SwitchState(item.entryMovingState);
        }

    }
    public override void FixedUpdateState(HexStateManager item)
    {

    }
    public override void ExitState(HexStateManager item)
    {

    }
    public override void OnCollisionEnter(HexStateManager item, Collider other)
    {

    }
    public override void OnCollisionExit(HexStateManager item, Collider other)
    {

    }
}
