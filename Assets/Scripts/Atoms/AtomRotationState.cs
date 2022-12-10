using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomRotationState : AtomBaseState
{
    private AtomStateManager item;
    public override void EnterState(AtomStateManager item)
    {
        this.item = item;
        //Debug.Log("Rotation enter");
        EventManager.AtomRotationEndEvent += SwitchState;
    }

    private void SwitchState(int hexIndexY)
    {
        //Debug.Log("Rotation enter2");

        item.UpdateCurrentRotId(hexIndexY);
        item.SwitchState(item.idleState);
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
        EventManager.AtomRotationEndEvent -= SwitchState;
    }
}
