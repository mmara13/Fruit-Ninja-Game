using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public Image fadeImage;
    private Spawner spawner;
    private Blade blade;

    AudioManager audioManager;

    private int score;

    private void Awake(){
        blade = FindObjectOfType<Blade>();  // Find the blade
        spawner = FindObjectOfType<Spawner>();  // Find the spawner
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();  // Find the audio manager
    }

    private void Start(){

        NewGame();

    } 
    private void NewGame(){ // Start a new game
        Time.timeScale = 1f;

        blade.enabled = true; // Enable the blade
        spawner.enabled = true; // Enable the spawner

        score = 0;
        scoreText.text = score.ToString();
        ClearScene();
    }

    private void ClearScene(){

        Fruit[] fruits = FindObjectsOfType<Fruit>(); // Find all the fruits in the scene

        foreach( Fruit fruit in fruits){
            Destroy(fruit.gameObject); // Destroy the fruit
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>(); // Find all the bombs in the scene

        foreach( Bomb bomb in bombs){
            Destroy(bomb.gameObject);   // Destroy the bomb
        }
    }

    public void IncreaseScore(int amount){ 
        score += amount;
        scoreText.text = score.ToString(); // Update the score text
        UpdateBladeColor();

    }

    private void UpdateBladeColor() // Update the blade color based on the score 
    //Acest block de cod a fost generat cu ajutorul GhatGBT-ului pentru a nu face hardcoding la culori
    {
        Color newColor;

        if (score < 10)
        {
            newColor = Color.white;
        }
        else if (score < 20)
        {
            newColor = Color.yellow;
        }
        else if (score < 30)
        {
            newColor = Color.green;
        }
        else if (score < 50)
        {
            newColor = Color.red;
        }
        else 
        {
            newColor = Color.magenta;
        }

        blade.SetTrailColor(newColor);
    }
    public void Explode(){ // Explode the bomb
        blade.enabled = false;
        spawner.enabled = false;

        StartCoroutine(ExplodeSequence()); // Start the explosion sequence 
        audioManager.PlaySFX(audioManager.bomb); // Play the bomb sound effect
    }
    private IEnumerator ExplodeSequence(){ // Explode sequence
        float elapsed = 0f;
        float duration = 0.5f;

        while( elapsed < duration){ // Fade to white

            float t = Mathf.Clamp01(elapsed / duration); // Clamp the value of t between 0 and 1
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);  // Lerp the color of the fade image from clear to white based on t


            Time.timeScale = 1f - t; // Slow down time based on t
            elapsed += Time.unscaledDeltaTime; // Increase the elapsed time based on the unscaled delta time

            yield return null;  // Wait for the next frame
        }

        yield return new WaitForSecondsRealtime(0.5f); // Wait for 0.5 seconds

        NewGame(); // Start a new game

        elapsed = 0f; // Reset the elapsed time

        while( elapsed < duration){ // Fade to clear

            float t = Mathf.Clamp01(elapsed / duration); // Clamp the value of t between 0 and 1
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t); // Lerp the color of the fade image from white to clear based on t

            elapsed += Time.unscaledDeltaTime; // Increase the elapsed time based on the unscaled delta time

            yield return null;
        }

    }
}