using UniRx;

namespace SpaceShooterPlus
{
    public class RandomModelReactive
    {
        public IntReactiveProperty State { get; }

        public RandomModelReactive(int state = 0)
        {
            this.State = new IntReactiveProperty(state);
        }

        public void Complete()
        {
            this.State.Value++;
        }

        public void Unlock()
        {
            this.State.Value++;
        }
    }
}
