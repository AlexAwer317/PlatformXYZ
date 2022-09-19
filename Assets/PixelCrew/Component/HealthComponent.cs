using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health; //Значение жизней
        [SerializeField] private UnityEvent _onDamage; //Запускаемый ивент при получение урона
        [SerializeField] private UnityEvent _onDie; //Ивент при смерти героя
        private int _healthMax; //Максимально возможное значение ХП
        
       
        
        private void Awake() 
        {
            _healthMax = _health; //Присваиваем значение переменной Максимального значение на старте игры
        }


        //Метод нанесения урона
        public void ApplyDamage(int damageValue)
        {
            _health -= damageValue;
            _onDamage?.Invoke();

            if(_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        //Метод востановления
        public void ApplyHeal(int healValue)
        {
            _health += healValue;
            if(_health > _healthMax)
            {
                _health = _healthMax;
            }
            // if(_health < _healthMax)
            // {
            // _health += healValue - _health;
            // }

        }
        
    }
}


