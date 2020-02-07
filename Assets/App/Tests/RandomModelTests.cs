using NUnit.Framework;
using SpaceShooterPlus;
using UniRx;

namespace SpaceShooterPlusTests
{
    [TestFixture]
    public class RandomModelTests
    {
        [Test]
        public void RandomModelStateChangesAreInvisible()
        {
            var model = new RandomModel(11);
            Assert.That(model.State, Is.EqualTo(11));

            var callCount = 0;
            model.ObserveEveryValueChanged(_ => callCount++);
            Assert.That(model.State, Is.EqualTo(11));
            Assert.That(callCount, Is.EqualTo(0));

            model.Unlock();
            Assert.That(model.State, Is.EqualTo(12));
            Assert.That(callCount, Is.EqualTo(0));

            model.Complete();
            Assert.That(model.State, Is.EqualTo(13));
            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void ReactiveRandomModelStateChangesAreObservable()
        {
            var model = new RandomModelReactive(11);
            Assert.That(model.State.Value, Is.EqualTo(11));

            var callCount = 0;
            model.State.Subscribe(_ => callCount++);
            Assert.That(model.State.Value, Is.EqualTo(11));
            Assert.That(callCount, Is.EqualTo(1));

            model.Unlock();
            Assert.That(model.State.Value, Is.EqualTo(12));
            Assert.That(callCount, Is.EqualTo(2));

            model.Complete();
            Assert.That(model.State.Value, Is.EqualTo(13));
            Assert.That(callCount, Is.EqualTo(3));
        }
    }
}
