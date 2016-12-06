using UnityEngine;
using System.Collections;

/// <summary>
/// SplashScreenController
/// <summary>
/// Author: Sam Meyer
/// <summary>
/// Controller handling all splash screen logic.
/// </summary>

public class SplashScreenController : MonoBehaviour {

    public float SplashDuration; //!< Amount of seconds the splashscreen will be active

    private void Start() {
        StartCoroutine(LoadMainMenu());
    }

    ///
    /// Waits a set amount of seconds before loading the Main Menu.
    ///
    private IEnumerator LoadMainMenu() {
        yield return new WaitForSeconds(SplashDuration);
        LoadingController.LoadScene(LoadingController.Scenes.MAIN_MENU);
    }
}