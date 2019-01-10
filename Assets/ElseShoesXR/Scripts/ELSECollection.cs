using UnityEngine;

[CreateAssetMenu(fileName = "New ELSE Collection", menuName="ELSE Collection")]
public class ELSECollection : ScriptableObject {

    [SerializeField]
    public GameObject[] productsPrefabs;
    [SerializeField]
    public ELSEMaterial[] materialsLibrary;

}
