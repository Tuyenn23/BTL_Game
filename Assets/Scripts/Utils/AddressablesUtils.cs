using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TigerForge;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game_DrawCar
{
    public class AddressablesUtils : MonoBehaviour
    {

        [SerializeField] private List<AssetReference> _particleReferences;
        private GameObject currentLevelGameObject;

        public void SpawnObject(int index)
        {
            StartCoroutine(Spawn(index));
        }

        private IEnumerator Spawn(int index)
        {
            AsyncOperationHandle<GameObject> request = Addressables.LoadAssetAsync<GameObject>(_particleReferences[index]);
            yield return request;
            if(currentLevelGameObject != null)
            {
                currentLevelGameObject.SetActive(false);
                Destroy(currentLevelGameObject);
            }
            GameObject objectLevel = request.Result;
            GameObject level = Instantiate(objectLevel);
            currentLevelGameObject = level;

        }
    }
}