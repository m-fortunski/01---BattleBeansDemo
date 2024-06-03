using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
using System.Net;
using System.Globalization;

public class GenerateMap : MonoBehaviour
{
    //objects
    public GameObject MapController;
    public GameObject Map;

    //Debug settings
    public bool DebugEnable;

    //Hexes settings
    public float HexLongDiameter = 250;
    public GameObject BaseHex;
    public GameObject HexRiver;
    public GameObject HexArea;
    public GameObject WaterHex;
    public float HexDeltaX = 187.494f;
    public float HexDeltaZ = 216.506f;
    public int SideSize_Z = 6;
    public int SideSize_X = 6;
    public int Base1_X = 1;
    public int Base1_Z = 3;
    public int Base2_X = 11;
    public int Base2_Z = 3;

    //Map settings
    public GameObject BorderX;
    public GameObject BorderXLong;
    public GameObject BorderZ;

    public int SideMountainsNumber = 3;
    public int SideHillsNumber = 3;
    public int SideForestNumber=10;
    public int SideSwampNumber=2;
    public int SideDesertNumber=2;

    public int BaseTreeCount = 0;
    public int BaseBushCount = 0;
    public int BaseRockCount = 0;
    public int PlainsTreeCount = 5;
    public int PlainsBushCount = 25;
    public int PlainsRockCount = 5;
    public int ForestTreeCount = 150;
    public int ForestBushCount = 150;
    public int ForestRockCount = 25;
    public float ForestWaterHeight = -2;
    public int HillsTreeCount = 10;
    public int HillsBushCount = 25;
    public int HillsRockCount = 100;
    public int MountainTreeCount = 10;
    public int MountainBushCount = 25;
    public int MountainRockCount = 100;
    public int SwampTreeCount = 50;
    public int SwampBushCount = 50;
    public int SwampRockCount = 5;
    public int SwampWaterPlantsCount = 500;
    public float SwampWaterHeight = -3;
    public int DesertTreeCount = 1;
    public int DesertBushCount =10;
    public int DesertRockCount = 10;
    public int RiverTreeCount = 25;
    public int RiverBushCount = 25;
    public int RiverRockCount = 10;
    public int RiverWaterPlantsCount = 500;
    public float RiverWaterHeight = -5;

    float WorldLimitsX, WorldLimitsZ;

    //Objects
    public GameObject Mountain1;
    public GameObject Mountain2;
    public GameObject Mountain3;
    public GameObject Mountain4;
    public GameObject Mountain5;

    //Materials
    public Material Base;
    public Material Plains;
    public Material Forest;
    public Material Heights;
    public Material Mountain;
    public Material River;
    public Material Desert;
    public Material Swamp;

    //Variables
    public GameObject ActiveHex;
    public GameObject ActiveObject;
    public GameObject NeighboringHex;
    public GameObject SpawnedObject;
    GameObject ObjectChild;
    Transform Child;
    Transform Rotation;
    Mesh ActiveHexMesh;
    Vector3 VectorCalculate;
    float ActiveX, ActiveZ;
    public Vector3 MapCenter;

