using NUnit.Framework;
using SpaceShooterPlus.MarkerMvp;
using SpaceShooterPlus.MarkerStates;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooterPlusTests
{
    [TestFixture]
    internal class MarkerMvpTests
    {
        [Test]
        public void ModelStateChangesAreVisible()
        {
            var model = new MarkerModel(new MarkerStateLocked());

            Assert.That(model.State.Value, Is.TypeOf(typeof(MarkerStateLocked)));

            model.Unlock();
            Assert.That(model.State.Value, Is.TypeOf(typeof(MarkerStateUnlocked)));

            model.Complete();
            Assert.That(model.State.Value, Is.TypeOf(typeof(MarkerStateCompleted)));
        }

        [Test]
        public void ModelStateChangesAreObservable()
        {
            var model = new MarkerModel(new MarkerStateLocked());

            Assert.That(model.State.Value, Is.TypeOf(typeof(MarkerStateLocked)));

            var callCount = 0;
            model.State.Subscribe(_ => callCount++);
            Assert.That(callCount, Is.EqualTo(1));

            model.Unlock();
            Assert.That(callCount, Is.EqualTo(2));

            model.Complete();
            Assert.That(callCount, Is.EqualTo(3));
        }

        [Test]
        public void ViewRequiresButtonComponent()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<MarkerView>();

            var button = gameObject.GetComponent<Button>();
            Assert.That(button, Is.Not.Null);
        }

        [Test]
        public void ViewInitializesButtonField()
        {
            var gameObject = new GameObject();
            var view = gameObject.AddComponent<MarkerView>();

            var button = view.Button;
            Assert.That(button, Is.Not.Null);
        }

        [Test]
        public void PresenterConnectsModelStateAndView()
        {
            var gameObject = new GameObject();
            var view = gameObject.AddComponent<MarkerView>();
            var model = new MarkerModel(new MarkerStateLocked());

            Assert.That(view.Button.interactable, Is.True);

            var presenter = new MarkerPresenter(view, model);

            Assert.That(view.Button.interactable, Is.False);

            model.Unlock();
            Assert.That(view.Button.interactable, Is.True);

            model.Complete();
            Assert.That(view.Button.interactable, Is.True);
            Assert.That(view.Button.colors.normalColor, Is.EqualTo(Color.red));
        }

        [Test]
        public void ViewButtonsAreClickable()
        {
            var gameObject = new GameObject();
            var view = gameObject.AddComponent<MarkerView>();

            int callCount = 0;
            view.Button.onClick.AddListener(() => callCount++);

            view.Button.onClick.Invoke();
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void ViewButtonsClicksAreObservable()
        {
            var gameObject = new GameObject();
            var view = gameObject.AddComponent<MarkerView>();

            int callCount = 0;
            view.Button.OnClickAsObservable().Subscribe(_ => callCount++);

            view.Button.onClick.Invoke();
            Assert.That(callCount, Is.EqualTo(1));
        }
    }
}
