using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AvailableShape
{
    public Mesh shape;
    public Mesh partPreviewModel;
    public Sprite menuIcon;
    public LinkedObject[] linkedObjects;
    public List<ELSEMaterial> availableMaterials;
}

