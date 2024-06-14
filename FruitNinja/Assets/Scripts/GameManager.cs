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
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start(){

        NewGame();

    } 
    private void NewGame(){
        Time.timeScale = 1f;

        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString();
        ClearScene();
    }

    private void ClearScene(){

        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach( Fruit fruit in fruits){
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach( Bomb bomb in bombs){
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int amount){
        score += amount;
        scoreText.text = score.ToString();
        UpdateBladeColor();

    }

    private void UpdateBladeColor()
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
    public void Explode(){
        blade.enabled = false;
        spawner.enabled = false;

        StartCoroutine(ExplodeSequence());
        audioManager.PlaySFX(audioManager.bomb);
    }
    private IEnumerator ExplodeSequence(){
        float elapsed = 0f;
        float duration = 0.5f;

        while( elapsed < duration){

            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);


            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        NewGame();

        elapsed = 0f;

        while( elapsed < duration){

            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

    }
}