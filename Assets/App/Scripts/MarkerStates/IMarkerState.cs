namespace SpaceShooterPlus.MarkerStates
{
    public interface IMarkerState
    {
        // Level marker can become unlocked and completed,
        // but cannot become locked again.
        IMarkerState Unlock();
        IMarkerState Complete();
    }
}
