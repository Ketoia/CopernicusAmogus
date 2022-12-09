using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldBaseState
{
    public abstract void EnterState(WorldStateManager item);
    public abstract void UpdateState(WorldStateManager item);
    public abstract void FixedUpdateState(WorldStateManager item);
    public abstract void ExitState(WorldStateManager item);
    public abstract void OnCollisionEnter(WorldStateManager item, Collider other);
    public abstract void OnCollisionExit(WorldStateManager item, Collider other);
}

