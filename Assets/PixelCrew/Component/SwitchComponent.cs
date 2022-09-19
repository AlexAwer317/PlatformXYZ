using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator; //Ссылка на аниматор
        [SerializeField] private bool _state; //Чек бокс включения или выключения стейта
        [SerializeField] private string _animationKey; //Ключ анимации
        

        //Метод изменения стейта и анимационного ключа
        public void Switch()
        {
            _state =!_state;
            _animator.SetBool(_animationKey, _state);
        }


        //Вывод метода свитч в интерфейс Юнити
        [ContextMenu("Switch")]
        public void SwitchIt()
        {
            Switch();
        }
    }
}