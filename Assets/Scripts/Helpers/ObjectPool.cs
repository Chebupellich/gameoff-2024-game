using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
   public List<GameObject> PrefabsForPool;
   
   private List<GameObject> _pooledObjects = new List<GameObject>();

   public GameObject GetObjectFromPool(string objectName)
   {
      var instance = _pooledObjects.FirstOrDefault(obj => obj.name == objectName);
      
      if (instance != null)
      {
         _pooledObjects.Remove(instance);
         
         instance.SetActive(true);
         
         return instance;
      }
      
      var prefab = PrefabsForPool.FirstOrDefault(obj => obj.name == objectName);
      if (prefab != null)
      {
         var newInstace = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
         
         newInstace.name = objectName;
         
         newInstace.transform.localPosition = Vector3.zero;
         
         return newInstace;
      }
      
      Debug.LogWarning("Prefab with name " + objectName + " not found");
      return null;
   }

   public void PoolObject(GameObject obj)
   {
      obj.SetActive(false);
      
      _pooledObjects.Add(obj);
   }
}