    //Functions
    GameObject FindChild(GameObject parent, string name)
    {
        if (DebugEnable == true) { Debug.Log("Finding child "+name+"..."); }
        Transform trans = parent.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            if (DebugEnable == true) { Debug.Log("Child found!"); }
            return childTrans.gameObject;
        }
        else
        {
            if (DebugEnable == true) { Debug.Log("Child not found!"); }
                return null; 
        }

    }
    void NextHexX()
    {
        if (DebugEnable == true) { Debug.Log("NextHex() - Hex X++"); }
        ActiveX = ActiveX + HexDeltaX;
    }
    void SetHexType(int HexType)
    {
        if (DebugEnable== true) { Debug.Log("SetHexType(" + HexType + ")"); }
        ActiveHex.AddComponent<HexData>();
        ActiveHex.GetComponent<HexData>().HexType = HexType;
        switch (HexType)
        {
            case 0:
                ActiveHex.GetComponent<MeshRenderer>().material = Base;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 1f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -1f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = BaseTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = BaseBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = BaseRockCount;
                break;
            case 1:
                ActiveHex.GetComponent<MeshRenderer>().material = Plains;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 2.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -2.5f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = PlainsTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = PlainsBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = PlainsRockCount;
                break;
            case 2:
                ActiveHex.GetComponent<MeshRenderer>().material = Forest;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 2.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -2.5f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = ForestTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = ForestBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = ForestRockCount;
                ActiveHex.GetComponent<HexData>().WaterHeight = ForestWaterHeight;
                break;
            case 3:
                ActiveHex.GetComponent<MeshRenderer>().material = Heights;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 7.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -2.5f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = HillsTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = HillsBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = HillsRockCount;
                break;
            case 4:
                ActiveHex.GetComponent<MeshRenderer>().material = Mountain;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 10f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = 0f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = MountainTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = MountainBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = MountainRockCount;
                break;
            case 5:
                ActiveHex.GetComponent<MeshRenderer>().material = River;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 2.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -2.5f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = RiverTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = RiverBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = RiverRockCount;
                ActiveHex.GetComponent<HexData>().NumberOfWaterPlants = RiverWaterPlantsCount;
                ActiveHex.GetComponent<HexData>().WaterHeight = RiverWaterHeight;

                break;
            case 6:
                ActiveHex.GetComponent<MeshRenderer>().material = Swamp;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 2.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -5f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = SwampTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = SwampBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = SwampRockCount;
                ActiveHex.GetComponent<HexData>().NumberOfWaterPlants = SwampWaterPlantsCount;
                ActiveHex.GetComponent<HexData>().WaterHeight = SwampWaterHeight;
                break;
            case 7:
                ActiveHex.GetComponent<MeshRenderer>().material = Desert;
                ActiveObject.GetComponent<MeshRenderer>().material = Desert;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 2.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -1f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = DesertTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = DesertBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = DesertRockCount;
                break;
        }
    }
    void ChangeHexType(int HexType)
    {
        if (DebugEnable == true) { Debug.Log("ChangeHexType(" + HexType + ")"); }
        ActiveHex.GetComponent<HexData>().HexType = HexType;
        switch (HexType)
        {
            case 0:
                ActiveHex.GetComponent<MeshRenderer>().material = Base;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 1f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -1f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = BaseTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = BaseBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = BaseRockCount;
                break;
            case 1:
                ActiveHex.GetComponent<MeshRenderer>().material = Plains;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 2.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -2.5f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = PlainsTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = PlainsBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = PlainsRockCount;
                break;
            case 2:
                ActiveHex.GetComponent<MeshRenderer>().material = Forest;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 2.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -2.5f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = ForestTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = ForestBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = ForestRockCount;
                ActiveHex.GetComponent<HexData>().WaterHeight = ForestWaterHeight;
                break;
            case 3:
                ActiveHex.GetComponent<MeshRenderer>().material = Heights;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 7.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -2.5f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = HillsTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = HillsBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = HillsRockCount;
                break;
            case 4:
                ActiveHex.GetComponent<MeshRenderer>().material = Mountain;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 10f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = 0f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = MountainTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = MountainBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = MountainRockCount;
                break;
            case 5:
                ActiveHex.GetComponent<MeshRenderer>().material = River;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 2.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -2.5f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = RiverTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = RiverBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = RiverRockCount;
                ActiveHex.GetComponent<HexData>().NumberOfWaterPlants = RiverWaterPlantsCount;
                ActiveHex.GetComponent<HexData>().WaterHeight = RiverWaterHeight;

                break;
            case 6:
                ActiveHex.GetComponent<MeshRenderer>().material = Swamp;
                ActiveObject.GetComponent<MeshRenderer>().material = Plains;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 2.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -5f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = SwampTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = SwampBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = SwampRockCount;
                ActiveHex.GetComponent<HexData>().NumberOfWaterPlants = SwampWaterPlantsCount;
                ActiveHex.GetComponent<HexData>().WaterHeight = SwampWaterHeight;
                break;
            case 7:
                ActiveHex.GetComponent<MeshRenderer>().material = Desert;
                ActiveObject.GetComponent<MeshRenderer>().material = Desert;
                ActiveHex.GetComponent<HexData>().TerrainHeightMax = 2.5f;
                ActiveHex.GetComponent<HexData>().TerrainHeightMin = -1f;
                ActiveHex.GetComponent<HexData>().NumberOfTrees = DesertTreeCount;
                ActiveHex.GetComponent<HexData>().NumberOfBushes = DesertBushCount;
                ActiveHex.GetComponent<HexData>().NumberOfRocks = DesertRockCount;
                break;
        }
    }
    void SetNeighboringHex(int Hex_X, int Hex_Z)
    {
        if (Hex_X < 0) { Hex_X = 0; }
        if (Hex_X >(SideSize_X*2)) { Hex_X = SideSize_X * 2; }
        if (Hex_Z < 0) { Hex_Z = 0; }
        if ((Hex_X%2==0)&&(Hex_Z>SideSize_Z-2)) { Hex_Z = SideSize_Z-1; }
        if ((Hex_X % 2 == 1) && (Hex_Z > SideSize_Z - 1)) { Hex_Z = SideSize_Z; }

        if (DebugEnable == true) { Debug.Log("SettingNeighboringHex(" + Hex_X + "." + Hex_Z + ")"); }
        NeighboringHex = GameObject.Find("Hex" + Hex_X + "." + Hex_Z);
    }
    int CheckIfNeighboringHexIsTheSameOrBase(int ActiveHexType, int OtherHexType)
    {
        int NeighboringHexType = NeighboringHex.GetComponent<HexData>().HexType;
        if ((ActiveHexType != NeighboringHexType) && (NeighboringHexType != OtherHexType)) { if (DebugEnable == true) { Debug.Log("CheckingIfNeighboringHexIsTheSame(0)"); } return 0; }
        else { if (DebugEnable == true) { Debug.Log("CheckingIfNeighboringHexIsTheSame(1)"); } return 1; }
    }
    int CheckNeighboringHexes(int ActiveHexType, int OtherHexType, int ActiveHex_X, int ActiveHex_Z)
    {
        int NeighboringHex_X;
        int NeighboringHex_Z;
        if (DebugEnable == true) { Debug.Log("CheckingNeighboringHexes..."); }
        int same = 0;

            NeighboringHex_X = ActiveHex_X - 1;
            if (ActiveHex_X % 2 == 1) { NeighboringHex_Z = ActiveHex_Z - 1; }
            else { NeighboringHex_Z = ActiveHex_Z; }
            SetNeighboringHex(NeighboringHex_X, NeighboringHex_Z);
            if (CheckIfNeighboringHexIsTheSameOrBase(ActiveHexType, OtherHexType) == 1) { same++; if (DebugEnable == true) { Debug.Log("1"); } }
            NeighboringHex_Z = NeighboringHex_Z + 1;
            SetNeighboringHex(NeighboringHex_X, NeighboringHex_Z);
            if (CheckIfNeighboringHexIsTheSameOrBase(ActiveHexType, OtherHexType) == 1) { same++; if (DebugEnable == true) { Debug.Log("2"); } }
            NeighboringHex_X = NeighboringHex_X + 1;
            if (ActiveHex_X % 2 == 1) { NeighboringHex_Z = NeighboringHex_Z + 1; }
            SetNeighboringHex(NeighboringHex_X, NeighboringHex_Z);
            if (CheckIfNeighboringHexIsTheSameOrBase(ActiveHexType, OtherHexType) == 1) { same++; if (DebugEnable == true) { Debug.Log("3"); } }
            NeighboringHex_X = NeighboringHex_X + 1;
            if (ActiveHex_X % 2 == 1) { NeighboringHex_Z = NeighboringHex_Z - 1; }
            SetNeighboringHex(NeighboringHex_X, NeighboringHex_Z);
            if (CheckIfNeighboringHexIsTheSameOrBase(ActiveHexType, OtherHexType) == 1) { same++; if (DebugEnable == true) { Debug.Log("4"); } }
            NeighboringHex_Z = NeighboringHex_Z - 1;
            SetNeighboringHex(NeighboringHex_X, NeighboringHex_Z);
            if (CheckIfNeighboringHexIsTheSameOrBase(ActiveHexType, OtherHexType) == 1) { same++; if (DebugEnable == true) { Debug.Log("5"); } }
            if (ActiveHex_Z>0) { NeighboringHex_X = NeighboringHex_X - 1; }
            if (ActiveHex_X % 2 == 0) { NeighboringHex_Z = NeighboringHex_Z - 1; }
            SetNeighboringHex(NeighboringHex_X, NeighboringHex_Z);
            if (CheckIfNeighboringHexIsTheSameOrBase(ActiveHexType, OtherHexType) == 1) { same++; if (DebugEnable == true) { Debug.Log("6"); } }
        return same;

    }
    double CalculateDistance(int posx1, int posx2, int posz1, int posz2)
    {
        int DistanceX = posx1 - posx2;
        int DistanceZ = posz1 - posz2;
        double distance = Mathf.Sqrt(Mathf.Pow(DistanceX, 2) + Mathf.Pow(DistanceZ, 2));
        if (DebugEnable == true) { Debug.Log("CalculatingDistance=" + distance); }
        return distance;
    }
    void SideGenerateBiomes(int SideX, int BaseX, int BaseZ, int BiomeType, int NumberOfHexes, int HexNotWanted1, int HexNotWanted2, int MinimalDistance)
    {
        if (DebugEnable == true) { Debug.Log("GeneratingBiomes..."); }
        int ActiveHexType;
        int pos_x = 0, pos_y = 0;
        for (int i = 0; i < NumberOfHexes;)
        {
            int proba = 0;
            int j = 0;
            while (j < 1)
            {
                pos_x = Random.Range((0+SideX), (SideSize_X + SideX));
                if (pos_x % 2 == 1) { pos_y = Random.Range(0, SideSize_Z + 1); if (DebugEnable == true) { Debug.Log("Z MAX = 7"); } }
                else { pos_y = Random.Range(0, SideSize_Z); }
                if (DebugEnable == true) { Debug.Log("NewPosition-" + pos_x + "." + pos_y); }
                ActiveHex = GameObject.Find("Hex" + pos_x + "." + pos_y);
                ActiveObject = FindChild(ActiveHex, "BaseHex" + pos_x + "." + pos_y);
                ActiveHexType = ActiveHex.GetComponent<HexData>().HexType;


                if ((CheckNeighboringHexes(HexNotWanted1, HexNotWanted2, pos_x, pos_y) < 1) && (ActiveHexType == 1) && (CalculateDistance(pos_x, BaseX, pos_y, BaseZ) >=MinimalDistance))
                {
                    if (DebugEnable == true) { Debug.Log("PositionOK"); }
                    j++;
                }
                else { if (DebugEnable == true) { Debug.Log("PositionNotOK"); } proba++; }
                if(proba == 100){ break; }
            }
            if (proba != 100)
            {
                ChangeHexType(BiomeType);
                if (BiomeType == 2 || BiomeType == 6)
                {
                    ActiveObject = Instantiate(WaterHex, new Vector3(ActiveHex.GetComponent<HexData>().HexX, ActiveHex.GetComponent<HexData>().WaterHeight, ActiveHex.GetComponent<HexData>().HexZ), Quaternion.Euler(-90, 0, 90));
                    ActiveObject.name = "WaterHex" + pos_x + "." + pos_y;
                    ActiveObject.AddComponent<MeshCollider>();
                    ActiveObject.GetComponent<MeshCollider>().sharedMesh = ActiveObject.GetComponent<MeshFilter>().sharedMesh;
                    ActiveObject.transform.parent = ActiveHex.transform;
                }
                if (BiomeType == 4)
                {
                    int n = Random.Range(0, 5);
                    switch (n)
                    {
                        case 0: SpawnedObject = Mountain1; break;
                        case 1: SpawnedObject = Mountain2; break;
                        case 2: SpawnedObject = Mountain3; break;
                        case 3: SpawnedObject = Mountain4; break;
                        case 4: SpawnedObject = Mountain5; break;
                    }
                    ActiveObject = Instantiate(SpawnedObject, new Vector3(ActiveHex.GetComponent<HexData>().HexX, 0, ActiveHex.GetComponent<HexData>().HexZ), Quaternion.Euler(0, Random.Range(0,360), 0));
                    ActiveObject.name = "MountainHex" + pos_x + "." + pos_y;
                    ActiveObject.transform.parent = ActiveHex.transform;
                    for (int k = 0; k < SpawnedObject.transform.childCount; k++)
                    {
                        ObjectChild = ActiveObject.transform.GetChild(k).gameObject;
                        ObjectChild.AddComponent<MeshCollider>();
                        ObjectChild.GetComponent<MeshCollider>().sharedMesh = ObjectChild.GetComponent<MeshFilter>().sharedMesh;
                    }
                }
                i++;
            }
            else { if (DebugEnable == true) { Debug.Log("ERROR! CANT FIND A POSITION!"); } break; }

        }
    }

    //PHYSICAL MAP
    void GenerateLeftSide()
        {
        if (DebugEnable == true) { Debug.Log("GeneratingLeftSide..."); }
            int k = 0;
            for (int i = 0; i < SideSize_X; i++)
            {
                if (k == 0)
                {
                    for (int j = 0; j < SideSize_Z; j++)
                    {
                        ActiveHex = Instantiate(HexArea, new Vector3(HexDeltaX * i-HexDeltaX*SideSize_X, -10, HexDeltaZ * j-HexDeltaZ*(SideSize_Z-1)/2), Quaternion.Euler(-90f, 0, 90));
                        ActiveHex.name = "Hex" + i + "." + j;
                        ActiveObject = Instantiate(BaseHex, new Vector3(HexDeltaX * i - HexDeltaX * SideSize_X, 0, HexDeltaZ * j - HexDeltaZ * (SideSize_Z - 1) / 2), Quaternion.Euler(-90f, 0, 90));
                        ActiveObject.name = "BaseHex" + i + "." + j;
                        ActiveHex.transform.parent = GameObject.Find("Hexes").transform;
                        ActiveObject.transform.parent = GameObject.Find("Hex" + i + "." + j).transform;
                        ActiveHex.GetComponent<MeshRenderer>().enabled = false;
                        SetHexType(1);
                        ActiveHex.GetComponent<HexData>().HexName = "Hex" + i + "." + j;
                        ActiveHex.GetComponent<HexData>().HexX = HexDeltaX * i - HexDeltaX * SideSize_X;
                        ActiveHex.GetComponent<HexData>().HexZ = HexDeltaZ * j - HexDeltaZ * (SideSize_Z - 1) / 2;
                }
                    k++;
                    NextHexX();
                }
                else
                {
                    for (int j = 0; j < SideSize_Z + 1; j++)
                    {
                        ActiveHex = Instantiate(HexArea, new Vector3(HexDeltaX * i - HexDeltaX * SideSize_X, 0, (HexDeltaZ * j) - (HexDeltaZ / 2) - HexDeltaZ * (SideSize_Z - 1) / 2), Quaternion.Euler(-90f,0, 90));
                        ActiveHex.name = "Hex" + i + "." + j;
                        ActiveObject = Instantiate(BaseHex, new Vector3(HexDeltaX * i - HexDeltaX * SideSize_X, 0, (HexDeltaZ * j) - (HexDeltaZ / 2) - HexDeltaZ * (SideSize_Z - 1) / 2), Quaternion.Euler(-90f, 0, 90));
                        ActiveObject.name = "BaseHex" + i + "." + j;
                        ActiveHex.transform.parent = GameObject.Find("Hexes").transform;
                        ActiveObject.transform.parent = GameObject.Find("Hex" + i + "." + j).transform;
                        ActiveHex.GetComponent<MeshRenderer>().enabled = false;
                        SetHexType(1);
                        ActiveHex.GetComponent<HexData>().HexName = "Hex" + i + "." + j;
                        ActiveHex.GetComponent<HexData>().HexX = HexDeltaX * i - HexDeltaX * SideSize_X;
                    ActiveHex.GetComponent<HexData>().HexZ = (HexDeltaZ * j) - (HexDeltaZ / 2) - HexDeltaZ * (SideSize_Z - 1) / 2;
                }
                    k--;
                    NextHexX();
                }

            }
        }
    void GenerateRiver()
        {
        if (DebugEnable == true) { Debug.Log("GeneratingRiver..."); }

        for (int i = 0; i < SideSize_Z; i++)
            {
            ActiveHex = Instantiate(HexArea, new Vector3(0, 0, HexDeltaZ * i - HexDeltaZ * (SideSize_Z - 1) / 2), Quaternion.Euler(-90f, 0, 90));
            ActiveHex.name = "Hex" + (SideSize_X) + "." + i;
            ActiveObject = Instantiate(BaseHex, new Vector3(0, 0, HexDeltaZ * i - HexDeltaZ * (SideSize_Z - 1) / 2), Quaternion.Euler(-90f, 0, 90));
            ActiveObject.name = "BaseHex" + (SideSize_X) + "." + i;
            ActiveHex.transform.parent = GameObject.Find("Hexes").transform;
            ActiveObject.transform.parent = GameObject.Find("Hex" + (SideSize_X) + "." + i).transform;
            ActiveHex.GetComponent<MeshRenderer>().enabled = false;
            SetHexType(5);
            ActiveHex.GetComponent<HexData>().HexName = "Hex" + SideSize_X + "." + i;
            ActiveHex.GetComponent<HexData>().HexX = 0;
            ActiveHex.GetComponent<HexData>().HexZ = HexDeltaZ * i - HexDeltaZ * (SideSize_Z - 1) / 2;
            ActiveObject = Instantiate(WaterHex, new Vector3(ActiveHex.GetComponent<HexData>().HexX, ActiveHex.GetComponent<HexData>().WaterHeight, ActiveHex.GetComponent<HexData>().HexZ), Quaternion.Euler(-90, 0, 90));
            ActiveObject.name = "WaterHex" + (SideSize_X) + "." + i;
            ActiveObject.AddComponent<MeshCollider>();
            ActiveObject.GetComponent<MeshCollider>().sharedMesh = ActiveObject.GetComponent<MeshFilter>().sharedMesh;
            ActiveObject.transform.parent = ActiveHex.transform;
        }
        MapCenter = new Vector3(ActiveX,0,HexDeltaZ*(SideSize_Z-1)/2);
            NextHexX();
            
        }
    void GenerateRightSide()
        {
        if (DebugEnable == true) { Debug.Log("GeneratingRightSide..."); }
        int k = 1;
            for (int i = 0; i < SideSize_X; i++)
            {
                if (k == 0)
                {
                    for (int j = 0; j < SideSize_Z; j++)
                    {
                        ActiveHex = Instantiate(HexArea, new Vector3(HexDeltaX * (i+1), 0, HexDeltaZ * j - HexDeltaZ * (SideSize_Z - 1) / 2), Quaternion.Euler(-90f, 0, 90));
                        ActiveHex.name = "Hex" + (i + SideSize_X+1) + "." + j;
                        ActiveObject = Instantiate(BaseHex, new Vector3(HexDeltaX * (i + 1), 0, HexDeltaZ * j - HexDeltaZ * (SideSize_Z - 1) / 2), Quaternion.Euler(-90f, 0, 90));
                        ActiveObject.name = "BaseHex" + (i + SideSize_X + 1) + "." + j;
                        ActiveHex.transform.parent = GameObject.Find("Hexes").transform;
                        ActiveObject.transform.parent = GameObject.Find("Hex" + (i + SideSize_X + 1) + "." + j).transform;
                        ActiveHex.GetComponent<MeshRenderer>().enabled = false;
                        SetHexType(1);
                        ActiveHex.GetComponent<HexData>().HexName = "Hex" + (i + SideSize_X + 1) + "." + j;
                        ActiveHex.GetComponent<HexData>().HexX = HexDeltaX * (i + 1);
                        ActiveHex.GetComponent<HexData>().HexZ = HexDeltaZ * j - HexDeltaZ * (SideSize_Z - 1) / 2;
                    }
                    k++;
                }
                else
                {
                    for (int j = 0; j < SideSize_Z + 1; j++)
                    {
                        ActiveHex = Instantiate(HexArea, new Vector3(HexDeltaX * (i + 1), 0, (HexDeltaZ * j) - (HexDeltaZ / 2) - HexDeltaZ * (SideSize_Z - 1) / 2), Quaternion.Euler(-90f, 0, 90));
                        ActiveHex.name = "Hex" + (i + SideSize_X+1) + "." + j;
                        ActiveObject = Instantiate(BaseHex, new Vector3(HexDeltaX * (i+1), 0, (HexDeltaZ * j) - (HexDeltaZ / 2) - HexDeltaZ * (SideSize_Z - 1) / 2), Quaternion.Euler(-90f, 0, 90));
                        ActiveObject.name = "BaseHex" + (i + SideSize_X + 1) + "." + j;
                        ActiveHex.transform.parent = GameObject.Find("Hexes").transform;
                        ActiveObject.transform.parent = GameObject.Find("Hex" + (i + SideSize_X + 1) + "." + j).transform;
                        ActiveHex.GetComponent<MeshRenderer>().enabled = false;
                        SetHexType(1);
                        ActiveHex.GetComponent<HexData>().HexName = "Hex" + (i + SideSize_X + 1) + "." + j;
                        ActiveHex.GetComponent<HexData>().HexX = HexDeltaX * (i + 1);
                        ActiveHex.GetComponent<HexData>().HexZ = (HexDeltaZ * j) - (HexDeltaZ / 2) - HexDeltaZ * (SideSize_Z - 1) / 2;
                    }
                    k--;
                }

            }
        }
    void GenerateBorders()
        {
        if (DebugEnable == true) { Debug.Log("GeneratingBorders..."); }
        float pos_x = 0, pos_z = 0, delta_x = 187.5f, delta_z = 216.5f;
            int k = 0;
            pos_x = GameObject.Find("Hex0.0").GetComponent<HexData>().HexX; pos_z = GameObject.Find("Hex0.0").GetComponent<HexData>().HexZ-HexDeltaZ/2;
            for (int i = 0; i < SideSize_X * 2 + 1; i++)
            {
                if (k == 0) { ActiveObject=Instantiate(BorderX, new Vector3(pos_x, 25, pos_z), Quaternion.Euler(-90f, 0, 90)); ActiveObject.transform.parent = GameObject.Find("WorldBorders").transform; k++; ActiveObject.AddComponent<MeshCollider>();ActiveObject.GetComponent<MeshCollider>().sharedMesh = ActiveObject.GetComponent<MeshFilter>().sharedMesh;
            }
                else { ActiveObject=Instantiate(BorderXLong, new Vector3(pos_x, 25, pos_z), Quaternion.Euler(-90f, 0, 90)); ActiveObject.transform.parent = GameObject.Find("WorldBorders").transform; k--; ActiveObject.AddComponent<MeshCollider>(); ActiveObject.GetComponent<MeshCollider>().sharedMesh = ActiveObject.GetComponent<MeshFilter>().sharedMesh; }
                pos_x = pos_x + delta_x;
            }
            k = 0;
            pos_x = GameObject.Find("Hex0."+(SideSize_Z-1)).GetComponent<HexData>().HexX; pos_z = GameObject.Find("Hex0." + (SideSize_Z - 1)).GetComponent<HexData>().HexZ + HexDeltaZ / 2;
            for (int i = 0; i < SideSize_X * 2 + 1; i++)
            {
                if (k == 0) { ActiveObject=Instantiate(BorderX, new Vector3(pos_x, 25, pos_z), Quaternion.Euler(-90f, 0, 90)); ActiveObject.transform.parent = GameObject.Find("WorldBorders").transform; k++; ActiveObject.AddComponent<MeshCollider>(); ActiveObject.GetComponent<MeshCollider>().sharedMesh = ActiveObject.GetComponent<MeshFilter>().sharedMesh; }
                else { ActiveObject=Instantiate(BorderXLong, new Vector3(pos_x, 25, pos_z), Quaternion.Euler(-90f, 0, 90)); ActiveObject.transform.parent = GameObject.Find("WorldBorders").transform; k--; ActiveObject.AddComponent<MeshCollider>(); ActiveObject.GetComponent<MeshCollider>().sharedMesh = ActiveObject.GetComponent<MeshFilter>().sharedMesh; }
                pos_x = pos_x + delta_x;
            }

            pos_x = GameObject.Find("Hex0.0").GetComponent<HexData>().HexX-HexLongDiameter/4; pos_z = GameObject.Find("Hex0.0").GetComponent<HexData>().HexZ;
            for (int i = 0; i < SideSize_Z; i++)
            {
                ActiveObject=Instantiate(BorderZ, new Vector3(pos_x, 25, pos_z), Quaternion.Euler(-90f, 0, 0)); ActiveObject.transform.parent = GameObject.Find("WorldBorders").transform; k++;
            ActiveObject.AddComponent<MeshCollider>(); ActiveObject.GetComponent<MeshCollider>().sharedMesh = ActiveObject.GetComponent<MeshFilter>().sharedMesh;
            pos_z = pos_z + delta_z; 
            }
            pos_x = GameObject.Find("Hex"+(2*SideSize_X)+".0").GetComponent<HexData>().HexX + HexLongDiameter / 4; pos_z = GameObject.Find("Hex" + (2 * SideSize_X) + ".0").GetComponent<HexData>().HexZ;
            for (int i = 0; i < SideSize_Z; i++)
            {
                ActiveObject=Instantiate(BorderZ, new Vector3(pos_x, 25, pos_z), Quaternion.Euler(-90f, 0, 0)); ActiveObject.transform.parent = GameObject.Find("WorldBorders").transform; k++; ActiveObject.AddComponent<MeshCollider>(); ActiveObject.GetComponent<MeshCollider>().sharedMesh = ActiveObject.GetComponent<MeshFilter>().sharedMesh;
            pos_z = pos_z + delta_z; 
            }
        }

    public void DisableWaterColliders()
    {
        int HexType;
        int k = 0;
        for (int i = 0; i < SideSize_X * 2 + 1; i++)
        {
            if (k == 0)
            {
                for (int j = 0; j < SideSize_Z; j++)
                {
                    ActiveHex = GameObject.Find("Hex" + i + "." + j);
                    HexType = ActiveHex.GetComponent<HexData>().HexType;
                    if (HexType==2||HexType==5||HexType==6)
                    {
                        ActiveObject = GameObject.Find("Water" + "Hex" + i + "." + j);
                        ActiveObject.GetComponent<MeshCollider>().enabled = false;
                    }
                }
                k++;
            }
            else
            {
                for (int j = 0; j < SideSize_Z + 1; j++)
                {
                    ActiveHex = GameObject.Find("Hex" + i + "." + j);
                    HexType = ActiveHex.GetComponent<HexData>().HexType;
                    if (HexType == 2 || HexType == 5 || HexType == 6)
                    {
                        ActiveObject = GameObject.Find("Water" + "Hex" + i + "." + j);
                        ActiveObject.GetComponent<MeshCollider>().enabled = false;
                    }
                }
                k--;
            }
        }


    }


    //SET HEXES
    void SetHexesLeft()
        {
        //set base
        if (DebugEnable == true) { Debug.Log("SettingBase..."); }
        ActiveHex = GameObject.Find("Hex" + Base1_X + "." + Base1_Z);
        ActiveObject = FindChild(ActiveHex, "BaseHex" + Base1_X + "." + Base1_Z);
        int ActiveHexType = ActiveHex.GetComponent<HexData>().HexType;
        ChangeHexType(0);

        //SET MOUNTAINS
        if (DebugEnable == true) { Debug.Log("SettingMountains..."); }
        SideGenerateBiomes(0,Base1_X, Base1_Z,4, SideMountainsNumber, 4, 5,3);

        //set hills
        SideGenerateBiomes(0, Base1_X, Base1_Z, 3, SideHillsNumber, 3, 5,3);

        //SET FORESTS
        SideGenerateBiomes(0, Base1_X, Base1_Z, 2, SideForestNumber, 0, 0, 2);

        //set swamps
        SideGenerateBiomes(0, Base1_X, Base1_Z, 6, SideSwampNumber, 6, 5,2);
        //set deserts
        SideGenerateBiomes(0, Base1_X, Base1_Z, 7, SideDesertNumber, 7, 0,2);
    }
    void SetHexesRight()
        {
        //set base
        if (DebugEnable == true) { Debug.Log("SettingBase..."); }
        ActiveHex = GameObject.Find("Hex" + Base2_X + "." + Base2_Z);
        ActiveObject = FindChild(ActiveHex, "BaseHex" + Base2_X + "." + Base2_Z);
        int ActiveHexType = ActiveHex.GetComponent<HexData>().HexType;
        ChangeHexType(0);

        //SET MOUNTAINS
        if (DebugEnable == true) { Debug.Log("SettingMountains..."); }
        SideGenerateBiomes(SideSize_X+1, Base2_X, Base2_Z, 4, SideMountainsNumber, 4, 5, 3);

        //set hills
        SideGenerateBiomes(SideSize_X+1, Base2_X, Base2_Z, 3, SideHillsNumber, 3, 5, 3);

        //SET FORESTS
        SideGenerateBiomes(SideSize_X+1, Base2_X, Base2_Z, 2, SideForestNumber, 0, 0, 2);

        //set swamps
        SideGenerateBiomes(SideSize_X+1, Base2_X, Base2_Z, 6, SideSwampNumber, 6, 5, 2);
        //set deserts
        SideGenerateBiomes(SideSize_X+1, Base2_X, Base2_Z, 7, SideDesertNumber, 7, 0, 2);
    }
    void Start()
    {
        MapController = GameObject.Find("MapController");
        Map = GameObject.Find("MapCanvas");
        GenerateLeftSide();
        GenerateRiver();
        GenerateRightSide();
        GenerateBorders();
        SetHexesLeft();
        SetHexesRight();
        MapController.GetComponent<RandomizeMesh>().SideSize_X = SideSize_X;
        MapController.GetComponent<RandomizeMesh>().SideSize_Z = SideSize_Z;
        MapController.GetComponent<RandomizeMesh>().HexDeltaX = HexDeltaX;
        MapController.GetComponent<RandomizeMesh>().HexDeltaZ = HexDeltaZ;
        MapController.GetComponent<RandomizeMesh>().DlSredHexa = HexLongDiameter;
        MapController.GetComponent<RandomizeMesh>().DebugEnabled = DebugEnable;
        MapController.GetComponent<RandomizeMesh>().enabled = true;
        Map.GetComponent<Canvas>().enabled = false;
        MapController.GetComponent<TimeController>().MapCenter = MapCenter;
    }

}