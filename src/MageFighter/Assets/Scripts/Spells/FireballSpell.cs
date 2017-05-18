using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using DigitalRuby.PyroParticles;
using UnityEngine;

namespace Assets.Scripts.Spells
{
    class FireballSpell : FireProjectileScript, ISpell
    {

        public AudioClip CastinAudio;


        private int _maxDamage = 100;
        public int Damage
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public AudioClip ReleaseAudio;


        public float Speed
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsCasted = false;

        void OnEnable()
        {
            IsCasted = false;
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

        public void Cast(float percentageOfPower, float spellSpeed = 1, float destroyTime = 10)
        {
            throw new NotImplementedException();
        }


        public void Cast(Vector3 direction, float percentageOfPower, float spellSpeed = 1, float destroyTime = 10)
        {
            throw new NotImplementedException();
        }

        void OnCollisionEnter(Collision collitionInfo)
        {
            if (IsCasted)
                return;

            if (collitionInfo.gameObject.tag == "Enemy")
            {
                //Debug.Log("Enemy hit)");
                var enemy = collitionInfo.gameObject.GetComponent<Enemy>();
                if (enemy == null) return;
                if (!enemy.IsDead)
                {
                    //if (Damage / _maxDamage <= 0.5)
                    //    Destroy();
                    enemy.GetHit(Damage);
                }
            }
        }
    }
}
