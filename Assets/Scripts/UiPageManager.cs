using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class UiPageManager : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer VideoPlayer, LoopVideoPlayer;

    [SerializeField]
    private CanvasGroup cg;

    private float fadeInTime = 2f;

    private void Start()
    {
        cg.alpha = 0f;
        cg.interactable = false;
        VideoPlayer.Prepare();
        StartCoroutine(CutsceneStartup());
    }

    private IEnumerator CutsceneStartup()
    {
        yield return new WaitForSeconds(1f);

        VideoPlayer.Play();

        yield return new WaitForSeconds(2f);

        //Load canvas group
        StartCoroutine(FadeInUi());

        yield return new WaitForSeconds(13f);
        VideoPlayer.enabled = false;

        LoopVideoPlayer.enabled = true;
        LoopVideoPlayer.Play();
    }

    private IEnumerator FadeInUi()
    {
        float timer = 0f;
        while(timer < fadeInTime)
        {
            timer += Time.deltaTime;

            cg.alpha = Mathf.Lerp(0f, 1f, timer / fadeInTime);

            yield return null;
        }

        cg.alpha = 1f;
        cg.interactable = true;
    }
}
