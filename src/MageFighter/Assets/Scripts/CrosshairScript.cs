using UnityEngine;
using System.Collections;

public class CrosshairScript : MonoBehaviour
{

    public Camera camera;
    public int distance = 1000;
    public Texture2D crosshairTexture;
    private GameObject _ObservedObject;

    public int ObserveTimeThreshold = 4;
    double _timeOBserving;
    Vector2 size  = new Vector2(90,10);
    public Texture2D progressBarEmpty  ;
    public Texture2D progressBarFull ;
    private GUIStyle currentStyle;

    public AudioClip AimSound1;
    public AudioClip AimSound2;
    public AudioClip AimSound3;
    private bool isAimSoundPlayed = false;
    private bool isAimSoundPlayedFirstTime = false;
    private bool isActionDone;

    // Use this for initialization
    void Start()
    {
    }

    private void InitStyles()
    {
        if (currentStyle == null)
        {
            currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
        }
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private void OnGUI()
    {
        InitStyles();

        GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width) / 2,
                (Screen.height - crosshairTexture.height) / 2, crosshairTexture.width, crosshairTexture.height)
               , crosshairTexture);

        if(_timeOBserving>0.2f)
        {
            // draw the background:
            GUI.BeginGroup(new Rect((Screen.width - crosshairTexture.width) / 2,
                (Screen.height - crosshairTexture.height) / 2 + crosshairTexture.height, size.x, size.y));

            GUI.Box(new Rect(0, 0, size.x, size.y), progressBarEmpty, currentStyle);

            // draw the filled-in part:
            GUI.BeginGroup(new Rect(0, 0, size.x * ((float)_timeOBserving / ObserveTimeThreshold), size.y));
            GUI.Box(new Rect(0, 0, size.x, size.y), progressBarFull);
            GUI.EndGroup();

            GUI.EndGroup();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var hitObject = GetHitObject();

        if (hitObject != _ObservedObject)
            StopObserveObject();

        UseObservableObject(hitObject, _timeOBserving / ObserveTimeThreshold >= 1);
        ObserveObject(hitObject);

        // for this example, the bar display is linked to the current time,
        // however you would set this value based on your desired display
        // eg, the loading progress, the player's health, or whatever.
        //RotateObject(hitObject);
    }

    void UseObservableObject(GameObject obj, bool thresholgComplete)
    {
        if (!thresholgComplete) return;
        if (obj == null) return;
        var actions = obj.GetComponent<IObservedAction>();
        if (actions == null) return;

        if (actions.HasAction)
        {
            DoObserableObjectAction(actions);
        }
        else
            PlayAimSound();

    }

    void PlayAimSound()
    {
        if (isAimSoundPlayed)
            return;
        isAimSoundPlayed = true;

        if(isAimSoundPlayedFirstTime == false)
        {
            isAimSoundPlayedFirstTime = true;
            this.GetComponent<AudioSource>().PlayOneShot(AimSound1);
            return;
        }
        AudioClip[] clips = new AudioClip[] { AimSound1, AimSound2, AimSound3 };
        this.GetComponent<AudioSource>().PlayOneShot(clips[Random.Range(0, clips.Length - 1)]);
    }

    void DoObserableObjectAction( IObservedAction actions)
    {
        if (isActionDone == true) return;
        if (!actions.HasAction) return;

        isActionDone = true;
    }

    private GameObject GetHitObject()
    {
        RaycastHit hit;
        Vector3 startingPoint = this.transform.position;
        if (Physics.Raycast(startingPoint, transform.forward, out hit, distance))
        {
            var obj = hit.transform.gameObject;
            return obj;
        }

        return null;
    }

    private void RotateObject(GameObject obj)
    {
        if (obj == null) return;
        obj.transform.Rotate(90, 0, 0);
    }

    private void ObserveObject(GameObject obj)
    {
        _ObservedObject = obj;
        if (_ObservedObject == null)
        {
            ResetObserveProgressBar();
            return;
        }
        var actions = _ObservedObject.GetComponent<IObservedAction>();
        if (actions != null && actions.IsObservable)
        {
            actions.BeingObserved();
            IncreaseObserveProgressBar();
        }
        else
        {
            ResetObserveProgressBar();
        }
    }

    private void StopObserveObject()
    {
        isActionDone = false;
        ResetObserveProgressBar();
        if (_ObservedObject == null)
        {
            return;
        }
        var actions = _ObservedObject.GetComponent<IObservedAction>();
        if (actions != null)
            actions.StopedBeingObserved();
        _ObservedObject = null;
    }

    private void IncreaseObserveProgressBar()
    {
        _timeOBserving += Time.deltaTime;
    }

    private void ResetObserveProgressBar()
    {
        _timeOBserving = 0;
        isAimSoundPlayed = false;
    }
}
