namespace SpaceShooterPlus
{
    public class RandomModel
    {
        public int State { get; private set;  }

        public RandomModel(int state = 0)
        {
            this.State = state;
        }

        public void Complete()
        {
            this.State++;
        }

        public void Unlock()
        {
            this.State++;
        }
    }
}
