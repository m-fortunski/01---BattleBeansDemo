using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    public int StartHour=6;
    public int StartMinute=00;
    public int Hour;
    public int Minute;
    public int Day;
    public int GameTimeMinutes;
    public float GameTime=0;
    public float DayCycleTime = 0;
    public float MinuteInGameVsReal=144;
    float timer=0;
    public int DawnHour=5;
    public int DuskHour=21;
    public Vector3 MapCenter;

    public float SunOrbitRadious = 2000;
    public float SunX = 1100;
    public Vector3 SunTransformVector;
    public Vector3 MoonTransformVector;
    Vector3 SunUp;
    Vector3 SunPositionOld;

    public TMP_Text Clock;
    public GameObject Sun;
    public GameObject Moon;
    public float SunZ;

    public Gradient SkyColor;
    public Gradient HorizonColor;
    public bool Timeflow = false;
    public float MoonTime;
    public int NumberOfStars=5000;
    public float StarDistance=5000;
    public GameObject StarPrefab;
    public Material Stars;
    GameObject ActiveObject;
    public float StarIntensity=0;

    public float gunwo;
    void Start()
    {
        Hour = StartHour;
        Minute = StartMinute;
        SunUp = new Vector3(0,0.2f, 0);
        DayCycleTime = StartHour * 60+StartMinute;
        if (DayCycleTime > (DawnHour * 60) && DayCycleTime < (DuskHour * 60)) { Sun.SetActive(true); }
        else { Moon.SetActive(true); GenerateStars(); }
    }

    void TheSun(float DayCycleTime)
    {
        SunTransformVector = Quaternion.AngleAxis(((DayCycleTime - DawnHour * 60) / ((DuskHour - DawnHour) * 60)) * 180, Vector3.right) * new Vector3(0, 0, -SunOrbitRadious);
        Sun.transform.position = SunTransformVector;
        Sun.transform.rotation = Quaternion.Euler(((DayCycleTime - DawnHour * 60) / ((DuskHour - DawnHour) * 60)) * 180, 0, 0);
        Sun.GetComponent<Light>().intensity = 0.5f+0.5f*(SunOrbitRadious - Mathf.Abs(Sun.transform.position.z)) / SunOrbitRadious;
    }

    void TheMoon(float DayCycleTime)
    {
        if (DayCycleTime > DawnHour*60) { MoonTime = DayCycleTime - DuskHour*60; }
        else { MoonTime = DayCycleTime + (1440-DuskHour*60); }
        MoonTransformVector = Quaternion.AngleAxis((MoonTime / ((24 - (DuskHour - DawnHour)) * 60)) * 180, Vector3.right) * new Vector3(0, 0, -SunOrbitRadious);
        Moon.transform.position = MoonTransformVector;
        Moon.transform.rotation = Quaternion.Euler((MoonTime / ((24 - (DuskHour - DawnHour)) * 60)) * 180, 0, 0);
    }

    void Dawn()
    {
        Moon.GetComponent<Light>().enabled=false;
        Moon.SetActive(false);
        Sun.SetActive(true);
        Sun.GetComponent<Light>().enabled = true;
        Sun.GetComponent<Light>().color = new Color(0.99f, 0.82f, 0.30f);
        Sun.GetComponent<Light>().intensity = 0.2f;
        Sun.transform.position = new Vector3(MapCenter.x, MapCenter.y, MapCenter.z-SunOrbitRadious);
        SunPositionOld=Sun.transform.position;
    }

    void Dusk()
    {
        Sun.GetComponent<Light>().enabled = false;
        Sun.SetActive(false);
        Moon.SetActive(true);
        Moon.GetComponent<Light>().enabled = true;
    }

    void GenerateStars()
    {
        for (int i = 0; i < NumberOfStars; i++)
        {
            float RotX, RotZ;
            RotX = Random.Range(-150, 150);
            RotZ = Random.Range(-90, 90);
            ActiveObject = Instantiate(StarPrefab, new Vector3(0, StarDistance, 0), Quaternion.Euler(-180, 0, 0));
            ActiveObject.GetComponent<MeshRenderer>().enabled = false;
            ActiveObject.transform.parent = GameObject.Find("Stars").transform;
            ActiveObject.transform.position = Quaternion.Euler(RotX,0, RotZ) * ActiveObject.transform.position;
            ActiveObject.transform.rotation = Quaternion.Euler(-180 + RotX, 0, -RotZ);
        }
        ActiveObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * 5);
    }

    void DeleteStars()
    {
        foreach (Transform child in GameObject.Find("Stars").transform)
        {
            Destroy(child.gameObject);
        }
    }

    void ProcessStars()
    {
        if (DayCycleTime >= (DuskHour - 1) * 60 && DayCycleTime < DuskHour * 60) { MoonTime = DayCycleTime - DuskHour * 60; }
        GameObject.Find("Stars").transform.rotation = Quaternion.Euler(-30 + 90 * (MoonTime / (24 * 60 - (DuskHour - DawnHour) * 60)), 0, 0);
        if (GameObject.Find("Stars").transform.childCount > 0)
        {
            if ((GameObject.Find("Stars").transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled == false) && (StarIntensity > 0.3))
            {
                foreach (Transform child in GameObject.Find("Stars").transform)
                {
                    child.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
        if (DayCycleTime > (DawnHour - 1) * 60 && DayCycleTime < DawnHour * 60) ;
        {
            StarIntensity = 2 * ((DawnHour * 60 - DayCycleTime) / 60);
            if (StarIntensity >0.1)
            {
                foreach (Transform child in GameObject.Find("Stars").transform)
                {
                    child.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * StarIntensity);
                }
            }
            else
            {
                if ((GameObject.Find("Stars").transform.childCount > 0)&&DayCycleTime<(DuskHour-1)*60)
                {
                    DeleteStars();
                }
            }
        }
        if (DayCycleTime < (DuskHour * 60) && DayCycleTime > (DuskHour - 1) * 60)
        {
            StarIntensity = 2 * ((DayCycleTime-(DuskHour-1)*60) / 60);
            if (StarIntensity > 0.1)
            {
                foreach (Transform child in GameObject.Find("Stars").transform)
                {
                    child.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * StarIntensity);
                }
            }
        }

        else if (DayCycleTime > DuskHour * 60 || DayCycleTime < DawnHour * 60) { StarIntensity = 2; }



    }

    void UpdateSkybox(float DayCycleTime)
    {
        float intensity = DayCycleTime / 1440;
        RenderSettings.skybox.SetColor("_SkyTint", SkyColor.Evaluate(intensity));
        RenderSettings.skybox.SetColor("_GroundColor", HorizonColor.Evaluate(intensity));
        if (DayCycleTime >=(DawnHour * 60)&& DayCycleTime <= (DuskHour * 60))
        {
            RenderSettings.skybox.SetFloat("_Exposure", 0.6f + (0.4f * (Mathf.Abs(intensity - (DawnHour / 60) + ((DuskHour - DawnHour) / 120)))));
        }
        if (Mathf.Abs(DayCycleTime - DuskHour*60)<=0.5f)
        {
            RenderSettings.skybox.SetFloat("_Exposure", 0.6f);
        }
        if (DayCycleTime > ((DawnHour - 1) * 60) && (DayCycleTime < (DawnHour * 60))) { RenderSettings.skybox.SetFloat("_AtmosphereThickness", 1 - 0.9f * (((DawnHour * 60) - DayCycleTime) / 60)); }
        if (DayCycleTime > ((DuskHour - 1) * 60) && DayCycleTime < ((DuskHour) * 60)) { RenderSettings.skybox.SetFloat("_AtmosphereThickness", 0.1f + 0.9f * (((DuskHour * 60) - DayCycleTime) / 60)); }
    }

    void Update()
    {
        //GameTime
        if (Timeflow == true)
        {
            //TimeFLow
            GameTime = GameTime + (Time.deltaTime * MinuteInGameVsReal / 60);
            DayCycleTime = DayCycleTime + (Time.deltaTime * MinuteInGameVsReal / 60);
            timer = timer + (Time.deltaTime * MinuteInGameVsReal / 60);
            GameTimeMinutes = (int)DayCycleTime;
            Day = Mathf.FloorToInt(GameTimeMinutes / 1440);
            Hour = Mathf.FloorToInt((GameTimeMinutes - (Day*1440)) / 60);
            Minute = GameTimeMinutes - (Hour * 60)-(Day*1440);

            //GameTime
            if (Mathf.Abs(timer - Minute) >= 1) { Minute++; }
            if (Minute >= 60) { Hour++; Minute = 0; timer = 0; }
            if (Hour >= 24) { Hour = 0; }
            if (Minute >= 10) { Clock.text = (Hour.ToString() + ":" + Minute.ToString());}
            else { Clock.text = (Hour.ToString() + ":" + "0" + Minute.ToString()); }

            //DayCycleTime
            if (DayCycleTime >= 1440) { DayCycleTime = DayCycleTime - 1440; }

            //Sunrise
            if (Mathf.Abs(DayCycleTime - DawnHour * 60) <= 1f) { Dawn(); }
            if (Mathf.Abs(DayCycleTime - ((DawnHour + 1) * 60)) <= 1f) { if (GameObject.Find("Stars").transform.childCount > 0) { DeleteStars(); } }

            //Sunset
            if (Mathf.Abs(DayCycleTime - DuskHour * 60) <= 1f) { Dusk(); }
            if (Mathf.Abs(DayCycleTime - ((DuskHour - 1) * 60)) <= 1f) { if (GameObject.Find("Stars").transform.childCount < (NumberOfStars - 1)) { GenerateStars();} }

            //Sun&Moon
            if ((DayCycleTime >= (DawnHour * 60)) &&(DayCycleTime<=(DuskHour*60))){ TheSun(DayCycleTime); }
            else { TheMoon(DayCycleTime); }

            //Stars
            if ((DayCycleTime >= ((DuskHour - 1) * 60)) || (DayCycleTime <= ((DawnHour+1) * 60))) { ProcessStars(); }

            //Skybox
            UpdateSkybox(DayCycleTime);
        }
    }
    }



