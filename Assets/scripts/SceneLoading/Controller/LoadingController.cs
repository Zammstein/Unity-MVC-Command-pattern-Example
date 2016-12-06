using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// LoadingController
/// <summary>
/// Author: Sam Meyer
/// <summary>
/// LoadingController functions as a simple bridge between scenes.
/// Call the static method 'LoadScene' from anywhere to start loading a scene.
/// The scene will automatically launch when done loading.
/// </summary>
public class LoadingController : MonoBehaviour {

    /// 
    /// These enums represent scenes that can be loaded. The value 
    /// of each enum is a reference to the index of the scene.
    /// See unity build settings for the correct indexes.
    /// 
    public enum Scenes {
        SPLASH_SCREEN = 0,
        LOADING_SCENE = 1,
        MAIN_MENU = 2
    }

    public static Scenes sceneToBeLoaded;
    private static AsyncOperation async;

    public Text loadingPercentageTextField;

    /// 
    /// Use this function to transition from the current scene to the target scene.
    /// 
    public static void LoadScene(Scenes scene) {
        sceneToBeLoaded = scene;

        // Start fading out the scene befre switching to the loading scene 
        EventManager.TriggerEvent(SceneLoadingEvents.FADE_OUT, SceneLoadingEvents.FADE_OUT_COMPLETE);
    }

    void Start() {
        EventManager.StartListening(SceneLoadingEvents.FADE_OUT_COMPLETE, SceneReadyForSwitch);
        Invoke("TriggerFadeInForNewScene", 0.1f);
    }

    void OnDestroy() {
        EventManager.StopListening(SceneLoadingEvents.FADE_OUT_COMPLETE, SceneReadyForSwitch);
    }

    /// 
    /// Trigger the fade in for the next scene with a minor delay to allowe all listeners in the scene to register.
    ///
    private void TriggerFadeInForNewScene() {
        EventManager.TriggerEvent(SceneLoadingEvents.FADE_IN, SceneLoadingEvents.FADE_IN_COMPLETE);
    }

    private void SceneReadyForSwitch(object[] arg0) {
        SceneManager.LoadScene((int)Scenes.LOADING_SCENE);
    }

    void Update() {
        if (SceneManager.GetActiveScene().name == "loading_scene") {
            if (async != null) {
                loadingPercentageTextField.text = (Math.Ceiling(async.progress * 100)).ToString() + "%";
                if (async.progress >= 0.9f) {
                    async.allowSceneActivation = true;
                    async = null;
                }
            } else {
                async = SceneManager.LoadSceneAsync((int)sceneToBeLoaded);
                async.allowSceneActivation = false;
            }
        }
    }
}