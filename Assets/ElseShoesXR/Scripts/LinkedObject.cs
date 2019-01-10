using UnityEngine;

[System.Serializable]
public class LinkedObject
{
    public bool isElement;
    [ConditionalHide("isElement", true)]
    public Sprite elementIcon;

    [ConditionalHide("isElement", true)]
    public ELSEMaterial material;

    public GameObject linkedGameObject;
    public Mesh partPreviewModel;
    [HideInInspector]
    public Mesh myMesh;

    [HideInInspector]
    public int defaultColorID;
}
