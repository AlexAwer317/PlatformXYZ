using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using PixelCrew.Utils;



namespace PixelCrew
{ 
    
    public class CheckCircleOverlap : MonoBehaviour 
    {
        [SerializeField] private float _radius;
        
        private Collider2D[] _interactionResult = new Collider2D[1];

        public GameObject[] GetObjectsInRange()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactionResult);

            var overLaps = new List<GameObject>();
            for (int i = 0; i < size; i++)
            {
                overLaps.Add(_interactionResult[i].gameObject);
            }

            return overLaps.ToArray();
        }    

        private void OnDrawGizmosSelected() 
        {
            Handles.color = HandlesUtils.TransparetRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
    }
}