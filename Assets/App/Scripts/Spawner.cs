using UniRx;
using UnityEngine;

namespace SpaceShooterPlus
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private Rect spawnArea;

        [SerializeField]
        public GameObject hazard;

        [SerializeField]
        public float spawnInterval = 1;

        public readonly Subject<GameObject> Spawned = new Subject<GameObject>();

        private void Start()
        {
            Observable.Timer(System.TimeSpan.FromSeconds(spawnInterval))
                .Repeat()
                .Where(_ => this.gameObject.activeInHierarchy)
                .Subscribe(_ => this.SpawnHazard());
        }

        private void SpawnHazard()
        {
            var spawnPosition = new Vector3
            {
                x = Random.Range(spawnArea.x, spawnArea.x + spawnArea.width),
                y = Random.Range(spawnArea.y, spawnArea.y + spawnArea.height),
                z = 0,
            };

            Quaternion spawnRotation = Random.rotation;
            var gameObject = Instantiate(hazard, spawnPosition, spawnRotation);

            this.Spawned.OnNext(gameObject);
        }
    }
}
