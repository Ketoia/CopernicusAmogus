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

    private List<AtomStateManager> atoms = new List<AtomStateManager>();
    [SerializeField] private List<HexStateManager> celestials;

    [SerializeField] private List<GameObject> prefabs;

    public WorldIdleState idleState = new WorldIdleState();
    public WorldMovingState movingState = new WorldMovingState();

    private WorldBaseState currentState;
    private WoodenButtons woodenButtons = new WoodenButtons();


    private Dictionary<Vector2, HexInfo> hexs = new Dictionary<Vector2, HexInfo>();
    private Vector3 mousePositionInScreen;
    private Vector3 mousePosition;

    private List<Vector2Int> allFlexibleIndex;
    //private float angle;
    //private List<Vector3> worldsPoints = new List<Vector3>();
    //private List<HexInfo> hexs = new List<HexInfo>();


    private List<int> quest;
    public List<AtomStateManager> Atoms => atoms;
    public List<HexStateManager> Celestals => celestials;
    public Dictionary<Vector2, HexInfo> Hexs => hexs;
    //private float[,,] hexs = new float[9,1,3];
    public Vector3 MousePositionInScreen => mousePositionInScreen;
    public Vector3 MousePosition => mousePosition;

    public List<Vector2Int> AllFlexibleIndex => allFlexibleIndex;
    public List<GameObject> Prefabs => prefabs;
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
        allFlexibleIndex = AllIndexesToMoveForward(MaxLayer, MaxPizzaSlices);
        currentState = idleState;

        currentState.EnterState(this);

        MissionManager.instance.FinishQuest();
        quest = MissionManager.instance.GenerateNewMission();

        EventManager.CalculateMisionProgressEvent += () => CheckQuest();
    }

    void CheckQuest()
    {
        //Sprawdzaj\
        List<int> list = new List<int>(4);
        int layer1 = 0;
        int layer2 = 0;
        int layer3 = 0;
        int layer4 = 0;

        for (int i = 0; i < atoms.Count; i++)
        {
            switch (atoms[i].CurrentId.x)
            {
                case 1:
                    layer1++;
                    break;
                case 2:
                    layer2++;
                    break;
                case 3:
                    layer3++;
                    break;
                case 4:
                    layer4++;
                    break;
                default:
                    break;
            }
            //list[atoms[i].CurrentId.x]++;
        }
        for (int i = 0; i < quest.Count; i++)
        {
            if (quest[0] == layer1 && quest[1] == layer2 && quest[2] == layer3 && quest[3] == layer4)
            {
                MissionManager.instance.FinishQuest();
                quest = MissionManager.instance.GenerateNewMission();
            }
        }
        Debug.Log(quest.Count + " " + layer1 + " " + layer2 + " " + layer3 + " " + layer4);
        //If true to        MissionManager.instance.FinishQuest();
        //                  quest = MissionManager.instance.GenerateNewMission();
    }

    void Update()
    {
        SetMousePos();
        currentState.UpdateState(this);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Button"))
                {
                    Vector2 mousePos = new Vector2(mousePosition.x, mousePosition.z);
                    Vector2 index = woodenButtons.CheckIndex(mousePos);
                    Vector2 hexPos = new Vector2(hexs[index].HexPos.x, hexs[index].HexPos.z);
                    Vector2 dir = woodenButtons.CheckDirection(hexPos, mousePos);
                    UpdateAtoms(woodenButtons.CheckOnDirection(hexPos, dir, Hexs));
                    EventManager.StartCalculateMisionProgressEvent();
                }
            }
        }
    }

    public void UpdateAtoms(List<Vector2Int> changedHexes)
    {
        for (int i = 0; i < changedHexes.Count; i++)
        {
            bool temp = true;
            for (int y = 0; y < atoms.Count; y++)
            {
                if (atoms[y].CurrentId == changedHexes[i])
                {
                    atoms[y].DestroyThis();
                    atoms.RemoveAt(y);
                    temp = false;
                    break;
                }
            }
            if (temp)
            {
                for (int x = 0; x < celestials.Count; x++)
                {

                    if (celestials[x].CurrentId == changedHexes[i])
                    {
                        break;
                    }
                    if (celestials[x].CurrentId.x == changedHexes[i].x)
                    {
                        AddAtom(changedHexes[i], celestials[x].Level);
                        break;
                    }
                }
            }

        }

    }

    public void AddAtom(Vector2Int index, int level)
    {
        GameObject atom = new GameObject();
        AtomStateManager atomManager = atom.AddComponent<AtomStateManager>();
        atomManager.level = level;
        atomManager.CurrentId = index;
        atomManager.World = this;
        atoms.Add(atomManager);
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

    private List<Vector2Int> AllIndexesToMoveForward(int layersAmount, int pizzaSlices)
    {
        List<Vector2Int> newList = new List<Vector2Int>();
        for (int x = 0; x < layersAmount; x++)
        {
            for (int y = 0; y < pizzaSlices; y++)
            {
                newList.Add(new Vector2Int(x + 1, y * (x + 1)));
            }
        }

        return newList;
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