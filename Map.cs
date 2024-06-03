using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int SideSize_X;
    public int SideSize_Z;

    public GameObject MapCanvas;
    public GameObject ActiveHex;
    public GameObject ActiveObject;
    public GameObject MapController;
    public GameObject GameMapController;
    public GameObject HexMap;
    public GameObject MapPlayer;

    public Material Base;
    public Material Plains;
    public Material Forest;
    public Material Heights;
    public Material Mountain;
    public Material River;
    public Material Desert;
    public Material Swamp;


    public int TotalHexes;
    public float DeltaX = 93.75f;
    public float DeltaY = 108.25f;

    // Start is called before the first frame update
    void Start()
    {
        SideSize_X = MapController.GetComponent<GenerateMap>().SideSize_X;
        SideSize_Z = MapController.GetComponent<GenerateMap>().SideSize_Z;
        Base = MapController.GetComponent<GenerateMap>().Base;
        Plains = MapController.GetComponent<GenerateMap>().Plains;
        Forest = MapController.GetComponent<GenerateMap>().Forest;
        Heights = MapController.GetComponent<GenerateMap>().Heights;
        Mountain = MapController.GetComponent<GenerateMap>().Mountain;
        River = MapController.GetComponent<GenerateMap>().River;
        Desert = MapController.GetComponent<GenerateMap>().Desert;
        Swamp = MapController.GetComponent<GenerateMap>().Swamp;

        int HexType;
        int k = 0;
        

        for (int i = 0; i < SideSize_X * 2 + 1; i++)
        {
            if (k == 0)
            {
                for (int j = 0; j < SideSize_Z; j++)
                {
                    ActiveHex = GameObject.Find("Hex" + i + "." + j);
                    ActiveObject = Instantiate(HexMap, GameMapController.transform);
                    ActiveObject.name = ("MapHEX" + i + "." + j);
                    ActiveObject.transform.localRotation = Quaternion.Euler(0, -180, 90);
                    ActiveObject.transform.localPosition = new Vector3(((-SideSize_X * DeltaX) + i * (DeltaX)), DeltaY/2+(j * DeltaY) - SideSize_Z * DeltaY / 2, -10);
                    HexType = ActiveHex.GetComponent<HexData>().HexType;
                    switch (HexType)
                    {
                        case 0:
                            ActiveObject.GetComponent<MeshRenderer>().material = Base;
                            break;
                        case 1:
                            ActiveObject.GetComponent<MeshRenderer>().material = Plains;

                            break;
                        case 2:
                            ActiveObject.GetComponent<MeshRenderer>().material = Forest;
                            break;
                        case 3:
                            ActiveObject.GetComponent<MeshRenderer>().material = Heights;

                            break;
                        case 4:
                            ActiveObject.GetComponent<MeshRenderer>().material = Mountain;
                            break;
                        case 5:
                            ActiveObject.GetComponent<MeshRenderer>().material = River;
                            break;
                        case 6:
                            ActiveObject.GetComponent<MeshRenderer>().material = Swamp;
                            break;
                        case 7:
                            ActiveObject.GetComponent<MeshRenderer>().material = Desert;

                            break;
                    }
                }
                k++;
            }
            else
            {
                for (int j = 0; j < SideSize_Z + 1; j++)
                {
                    ActiveHex = GameObject.Find("Hex" + i + "." + j);
                    ActiveObject = Instantiate(HexMap, GameMapController.transform);
                    ActiveObject.name = ("MapHEX" + i + "." + j);
                    ActiveObject.transform.localRotation = Quaternion.Euler(0, -180, 90);
                    ActiveObject.transform.localPosition= new Vector3(((-SideSize_X * DeltaX) + i * (DeltaX)), DeltaY/2 + (j * DeltaY) - DeltaY / 2 - SideSize_Z * DeltaY / 2, -10);
                    HexType = ActiveHex.GetComponent<HexData>().HexType;
                    switch (HexType)
                    {
                        case 0:
                            ActiveObject.GetComponent<MeshRenderer>().material = Base;
                            break;
                        case 1:
                            ActiveObject.GetComponent<MeshRenderer>().material = Plains;

                            break;
                        case 2:
                            ActiveObject.GetComponent<MeshRenderer>().material = Forest;
                            break;
                        case 3:
                            ActiveObject.GetComponent<MeshRenderer>().material = Heights;

                            break;
                        case 4:
                            ActiveObject.GetComponent<MeshRenderer>().material = Mountain;
                            break;
                        case 5:
                            ActiveObject.GetComponent<MeshRenderer>().material = River;
                            break;
                        case 6:
                            ActiveObject.GetComponent<MeshRenderer>().material = Swamp;
                            break;
                        case 7:
                            ActiveObject.GetComponent<MeshRenderer>().material = Desert;

                            break;
                    }
                }
                k--;
            }
        }


    }

    public void ActivateMap()
    {
        ActiveObject = GameObject.Find("MapCanvas");
        ActiveObject.GetComponent<Canvas>().enabled = true;
        
    }
    public void DeActivateMap()
    {
        ActiveObject = GameObject.Find("MapCanvas");
        ActiveObject.GetComponent<Canvas>().enabled = false;

    }




    // Update is called once per frame
    void Update()
    {

    }
}
