using System;
using System.Runtime.Serialization;
using SpaceShooterPlus.MarkerStates;

namespace SpaceShooterPlus.MarkerMvp
{
    [Serializable]
    public class MarkerModel : ISerializable
    {
        public IMarkerState State { get; private set; }

        public MarkerModel(IMarkerState state)
        {
            this.State = state;
        }

        public void Complete()
        {
            this.State = this.State.Complete();
        }

        public void Unlock()
        {
            this.State = this.State.Unlock();
        }

        protected MarkerModel(SerializationInfo info, StreamingContext context)
        {
            this.State = (IMarkerState)info.GetValue("State", typeof(IMarkerState));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("State", this.State, typeof(IMarkerState));
        }
    }
}
