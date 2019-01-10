using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

    public float minimumTime;
    public Image[] splashImages;
    public Text[] splashTexts;
    public Slider slider;
    public Text percentageText;
    public GameObject startButton;
    public GameObject collectionCanvas;

    //public Text progressTxt;
    private float t;
    private Color fadingAlpha_white;
    private Color fadingAlpha_black;

    private float counter;

    private AsyncOperation operation;

    private void Update()
    {
        counter += counter + Time.deltaTime;
    }

    IEnumerator Start()
    {
        Screen.fullScreen = true;
        StartCoroutine(LoadAsync(1));
        //async.allowSceneActivation = true;
        while (Time.time < minimumTime) // !async.isDone || 
        {
            fadingAlpha_white = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), t);
            fadingAlpha_black = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), t);

            foreach (Image img in splashImages)
                img.color = fadingAlpha_white;

            foreach (Text txt in splashTexts)
                txt.color = fadingAlpha_black;

            t += Time.deltaTime / 1;
            yield return null;
        }
        startButton.SetActive(true);
        slider.gameObject.SetActive(false);
    }

    public void StartButtonClick()
    {
        collectionCanvas.SetActive(true);
    }

    public void Demo2018BtnClick()
    {
        operation.allowSceneActivation = true;
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            percentageText.text = Mathf.Ceil(progress * 100) + "%";
            yield return null;
        }
    }

}
