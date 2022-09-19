using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{

    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero; //Ссылка на скрипт героя

/*
   private void Update() 
   {
        var horizontal = Input.GetAxis("Horizontal");
        _hero.SetDirection(horizontal);
        
        if(Input.GetKey(KeyCode.A))
        {
            _hero.SetDirection(-1);
        } 
        else if(Input.GetKey(KeyCode.D))
        {
            _hero.SetDirection(1);
        }
        else
        {
            _hero.SetDirection(0);
        }

        if(Input.GetButtonUp("Fire2"))
        {
            _hero.SaySomething();
        }
   }*/

        //Горизонтальное перемещение
        public void OnHorizontalMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        //Активация действия по кнопке
        public void OnSaySomething(InputAction.CallbackContext context)
        {
            if(context.canceled)
                _hero.SaySomething();
        }

        //Взаимодействие с активными объектами
        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.canceled)
                _hero.Interact();
        }
    }
}
