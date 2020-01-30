using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlanetsButtons : MonoBehaviour
{ 
    public TriangleMapFromLatLong map;
    public GameObject controlPanel;
    public GameObject slider;

    protected GameObject arrow;
    public GameObject scrollview;
    protected PointMap[] planets;
    protected ClippingPlane[] cp;

    protected SortedDictionary<int, PointMap> visiblePlanets;
    protected SortedDictionary<int, PointMap> invisiblePlanets;

    protected PointGraphs[] graphs;

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
        Vector3 pos = Camera.main.transform.position;
        pos.x = newValue;
        Camera.main.transform.position = pos;
    }

    // Toggle Buttons to hide or show the planets
    public void TemperatureToggle(bool newValue)
    {
        planets = map.GetPlanets();
        invisiblePlanets = map.GetDict(0);
        visiblePlanets = map.GetDict(1);
        planets[0].Hide(!newValue);
        planets[0].SetHiddenStatus(!newValue);

        graphs = map.GetGraphs();

        ShiftPlanets(newValue, 0, graphs);

        
    }

    public void SpeedToggle(bool newValue)
    {
        planets = map.GetPlanets();
        invisiblePlanets = map.GetDict(0);
        visiblePlanets = map.GetDict(1);
        planets[1].Hide(!newValue);
        planets[1].SetHiddenStatus(!newValue);

        graphs = map.GetGraphs();

        ShiftPlanets(newValue, 1, graphs);

        
    }

    public void SalinityToggle(bool newValue)
    {
        planets = map.GetPlanets();
        invisiblePlanets = map.GetDict(0);
        visiblePlanets = map.GetDict(1);
        planets[2].Hide(!newValue);
        planets[2].SetHiddenStatus(!newValue);

        graphs = map.GetGraphs();

        ShiftPlanets(newValue, 2, graphs);

        
    }

    /// ///////CHANGED THIS

    public void ClippingToggle(bool newValue)
    {
        planets = map.GetPlanets();
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].EnableClippingPlane(newValue);
        }

    }

    public void IndependenceToggle(bool newValue)
    {
        planets = map.GetPlanets();
        for (int i = 0; i < planets.Length; i++)
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



    private void ShiftPlanets(bool toggleVal, int indiceOfPlanet, PointGraphs[] graphs)
    {
        if (!toggleVal)
        {
            invisiblePlanets[indiceOfPlanet] = planets[indiceOfPlanet];
            visiblePlanets.Remove(indiceOfPlanet);
            RefreshPositions(invisiblePlanets, visiblePlanets, graphs);   
        }
        else
        {
            visiblePlanets[indiceOfPlanet] = planets[indiceOfPlanet];
            invisiblePlanets.Remove(indiceOfPlanet);
            RefreshPositions(invisiblePlanets, visiblePlanets, graphs);
        }

        if(visiblePlanets.Count + graphs.Length > 1)
        {
            slider.SetActive(true);
            slider.GetComponent<Slider>().maxValue = (visiblePlanets.Count + graphs.Length - 1) * ECARTEMENT;
            slider.GetComponent<Slider>().GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (MAX_SLIDER_SIZE / (visiblePlanets.Count + graphs.Length + invisiblePlanets.Count)) * (visiblePlanets.Count + graphs.Length));
        }
        else
        {
            slider.SetActive(false);
        }
    }

    private void RefreshPositions(SortedDictionary<int,PointMap> invisible, SortedDictionary<int, PointMap> visible, PointGraphs[] graphs)
    {
        int i = 0;
        foreach(int key in visible.Keys)
        {
            visible[key].transform.position = new Vector3(ECARTEMENT * i, 0, 0);
            visible[key].transform.SendMessage("RefreshLasers", true);
            i++;
        }
        for (int j = 0; j < graphs.Length; j++)
        {
            graphs[j].transform.position = new Vector3(ECARTEMENT * i, 0, 0);
            i++;
        }
        foreach(int key in invisible.Keys)
        {
            invisible[key].transform.position = new Vector3(ECARTEMENT * i, 0, 0);
            invisible[key].transform.SendMessage("RefreshLasers", false);
            i++;
        }
    }

}
