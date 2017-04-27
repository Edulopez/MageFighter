using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;

public class SpellCastingScript : MonoBehaviour {


    RigidHand _hand;
    public GameObject Palm;
    public float MaxScaleSpellObject = 0.5f;
    private GameObject SpellObject;

    public int MaxCastingSpellTime = 3;
    private float _CastingTime = 0;

    public float TimeToReleaseSpell = 0.01f;
    private float _ReleaseSpellTime = 0;
    
    private GUIStyle currentStyle;
    public Texture2D crosshairTexture;
    private Vector2 size = new Vector2(90, 10);
    public Texture2D progressBarEmpty;
    public Texture2D progressBarFull;
    
    void Start () {
        _hand = this.GetComponent<RigidHand>();
        //Debug.Log(this.name + (_hand == null) );
	}
    void Destroy()
    {
        Debug.Log(this.name + " Destroyed ");
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
       // Debug.Log(this.name + " "+ gesture);
        MakeSpellAction(gesture);
    }

    /// <summary>
    /// Make the spell actions depending on the gesture
    /// </summary>
    /// <param name="gesture"></param>
    void MakeSpellAction(HandGesture gesture)
    {
        if(SpellObject != null)
        {
            SpellObject.transform.position = Palm.transform.position;
            SpellObject.transform.rotation = Palm.transform.rotation;

        }

        if (gesture == HandGesture.Missing)
        {
            ReleaseSpell();
        }
        else if(gesture == HandGesture.Closed)
        {
            CastSpell();
        }
        else if (gesture == HandGesture.Open)
        {
            ReleaseSpell();
        }
        
    }

    /// <summary>
    /// Start the spell casting process
    /// </summary>
    void CastSpell()
    {
        if (_CastingTime == 0)
            _CastingTime = LevelManager.GameTimer;
        
        if (SpellObject == null)
            SpellObject = ObjectPooling.Current.GetPooledObject();

        float percentaje = Mathf.Min( Mathf.Abs( (LevelManager.GameTimer - _CastingTime )/ MaxCastingSpellTime) , 1.0f);
        float scaleValue = MaxScaleSpellObject * percentaje;

        Debug.Log(this.name + ", Charging");
        
        SpellObject.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    }
    
    /// <summary>
    /// Release casted spell
    /// </summary>
    void ReleaseSpell( bool CheckReleaseTime = true)
    {
        if (_CastingTime == 0) return;

        if (_ReleaseSpellTime == 0)
            _ReleaseSpellTime = LevelManager.GameTimer;

        Debug.Log(this.name + ", Shooting");

        // Cast spell and release object
        SpellObject.GetComponent<SpellScript>()
            .Cast(
             Mathf.Min(1f,_CastingTime/MaxCastingSpellTime)
            ,1f, 10f
            );

        SpellObject = null;
        // Reset variables
        ResetTimerVariables();
    }

    private void ResetTimerVariables()
    {
        _CastingTime = 0;
        _ReleaseSpellTime = 0;
    }
}
