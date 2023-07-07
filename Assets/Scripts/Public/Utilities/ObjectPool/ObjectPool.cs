using UnityEngine;

public struct ObjectPool
{
    private GameObject prefab;

    private GameObject[] pool_Objects;
    
    int lastIndex;//points to the element that was last pulled and activated(end index for active objects (0..lastIndex))
    public ObjectPool(GameObject aPrefab, int aCount)
    {
        prefab = MonoBehaviour.Instantiate(aPrefab);
        prefab.SetActive(false);

        pool_Objects = new GameObject[aCount];

        lastIndex = -1;

        for (int i = 0; i < aCount; i++)
        {
            GameObject obj = MonoBehaviour.Instantiate(prefab);
            obj.SetActive(false);
            pool_Objects[i] = obj;
        }
    }

    public GameObject PullObjectFromPool()
    {
        //Shift the lastIndex by one and return that element in the array to activate the gameObject
        if (lastIndex < pool_Objects.Length - 1)
        {
            lastIndex++;
         
            return pool_Objects[lastIndex];
        }
        else
            return null;

    }

    public void DeactivateFirstObject()        //Sort the pool_object and point to the new avaiblable object
    {
        DeactivateObject(pool_Objects[0]);
    }

    public void DeactivateObject(GameObject aObject)        //Find the requested object in the active region of the array and deactivates it 
    {
        for (int i = 0; i < lastIndex; i++)
        {
            //find the requested object and swap it with the object before the lastindex and then move the lastIndex by -1

            if (pool_Objects[i] == aObject)
            {
                GameObject swapObj = pool_Objects[i];

                swapObj.SetActive(false);

                pool_Objects[i] = pool_Objects[lastIndex];
                pool_Objects[lastIndex] = swapObj;

                lastIndex--;
             
                break;
            }
        }
    }
    
    public GameObject Spawn(Vector3 aPosition, Quaternion aRotation)
    {
        GameObject obj = PullObjectFromPool();
        
        if (obj == null)
            return null;
        
        obj.transform.position = aPosition;
        obj.transform.rotation = aRotation;

        obj.SetActive(true);


        return obj;
    }

    public GameObject[] GetActiveObjects()
    {
        GameObject[] enabledObj=new GameObject[lastIndex+1];
        for (int i = 0; i <= lastIndex; i++)
        {
            enabledObj[i] = pool_Objects[i];
        }

        return enabledObj;
    }

    public GameObject[] GetObjects()
    {
        GameObject[] enabledObj = new GameObject[pool_Objects.Length];
        for (int i = 0; i < pool_Objects.Length; i++)
        {
            enabledObj[i] = pool_Objects[i];
        }

        return enabledObj;
    }

    public int GetTotalCount()
    {
        return pool_Objects.Length;
    }

    public int GetActiveObjectCount()
    {
        return lastIndex;
    }
}
