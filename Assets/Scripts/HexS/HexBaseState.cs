using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HexBaseState 
{
    public abstract void EnterState(HexStateManager item);
    public abstract void UpdateState(HexStateManager item);
    public abstract void FixedUpdateState(HexStateManager item);
    public abstract void ExitState(HexStateManager item);
    public abstract void OnCollisionEnter(HexStateManager item, Collider other);
    public abstract void OnCollisionExit(HexStateManager item, Collider other);
}

