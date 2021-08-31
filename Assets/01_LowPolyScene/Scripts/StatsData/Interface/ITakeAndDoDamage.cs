using System.Xml.Serialization;
using UnityEngine;

namespace OpenWorldDemo.LowPolyScene
{
    //T和K之间的交互
    public interface ITakeAndDoDamage<T,K> where T:BaseStats where K: BaseStats
    {
        int TakeDamage(T attacker, K defender,bool isCritical);
    }
}