using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomStateManager : MonoBehaviour
{
    //[SerializeField] private List<GameObject> atoms = new List<GameObject>();
    [SerializeField] private WorldStateManager world = new WorldStateManager();
    //[SerializeField] private Vector2 startPosition = new Vector2();

    public AtomIdleState idleState = new AtomIdleState();
    public AtomMovingState movingState = new AtomMovingState();


    private AtomBaseState currentState;

    //private List<HexInfo> hexs;
    private Vector2 iD;

    public WorldStateManager World => world;

    void Start()
    {
        //SetPos(startPosition);
        currentState = idleState;

        currentState.EnterState(this);
    }

    //private void SetPos(Vector3 position)
    //{
    //    Vector3 newPos = new Vector3(world.Hexs[startPosition].HexPos.x, world.Hexs[startPosition].HexPos.y, -1);
    //    transform.position = newPos;
    //}

    void Update()
    {
        Debug.Log(world.Hexs.Count);
        currentState.UpdateState(this);



    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }
    public void SwitchState(AtomBaseState state)
    {
        currentState.ExitState(this);

        currentState = state;
        state.EnterState(this);

    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnCollisionEnter(this, other);
    }
    private void OnTriggerExit(Collider other)
    {
        currentState.OnCollisionExit(this, other);
    }
}