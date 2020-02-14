using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using UniRx;

using SpaceShooterPlus.LevelMvp;
using SpaceShooterPlus.MapMvp;
using SpaceShooterPlus.MarkerMvp;
using SpaceShooterPlus.MarkerStates;

namespace SpaceShooterPlus.GameMvp
{
    public class GamePresenter : MonoBehaviour
    {
        private MapModel mapModel;

        [SerializeField]
        private string saveFileLocation = "SaveGame.xml";

        public string SaveFileLocation
        {
            get => saveFileLocation;
            private set => saveFileLocation = value;
        }

        private async void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            await SceneManager.LoadSceneAsync("MapScene").AsObservable();

            var mapView = FindObjectOfType<MapView>();
            var mapModel = this.LoadMapModel() ?? this.MakeMapModel(mapView);
            var mapPresenter = new MapPresenter(mapView, mapModel);

            this.mapModel = mapModel;

            mapPresenter.OnButtonClickAsObservable
                .Subscribe(x => LoadFightSceneAsync(x));
        }

        private UnityAction<Scene, LoadSceneMode> CreateActionOnSceneLoaded(MarkerModel markerModel)
        {
            return delegate (Scene scene, LoadSceneMode mode)
            {
                if (!scene.isLoaded)
                {
                    return;
                }

                var index = mapModel.MarkerModels.IndexOf(markerModel);
                int requiredScore = 5 + (3 * index);
                int initialHealth = 3;

                var levelManager = FindObjectOfType<LevelManager>();
                levelManager.RequiredScore = requiredScore;
                levelManager.InitialHealth = initialHealth;
            };
        }

        private async Task LoadFightSceneAsync(MarkerModel markerModel)
        {
            var unityAction = this.CreateActionOnSceneLoaded(markerModel);

            SceneManager.sceneLoaded += unityAction;
            await SceneManager.LoadSceneAsync("FightScene").AsObservable();
            SceneManager.sceneLoaded -= unityAction;

            var levelManager = FindObjectOfType<LevelManager>();

            levelManager.OnButtonClickAsObservable
                .Subscribe(x => LoadMapSceneAsync(markerModel, x));

            levelManager.OnButtonClickAsObservable
                .Subscribe(_ => Debug.Log($"{nameof(GamePresenter)}.{nameof(levelManager.OnButtonClickAsObservable)}"));
        }

        private async Task LoadMapSceneAsync(MarkerModel markerModel, LevelModel levelModel)
        {
            await SceneManager.LoadSceneAsync("MapScene").AsObservable();

            var mapView = FindObjectOfType<MapView>();
            var mapModel = this.mapModel;

            if (levelModel != null)
            {
                bool isVictory = levelModel.IsVictory.Value;

                var markerState = markerModel.State.Value.GetType();
                bool markerAlreadyCompleted = markerState == typeof(MarkerStateCompleted);

                if (isVictory && !markerAlreadyCompleted)
                {
                    var currMarker = markerModel;
                    var currMarkerIndex = mapModel.MarkerModels.IndexOf(currMarker);

                    Debug.Log($"{currMarker} {currMarkerIndex} Unlocked -> Completed");
                    currMarker.Complete();

                    var nextMarkerIndex = currMarkerIndex + 1;
                    if (nextMarkerIndex <= mapModel.MarkerModels.Count)
                    {
                        var nextMarker = mapModel.MarkerModels[nextMarkerIndex];

                        Debug.Log($"{nextMarker} {nextMarkerIndex} Locked -> Unlocked");
                        nextMarker.Unlock();
                    }
                }
            }

            var mapPresenter = new MapPresenter(mapView, mapModel);

            this.mapModel = mapModel;

            mapPresenter.OnButtonClickAsObservable
                .Subscribe(x => LoadFightSceneAsync(x));

            mapPresenter.OnButtonClickAsObservable
                .Subscribe(_ => Debug.Log($"{nameof(GamePresenter)}.{nameof(mapPresenter.OnButtonClickAsObservable)}"));
        }

        private void OnDestroy()
        {
            //SaveMapModel(this.mapModel);
        }

        private IEnumerator LoadAsyncScene(string scene)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        private MapModel MakeMapModel(MapView mapView)
        {
            var mapModel = new MapModel();

            int viewsCount = mapView.MarkerViews.Count;
            if (viewsCount > 0)
            {
                //First level must be unlocked
                var markerModel = new MarkerModel(new MarkerStateUnlocked());
                mapModel.MarkerModels.Add(markerModel);
            }
            for (int i = 1; i < viewsCount; i++)
            {
                //Remaining levels should be locked
                var markerModel = new MarkerModel(new MarkerStateLocked());
                mapModel.MarkerModels.Add(markerModel);
            }

            return mapModel;
        }

        private MapModel LoadMapModel()
        {
            if (!File.Exists(this.SaveFileLocation)) { return null; }

            Stream stream = File.Open(this.SaveFileLocation, FileMode.Open);
            var formatter = new BinaryFormatter();
            var newMapModel = (MapModel)formatter.Deserialize(stream);
            stream.Close();
            return newMapModel;
        }

        private void SaveMapModel(MapModel oldMapModel)
        {
            Stream stream = File.Open(this.SaveFileLocation, FileMode.Create);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, oldMapModel);
            stream.Close();
        }
    }
}
