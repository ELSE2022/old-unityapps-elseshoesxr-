using UnityEngine;

[System.Serializable]
public class ConfigurablePart
{
    public string name;
    public Sprite partIcon;
    public MeshFilter meshFilter;
    public Renderer renderer;
    [HideInInspector]
    public Material selectedMaterial;
    //[HideInInspector]
    public DefaultMaterial defaultMaterial;
    [HideInInspector]
    public int selectedMatID;
    public int defaultShapeIndex;
    public Vector3 customRotatedPosition;
    public AvailableShape[] availableShapes;
}
