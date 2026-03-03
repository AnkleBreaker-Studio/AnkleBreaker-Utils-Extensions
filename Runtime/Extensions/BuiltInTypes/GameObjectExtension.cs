using System.Collections.Generic;
using UnityEngine;

namespace AnkleBreaker.Utils.Extensions
{
    public static class GameObjectExtension
    {
        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
                child.gameObject.SetLayerRecursively(layer);
        }
		
		public static void SetTagRecursively(this GameObject obj, string tag)
        {
            obj.tag = tag;

            foreach (Transform child in obj.transform)
                child.gameObject.SetTagRecursively(tag);
        }

        public static Bounds CalculateObjectBounds(this GameObject obj, List<Collider> collidersToExclude = null)
        {
            Collider[] colliders = obj.GetComponentsInChildren<Collider>(true);
            bool hasExclusions = collidersToExclude != null && collidersToExclude.Count > 0;
            
            Bounds bounds = default;
            bool initialized = false;
            
            for (int i = 0; i < colliders.Length; i++)
            {
                if (hasExclusions && collidersToExclude.Contains(colliders[i]))
                    continue;
                    
                if (!initialized)
                {
                    bounds = colliders[i].bounds;
                    initialized = true;
                }
                else
                {
                    bounds.Encapsulate(colliders[i].bounds);
                }
            }
            
            return initialized ? bounds : new Bounds(obj.transform.position, Vector3.zero);
        }
    }
}
