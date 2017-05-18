using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public Text countDownText;
    private static float _timer = 0;
    public float maxTime = 0;
    public AudioClip finalSound;

    public GameObject enemiesPoolingObject;
    public GameObject BoosPoolingPoolingObject;
    public static float GameTimer { get { return _timer; } private set { } }

    private bool FirstBossSpawn = false;
    private bool SecondBossSpawn = false;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        UpdateTime();
        TimeEvents();

    }

    private void TimeEvents()
    {
        if(!FirstBossSpawn && _timer > 80)
        {
            FirstBossSpawn = true;
            var objectPooling = BoosPoolingPoolingObject.GetComponent<SpawnEnemiesPolling>();
            objectPooling.spawnInterval = 8;
        }

        if (!SecondBossSpawn && _timer > 80)
        {
            SecondBossSpawn = true;
            var objectPooling = BoosPoolingPoolingObject.GetComponent<SpawnEnemiesPolling>();
            objectPooling.spawnInterval = 8;
        }


    }

    void UpdateTime()
    {
        if (_timer < maxTime)
        {
            countDownText.text = ((int)(maxTime - _timer)).ToString();
            if (_timer >= (maxTime / 4))
                countDownText.color = new Color(255, 0, 0);
        }
        else
        {
           // StartCoroutine("EndGame");
        }
        // increment timer over time
        _timer += Time.deltaTime;
    }

    IEnumerator EndGame()
    {
        //Time.timeScale = 0;
        float fadeTime = GameObject.Find("Main Camera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
       // PlayerPrefs.SetInt("Gotas", Cubeta.Gota);

        gameObject.GetComponent<AudioSource>().PlayOneShot(finalSound);
        //Application.LoadLevel("Result");
    }
}
