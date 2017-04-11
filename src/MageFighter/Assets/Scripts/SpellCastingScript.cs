using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;

public class SpellCastingScript : MonoBehaviour {

    RigidHand _hand;
    public float MaxScaleSpellObject = 0.5f;
    private GameObject SpellObject;

    public int MaxChargeSpellTime = 3;
    private float _startedChargeTime = 0;

    public float TimeToReleaseSpell = 0.5f;
    private float _startedReleaseSpellTime = 0;
    
    private GUIStyle currentStyle;
    public Texture2D crosshairTexture;
    private Vector2 size = new Vector2(90, 10);
    public Texture2D progressBarEmpty;
    public Texture2D progressBarFull;

    // Use this for initialization
    void Start () {
        _hand = this.GetComponent<RigidHand>();
        Debug.Log(this.name + (_hand == null) );
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

    //private void OnGUI()
    //{
    //    InitStyles();

    //    GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width) / 2,
    //            (Screen.height - crosshairTexture.height) / 2, crosshairTexture.width, crosshairTexture.height)
    //           , crosshairTexture);

    //    if (_startedChargeTime > 0.1f)
    //    {
    //        // draw the background:
    //        GUI.BeginGroup(new Rect((Screen.width - crosshairTexture.width) / 2,
    //            (Screen.height - crosshairTexture.height) / 2 + crosshairTexture.height, size.x, size.y));

    //        GUI.Box(new Rect(0, 0, size.x, size.y), progressBarEmpty, currentStyle);

    //        // draw the filled-in part:
    //        GUI.BeginGroup(new Rect(0, 0, size.x * ((float)_startedChargeTime / MaxChargeSpellTime), size.y));
    //        GUI.Box(new Rect(0, 0, size.x, size.y), progressBarFull);
    //        GUI.EndGroup();

    //        GUI.EndGroup();

    //        Debug.Log(this.name + "Drawing");
    //    }
    //}

    // Update is called once per frame
    void Update ()
    {
        if (_hand == null)
            return;
        var gesture = _hand.CurrentGesture;
        Debug.Log(this.name + " "+ gesture);
        SpellAction(gesture);
    }

    void SpellAction(HandGesture gesture)
    {
        if (gesture == HandGesture.Closed)
        {
            ChargeSpell();
        }
        else if (gesture == HandGesture.Open)
        {
            ReleaseSpell();
        }
    }

    void ChargeSpell()
    {
        if (_startedChargeTime == 0)
            _startedChargeTime = LevelManager.GameTimer;
        
        SpellObject = ObjectPooling.Current.GetPooledObject();
        float percentaje = Mathf.Min( Mathf.Abs( (LevelManager.GameTimer - _startedChargeTime )/ MaxChargeSpellTime) , 1.0f);
        float scaleValue = MaxScaleSpellObject * percentaje;

        Debug.Log(this.name + ", Charging");
        SpellObject.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        SpellObject.transform.position = new Vector3(0, -0.150f, 0);
    }
    
    void ReleaseSpell()
    {
        if (_startedChargeTime == 0) return;

        if (_startedReleaseSpellTime == 0)
            _startedReleaseSpellTime = LevelManager.GameTimer;

        if (LevelManager.GameTimer - _startedReleaseSpellTime < TimeToReleaseSpell)
            return;
        _startedChargeTime = 0;
        _startedReleaseSpellTime = 0;

        Debug.Log(this.name + ", Shooting");
        //SpellObject.transform.localScale = new Vector3(0, 0, 0);
        SpellObject.GetComponent<SpellScript>().Cast();
    }

    void CastSpell()
    {

    }
}
