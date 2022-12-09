using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexStateManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> atoms = new List<GameObject>();
    [SerializeField] private WorldStateManager world = new WorldStateManager();
    [SerializeField] private Vector2 startPosition = new Vector2();

    public HexIdleState idleState = new HexIdleState();
    public HexEntryMovingState entryMovingState = new HexEntryMovingState();
    public HexMovingState movingState = new HexMovingState();
    public HexRotatingState rotatingState = new HexRotatingState();


    private HexBaseState currentState;

    //private List<HexInfo> hexs;
    //private Vector2 iD;
    private Vector3 lastMousePos;
    private Vector2 lastId;
    private List<Vector2> allFlexibleIndex;

    public Vector2 LastId { get { return lastId; } set { lastId = value; } }
    public Vector3 LastMousePos { get { return lastMousePos; } set { lastMousePos = value; } }
    public List<Vector2> AllFlexibleIndex => allFlexibleIndex;
    public WorldStateManager World => world;
    

    void Start()
    {
        allFlexibleIndex = AllIndexesToMoveForward(world.MaxLayer, world.MaxPizzaSlices);
        lastId = startPosition;
        currentState = idleState;

        currentState.EnterState(this);
    }

    public void SetPos(Vector3 position)
    {
        Vector3 newPos = new Vector3(world.Hexs[position].HexPos.x, 0, world.Hexs[position].HexPos.z);
        transform.position = newPos;
    }

    void Update()
    {
        //Debug.Log(world.Hexs.Count);
        currentState.UpdateState(this);



    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }
    public void SwitchState(HexBaseState state)
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



    private List<Vector2> AllIndexesToMoveForward(int layersAmount, int pizzaSlices)
    {
        List<Vector2> newList = new List<Vector2>();
        for (int x = 0; x < layersAmount; x++)
        {
            for (int y = 0; y < pizzaSlices; y++)
            {
                newList.Add(new Vector2(x + 1, y * (x + 1)));
            }
        }

        return newList;
    }
}
