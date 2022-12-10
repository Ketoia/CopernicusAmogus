using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomMovingState : AtomBaseState
{
    private AtomStateManager item;
    private AtomStateManager atom;
    //private Vector2Int prevId;
    public override void EnterState(AtomStateManager item)
    {
        this.item = item;
        Debug.Log("Rotation enter");
        EventManager.AtomMoveEndEvent += SwitchState;
    }

    private void SwitchState(Vector2Int hexIndex)
    {
        Debug.Log("Rotation enter2");
        Vector2Int prevId = item.CurrentId;
        Debug.Log("prevID " + item.CurrentId);

        Vector2Int newId = item.UpdateCurrentMoveId(hexIndex);
        Debug.Log("currentID " + item.CurrentId);
        SwapLayer(prevId, newId);
        item.CurrentId = newId;
        item.SwitchState(item.idleState);
    }

    public void SwapLayer(Vector2Int prevId, Vector2Int newId)
    {
        for (int i = 0; i < item.World.Atoms.Count; i++)
        {
            atom = item.World.Atoms[i];
            if (atom.CurrentId == newId)
            {
                if (atom.Level != item.Level)
                {
                    atom.CurrentId = prevId;
                    atom.SetPos(prevId);
                }
                else
                {
                    item.Level++;
                    item.World.Atoms.RemoveAt(i);
                    atom.DestroyThis();
                }
                break;
            }
        }
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
        EventManager.AtomMoveEndEvent -= SwitchState;
    }
}
