using System;
using System.Runtime.Serialization;

namespace SpaceShooterPlus.MarkerStates
{
    [Serializable]
    public class MarkerStateCompleted : IMarkerState, ISerializable
    {
        public IMarkerState Unlock()
        {
            throw new System.NotImplementedException();
        }

        public IMarkerState Complete()
        {
            throw new System.NotImplementedException();
        }

        public MarkerStateCompleted() { }

        protected MarkerStateCompleted(SerializationInfo info, StreamingContext context) { }

        public void GetObjectData(SerializationInfo info, StreamingContext context) { }
    }
}
