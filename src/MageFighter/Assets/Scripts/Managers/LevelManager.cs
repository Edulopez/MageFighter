using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Characters;

public class LevelManager : MonoBehaviour {

    public Text countDownText;

    public Text PointsText;

    private static float _timer = 0;
    public float maxTime = 0;
    public AudioClip finalSound;

    public GameObject enemiesPoolingObject;
    public GameObject BoosPoolingPoolingObject;
    public static float GameTimer { get { return _timer; } private set { } }

    private bool bossSpawn = false;
    
    public  GameObject player = null;

    private PlayerStats _playerStats;

    protected void Start()
    {
        _playerStats = player.GetComponent<PlayerStats>();
    }
    // Update is called once per frame
    void Update () {

        UpdateUI();
        TimeEvents();
    }

    private void TimeEvents()
    {
        if(!bossSpawn && _timer > 10)
        {
            bossSpawn = true;
            BoosPoolingPoolingObject.SetActive(true);
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

    void UpdateUI()
    {
        UpdateTime();

        PointsText.text = _playerStats.points.ToString();
    }
}
