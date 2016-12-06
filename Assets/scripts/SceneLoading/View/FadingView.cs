using UnityEngine;
using System.Collections;
/// <summary>
/// FadingView
/// <summary>
/// Author: Sam Meyer
/// <summary>
/// The FadingView must be placed in every scene as a black UI layer covering the entire camera view.
/// The LoadingController will use this to nicely fade between scenes.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class FadingView : MonoBehaviour {

    public float fadeSpeed = 2f;

    private CanvasGroup overlayCanvasGroup;

    void Start() {
        overlayCanvasGroup = GetComponent<CanvasGroup>();

        overlayCanvasGroup.alpha = 1;
        overlayCanvasGroup.blocksRaycasts = false;

        EventManager.StartListening(SceneLoadingEvents.FADE_IN, FadeIn);
        EventManager.StartListening(SceneLoadingEvents.FADE_OUT, FadeOut);
    }

    void OnDestroy() {
        EventManager.StopListening(SceneLoadingEvents.FADE_IN, FadeIn);
        EventManager.StopListening(SceneLoadingEvents.FADE_OUT, FadeOut);
    }

    /// 
    /// Starts to fade in
    /// 
    private void FadeIn(object[] callbackEvent) {
        StopAllCoroutines();
        StartCoroutine(FadeInOverlay((string)callbackEvent[0]));
    }

    /// 
    /// Starts to fade out
    /// 
    private void FadeOut(object[] callbackEvent) {
        StopAllCoroutines();
        StartCoroutine(FadeOutOverlay((string)callbackEvent[0]));
    }

    /// 
    /// Fades in
    /// 
    IEnumerator FadeInOverlay(string callbackEvent) {
        overlayCanvasGroup.alpha = 1;
        overlayCanvasGroup.blocksRaycasts = false;
        while (overlayCanvasGroup.alpha > 0.05) {
            overlayCanvasGroup.alpha = Mathf.Lerp(overlayCanvasGroup.alpha, 0, Time.deltaTime * fadeSpeed);
            yield return null;
        }

        if (overlayCanvasGroup.alpha < 0.05) {
            EventManager.TriggerEvent(callbackEvent);
        }
    }

    /// 
    /// Fades out
    /// 
    IEnumerator FadeOutOverlay(string callbackEvent) {
        overlayCanvasGroup.alpha = 0;
        overlayCanvasGroup.blocksRaycasts = true;
        while (overlayCanvasGroup.alpha < 0.95) {
            overlayCanvasGroup.alpha = Mathf.Lerp(overlayCanvasGroup.alpha, 1, Time.deltaTime * fadeSpeed);
            yield return null;
        }

        if (overlayCanvasGroup.alpha > 0.95) {
            EventManager.TriggerEvent(callbackEvent);
        }
    }

    /// 
    /// Fades the overlay from and to a specific alpha value and fires an event after it is complete
    /// 
    IEnumerator FadeOverlay(float from, float to, string completeEvent) {
        overlayCanvasGroup.alpha = from;
        while (overlayCanvasGroup.alpha > to) {
            overlayCanvasGroup.alpha = Mathf.Lerp(overlayCanvasGroup.alpha, to, Time.deltaTime * fadeSpeed);
            yield return null;
        }

        if (overlayCanvasGroup.alpha <= to) {
            EventManager.TriggerEvent(completeEvent);
        }
    }
}
