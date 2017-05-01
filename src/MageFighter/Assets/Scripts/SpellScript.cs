using UnityEngine;
using System.Collections;

public class SpellScript : MonoBehaviour {

    private int _maxDamage = 100;
    public int damage = 10;

    private int _maxSpeed = 10;
    public float speed = 1;

    private Vector3 Direction = Vector3.forward;

    public bool Casted = false;

    public AudioClip CastinAudio;
    public AudioClip ReleaseAudio;
    
    void OnEnable()
    {
        var audio = this.GetComponent<AudioSource>();

        if (audio != null)
        {
            audio.clip = CastinAudio;
            audio.loop = true;
            audio.Play();
        }
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
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Cast spell and destroy it after a few seconds
    /// </summary>
    public void Cast(float percentageOfPower,float spellSpeed = 1, float destroyTime = 10f)
    {
        damage =  (int) ( Mathf.Max(1,(_maxDamage * percentageOfPower)));
        Debug.Log("Casting spell " + damage);
        var audio = this.GetComponent<AudioSource>();

        if (audio != null)
        {
            audio.clip = ReleaseAudio;
            audio.loop = false;
            audio.Play();
        }

        Invoke("DestroySpell", destroyTime);
        Casted = true;
    }
 

    void OnCollisionEnter(Collision collitionInfo)
    {
        if (!Casted)
            return;

        if(collitionInfo.gameObject.tag == "Enemy")
        {
            //Debug.Log("Enemy hit)");
            var enemy = collitionInfo.gameObject.GetComponent<Enemy>();
            if (!enemy.IsDead)
            {
                Destroy();
                enemy.GetHit(damage);
            }
        }

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
