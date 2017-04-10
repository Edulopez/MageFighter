using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public Text countDownText;
    private static float _timer = 0;
    public float maxTime = 0;

    public AudioClip finalSound;

    public static float GameTimer { get { return _timer; } private set { } }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        UpdateTime();
        TimeEvents();

    }

    private void TimeEvents()
    { }

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
