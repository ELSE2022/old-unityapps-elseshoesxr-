using System;
using UnityEngine;


public sealed class SystemManager
{
    private static readonly Lazy<SystemManager> lazy =
        new Lazy<SystemManager>(() => new SystemManager());

    public static SystemManager Instance { get { return lazy.Value; } }

    public int selectedModelID;
    public int selectedPartID;
    public int selectedMatID;
    public int selectedColID;
    public int selectedOptionID;
    public int selectedElementID;
    public int selectedElementColorID;

    public int confirmedMatID;

    public SelectionCircle[] selectionCircles;

    public string message;

    private SystemManager()
    {
        Debug.Log("..::System Manager Initialized::..");

        selectionCircles = GameObject.FindObjectsOfType<SelectionCircle>();
    }
}



