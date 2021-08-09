using UnityEngine;

namespace OpenWorldDemo.LowPolyScene
{
    [CreateAssetMenu(fileName = "New EnemyData", menuName = "LowPoly/Enemy/Stats")]
    public class EnemyStatsData_SO: CharacterStatsData_SO
    {

        [Header("Enemy Info")]
        public EnemyType Type;
        public bool IsGuard;
        public float SightRange;
        public float MaxChaseRange;

        [Header("Patrol State")]
        public float PatrolRange;
        public float LookAroundTime;

    }
}