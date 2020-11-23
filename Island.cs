using System;
using UnityEngine;
using UnityEngine.UI;

public class Island
{
    public int id;
    public GameObject unityObject;
    public GameObject button;
    public string name;
    public int cost;
    public bool active;

    public Island(int newId, GameObject newObject, GameObject itsButton, string newName, int newCost, bool isActive)
    {
        id = newId;
        unityObject = newObject;
        button = itsButton;
        name = newName;
        cost = newCost;
        active = isActive;
        unityObject.SetActive(active);
    }
}
