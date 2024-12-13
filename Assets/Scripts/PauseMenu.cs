using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Options")]
    public Slider volumeFX;
    public Slider volumeMaster;
    public Toggle mute;
    public AudioMixer mixer;
    public AudioSource fxSource;
    public AudioClip clickSound;
    private float lastVolume;
    [Header("Panels")]
    public GameObject optionPanel;
    public GameObject gameOverUI;
    [Header("Pause")]
    [SerializeField] private GameObject Pause;

    private MenuGameOver menuGameOver;


    private void Awake()
    {
        volumeFX.onValueChanged.AddListener(ChangeVolumeFX);
        volumeMaster.onValueChanged.AddListener(ChangeVolumeMaster);
        Pause.SetActive(false);
        menuGameOver = FindAnyObjectByType<MenuGameOver>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Pause.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }
    }
    public void SetMute()
    {

        if (mute.isOn)
        {
            mixer.GetFloat("VolMaster", out lastVolume);
            mixer.SetFloat("VolMaster", -80);
        }

        else mixer.SetFloat("VolMaster", lastVolume);
    }

    public void openPanel(GameObject panel)
    {

        optionPanel.SetActive(false);



        panel.SetActive(true);
        PlaySoundButton();
    }

    public void ChangeVolumeMaster(float v)
    {
        mixer.SetFloat("VolMaster", v);
    }
    public void ChangeVolumeFX(float v)
    {
        mixer.SetFloat("VolFX", v);
    }
    public void PlaySoundButton()
    {
        fxSource.PlayOneShot(clickSound);
    }
    #region Pause
    private void PauseGame(bool status)
    {
        Pause.SetActive(status);
        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    #endregion

    public void Resume()
    {
        Pause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }
    public void volver()
    {
        Pause.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Exits play mode
#endif
    }

    public void gameOver()
    {
        gameObject.SetActive(true);
    }

   
}