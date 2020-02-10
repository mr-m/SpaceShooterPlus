using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
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
                .Subscribe(async x =>
                {
                    Debug.Log($"{nameof(mapPresenter.OnButtonClickAsObservable)} {x}");

                    await SceneManager.LoadSceneAsync("FightScene").AsObservable();

                    var index = mapModel.MarkerModels.IndexOf(x);
                    int requiredScore = 5 + (3 * index);
                    int initialHealth = 3;

                    var levelManager = FindObjectOfType<LevelManager>();
                    levelManager.RequiredScore = requiredScore;
                    levelManager.InitialHealth = initialHealth;

                    levelManager.OnButtonClickAsObservable
                        .Subscribe(y =>
                        {
                            SceneManager.UnloadSceneAsync("FightScene");
                            x.Complete();
                        });

                    //var levelView = FindObjectOfType<LevelView>();
                    //var levelModel = new LevelModel(requiredScore, initialHealth);
                    //var levelPresenter = new LevelPresenter(levelView, levelModel);
                });

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
