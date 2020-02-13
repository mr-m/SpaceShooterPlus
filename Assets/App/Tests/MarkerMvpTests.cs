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
        /// <summary>
        /// Model state should change when model requests.
        /// </summary>
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

        /// <summary>
        /// Model state changes should be observable.
        /// </summary>
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

        /// <summary>
        /// Views should require Button component.
        /// </summary>
        [Test]
        public void ViewRequiresButtonComponent()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<MarkerView>();

            var button = gameObject.GetComponent<Button>();
            Assert.That(button, Is.Not.Null);
        }

        /// <summary>
        /// Views should initialize Button field.
        /// </summary>
        [Test]
        public void ViewInitializesButtonField()
        {
            var gameObject = new GameObject();
            var view = gameObject.AddComponent<MarkerView>();

            var button = view.Button;
            Assert.That(button, Is.Not.Null);
        }

        /// <summary>
        /// Presenter should reflect Model's current state on a View.
        /// </summary>
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
            Assert.That(view.Button.colors.normalColor, Is.EqualTo(Color.yellow));
            Assert.That(view.Button.colors.highlightedColor, Is.EqualTo(Color.yellow));
        }

        /// <summary>
        /// View buttons should be clickable.
        /// </summary>
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

        /// <summary>
        /// View buttons' clicks should be observable.
        /// </summary>
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
