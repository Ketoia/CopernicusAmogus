using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomStateManager : MonoBehaviour
{
    //[SerializeField] private List<GameObject> atoms = new List<GameObject>();
    [SerializeField] private WorldStateManager world = new WorldStateManager();
    [SerializeField] private Vector2Int startPosition = new Vector2Int();
    //[SerializeField] private Vector2 startPosition = new Vector2();

    public AtomIdleState idleState = new AtomIdleState();
    public AtomMovingState movingState = new AtomMovingState();
    public AtomRotationState rotationState = new AtomRotationState();

    private AtomBaseState currentState;

    public HexRotatingState hexRotatingState;

    //private List<HexInfo> hexs;
    //private int layerId;
    public int level = 0;
    private Vector2Int currentId;
    private Vector2Int previousCelestianBodyHexId;
    private List<Vector2> allFlexibleIndex;
    private HexMovingState hexMovingState;

    public Vector2Int CurrentId { get { return currentId; } set { currentId = value; } }
    public Vector2Int PreviousCelestianBodyHexId { get { return previousCelestianBodyHexId; } set { previousCelestianBodyHexId = value; } }
    public int Level { get { return level; } set { level = value; } }
    public HexMovingState HexMovingState { get { return hexMovingState; } set { hexMovingState = value; } }
    public List<Vector2> AllFlexibleIndex => allFlexibleIndex;

    public WorldStateManager World => world;
    

    void Start()
    {
        //SetPos(startPosition);
        currentId = startPosition;
        currentState = idleState;

        currentState.EnterState(this);
    }

    public void SetPos(Vector2 position)
    {
        Vector3 newPos = new Vector3(world.Hexs[position].HexPos.x, 0, world.Hexs[position].HexPos.z);
        transform.position = newPos;
    }
    //private void SetPos(Vector3 position)
    //{
    //    Vector3 newPos = new Vector3(world.Hexs[startPosition].HexPos.x, world.Hexs[startPosition].HexPos.y, -1);
    //    transform.position = newPos;
    //}

    void Update()
    {
       // Debug.Log(world.Hexs.Count);
        currentState.UpdateState(this);



    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }
    public void SwitchState(AtomBaseState state)
    {
        if (this == null)
            return; 
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

    public void UpdateCurrentRotId(int hexIndexY)
    {
        int diff = hexIndexY - previousCelestianBodyHexId.y;
        int mod = world.MaxPizzaSlices * currentId.x;
        currentId.y = (((currentId.y + diff) % mod) + mod) % mod;
    }

    public Vector2Int UpdateCurrentMoveId(Vector2Int hexIndex)
    {

        int layersCount = hexIndex.x - previousCelestianBodyHexId.x;

        int pizzaSlice = currentId.y / currentId.x;
        int diff = pizzaSlice * layersCount;
        
        //Debug.Log(gameObject.name + " " + diff);
        return new Vector2Int(currentId.x + layersCount, currentId.y + diff);
        //currentId.x + layersCount;
        //currentId.y += diff;

    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        currentState = null;
    }
}
