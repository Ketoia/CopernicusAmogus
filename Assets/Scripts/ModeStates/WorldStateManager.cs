using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStateManager : MonoBehaviour
{
    [SerializeField] private int maxLayer;
    [SerializeField] private int maxPizzaSlices;
    //[SerializeField] private int pointsCount;
    [SerializeField] private float radius;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject testPrefab;

    public WorldIdleState idleState = new WorldIdleState();
    public WorldMovingState movingState = new WorldMovingState();

    private WorldBaseState currentState;




    private IDictionary<Vector2, HexInfo> hexs = new Dictionary<Vector2, HexInfo>();
    private Vector3 mousePositionInScreen;
    private Vector3 mousePosition;
    //private float angle;
    //private List<Vector3> worldsPoints = new List<Vector3>();
    //private List<HexInfo> hexs = new List<HexInfo>();

    [Header("Zagadki")]
    [SerializeField] private List<LevelInfo> levelInfos;



    public IDictionary<Vector2, HexInfo> Hexs => hexs;
    //private float[,,] hexs = new float[9,1,3];
    public Vector3 MousePositionInScreen => mousePositionInScreen;
    public Vector3 MousePosition => mousePosition;
    public int MaxLayer => maxLayer;
    public int MaxPizzaSlices => maxPizzaSlices;
    void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            List<Vector3> allPos = AddNewLayer(i + 1, radius * (i+1));
            for (int x = 0; x < allPos.Count; x++)
            {
                HexInfo hex = new HexInfo();
                hex.RowIndex = i + 1;
                hex.HexIndex = x;
                hex.HexPos = allPos[x];
                hexs.Add(new Vector2(i + 1,x), hex);
                //hexs.Add(hex);

            }

        }

        currentState = idleState;

        currentState.EnterState(this);
    }

    void Update()
    {
        SetMousePos();
        currentState.UpdateState(this);


    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }
    public void SwitchState(WorldBaseState state)
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

    public void SetMousePos()
    {
        mousePositionInScreen = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePositionInScreen);
        float height = 0f;
        float dist = (height-ray.origin.y) / ray.direction.y;

        mousePosition = ray.GetPoint(dist);
        //mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePositionInScreen.x, 0 , mousePositionInScreen.y));
    }
    public List<Vector3> AddNewLayer(int row, float radius)
    {
        float angle = 60f * Mathf.Deg2Rad;


        List<Vector3> worldsPoints = new List<Vector3>();
        Vector3 lastMainPos = new Vector3();
        for (int i = 0; i < 6; i++)
        {
            Vector3 lastPos = new Vector3();
            Vector3 pos = GetPosOfXY(angle, i, radius);
            lastMainPos = pos;
            worldsPoints.Add(pos);
            Instantiate(prefab, pos, Quaternion.identity, transform);
            pos = GetPosOfXY(angle, i + 1, radius);
            for (int z = 0; z < row - 1; z++)
            {
                Vector3 diff = (pos - lastMainPos) / row;
                Vector3 newPos = lastMainPos + lastPos + diff;

                lastPos += diff;
                worldsPoints.Add(newPos);
                Instantiate(testPrefab, newPos, Quaternion.identity, transform);
            }
        }
        return worldsPoints;
    }

    private Vector3 GetPosOfXY(float angle, int i, float radius)
    {
        float angleNow = i * -angle;
        float x = Mathf.Cos(angleNow) * radius;
        float y = Mathf.Sin(angleNow) * radius;
        return new Vector3(x + transform.position.x, transform.position.y- 1, y + transform.position.z);
    }

}

public struct HexInfo
{
    private int rowIndex;
    private int hexIndex;
    private Vector3 hexPos;

    public int RowIndex { get { return rowIndex; } set { rowIndex = value; } }
    public int HexIndex { get { return hexIndex; } set { hexIndex = value; } }
    public Vector3 HexPos { get { return hexPos; } set { hexPos = value; } }
}

[System.Serializable]
public struct LevelInfo
{
    [SerializeField] private int level;
    [SerializeField] private List<AllBodies> allBodies;
}

[System.Serializable]
public struct AllBodies
{
    [SerializeField] private Vector2 cords;
    [SerializeField] private BodyType bodyType;
    [SerializeField] private BodyLevel bodyLevel;
}

public enum BodyType
{
    CelestianBody,
    Atom
}

public enum BodyLevel
{
    Mercury,
    Venus,
    Earth,
    Jupiter,
    Starurn,
    Uranus,
    Neptun,
    Moon,
    Sun
}