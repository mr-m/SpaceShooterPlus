using NUnit.Framework;
using SpaceShooterPlus.MarkerMvp;
using SpaceShooterPlus.MarkerStates;

namespace SpaceShooterPlusTests
{
    [TestFixture]
    internal class MarkerStateTests
    {
        [Test]
        public void StateChangeReturnsNewState()
        {
            Assert.That(() => new MarkerStateLocked().Unlock(), Is.TypeOf(typeof(MarkerStateUnlocked)));
            Assert.That(() => new MarkerStateLocked().Complete(), Throws.Exception);
            Assert.That(() => new MarkerStateUnlocked().Unlock(), Throws.Exception);
            Assert.That(() => new MarkerStateUnlocked().Complete(), Is.TypeOf(typeof(MarkerStateCompleted)));
            Assert.That(() => new MarkerStateCompleted().Unlock(), Throws.Exception);
            Assert.That(() => new MarkerStateCompleted().Complete(), Throws.Exception);
        }

        [Test]
        public void StateChangeAreReflectedInModel()
        {
            Assert.That(() => { var x = new MarkerModel(new MarkerStateLocked()); x.Unlock(); return x.State.Value; }, Is.TypeOf(typeof(MarkerStateUnlocked)));
            Assert.That(() => { var x = new MarkerModel(new MarkerStateLocked()); x.Complete(); return x.State.Value; }, Throws.Exception);
            Assert.That(() => { var x = new MarkerModel(new MarkerStateUnlocked()); x.Unlock(); return x.State.Value; }, Throws.Exception);
            Assert.That(() => { var x = new MarkerModel(new MarkerStateUnlocked()); x.Complete(); return x.State.Value; }, Is.TypeOf(typeof(MarkerStateCompleted)));
            Assert.That(() => { var x = new MarkerModel(new MarkerStateCompleted()); x.Unlock(); return x.State.Value; }, Throws.Exception);
            Assert.That(() => { var x = new MarkerModel(new MarkerStateCompleted()); x.Complete(); return x.State.Value; }, Throws.Exception);
        }
    }
}
