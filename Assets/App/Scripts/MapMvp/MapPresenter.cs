using System;
using SpaceShooterPlus.MarkerMvp;
using UniRx;
using UnityEngine;

namespace SpaceShooterPlus.MapMvp
{
    public class MapPresenter
    {
        private readonly MapView mapView;
        private readonly MapModel mapModel;
        public IObservable<MarkerModel> OnButtonClickAsObservable;

        public MapPresenter(MapView view, MapModel model)
        {
            Debug.Log($"{nameof(MapPresenter)}.{nameof(MapPresenter)}()");

            this.mapView = view;
            this.mapModel = model;

            var zip = Observable.Zip(
                view.MarkerViews.ToObservable(),
                model.MarkerModels.ToObservable(),
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
