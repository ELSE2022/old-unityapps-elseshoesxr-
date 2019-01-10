using UnityEngine;
#if VUFORIA
using Vuforia;
#endif

public class ARBtn : MonoBehaviour {

    public Renderer floorRenderer;
    public Camera normalCam;
    public Camera ARCam;
    public GameObject catalogueBtn;

    public static bool isAR;
#if VUFORIA
    public delegate void ARSwitch(bool status);
    public static event ARSwitch OnSwitch;

    public void SwitchCam()
    {
        normalCam.enabled = isAR;
        ARCam.enabled = !isAR;
        catalogueBtn.SetActive(isAR);
        floorRenderer.enabled = isAR;

        isAR = !isAR;

        if (VuforiaRuntime.Instance.InitializationState == VuforiaRuntime.InitState.NOT_INITIALIZED)
        {
            VuforiaConfiguration.Instance.Vuforia.DelayedInitialization = false;

            VuforiaRuntime.Instance.InitVuforia();

            ARCam.GetComponent<VuforiaBehaviour>().enabled = true;
            ARCam.GetComponent<DefaultInitializationErrorHandler>().enabled = true;
        }

        if (OnSwitch != null)
            OnSwitch(isAR);
    }
#endif
}

