using System;
using System.Runtime.Serialization;

namespace SpaceShooterPlus.MarkerStates
{
    [Serializable]
    public class MarkerStateUnlocked : IMarkerState, ISerializable
    {
        public IMarkerState Unlock()
        {
            throw new System.NotImplementedException();
        }

        public IMarkerState Complete()
        {
            return new MarkerStateCompleted();
        }

        public MarkerStateUnlocked() { }

        protected MarkerStateUnlocked(SerializationInfo info, StreamingContext context) { }

        public void GetObjectData(SerializationInfo info, StreamingContext context) { }
    }
}
