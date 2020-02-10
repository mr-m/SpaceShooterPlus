using SpaceShooterPlus.MarkerMvp;
using UniRx;
using UnityEngine;

namespace SpaceShooterPlus.MapMvp
{
    [ExecuteAlways]
    [RequireComponent(typeof(ChildListReactive))]
    public class MapView : MonoBehaviour
    {
        public IReadOnlyReactiveCollection<MarkerView> MarkerViews { get => this.markerViews; }

        [SerializeField]
        private ReactiveCollection<MarkerView> markerViews;

        private void Awake()
        {
            Debug.Log($"{nameof(MapView)}.{nameof(Awake)}()");

            var childList = GetComponent<ChildListReactive>();

            this.markerViews = new ReactiveCollection<MarkerView>();

            childList.Children
                .ObserveAdd()
                .Select(x => x.Value.GetComponent<MarkerView>())
                .Where(x => x != null)
                .Subscribe(x => this.markerViews.Add(x));
        }
    }
}
