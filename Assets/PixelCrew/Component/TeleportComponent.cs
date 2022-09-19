using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{

    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform; //Конечное место назначения телепорта


        //Метод телепорта
        public void Teleport(GameObject target)
        {
            target.transform.position = _destTransform.position; //Передаем значение трансформа точки назначения телепортируемому объекту
        }
    }
}