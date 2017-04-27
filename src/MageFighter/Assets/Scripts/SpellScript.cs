﻿using UnityEngine;
using System.Collections;

public class SpellScript : MonoBehaviour {

    private int _maxDamage = 500;
    public float damage = 10;

    private int _maxSpeed = 10;
    public float speed = 1;

    private Vector3 Direction = Vector3.forward;

    public bool Casted = false;

    void OnEnable()
    {
        Invoke("DestroySpell", 200f);
    }
    void OnDisable()
    {
        CancelInvoke();
    }
    void Destroy()
    {
        DestroySpell();
    }
    void DestroySpell()
    {
        Debug.Log("Destroying spell");
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Cast spell and destroy it after a few seconds
    /// </summary>
    public void Cast(float percentageOfPower,float spellSpeed = 1, float destroyTime = 10f)
    {
        damage = _maxDamage * percentageOfPower;
        Cast();
    }

    /// <summary>
    /// Cast spell and destroy it after a few seconds
    /// </summary>
    public void Cast(  float spellSpeed = 1, float destroyTime = 10f)
    {
        Invoke("DestroySpell", destroyTime);
        Casted = true;
    }

    public void Update()
    {
        if (Casted)
        {
            Direction = Vector3.down;
            Direction = Direction * speed * Time.deltaTime;
            this.transform.Translate(Direction);
        }
    }
}
