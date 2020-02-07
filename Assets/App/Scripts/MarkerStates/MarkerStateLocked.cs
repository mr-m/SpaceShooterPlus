using System;
using System.Runtime.Serialization;

namespace SpaceShooterPlus.MarkerStates
{
    [Serializable]
    public class MarkerStateLocked : IMarkerState, ISerializable
    {
        public IMarkerState Unlock()
        {
            return new MarkerStateUnlocked();
        }

        public IMarkerState Complete()
        {
            throw new System.NotImplementedException();
        }

        public MarkerStateLocked() { }

        protected MarkerStateLocked(SerializationInfo info, StreamingContext context) { }

        public void GetObjectData(SerializationInfo info, StreamingContext context) { }
    }
}
