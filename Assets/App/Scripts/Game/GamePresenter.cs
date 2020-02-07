using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.SceneManagement;

using UniRx;

using SpaceShooterPlus.MapMvp;
using SpaceShooterPlus.MarkerMvp;

namespace SpaceShooterPlus.GameMvp
{
    public class GamePresenter : MonoBehaviour
    {
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

            var mapPresenter = FindObjectOfType<MapPresenter>();

            var markerViews = await mapPresenter.ViewMarkers.ToList();

            var mapModelLoaded = this.LoadMapModel();
            var mapModelNew = new MapModel(mapPresenter.ViewMarkers);
            var mapModel = mapModelLoaded ?? mapModelNew;

            var markerModels = await mapModel.MarkerModels.ToList();

            var zip = Observable.Zip(
                markerViews.ToObservable(),
                markerModels.ToObservable(),
                (x, y) => new MarkerPresenter(x, y)
            );

            var markerPresenters = await zip.ToList();

            //SaveGameModel(mapModel);
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
