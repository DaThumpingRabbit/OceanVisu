using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlanetsButtons : MonoBehaviour
{ 
    public TriangleMapFromLatLong map;
    public Camera camera;
    public GameObject controlPanel;
    public GameObject slider;

    protected GameObject arrow;
    public GameObject scrollview;
    protected PointMap[] planets;
    protected ClippingPlane[] cp;

    protected SortedDictionary<int, PointMap> visiblePlanets;
    protected SortedDictionary<int, PointMap> invisiblePlanets;

    private const int ECARTEMENT = 600;
    private const int MAX_SLIDER_SIZE = 500;

    public void Start()
    {
        slider = GameObject.Find("Slider");
        arrow = GameObject.Find("Arrow");
    }
    

    //Button for control panel
    public void OpenPanel()
    {
        bool status = controlPanel.activeSelf;
        controlPanel.SetActive(!status);
        
    }

    public void ShowPlanetDropDown()
    {
        arrow.transform.Rotate(new Vector3(0, 0, 180));
        bool status = scrollview.activeSelf;
        scrollview.SetActive(!status);
    }

    /// ////////ADDED THE SETHIDDENSTATUS IN ALL FUNC
    /// 
    //Slider that shifts camera
    public void SliderChanged(float newValue)
    {
        Vector3 pos = camera.transform.position;
        pos.x = newValue;
        camera.transform.position = pos;
    }

    // Toggle Buttons to hide or show the planets
    public void TemperatureToggle(bool newValue)
    {
        planets = map.GetPlanets();
        invisiblePlanets = map.GetDict(0);
        visiblePlanets = map.GetDict(1);
        planets[0].Hide(!newValue);
        planets[0].SetHiddenStatus(!newValue);
        
        shiftPlanets(newValue, 0);


    }

    public void SpeedToggle(bool newValue)
    {
        planets = map.GetPlanets();
        invisiblePlanets = map.GetDict(0);
        visiblePlanets = map.GetDict(1);
        planets[1].Hide(!newValue);
        planets[1].SetHiddenStatus(!newValue);
        
        shiftPlanets(newValue, 1);

    }

    public void SalinityToggle(bool newValue)
    {
        planets = map.GetPlanets();
        invisiblePlanets = map.GetDict(0);
        visiblePlanets = map.GetDict(1);
        planets[2].Hide(!newValue);
        planets[2].SetHiddenStatus(!newValue);
        
        shiftPlanets(newValue, 2);
    }

    public void MixToggle(bool newValue)
    { 
        planets = map.GetPlanets();
        invisiblePlanets = map.GetDict(0);
        visiblePlanets = map.GetDict(1);
        planets[3].Hide(!newValue);
        planets[3].SetHiddenStatus(!newValue);
        
        shiftPlanets(newValue, 3);

    }
    /// ///////CHANGED THIS

    public void ClippingToggle(bool newValue)
    {
        planets = map.GetPlanets();
        for (int i = 0;i<4;i++)
        {
            planets[i].EnableClippingPlane(newValue);
        }

    }

    public void IndependenceToggle(bool newValue)
    {
        planets = map.GetPlanets();
        for (int i = 0; i < 4; i++)
        {
            planets[i].GetComponent<Interact>().SetIndependentMovement(newValue);
        }
    }

    /// //////////ADDED THIS
    public void RealignAll()
    {
        planets = map.GetPlanets();
        cp = map.GetClippingPlanes();
        for(int i = 0; i<planets.Length;i++)
        {
            planets[i].transform.position = new Vector3(ECARTEMENT*i, 0, 0);
            planets[i].transform.rotation = new Quaternion();
            cp[i].transform.parent.position = new Vector3(ECARTEMENT * i, 0, 0);
            
        }
    }



    private void shiftPlanets(bool toggleVal, int indiceOfPlanet)
    {
        if (!toggleVal)
        {
            invisiblePlanets[indiceOfPlanet] = planets[indiceOfPlanet];
            visiblePlanets.Remove(indiceOfPlanet);
            RefreshPositions(invisiblePlanets, visiblePlanets);   
        }
        else
        {
            visiblePlanets[indiceOfPlanet] = planets[indiceOfPlanet];
            invisiblePlanets.Remove(indiceOfPlanet);
            RefreshPositions(invisiblePlanets, visiblePlanets);
        }
        if(visiblePlanets.Count>1)
        {
            slider.SetActive(true);
            slider.GetComponent<Slider>().maxValue = (visiblePlanets.Count - 1) * ECARTEMENT;
            slider.GetComponent<Slider>().GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (MAX_SLIDER_SIZE / (visiblePlanets.Count + invisiblePlanets.Count)) * visiblePlanets.Count);

        }
        else if (visiblePlanets.Count <=1)
        {
            slider.SetActive(false);
        }
    }

    private void RefreshPositions(SortedDictionary<int,PointMap> invisible, SortedDictionary<int, PointMap> visible)
    {
        int i = 0;
        foreach(int key in visible.Keys)
        {
            visible[key].transform.position = new Vector3(ECARTEMENT * i, 0, 0);
            i++;
        }
        foreach(int key in invisible.Keys)
        {
            invisible[key].transform.position = new Vector3(ECARTEMENT * i, 0, 0);
            i++;
        }
    }

}
