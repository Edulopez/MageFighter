using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int Damage = 1;
    public int Speed = 1;
    public int BackSpeed = 5;

    private int _initialHealth;
    public int Health = 50;
    public bool CanMove = false;

    private Animator _animator;
    private bool _isTakingDamage = false;

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

                Debug.Log(res[0].name);
                if (res != null && res.Length > 0)
                    _player = res[0];
            }
            return _player;
        }
    }

    public bool IsDead
    {
        get
        { return Health < 1; }
    }

    public AudioClip HurtSound = null;

	void Start ()
    {
        _initialHealth = Health;
        _animator = this.GetComponent<Animator>();
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Destroy()
    {
        Reset();
        gameObject.SetActive(false);
    }
    void Reset()
    {
        CanMove = true;
        Health = _initialHealth;
        _animator.SetBool("IsTakingDamage", false);
        _animator.SetBool("IsDead", false);
        LocalRigibody.velocity = Vector3.zero;
        var body = this.GetComponent<Rigidbody>();
        if (body != null)
        {
            body.detectCollisions = true;
            body.useGravity = true;
        }
    }

    void Update ()
    {
        CheckStatus();
        FacePlayer();
        if (_isTakingDamage && CanMove)
        {
            this.transform.Translate(Vector3.back * BackSpeed * Time.deltaTime);
        }
        else if (CanMove)
        {
            this.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }

        if (Vector3.Distance(this.transform.position, _player.transform.position) <= 4)
        {
            CanMove = false;
            LocalRigibody.velocity = Vector3.zero;
        }
        else
            CanMove = true;

        FixPosition();
    }

    //@TODO Remove this
    void FixPosition()
    {
        if (this.transform.position.y < -10)
            Destroy();
    }

    void FacePlayer()
    {
        this.transform.LookAt(Player.transform);
    }

    void CheckStatus()
    {
        if (IsDead)
        {
            Invoke("Destroy", 4f);
            if(_animator != null)
                _animator.SetBool("IsDead", true);

            var body = this.GetComponent<Rigidbody>();
            if (body != null)
            {
                body.detectCollisions = false;
                body.useGravity = false;
            }
        }
    }

    public void GetHit(int damage)
    {
        Health -= damage;

        if (_animator != null)
            _animator.SetBool("IsTakingDamage", true);

        this.transform.Translate(Vector3.back * Time.deltaTime);

       var audioSource = this.GetComponent<AudioSource>();
        audioSource.PlayOneShot(HurtSound);

        StartCoroutine(SetAnimationVariable("IsTakingDamage", 1f, false));
    }

    IEnumerator SetAnimationVariable(string varNAme , float time, bool value)
    {
        yield return new WaitForSeconds(1f);
        _animator.SetBool(varNAme, false);
    }
}
