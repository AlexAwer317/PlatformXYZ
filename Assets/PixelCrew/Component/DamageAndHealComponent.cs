using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{

    public class DamageAndHealComponent : MonoBehaviour
    {
        [SerializeField] private int _damage; //Размер урона
        [SerializeField] private int _heal; //Размер хила
        

        //Урон
        public void ApplyDamage(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
            healthComponent.ApplyDamage(_damage);
            }
        }

        //ХИл
        public void ApplyHeal(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if(healthComponent != null)
            {
                healthComponent.ApplyHeal(_heal);
            }
        }
    }
}
