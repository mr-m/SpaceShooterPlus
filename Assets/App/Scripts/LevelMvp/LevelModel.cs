using System;
using System.Runtime.Serialization;
using UniRx;

namespace SpaceShooterPlus.LevelMvp
{
    [Serializable]
    public class LevelModel : ISerializable
    {
        public IntReactiveProperty Health { get; private set; }

        [field: NonSerialized]
        public IReadOnlyReactiveProperty<bool> IsDead { get; private set; }

        public LevelModel(int initialHealth)
        {
            this.Health = new IntReactiveProperty(initialHealth);

            this.IsDead = this.Health
                .Select(x => x <= 0)
                .ToReactiveProperty();
        }

        protected LevelModel(SerializationInfo info, StreamingContext context)
        {
            this.Health = new IntReactiveProperty(info.GetInt32("Health"));

            this.IsDead = this.Health
                .Select(x => x <= 0)
                .ToReactiveProperty();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Health", this.Health.Value, typeof(int));
            info.AddValue("IsDead", this.IsDead.Value, typeof(bool));
        }
    }
}
