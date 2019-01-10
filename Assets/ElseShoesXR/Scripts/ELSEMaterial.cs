using UnityEngine;

[CreateAssetMenu(fileName = "New ELSE Material", menuName = "ELSE Material")]
public class ELSEMaterial : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public Sprite colorIcon;
    public Material[] matVariations;
}
