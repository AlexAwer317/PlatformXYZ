using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action; //Ссылка на юнити ивент


        //Метод активации взаимодействия
        public void Interact()
        {
            _action?.Invoke();
        }
    }
}
