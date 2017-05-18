using UnityEngine;
using System.Collections;
using Assets.Scripts.Characters;

public class Enemy : MonoBehaviour {

    public int Damage = 1;
    public int Speed = 1;
    public int BackSpeed = 5;
    public int points = 2;

    protected int _initialHealth;
    public int Health = 50;
    public bool CanMove = false;

    protected Animator _animator;
    protected bool _isTakingDamage = false;

    [HideInInspector]
    public Rigidbody _rigibody = null;

    [HideInInspector]
    public Rigidbody LocalRigibody
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

    [HideInInspector]
    public static GameObject _player = null;
    public GameObject Player
    {
        get
        {
            if (_player == null)
            {
                var res = GameObject.FindGameObjectsWithTag("Player");

                //Debug.Log(res[0].name);
                if (res != null && res.Length > 0)
                    _player = res[0];
            }
            return _player;
        }
    }

    protected bool IsRewardGiven = false;
    public bool IsDead
    {
        get
        { return Health < 1; }
    }

    public AudioClip HurtSound = null;

    protected void Start ()
    {
        _initialHealth = Health;
        _animator = this.GetComponent<Animator>();
    }

    protected void OnDisable()
    {
        CancelInvoke();
    }

    protected void Destroy()
    {
        Reset();
        gameObject.SetActive(false);
    }
    protected void Reset()
    {
        IsRewardGiven = false;
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

    protected void Update ()
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
    protected void FixPosition()
    {
        if (this.transform.position.y < -10)
            Destroy();
    }

    protected void FacePlayer()
    {
        this.transform.LookAt(Player.transform);
    }

    protected void CheckStatus()
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

            if(!IsRewardGiven)
            {
                var playerStats = Player.GetComponent<PlayerStats>();
                playerStats.AddPoints(points);
                IsRewardGiven = true;
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

    protected IEnumerator SetAnimationVariable(string varNAme , float time, bool value)
    {
        yield return new WaitForSeconds(1f);
        _animator.SetBool(varNAme, false);
    }
}
