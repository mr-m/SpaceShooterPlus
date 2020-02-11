using System;
using System.Runtime.Serialization;
using SpaceShooterPlus.MarkerStates;
using UniRx;

namespace SpaceShooterPlus.MarkerMvp
{
    [Serializable]
    public class MarkerModel : ISerializable
    {
        public IReadOnlyReactiveProperty<IMarkerState> State => this.state;

        private readonly ReactiveProperty<IMarkerState> state;

        public MarkerModel(IMarkerState state)
        {
            this.state = new ReactiveProperty<IMarkerState>(state);
        }

        public void Complete()
        {
            this.state.Value = this.state.Value.Complete();
        }

        public void Unlock()
        {
            this.state.Value = this.state.Value.Unlock();
        }

        protected MarkerModel(SerializationInfo info, StreamingContext context)
        {
            var state = (IMarkerState)info.GetValue(nameof(this.state), typeof(IMarkerState));
            this.state = new ReactiveProperty<IMarkerState>(state);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.state), this.state.Value, typeof(IMarkerState));
        }
    }
}
