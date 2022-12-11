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
        //Debug.Log("Rotation enter");
        EventManager.AtomMoveEndEvent += SwitchState;
    }

    private void SwitchState(Vector2Int hexIndex)
    {
        //Debug.Log("Rotation enter2");
        Vector2Int prevId = item.CurrentId;
       // Debug.Log("prevID " + item.CurrentId);

        Vector2Int newId = item.UpdateCurrentMoveId(hexIndex);
        //Debug.Log("currentID " + item.CurrentId);
        if (prevId != newId)
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

        Vector2 indexVec = V3ToV2(item.World.Hexs[item.CurrentId].HexPos).normalized;

        //Vector2 firstVec3 = V3ToV2(item.World.MousePosition);

        //float firstLayerDist = V3ToV2(item.World.Hexs[indexesToMoveForward[0]].HexPos).magnitude;
        //float LastLayerDist = V3ToV2(item.World.Hexs[indexesToMoveForward[indexesToMoveForward.Count - 1]].HexPos).magnitude;

        Vector2 test = indexVec.normalized * item.HexMovingState.vecDot;
        // Debug.Log("Vector3 mouse pos: " + firstVec3);
        // Debug.Log("Vector3 ball pos: " + indexVec);
        //  Debug.Log("Dot product: " + Vector2.Dot(firstVec3, indexVec));

        Vector2 pos = test;
        item.transform.position = new Vector3(pos.x, 0, pos.y);

    }
    public Vector2 V3ToV2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
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
