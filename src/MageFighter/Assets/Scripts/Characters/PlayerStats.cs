
using UnityEngine;

namespace Assets.Scripts.Characters
{
    class PlayerStats: MonoBehaviour
    {
        public int health;

        public int points;

        public void AddPoints(int _points)
        {
            points += _points;
        }
    }
}
