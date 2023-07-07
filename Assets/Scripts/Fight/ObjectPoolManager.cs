using Core.EventSystem;
using Fight.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Fight
{
    public class ObjectPoolManager
    {
        static Dictionary<GameObject, ObjectPool> m_objectPools=new Dictionary<GameObject, ObjectPool>();

        public static GameObject Spawn(GameObject aPrefab, Vector3 aPosition, Quaternion aRotation)
        {
            return m_objectPools[aPrefab].Spawn(aPosition, aRotation);
        }

        public static void DeSpawn(GameObject aPrefab, GameObject aInstance)
        {
            m_objectPools[aPrefab].DeactivateObject(aInstance);
        }
    
        public static ObjectPool CreateObjectPool(GameObject aPrefab,int amount)
        {
            //Create new instance if NOT in the list already
            if(!m_objectPools.ContainsKey(aPrefab))
            { 
                m_objectPools.Add(aPrefab, new ObjectPool(aPrefab, amount));
            }

            return m_objectPools[aPrefab];
        }

    }
}