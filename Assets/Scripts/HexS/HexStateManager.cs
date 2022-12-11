using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexStateManager : MonoBehaviour
{
    //[SerializeField] private List<GameObject> atoms = new List<GameObject>();
    [SerializeField] private WorldStateManager world = new WorldStateManager();
    [SerializeField] private Vector2Int startPosition = new Vector2Int();
    [SerializeField] private int level;

    public HexIdleState idleState = new HexIdleState();
    public HexEntryMovingState entryMovingState = new HexEntryMovingState();
    public HexMovingState movingState = new HexMovingState();
    public HexRotatingState rotatingState = new HexRotatingState();


    private HexBaseState currentState;

    //private List<HexInfo> hexs;
    //private Vector2 iD;
    
    private Vector3 lastMousePos;
    private Vector2Int currentId;
    
    private List<Vector2Int> orbitIndexes;

    public Vector2Int CurrentId { get { return currentId; } set { currentId = value; } }
    public Vector3 LastMousePos { get { return lastMousePos; } set { lastMousePos = value; } }
    
    public List<Vector2Int> OrbitIndexes => orbitIndexes;
    public WorldStateManager World => world;
    public int Level => level;

    void Start()
    {
        
        currentId = startPosition;
        currentState = idleState;

        currentState.EnterState(this);
    }

    public void SetPos(Vector2 position)
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

    public Vector2Int GetNearestPoint(List<Vector2Int> gridIndexes)
    {
        Vector2Int index = currentId;
        float diff = (world.MousePosition - world.Hexs[index].HexPos).magnitude;
        for (int i = 0; i < gridIndexes.Count; i++)
        {
            float newDiff = (transform.position - world.Hexs[gridIndexes[i]].HexPos).magnitude;
            if (newDiff < diff)
            {
                diff = newDiff;
                index = gridIndexes[i];
            }
        }
        return index;
    }


    public Vector2 V3ToV2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    public Vector3 V2ToV3(Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }



    public Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2/*, out bool found*/)
    {
        float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);

        if (tmp == 0)
        {
            // No solution!
            //found = false;
            return Vector2.zero;
        }

        float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;

        //found = true;

        return new Vector2(
            B1.x + (B2.x - B1.x) * mu,
            B1.y + (B2.y - B1.y) * mu
        );
    }
    public void SetupOrbit(Vector2Int index)
    {
        orbitIndexes = GetIndexesToRotate(index);
    }

    public List<Vector2Int> GetIndexesToRotate(Vector2Int index)
    {
        List<Vector2Int> newList = new List<Vector2Int>();
        //float slice = index.y / index.x;
        for (int x = 0; x < index.x * world.MaxPizzaSlices; x++)
        {
            newList.Add(new Vector2Int(index.x, x));
        }

        return newList;
    }
    public Vector2Int UpdateCurrentMoveId(Vector2Int previousCelestianBodyHexId, Vector2Int hexIndex)
    {

        int layersCount = hexIndex.x - previousCelestianBodyHexId.x;

        int pizzaSlice = currentId.y / currentId.x;
        int diff = pizzaSlice * layersCount;

        //Debug.Log(gameObject.name + " " + new Vector2Int(previousCelestianBodyHexId.x, currentId.y + diff));
        return new Vector2Int(hexIndex.x, currentId.y + diff);
        //currentId.x + layersCount;
        //currentId.y += diff;

    }
    //public bool IsFlexible()
    //{
    //    if (world.)
    //    {

    //    }
    //}
}
