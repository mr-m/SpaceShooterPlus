using NUnit.Framework;
using SpaceShooterPlus.MarkerMvp;
using SpaceShooterPlus.MarkerStates;
using UniRx;

namespace SpaceShooterPlusTests
{
    [TestFixture]
    internal class MarkersCollectionTests
    {
        [Test]
        public void ModelsCollectionObserveAddWorks()
        {
            var models = new ReactiveCollection<MarkerModel> {
                new MarkerModel(new MarkerStateCompleted()),
                new MarkerModel(new MarkerStateUnlocked()),
                new MarkerModel(new MarkerStateLocked()),
            };

            int countDiff = 0;
            models.ObserveAdd().Subscribe(_ => countDiff++);
            Assert.That(countDiff, Is.EqualTo(0));

            models.Add(new MarkerModel(new MarkerStateLocked()));
            Assert.That(countDiff, Is.EqualTo(1));

            models.Add(new MarkerModel(new MarkerStateLocked()));
            Assert.That(countDiff, Is.EqualTo(2));
        }

        [Test]
        public void ModelsCollectionObserveRemoveWorks()
        {
            var models = new ReactiveCollection<MarkerModel> {
                new MarkerModel(new MarkerStateCompleted()),
                new MarkerModel(new MarkerStateUnlocked()),
                new MarkerModel(new MarkerStateLocked()),
            };

            int countDiff = 0;
            models.ObserveRemove().Subscribe(_ => countDiff--);
            Assert.That(countDiff, Is.EqualTo(0));

            models.RemoveAt(0);
            Assert.That(countDiff, Is.EqualTo(-1));

            models.RemoveAt(0);
            Assert.That(countDiff, Is.EqualTo(-2));
        }

        [Test]
        public void ModelsCollectionObserveReplaceWorks()
        {
            var models = new ReactiveCollection<MarkerModel> {
                new MarkerModel(new MarkerStateCompleted()),
                new MarkerModel(new MarkerStateUnlocked()),
                new MarkerModel(new MarkerStateLocked()),
            };

            int callCount = 0;
            models.ObserveReplace().Subscribe(_ => callCount++);
            Assert.That(callCount, Is.EqualTo(0));

            models[1] = new MarkerModel(new MarkerStateCompleted());
            Assert.That(callCount, Is.EqualTo(1));

            models[2] = new MarkerModel(new MarkerStateUnlocked());
            Assert.That(callCount, Is.EqualTo(2));
        }

        [Test]
        public void EachModelInCollectionIsObservable()
        {
            var models = new ReactiveCollection<MarkerModel> {
                new MarkerModel(new MarkerStateCompleted()),
                new MarkerModel(new MarkerStateUnlocked()),
                new MarkerModel(new MarkerStateLocked()),
            };

            int callCount = 0;
            models.ToObservable().Subscribe(x => x.State.Subscribe(_ => callCount++));
            Assert.That(callCount, Is.EqualTo(3));

            models[1].Complete();
            Assert.That(callCount, Is.EqualTo(4));

            models[2].Unlock();
            Assert.That(callCount, Is.EqualTo(5));
        }

        [Test]
        public void EvenNewModelsInCollectionCanBeObservable()
        {
            var models = new ReactiveCollection<MarkerModel> {
                new MarkerModel(new MarkerStateCompleted()),
                new MarkerModel(new MarkerStateUnlocked()),
                new MarkerModel(new MarkerStateLocked()),
            };

            int callCount = 0;

            models.ToObservable().Subscribe(x => x.State.Subscribe(_ => callCount++));
            Assert.That(callCount, Is.EqualTo(3));

            models.ObserveAdd().Subscribe(x => x.Value.State.Subscribe(_ => callCount++));
            Assert.That(callCount, Is.EqualTo(3));

            models.ObserveRemove().Subscribe(x => x.Value.State.Subscribe(_ => callCount++));
            Assert.That(callCount, Is.EqualTo(3));

            models[1].Complete();
            Assert.That(callCount, Is.EqualTo(4));

            models.Add(new MarkerModel(new MarkerStateLocked()));
            Assert.That(callCount, Is.EqualTo(5));

            models.RemoveAt(0);
            Assert.That(callCount, Is.EqualTo(6));
        }
    }
}
