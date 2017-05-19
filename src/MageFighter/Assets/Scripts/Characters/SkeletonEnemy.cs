using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    class SkeletonEnemy : Enemy
    {
        public AudioClip startAudioClip = null;

        protected new void Start()
        {
            _initialHealth = Health;
            _animator = this.GetComponent<Animator>();
            this.GetComponent<AudioSource>().PlayOneShot(startAudioClip);
        }

        protected new void CheckStatus()
        {
            if (IsDead)
            {
                if (_animator != null)
                    _animator.SetBool("IsDead", true);

                var body = this.GetComponent<Rigidbody>();
                if (body != null)
                {
                    body.detectCollisions = false;
                    body.useGravity = false;
                }

                if (!IsRewardGiven)
                {
                    var playerStats = Player.GetComponent<PlayerStats>();
                    playerStats.AddPoints(points);
                    IsRewardGiven = true;
                }
                Debug.Log("Killing skeleton");
                Destroy();
            }
        }
    }
}
