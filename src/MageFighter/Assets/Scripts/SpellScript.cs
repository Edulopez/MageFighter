using UnityEngine;
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
        Invoke("DestroySpell", 20f);
    }

    void OnDisabled()
    {
        CancelInvoke();
    }
    void DestroySpell()
    {
        Debug.Log("Destroying spell");
        gameObject.SetActive(false);
    }

    public void Cast(float percentageOfPower)
    {
        damage = _maxDamage * percentageOfPower;
        Cast();
    }

    public void Cast()
    {
        Invoke("DestroySpell", 3f);
        Casted = true;
    }

    public void Update()
    {
        Direction = Vector3.forward;
        if (Casted)
        {
            Direction.z = Direction.z * speed * Time.deltaTime;
            this.transform.Translate(Direction);
        }
    }
}
