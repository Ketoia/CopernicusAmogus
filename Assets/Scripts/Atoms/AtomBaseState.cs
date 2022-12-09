using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AtomBaseState 
{
    public abstract void EnterState(AtomStateManager item);
    public abstract void UpdateState(AtomStateManager item);
    public abstract void FixedUpdateState(AtomStateManager item);
    public abstract void ExitState(AtomStateManager item);
    public abstract void OnCollisionEnter(AtomStateManager item, Collider other);
    public abstract void OnCollisionExit(AtomStateManager item, Collider other);
}

