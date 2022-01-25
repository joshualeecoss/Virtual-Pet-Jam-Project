using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private const float PET_WALK_TIMEOUT = 224f / 60f;
    private const float LIGHT_UP_SIGN_TIMEOUT = 79f / 60f;
    public Animator petAnimator;
    public Animator menuAnimator;
    public VirtualPetController petController;
    public MenuCanvasController menuCanvasController;

    private void Start()
    {
        StartCoroutine(Startup());
    }

    private void Update() {
        if (petController.LightUpSign()) {
            menuAnimator.Play("LightUpSign");
            petController.SetLightUpSign();
        }
        if (menuCanvasController.GetLightUpDone()) {
            menuAnimator.Play("MoveButtons");
            menuCanvasController.LightUpDone();
        }
    }

    public void PlayButtonPressed() {
        SceneManager.LoadScene("Main");
    }

    public void QuitButtonPressed() {
        Application.Quit();
    }

    public IEnumerator Startup() {
        yield return new WaitForSeconds(1.5f);
        petAnimator.SetBool("startMenu", true);
    }

}
