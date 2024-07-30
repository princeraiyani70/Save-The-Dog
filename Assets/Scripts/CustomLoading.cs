using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CustomLoading : MonoBehaviour
{
    [SerializeField] private int m_SceneToLoad;
    [SerializeField] private float m_PreloadMaximumTime = 3f;

    [SerializeField] private Slider ProgressSlider;

    private bool m_IsLoadingComplete = false;

    private void Start()
    {

        StartCoroutine(LoadSceneAsync());

        //FadeInOutManager.Instance.FadeOut(Color.white, 1, DG.Tweening.Ease.Linear);
        //yield return new WaitForSeconds(2);
        //FadeInOutManager.Instance.FadeIn(Color.white, 1, DG.Tweening.Ease.Linear);

    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_SceneToLoad, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        float timer = 0f;
        ProgressSlider.DOValue(1, m_PreloadMaximumTime).SetEase(Ease.InOutQuad);
        while (!m_IsLoadingComplete)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            // Update the progress bar or spinner animation here
          //  ProgressSlider.value = progress;
            if (progress >= 1f && timer >= m_PreloadMaximumTime)
            {
                m_IsLoadingComplete = true;
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

     
    }
}
