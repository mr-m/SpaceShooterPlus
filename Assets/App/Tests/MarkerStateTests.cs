using NUnit.Framework;
using SpaceShooterPlus.MarkerStates;

namespace SpaceShooterPlusTests
{
    [TestFixture]
    internal class MarkerStateTests
    {
        [Test]
        public void MarkerStateTransitionsAreValid()
        {
            Assert.That(() => new MarkerStateLocked().Unlock(), Is.TypeOf(typeof(MarkerStateUnlocked)));
            Assert.That(() => new MarkerStateLocked().Complete(), Throws.Exception);
            Assert.That(() => new MarkerStateUnlocked().Unlock(), Throws.Exception);
            Assert.That(() => new MarkerStateUnlocked().Complete(), Is.TypeOf(typeof(MarkerStateCompleted)));
            Assert.That(() => new MarkerStateCompleted().Unlock(), Throws.Exception);
            Assert.That(() => new MarkerStateCompleted().Complete(), Throws.Exception);
        }
    }
}
