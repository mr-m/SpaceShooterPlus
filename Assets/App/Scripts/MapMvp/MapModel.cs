using System;
using UniRx;
using SpaceShooterPlus.MarkerMvp;

namespace SpaceShooterPlus.MapMvp
{
    [Serializable]
    public class MapModel
    {
        public ReactiveCollection<MarkerModel> MarkerModels { get; }

        public MapModel()
        {
            this.MarkerModels = new ReactiveCollection<MarkerModel>();
        }
    }
}
