using UnityEngine;

public class MainScreen : MonoBehaviour {

    private Canvas canvas;

    #region Singleton
    //Singleton pattern implementation.
    private static MainScreen _instance;

    public static MainScreen Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            canvas = GetComponent<Canvas>();
        }
    }
    #endregion

    public void Refresh()
    {
        canvas.enabled = false;
        canvas.enabled = true;
    }
}
