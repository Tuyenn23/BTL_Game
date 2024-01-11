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

        private GameObject currentLevelGameObject;

        [SerializeField]
        private DataWaveSO dataWaveSO;
        [SerializeField]
        public List<GameObject> listWaves;

        private static AddressablesUtils instance;

        public static AddressablesUtils Instance { get => instance; }

        private void Awake()
        {
            instance = this;
        }

        public void SpawnObject(int indexLevel, int indexWave)
        {
            StartCoroutine(Spawn(indexLevel, indexWave));
        }

/*        private IEnumerator Spawn(int index)
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

        }*/

        private IEnumerator Spawn(int indexLevel, int indexWave)
        {
            if(listWaves.Count == 0)
            {
                listWaves = dataWaveSO.GetListWaveByIdLevel(indexLevel);
            }
            yield return null;
            if (currentLevelGameObject != null)
            {
                currentLevelGameObject.SetActive(false);
                Destroy(currentLevelGameObject);
            }
            GameObject objectLevel = listWaves[indexWave];
            GameObject level = Instantiate(objectLevel);
            currentLevelGameObject = level;
        }
    }
}