using UnityEngine;

public class ModelSelection : MonoBehaviour {

    #region Singleton
    //Singleton pattern implementation.
    private static ModelSelection _instance;

    public static ModelSelection Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public Transform models;
    public Transform circleMask;

}
