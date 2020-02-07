using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace SpaceShooterPlus.MapMvp
{
    [ExecuteAlways]
    [RequireComponent(typeof(ChildList))]
    class MapPresenter : MonoBehaviour
    {
        public IObservable<Button> ViewMarkers;

        private void Start()
        {
            var childList = GetComponent<ChildList>();

            this.ViewMarkers = childList.Children
                .ToObservable()
                .Select(x => x.GetComponent<Button>());

            this.ViewMarkers
                .Subscribe(x => Debug.Log(x)).AddTo(this);
        }
    }
}
