using System;
using UnityEngine.UI;
using UniRx;
using SpaceShooterPlus.MarkerMvp;
using SpaceShooterPlus.MarkerStates;

namespace SpaceShooterPlus.MapMvp
{
    [Serializable]
    public class MapModel
    {
        public IObservable<MarkerModel> MarkerModels { get; private set; }

        public MapModel(IObservable<Button> views)
        {
            this.MarkerModels = views
                .Select((x, i) =>
                {
                    if (i == 0)
                    {
                        IMarkerState markerState = new MarkerStateUnlocked();
                        var markerModel = new MarkerModel(markerState);
                        return markerModel;
                    }
                    else
                    {
                        IMarkerState markerState = new MarkerStateLocked();
                        var markerModel = new MarkerModel(markerState);
                        return markerModel;
                    }
                });
        }
    }
}
