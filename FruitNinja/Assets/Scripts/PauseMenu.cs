using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    // Start is called before the first frame update


    AudioManager audioManager;
    [SerializeField] GameObject mute;
    [SerializeField] GameObject play;



    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Pause(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        audioManager.PlaySFX(audioManager.pause);
        audioManager.Stop();

    }
    public void Resume(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        audioManager.PlaySFX(audioManager.unpause);
        if (audioManager.isMuted == true)
        {

        }
        else
        {
            audioManager.Start();
        }

    }

    public void Restart(){
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1;
    }

    public void SoundOff()
    {
        mute.SetActive(false);
        play.SetActive(true);
        audioManager.Stop(); //we dont have to do this because we only have to save the state (on/off) for when the game is resumed
        audioManager.isMuted = false;

    }

    public void SoundOn()
    {
        mute.SetActive(true);
        play.SetActive(false);
        //audioManager.Start(); //same here
        audioManager.isMuted = true;
    }

}
