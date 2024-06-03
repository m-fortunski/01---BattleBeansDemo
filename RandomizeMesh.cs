using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeMesh : MonoBehaviour
{
    public GameObject MapController;

    //Settings - from main script
    public bool DebugEnabled;
    public float DlSredHexa = 250f;
    public float PromienSpawnu = 250/2;
    public float HexDeltaX = 187.494f;
    public float HexDeltaZ = 216.506f;
    public int SideSize_X = 6;
    public int SideSize_Z = 6;

    //objects
    public GameObject Tree1;
    public GameObject Tree2;
    public GameObject Tree3;
    public GameObject Tree4;
    public GameObject Tree5;
    public GameObject Tree6;
    public GameObject Tree7;
    public GameObject Tree8;
    public GameObject Tree9;
    public GameObject Tree10;
    public GameObject TreeDry1;
    public GameObject TreeSwamp1;
    public GameObject TreeSwamp2;
    public GameObject TreeSwamp3;
    public GameObject Cactus1;
    public GameObject Cactus2;
    public GameObject Cactus3;
    public GameObject WaterPlants1;
    public GameObject WaterPlants2;
    public GameObject WaterPlants3;
    public GameObject WaterPlants4;
    public GameObject WaterPlants5;

    public GameObject Bush1;
    public GameObject Bush2;
    public GameObject Bush3;
    public GameObject Bush4;
    public GameObject Bush5;

    public GameObject Rock1;
    public GameObject Rock2;
    public GameObject Rock3;
    public GameObject Rock4;
    public GameObject Rock5;

    public int TreeCount=0;
    public int BushCount = 0;
    public int RockCount = 0;
    public int CactusCount = 0;
    public int WaterPlantsCount = 0;
    public GameObject RandomizedItem;
    public GameObject SpawnedObject;

    //Player controller
    public float scaleX = 1f;
    public float scaleY = 1f;
    public float invertscaleX = 1f;
    public float invertscaleY = 1f;

    //From hex data
    public float WysokoscTerenuMin = -5;
    public float WysokoscTerenuMax = 10;

    Transform Child;
    public GameObject ActiveHex;
    public GameObject ActiveObject;
    public GameObject ObjectChild;
    public GameObject HitObject;
    public Vector3[] vertPositions;
    public Vector3[] vertices;
    public float deltaProsteZ=15.625f;
    public float deltaDzikieX=13.53167f;
    public float deltaDzikieZ=7.81252f;
    public int LiczbaPktNaScianieP = 9;
    Renderer Renderer;

    Vector3 RandomizeCoordinates(float BaseX, float BaseZ)
    {
        float RandX = Random.Range(-PromienSpawnu, PromienSpawnu);
        float RandZ = Random.Range(-PromienSpawnu, PromienSpawnu);
        RandX = RandX + BaseX;
        RandZ = RandZ + BaseZ;
        Vector3 Position = new Vector3(RandX, 50, RandZ);
        return Position;
    }
    int SearchForVertCoords(float x, float y)
    {
        if (DebugEnabled == true) { Debug.Log("Searching for vert ..."+" X:"+x+" Y:"+y); }
        int j=0;
        for (int i = 0; i < vertPositions.Length - 1; i++)
        {
            if ((Mathf.Abs(vertPositions[i].x-x)<1f)&& (Mathf.Abs(vertPositions[i].y - y) < 1f)) { j = i; if (DebugEnabled == true) { Debug.Log("Vert found!"); } break;}
        }
        return j;
    }
    void RandomMesh(Mesh mesh)
    {
        if (DebugEnabled == true) { Debug.Log("Randomizing mesh..."); }
        //randomizuj
        for (int i = 0; i < vertPositions.Length-1; i++)
        {
            vertPositions[i].z = vertPositions[i].z + Random.Range(WysokoscTerenuMin, WysokoscTerenuMax); 
        }


        if (DebugEnabled == true) { Debug.Log("Setting border heights..."); }
        //granice na wysokoœæ 0
        float x = -DlSredHexa * 0.4330128f;
        float y = (-DlSredHexa / 4f)-((DlSredHexa/2)/(LiczbaPktNaScianieP-1));
        int k;

        for (int l = 0; l < LiczbaPktNaScianieP; l++) {
            y = y + deltaProsteZ;
            k = SearchForVertCoords(x,y);
            vertPositions[k].z = 0;
            if (DebugEnabled == true) { Debug.Log("1:" + l + " X=" + x + " Y=" + y); }

        }
        for (int l = 0; l < LiczbaPktNaScianieP-1; l++)
        {
            x = x + deltaDzikieX;
            y = y + deltaDzikieZ;
            k = SearchForVertCoords(x, y);
            vertPositions[k].z = 0;
            if (DebugEnabled == true) { Debug.Log("2:" + l + " X=" + x + " Y=" + y); }
        }
        for (int l = 0; l < LiczbaPktNaScianieP-1; l++)
        {
            x = x + deltaDzikieX;
            y = y - deltaDzikieZ;
            k = SearchForVertCoords(x, y);
            vertPositions[k].z = 0;
            if (DebugEnabled == true) { Debug.Log("3:" + l + " X=" + x + " Y=" + y); }
        }
        for (int l = 0; l < LiczbaPktNaScianieP - 1; l++)
        {
            y = y - deltaProsteZ;
            k = SearchForVertCoords(x, y);
            vertPositions[k].z = 0;
            if (DebugEnabled == true) { Debug.Log("4:" + l + " X=" + x + " Y=" + y); }
        }
        for (int l = 0; l < LiczbaPktNaScianieP - 1; l++)
        {
            x = x - deltaDzikieX;
            y = y - deltaDzikieZ;
            k = SearchForVertCoords(x, y);
            vertPositions[k].z = 0;
            if (DebugEnabled == true) { Debug.Log("5:" + l + " X=" + x + " Y=" + y); }

        }
        for (int l = 0; l < LiczbaPktNaScianieP - 1; l++)
        {
            x = x - deltaDzikieX;
            y = y + deltaDzikieZ;
            k = SearchForVertCoords(x, y);
            vertPositions[k].z = 0;
            if (DebugEnabled == true) { Debug.Log("6:" + l + " X=" + x + " Y=" + y); }
        }



        ActiveObject.GetComponent<MeshFilter>().mesh.vertices = vertPositions;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        ActiveObject.GetComponent<MeshCollider>().sharedMesh = ActiveObject.GetComponent<MeshFilter>().mesh;
    }
    void CreateRiver(Mesh mesh)
    {
        float x, y1, y2, RiverHeight;
        bool nieparzyste;
        int k;
        if (DebugEnabled == true) { Debug.Log("Creating river on mesh..."); }
        //Generuj rzekê
        nieparzyste = false;
        RiverHeight = -10;
        y1 = 0;
        x = -DlSredHexa * 0.4330128f;
        for (int i = 0; i < LiczbaPktNaScianieP; i++)
        {
            k = SearchForVertCoords(x, y1);
            vertPositions[k].z = vertPositions[k].z + RiverHeight;
            x = x + deltaDzikieX * 2;
        }
        y1 = y1 + deltaDzikieZ;
        y2 = y1 - 2 * deltaDzikieZ;

  

        for (int l = 0; l < 2; l++)
        {
            if (nieparzyste == true)
            {
                x = -DlSredHexa * 0.4330128f;
                for (int i = 0; i < LiczbaPktNaScianieP; i++)
                {
                    k = SearchForVertCoords(x, y1);
                    vertPositions[k].z = vertPositions[k].z + RiverHeight;
                    x = x + deltaDzikieX * 2;
                }

                x = -DlSredHexa * 0.4330128f;
                for (int i = 0; i < LiczbaPktNaScianieP; i++)
                {
                    k = SearchForVertCoords(x, y2);
                    vertPositions[k].z = vertPositions[k].z + RiverHeight;
                    x = x + deltaDzikieX * 2;
                }
                y1 = y1 + deltaDzikieZ;
                y2 = y2 - deltaDzikieZ;
                nieparzyste = false;
               
            }
            else
            {
                x = -DlSredHexa * 0.4330128f + deltaDzikieX;
                for (int i = 0; i < LiczbaPktNaScianieP - 1; i++)
                {
                    k = SearchForVertCoords(x, y1);
                    vertPositions[k].z = vertPositions[k].z + RiverHeight;
                    x = x + deltaDzikieX * 2;
                }
                x = -DlSredHexa * 0.4330128f + deltaDzikieX;
                for (int i = 0; i < LiczbaPktNaScianieP - 1; i++)
                {
                    k = SearchForVertCoords(x, y2);
                    vertPositions[k].z = vertPositions[k].z + RiverHeight;
                    x = x + deltaDzikieX * 2;
                }
                y1 = y1 + deltaDzikieZ;
                y2 = y2 - deltaDzikieZ;
                nieparzyste = true;
            }
        }
        x = -DlSredHexa * 0.4330128f + deltaDzikieX;
        for (int i = 0; i < LiczbaPktNaScianieP - 1; i++)
        {
            k = SearchForVertCoords(x, y1);
            vertPositions[k].z = vertPositions[k].z + RiverHeight / 2;
            x = x + deltaDzikieX * 2;
        }
        x = -DlSredHexa * 0.4330128f + deltaDzikieX;
        for (int i = 0; i < LiczbaPktNaScianieP - 1; i++)
        {
            k = SearchForVertCoords(x, y2);
            vertPositions[k].z = vertPositions[k].z + RiverHeight / 2;
            x = x + deltaDzikieX * 2;
        }
        ActiveObject.GetComponent<MeshFilter>().mesh.vertices = vertPositions;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        ActiveObject.GetComponent<MeshCollider>().sharedMesh = ActiveObject.GetComponent<MeshFilter>().mesh;
    }
    void RandomizeHex()
    {
        if (DebugEnabled == true) { Debug.Log("Randomizing hex..."); }
        ActiveObject.AddComponent<MeshCollider>();
        Mesh mesh1 = ActiveObject.GetComponent<MeshFilter>().mesh;
        vertPositions = ActiveObject.GetComponent<MeshFilter>().mesh.vertices;
        int HexType = ActiveHex.GetComponent<HexData>().HexType;
        WysokoscTerenuMin = ActiveHex.GetComponent<HexData>().TerrainHeightMin;
        WysokoscTerenuMax = ActiveHex.GetComponent<HexData>().TerrainHeightMax;
        int TreeCount = ActiveHex.GetComponent<HexData>().NumberOfTrees;
        int BushCount = ActiveHex.GetComponent<HexData>().NumberOfBushes;
        int RockCount = ActiveHex.GetComponent<HexData>().NumberOfRocks;
        int WaterPlantsCount = ActiveHex.GetComponent<HexData>().NumberOfWaterPlants;


        RandomMesh(mesh1);
        if (HexType == 5)
        {
            CreateRiver(mesh1);
            SpawnObjects(3, WaterPlantsCount);
        }
        if (HexType == 6)
        {
            SpawnObjects(3, WaterPlantsCount);
        }
        SpawnObjects(2, RockCount);
        SpawnObjects(1, BushCount);
        SpawnObjects(0, TreeCount);
    }
    void RandomizeTerrain()
    {
        int k=0;
        for (int i = 0; i < SideSize_X*2+1; i++)
        {
            if (k == 0)
            {
                for (int j = 0; j < SideSize_Z; j++)
                {
                    if (DebugEnabled == true) { Debug.Log("Hex: "+i+"."+j); }
                    ActiveHex = GameObject.Find("Hex" + i + "." + j);
                    ActiveObject = GameObject.Find("BaseHex" + i + "." + j);
                    RandomizeHex();
                }
                k++;
            }
            else
            {
                for (int j = 0; j < SideSize_Z+1; j++)
                {
                    if (DebugEnabled == true) { Debug.Log("Hex: " + i + "." + j); }
                    ActiveHex = GameObject.Find("Hex" + i + "." + j);
                    ActiveObject = GameObject.Find("BaseHex" + i + "." + j);
                    RandomizeHex();
                }
                k--;
            }
        }


    }
    void SpawnObjects(int ItemType, int ItemCount)
    {
        int c = 0;
        string ObjectName = "";
        if (ItemType != 3)
        {
            for (int i = 0; i < ItemCount; i++)
            {
                int j = 0;
                //0 - drzewa, 1 - krzaki, 2 - kamienie
                if (ItemType == 0)
                {
                    if (DebugEnabled == true) { Debug.Log("Trees"); }
                    ObjectName = "Tree";
                    TreeCount = TreeCount + 1;
                    c = TreeCount;
                    if (ActiveHex.GetComponent<HexData>().HexType == 7)
                    {
                        RandomizedItem = TreeDry1;
                    }
                    else if (ActiveHex.GetComponent<HexData>().HexType == 6)
                    {
                        j = Random.Range(1, 4);
                        switch (j)
                        {
                            case 1: RandomizedItem = TreeSwamp1; break;
                            case 2: RandomizedItem = TreeSwamp2; break;
                            case 3: RandomizedItem = TreeSwamp3; break;
                        }

                    }
                    else
                    {
                        j = Random.Range(1, 11);
                        switch (j)
                        {

                            case 1: RandomizedItem = Tree1; break;
                            case 2: RandomizedItem = Tree2; break;
                            case 3: RandomizedItem = Tree3; break;
                            case 4: RandomizedItem = Tree4; break;
                            case 5: RandomizedItem = Tree5; break;
                            case 6: RandomizedItem = Tree6; break;
                            case 7: RandomizedItem = Tree7; break;
                            case 8: RandomizedItem = Tree8; break;
                            case 9: RandomizedItem = Tree7; break;
                            case 10: RandomizedItem = Tree8; break;
                        }
                    }


                }
                else if (ItemType == 1)
                {
                    if (DebugEnabled == true) { Debug.Log("Bushes"); }
                    if (ActiveHex.GetComponent<HexData>().HexType == 7)
                    {
                        j = Random.Range(1, 4);
                        ObjectName = "Cactus";
                        CactusCount = CactusCount + 1;
                        c = BushCount;
                        switch (j)
                        {
                            case 1: RandomizedItem = Cactus1; break;
                            case 2: RandomizedItem = Cactus2; break;
                            case 3: RandomizedItem = Cactus3; break;

                        }
                    }
                    else
                    {
                        j = Random.Range(1, 6);
                        ObjectName = "Bush";
                        BushCount = BushCount + 1;
                        c = BushCount;
                        switch (j)
                        {
                            case 1: RandomizedItem = Bush1; break;
                            case 2: RandomizedItem = Bush2; break;
                            case 3: RandomizedItem = Bush3; break;
                            case 4: RandomizedItem = Bush4; break;
                            case 5: RandomizedItem = Bush5; break;
                        }
                    }
                }
                else
                {
                    if (DebugEnabled == true) { Debug.Log("Rocks"); }
                    j = Random.Range(1, 6);
                    ObjectName = "Rock";
                    RockCount = RockCount + 1;
                    c = RockCount;
                    switch (j)
                    {
                        case 1: RandomizedItem = Rock1; break;
                        case 2: RandomizedItem = Rock2; break;
                        case 3: RandomizedItem = Rock3; break;
                        case 4: RandomizedItem = Rock4; break;
                        case 5: RandomizedItem = Rock5; break;
                    }
                }


                SpawnObject(RandomizedItem, ActiveHex.GetComponent<HexData>().HexX, ActiveHex.GetComponent<HexData>().HexZ);
                SpawnedObject.name = ObjectName + "." + c;
                if (ItemType != 1 || ActiveHex.GetComponent<HexData>().HexType == 7)
                {
                    if (SpawnedObject.transform.childCount > 0)
                    {
                        for (int k = 0; k < SpawnedObject.transform.childCount; k++)
                        {
                            ObjectChild = SpawnedObject.transform.GetChild(k).gameObject;
                            ObjectChild.AddComponent<MeshCollider>();
                            ObjectChild.GetComponent<MeshCollider>().sharedMesh = ObjectChild.GetComponent<MeshFilter>().sharedMesh;

                        }
                    }
                    else
                    {

                        SpawnedObject.AddComponent<MeshCollider>();
                        SpawnedObject.GetComponent<MeshCollider>().sharedMesh = SpawnedObject.GetComponent<MeshFilter>().sharedMesh;
                    }
                }
                SpawnedObject.transform.parent = ActiveHex.transform;

            }
        }
        else
        {
            for (int i = 0; i < ItemCount; i++)
            {
                int j = 0;

                    if (DebugEnabled == true) { Debug.Log("Water plants"); }
                    j = Random.Range(1, 6);
                    ObjectName = "WaterPlant";
                    WaterPlantsCount = WaterPlantsCount + 1;
                    c = WaterPlantsCount;
                    switch (j)
                    {
                        case 1: RandomizedItem = WaterPlants1; break;
                        case 2: RandomizedItem = WaterPlants2; break;
                        case 3: RandomizedItem = WaterPlants3; break;
                        case 4: RandomizedItem = WaterPlants4; break;
                        case 5: RandomizedItem = WaterPlants5; break;
                    }


                SpawnWaterObject(RandomizedItem, ActiveHex.GetComponent<HexData>().HexX, ActiveHex.GetComponent<HexData>().HexZ);
                SpawnedObject.name = ObjectName + "." + c;
                SpawnedObject.transform.parent = ActiveHex.transform;
            }
        }
    }
    void SpawnObject(GameObject Item, float BaseX, float BaseZ)
    {
        Renderer = ActiveHex.GetComponent<MeshRenderer>();
        if (DebugEnabled == true){Debug.Log("Spawning...");}
        for (int i = 0; i < 2; i++)
        {
            Ray landingRay = new Ray(RandomizeCoordinates(BaseX, BaseZ), Vector3.down);
            RaycastHit hit;
            Physics.Raycast(landingRay, out hit, 75);
            if (hit.collider != null) { HitObject = hit.transform.gameObject; }
            else { HitObject = ActiveHex; }
            if (HitObject.name==("Base"+ActiveHex.name))
            {
                if (DebugEnabled == true){ Debug.Log(hit.point.ToString()); }
                SpawnedObject = Instantiate(Item, hit.point, Quaternion.Euler(0, Random.Range(0, 360), 0));

                HitObject = null;
                break;
            }
            else { i--; if (DebugEnabled == true) { Debug.Log("Brak spawnu"); } }
        }
    }
    void SpawnWaterObject(GameObject Item, float BaseX, float BaseZ)
    {
        if (DebugEnabled == true) { Debug.Log("Spawning..."); }
        for (int i = 0; i < 2; i++)
        {
            //WA¯NE - W RAYCASTACH X, Y(czyli WYSOKOŒÆ), Z (!)
            Ray landingRay1 = new Ray(RandomizeCoordinates(BaseX, BaseZ), Vector3.down);
            RaycastHit hit1;
            Physics.Raycast(landingRay1, out hit1, 60);
            if (hit1.collider != null) { HitObject = hit1.transform.gameObject; }
            else { HitObject = ActiveHex; }
            if (HitObject.name == ("Water" + ActiveHex.name))
            {
                Ray landingRay2 = new Ray(new Vector3(hit1.point.x,hit1.point.y-0.05f,hit1.point.z), Vector3.down);
                RaycastHit hit2;
                Physics.Raycast(landingRay2, out hit2, 10);
                if (hit2.collider != null) { HitObject = hit2.transform.gameObject; }
                else { HitObject = ActiveHex; }
                if ((hit1.point.y - hit2.point.y<2.5f))
                {
                    if (DebugEnabled == true) { Debug.Log(hit1.point.ToString()); }
                    SpawnedObject = Instantiate(Item, hit1.point, Quaternion.Euler(0, Random.Range(0, 360), 0));


                    HitObject = null;
                    break;
                }
                else { i--; if (DebugEnabled == true) { Debug.Log("Brak spawnu"); } }

            }

            else { i--; if (DebugEnabled == true) { Debug.Log("Brak spawnu"); } }
        }
    }

    // Start is called before the first frame update
    void Start()

    {
        RandomizeTerrain();
        MapController.GetComponent<GenerateMap>().DisableWaterColliders();

    }
}
