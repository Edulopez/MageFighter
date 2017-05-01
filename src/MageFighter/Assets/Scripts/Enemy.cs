using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int Damage = 1;
    public int Speed = 1;
    public int Health = 1;
    public bool CanMove = false;

    Rigidbody _rigibody = null;
    Rigidbody LocalRigibody
    {
        get
        {
            if (_rigibody == null)
            {
                var res = this.GetComponent<Rigidbody>();
                if (res != null)
                    _rigibody = res;
            }
            return _rigibody;
        }
    }


    static GameObject _player = null;
    public GameObject Player
    {
        get
        {
            if (_player == null)
            {
                var res = GameObject.FindGameObjectsWithTag("Player");
                if (res != null && res.Length > 0)
                    _player = res[0];
            }
            return _player;
        }
    }

	void Start ()
    {
	
	}

    void OnDisable()
    {
        CancelInvoke();
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

    void Update ()
    {
        CheckStatus();
        FacePlayer();

        if(CanMove)
        {
            this.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
        if(Vector3.Distance(this.transform.position,_player.transform.position) <= 1)
        {
            CanMove = false;
            LocalRigibody.velocity = Vector3.zero;
        }
        FixPosition();
    }
    //@TODO Remove this
    void FixPosition()
    {
        if (this.transform.position.y < 0)
            this.transform.position = new Vector3(this.transform.position.x, 5, this.transform.position.z);
    }

    void FacePlayer()
    {
        this.transform.LookAt(Player.transform);
    }

    void CheckStatus()
    {
        if (Health <= 0)
        {
            Invoke("Destroy", 4f);
            var animator = this.GetComponent<Animator>();
            if(animator != null)
                animator.SetBool("IsDead", true);
        }
    }

    public void GetHit(int damage)
    {
        Health -= damage;
    }
}
