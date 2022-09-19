 using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{

    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string _tag; //Тэг объекта с которым случится колизия
        [SerializeField] private EnterEvent _action; //Ивент запускаемый по случаю колизии

        //Определение колизии
        private void OnCollisionEnter2D(Collision2D other)    
        {
            if(other.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(other.gameObject);
            }
        }
        
        //Класс получающий ивенты
        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {

        }
    }
}