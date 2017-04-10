using UnityEngine;
using System.Collections;

using Leap;
using Leap.Unity;

public class SpellScript : MonoBehaviour {

    RigidHand _hand;
    public float MaxScaleSpellObject = 0.5f;
    public GameObject SpellObject;

    public int MaxChargeSpellTime = 3;
    private float _startedChargeTime = 0;

	// Use this for initialization
	void Start () {
        _hand = this.GetComponent<RigidHand>();
        Debug.Log(this.name + (_hand == null) );
	}
	
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
            ShootSpell();
        }
    }

    void ChargeSpell()
    {
        if (_startedChargeTime == 0)
            _startedChargeTime = LevelManager.GameTimer;

        float percentaje = Mathf.Min( Mathf.Abs( (LevelManager.GameTimer - _startedChargeTime )/ MaxChargeSpellTime) , 1.0f);
        float scaleValue = MaxScaleSpellObject * percentaje;

        Debug.Log(this.name + ", Charging" + scaleValue);
        SpellObject.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    }
    
    void ShootSpell()
    {
        if (_startedChargeTime == 0) return;

        Debug.Log(this.name + ", Shooting");
        _startedChargeTime = 0;
        SpellObject.transform.localScale = new Vector3(0, 0, 0);
    }
}
