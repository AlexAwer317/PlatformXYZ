using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    
    public class EnterTriggerComponent : MonoBehaviour
    {

        [SerializeField] private string _tag; //Тэг объекта с которым определяем взаимодействие
        [SerializeField] private EnterEvent _action; //Ивент, запускаемый при столкновение

        //Определение столкновения
         private void OnTriggerEnter2D(Collider2D other)
         {
            if(other.gameObject.CompareTag(_tag))
            {
                    _action?.Invoke(other.gameObject);
            }
         }
    }
}