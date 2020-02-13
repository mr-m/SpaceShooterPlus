using System;
using SpaceShooterPlus.MarkerMvp;
using UniRx;
using UnityEngine;

namespace SpaceShooterPlus.MapMvp
{
    public class MapPresenter
    {
        private readonly MapView view;
        private readonly MapModel model;

        public IObservable<MarkerModel> OnButtonClickAsObservable;

        public MapPresenter(MapView mapView, MapModel mapModel)
        {
            Debug.Log($"{nameof(MapPresenter)}.{nameof(MapPresenter)}()");

            this.view = mapView;
            this.model = mapModel;

            var zip = Observable.Zip(
                this.view.MarkerViews.ToObservable(),
                this.model.MarkerModels.ToObservable(),
                (x, y) =>
                {
                    Debug.Log($"Zip: {x} + {y}");
                    return new MarkerPresenter(x, y);
                });

            this.OnButtonClickAsObservable = zip
                .Select(x => x.OnButtonClickAsObservable)
                .Concat();

            zip.Subscribe();
        }
    }
}
