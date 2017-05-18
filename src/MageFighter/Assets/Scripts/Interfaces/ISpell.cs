using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ISpell
    {

        //int Damage { get; set; }

        //float Speed { get; set; }

        //bool IsCasted{ get; set; }

        //AudioClip CastinAudio { get; set; }

        //AudioClip ReleaseAudio { get; set; }

        void Cast(float percentageOfPower, float spellSpeed = 1, float destroyTime = 10f);
        void Cast(Vector3 direction, float percentageOfPower, float spellSpeed = 1, float destroyTime = 10f);

    }
}
