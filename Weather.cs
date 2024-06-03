using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public GameObject MapController;
    public bool TimeFlow;
    public float DayCycletime;
    public float WindSpeed = 5;
    public int NumberOfClouds = 100;
    public int WantedNumberOfClouds;
    public int WeatherChangeInterval = 30;
    public int SideSizeX;
    public int SideSizeZ;

    public GameObject Cloud1;
    public GameObject Cloud2;
    public GameObject Cloud3;
    public GameObject Cloud4;
    public GameObject Cloud5;
    public Material CloudGrey;
    public Material CloudDark;
    public GameObject FogLight;
    public GameObject FogHeavy;
    public GameObject LightRain;
    public GameObject HeavyRain;
    public GameObject WeatherAnomaly;

    GameObject RandomCloud;
    GameObject Cloud;
    GameObject WeatherComponents;
    public GameObject Hex;
    bool NumberOfCloudsChanged;
    bool WeatherChanged=false;
    int k;
    public float cloudTimer;
    public float weatherTimer;
    bool MorningFogActive;
    public GameObject[,] WeatherAnomalies=new GameObject[1,5];

    void Start()
    {
        MapController = GameObject.Find("MapController");
        SideSizeX = MapController.GetComponent<GenerateMap>().SideSize_X;
        SideSizeZ = MapController.GetComponent<GenerateMap>().SideSize_Z;
        WeatherComponents = GameObject.Find("Clouds");
        for (int i = 0; i < NumberOfClouds; i++)
        {
            SpawnCloud(Random.Range(-5000, 5000), Random.Range(-5000, 5000), i);
        }
        WantedNumberOfClouds = NumberOfClouds;
        WeatherChanged = false;
        NumberOfCloudsChanged = false;
        SpawnLightRain(6, 3);
        //SpawnWeatherAnomalyRain(6, 3);
        
    }

    void SpawnCloud(int x, int z, int i)
    {
        int j = Random.Range(0, 4);
        switch (j)
        {
            case 0:
                RandomCloud = Cloud1;
                break;
            case 1:
                RandomCloud = Cloud2;
                break;
            case 2:
                RandomCloud = Cloud3;
                break;
            case 3:
                RandomCloud = Cloud4;
                break;
            case 4:
                RandomCloud = Cloud5;
                break;
        }
        RandomCloud = Instantiate(RandomCloud, new Vector3(x, 1000 + Random.Range(-25, 25), z), Quaternion.Euler(0, Random.Range(-180,180), 0), GameObject.Find("Clouds").transform);
        RandomCloud.name = "Cloud" + i;
        RandomCloud.transform.tag = "Sky";
        RandomCloud.layer = 8;
    }

    Vector2 RandomHex()
    {
        int x = Random.Range(0, 2 * SideSizeX);
        int z;
        if (x % 2 == 0) { z = Random.Range(0, SideSizeZ); }
        else { z= Random.Range(0, SideSizeZ-1); }
        return new Vector2(x, z);
    }

    void SpawnMorningFog(int HexX, int HexZ)
    {
        GameObject Fog;
        Hex = GameObject.Find("Hex" + HexX + "." + HexZ);

        if(Hex.GetComponent<HexData>().HexType == 5|| Hex.GetComponent<HexData>().HexType==6)
        {
            Fog=Instantiate(FogHeavy, new Vector3(Hex.transform.position.x, 0, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        else if(Hex.GetComponent<HexData>().HexType == 7)
        {
            Fog = Instantiate(FogLight, new Vector3(Hex.transform.position.x, -2.5f, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        else if(Hex.GetComponent<HexData>().HexType == 3|| Hex.GetComponent<HexData>().HexType == 4)
        {
            Fog = Instantiate(FogLight, new Vector3(Hex.transform.position.x, 5, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        else
        {
            Fog = Instantiate(FogLight, new Vector3(Hex.transform.position.x, 0, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        Fog.name = "WeatherEventHex" + HexX + "." + HexZ;
        Fog.transform.parent = GameObject.Find("Weather").transform;
        Fog.GetComponent<WeatherEventData>().EventType = -1;
        Fog.GetComponent<WeatherEventData>().HexX = HexX;
        Fog.GetComponent<WeatherEventData>().HexZ = HexZ;
    }

    void GenerateMorningFog()
    {
        for (int i = 0; i < SideSizeX * 2 + 1; i++)
        {
            if (i % 2 != 0)
            {
                for (int j = 0; j < SideSizeZ + 1; j++)
                {
                    SpawnMorningFog(i, j);

                }
            }
            else
            {
                for (int j = 0; j < SideSizeZ; j++)
                {
                    SpawnMorningFog(i, j);
                }
            }
        }
    }

    //WEATHER EVENTS
    void SpawnLightFog(int HexX, int HexZ)
    {
        GameObject Event;
        Hex = GameObject.Find("Hex" + HexX + "." + HexZ);
        if (Hex.GetComponent<HexData>().HexType == 5 || Hex.GetComponent<HexData>().HexType == 6)
        {
            Event=Instantiate(FogLight, new Vector3(Hex.transform.position.x, 0, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        else if (Hex.GetComponent<HexData>().HexType == 7)
        {
            Event=Instantiate(FogLight, new Vector3(Hex.transform.position.x, -2.5f, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        else if (Hex.GetComponent<HexData>().HexType == 3 || Hex.GetComponent<HexData>().HexType == 4)
        {
            Event=Instantiate(FogLight, new Vector3(Hex.transform.position.x, 5, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        else
        {
            Event=Instantiate(FogLight, new Vector3(Hex.transform.position.x, 0, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        Event.name = "WeatherEventHex" + HexX + "." + HexZ;
        GameObject.Find("Hex" + HexX + "." + HexZ).GetComponent<HexData>().CurrentWeather = -1;
        Event.GetComponent<WeatherEventData>().EventType = -1;
        Event.GetComponent<WeatherEventData>().HexX = HexX;
        Event.GetComponent<WeatherEventData>().HexZ = HexZ;
        Event.transform.parent = GameObject.Find("Weather").transform;
    }

    void SpawnHeavyFog(int HexX, int HexZ)
    {
        GameObject Event;
        Hex = GameObject.Find("Hex" + HexX + "." + HexZ);
        if (Hex.GetComponent<HexData>().HexType == 5 || Hex.GetComponent<HexData>().HexType == 6)
        {
            Event=Instantiate(FogHeavy, new Vector3(Hex.transform.position.x, 0, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        else if (Hex.GetComponent<HexData>().HexType == 7)
        {
            Event=Instantiate(FogHeavy, new Vector3(Hex.transform.position.x, -2.5f, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        else if (Hex.GetComponent<HexData>().HexType == 3 || Hex.GetComponent<HexData>().HexType == 4)
        {
            Event=Instantiate(FogHeavy, new Vector3(Hex.transform.position.x, 5, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        else
        {
            Event=Instantiate(FogHeavy, new Vector3(Hex.transform.position.x, 0, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        }
        Event.name = "WeatherEventHex" + HexX + "." + HexZ;
        GameObject.Find("Hex" + HexX + "." + HexZ).GetComponent<HexData>().CurrentWeather = -2;
        Event.GetComponent<WeatherEventData>().EventType = -2;
        Event.GetComponent<WeatherEventData>().HexX = HexX;
        Event.GetComponent<WeatherEventData>().HexZ = HexZ;
        Event.transform.parent = GameObject.Find("Weather").transform;
    }

    void SpawnLightRain(int HexX, int HexZ)
    {
        GameObject Event;
        Hex = GameObject.Find("Hex" + HexX + "." + HexZ);
        Event=Instantiate(LightRain, new Vector3(Hex.transform.position.x, 0, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        Event.name = "WeatherEventHex" + HexX + "." + HexZ;
        GameObject.Find("Hex" + HexX + "." + HexZ).GetComponent<HexData>().CurrentWeather = 1;
        Event.GetComponent<WeatherEventData>().EventType = 1;
        Event.GetComponent<WeatherEventData>().HexX = HexX;
        Event.GetComponent<WeatherEventData>().HexZ = HexZ;
        Event.transform.parent = GameObject.Find("Weather").transform;
    }

    void SpawnHeavyRain(int HexX, int HexZ)
    {
        GameObject Event;
        Hex = GameObject.Find("Hex" + HexX + "." + HexZ);
        Event=Instantiate(HeavyRain, new Vector3(Hex.transform.position.x, 0, Hex.transform.position.z), Quaternion.Euler(0, 0, 0));
        Event.name = "WeatherEventHex" + HexX + "." + HexZ;
        GameObject.Find("Hex" + HexX + "." + HexZ).GetComponent<HexData>().CurrentWeather = 2;
        Event.GetComponent<WeatherEventData>().EventType = 2;
        Event.GetComponent<WeatherEventData>().HexX = HexX;
        Event.GetComponent<WeatherEventData>().HexZ = HexZ;
        Event.transform.parent = GameObject.Find("Weather").transform;
    }

    void SpawnWeatherAnomalyRain(int HexX, int HexZ)
    {
        GameObject Anomaly = WeatherAnomaly;
        Anomaly = Instantiate(Anomaly, GameObject.Find("Hex" + HexX + "." + HexZ).transform.position, Quaternion.Euler(0, 0, 0), GameObject.Find("Weather").transform);
        Anomaly.name = "WeatherAnomaly" + HexX + "." + HexZ;
        Anomaly.GetComponent<WeatherAnomaly>().AnomalyType = 1;
        Anomaly.GetComponent<WeatherAnomaly>().BaseHexX = HexX;
        Anomaly.GetComponent<WeatherAnomaly>().BaseHexZ = HexZ;
    }

    void SpawnWeatherAnomalyFog(int HexX, int HexZ)
    {
        GameObject Anomaly = WeatherAnomaly;
        Anomaly = Instantiate(Anomaly, new Vector3(HexX, 0, HexZ), Quaternion.Euler(0, 0, 0));
        Anomaly.transform.parent = GameObject.Find("Weather").transform;
        Anomaly.name = "WeatherAnomaly" + HexX + "." + HexZ;
        Anomaly.GetComponent<WeatherAnomaly>().AnomalyType = -1;
        Anomaly.GetComponent<WeatherAnomaly>().BaseHexX = HexX;
        Anomaly.GetComponent<WeatherAnomaly>().BaseHexZ = HexZ;
    }

    void ProceduralWeather()
    {
        if (WeatherChanged == false)
        {
            Debug.Log("PROCEDURALWEATHER");
            Vector2 RandomCords;
            WeatherChanged = true;
            RandomCords = RandomHex();
            Hex = GameObject.Find("Hex" + RandomCords.x + "." + RandomCords.y);
            int k = Random.Range(-1, 1);
            if (Hex.GetComponent<HexData>().CurrentWeather == 0)
            {
                Hex.GetComponent<HexData>().CurrentWeather += k;
                if (k > 0)
                {
                    SpawnWeatherAnomalyRain((int)RandomCords.x, (int)RandomCords.y);
                }
                else if (k < 0)
                {
                    SpawnWeatherAnomalyFog((int)RandomCords.x, (int)RandomCords.y);
                }
                else { }
            }
            else { }
        }
        else if (weatherTimer > WeatherChangeInterval) { weatherTimer = 0; WeatherChanged = false; }
        weatherTimer += Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        //GameTime
        TimeFlow = MapController.GetComponent<TimeController>().Timeflow;
        DayCycletime = MapController.GetComponent<TimeController>().DayCycleTime;

        //Clouds
        if (NumberOfCloudsChanged == false) { WantedNumberOfClouds = WantedNumberOfClouds + Random.Range(-1, 1); NumberOfCloudsChanged = true; cloudTimer = 0; if (WantedNumberOfClouds < 0) { WantedNumberOfClouds = 0; } }
        cloudTimer = cloudTimer + Time.deltaTime;
        if (cloudTimer > 5) { NumberOfCloudsChanged = false; }
        if (TimeFlow == true)
        {
            foreach(Transform Child in WeatherComponents.transform)
            {
                Child.transform.position += Vector3.forward * WindSpeed * Time.deltaTime * (MapController.GetComponent<TimeController>().MinuteInGameVsReal / 144);
                if (Child.transform.position.z > 5000) { Destroy(Child.gameObject); }
            }
        }
        if (WeatherComponents.transform.childCount < WantedNumberOfClouds)
        {
            SpawnCloud(Random.Range(-5000, 5000),-5000, 0);
        }

        if (Mathf.Abs(DayCycletime - 4 * 60)<1f)
        {
            if (MorningFogActive == false)
            {
                GenerateMorningFog();
                MorningFogActive = true;
            }
        }
        if (Mathf.Abs(DayCycletime - 5 * 60)<1f)
        {
            MorningFogActive = false;
        }

        //Weather
        //ProceduralWeather();
    }
}
