using UnityEngine;

namespace FirstARPG.Miscs
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;
        public bool Switch;

        static bool _hasSpawned = false;

        private void Awake() {
            if (_hasSpawned || !Switch) return;

            SpawnPersistentObjects();

            _hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}