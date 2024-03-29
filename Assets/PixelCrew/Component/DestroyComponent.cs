using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class DestroyComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        public void DestroyObject()
        {
            Destroy(_objectToDestroy);
        }
    }
}