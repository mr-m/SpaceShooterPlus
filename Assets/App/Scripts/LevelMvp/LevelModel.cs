using System;
using System.Runtime.Serialization;
using UniRx;

namespace SpaceShooterPlus.LevelMvp
{
    [Serializable]
    public class LevelModel : ISerializable
    {
        public IntReactiveProperty RequiredScore { get; }

        public IntReactiveProperty Score { get; }

        [field: NonSerialized]
        public IReadOnlyReactiveProperty<bool> IsVictory { get; }

        public IntReactiveProperty InitialHealth { get; }

        public IntReactiveProperty Health { get; }

        [field: NonSerialized]
        public IReadOnlyReactiveProperty<bool> IsDefeat { get; }

        [field: NonSerialized]
        public IReadOnlyReactiveProperty<bool> IsPlaying { get; }

        public LevelModel(int requiredScore, int initialHealth)
        {
            this.RequiredScore = new IntReactiveProperty(requiredScore);

            this.Score = new IntReactiveProperty(0);

            this.IsVictory = this.Score
                .Select(x => x >= this.RequiredScore.Value)
                .ToReactiveProperty();

            this.InitialHealth = new IntReactiveProperty(initialHealth);

            this.Health = new IntReactiveProperty(initialHealth);

            this.IsDefeat = this.Health
                .Select(x => x <= 0)
                .ToReactiveProperty();

            this.IsPlaying = Observable.CombineLatest(
                this.IsVictory, this.IsDefeat,
                (v, d) => (!v && !d)).ToReactiveProperty();
        }

        protected LevelModel(SerializationInfo info, StreamingContext context)
        {
            this.RequiredScore = new IntReactiveProperty(info.GetInt32(nameof(this.RequiredScore)));
            this.Score = new IntReactiveProperty(info.GetInt32(nameof(this.Score)));
            this.IsVictory = this.Score
                .Select(x => x >= this.RequiredScore.Value)
                .ToReactiveProperty();

            this.InitialHealth = new IntReactiveProperty(info.GetInt32(nameof(this.InitialHealth)));
            this.Health = new IntReactiveProperty(info.GetInt32(nameof(this.Health)));
            this.IsDefeat = this.Health
                .Select(x => x <= 0)
                .ToReactiveProperty();

            this.IsPlaying = Observable.CombineLatest(
                this.IsVictory, this.IsDefeat,
                (v, d) => (!v && !d)).ToReactiveProperty();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.RequiredScore), this.RequiredScore.Value, typeof(int));
            info.AddValue(nameof(this.Score), this.Score.Value, typeof(int));
            info.AddValue(nameof(this.IsVictory), this.IsVictory.Value, typeof(bool));

            info.AddValue(nameof(this.InitialHealth), this.InitialHealth.Value, typeof(int));
            info.AddValue(nameof(this.Health), this.Health.Value, typeof(int));
            info.AddValue(nameof(this.IsDefeat), this.IsDefeat.Value, typeof(bool));

            info.AddValue(nameof(this.IsPlaying), this.IsPlaying.Value, typeof(bool));
        }
    }
}
