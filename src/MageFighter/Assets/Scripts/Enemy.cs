using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int Damage = 1;
    public int Speed = 1;
    public int Health = 1;
    public bool CanMove = false;

    Rigidbody _rigibody;
    GameObject _player = null;
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
