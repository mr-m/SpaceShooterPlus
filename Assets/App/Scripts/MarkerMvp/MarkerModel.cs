using System;
using System.Runtime.Serialization;
using SpaceShooterPlus.MarkerStates;
using UniRx;

namespace SpaceShooterPlus.MarkerMvp
{
    [Serializable]
    public class MarkerModel : ISerializable
    {
        public ReactiveProperty<IMarkerState> State { get; }

        public MarkerModel(IMarkerState state)
        {
            this.State = new ReactiveProperty<IMarkerState>(state);
        }

        public void Complete()
        {
            this.State.Value = this.State.Value.Complete();
        }

        public void Unlock()
        {
            this.State.Value = this.State.Value.Unlock();
        }

        protected MarkerModel(SerializationInfo info, StreamingContext context)
        {
            var state = (IMarkerState)info.GetValue(nameof(this.State), typeof(IMarkerState));
            this.State = new ReactiveProperty<IMarkerState>(state);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.State), this.State.Value, typeof(IMarkerState));
        }
    }
}
