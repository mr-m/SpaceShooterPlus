using NUnit.Framework;
using SpaceShooterPlus;
using SpaceShooterPlus.MapMvp;
using SpaceShooterPlus.MarkerMvp;
using SpaceShooterPlus.MarkerStates;
using UniRx;
using UnityEngine;

namespace SpaceShooterPlusTests
{
    [TestFixture]
    internal class MapMvpTests
    {
        [Test]
        public void ViewRequiresChildListReactiveComponent()
        {
            var parent = new GameObject();
            var view = parent.AddComponent<MapView>();
            var list = parent.GetComponent<ChildListReactive>();

            Assert.That(list, Is.Not.Null);
        }

        [Test]
        public void ViewUpdatesChildListReactiveComponent()
        {
            var parent = new GameObject();
            var view = parent.AddComponent<MapView>();
            var list = parent.GetComponent<ChildListReactive>();

            Assert.That(list.Children.Count, Is.EqualTo(0));

            var child = new GameObject();
            child.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(1));
        }

        [Test]
        public void ViewInitializesMarkerViewsField()
        {
            var parent = new GameObject();
            var view = parent.AddComponent<MapView>();

            Assert.That(view.MarkerViews, Is.Not.Null);
        }

        [Test]
        public void ViewAddsMarkerViewComponents()
        {
            var parent = new GameObject();
            var view = parent.AddComponent<MapView>();
            var list = parent.GetComponent<ChildListReactive>();

            Assert.That(list.Children.Count, Is.EqualTo(0));
            Assert.That(view.MarkerViews.Count, Is.EqualTo(0));

            //Add a child with a MarkerView component
            var child1 = new GameObject();
            child1.AddComponent<MarkerView>();
            child1.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(1));
            Assert.That(view.MarkerViews.Count, Is.EqualTo(1));

            //Add a child without a required MarkerView component
            var child2 = new GameObject();
            child2.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(2));
            Assert.That(view.MarkerViews.Count, Is.EqualTo(1));

            //TODO: 'callCount' should be increasing
            //Add MarkerView component to a child without it
            child2.AddComponent<MarkerView>();
            Assert.That(list.Children.Count, Is.EqualTo(2));
            Assert.That(view.MarkerViews.Count, Is.EqualTo(1));

            //Add a child with a MarkerView component
            var child3 = new GameObject();
            child3.AddComponent<MarkerView>();
            child3.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(3));
            Assert.That(view.MarkerViews.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Presenter should connect Model's and View's children.
        /// </summary>
        [Test]
        public void PresenterConnectsModelAndView()
        {
            var mapModel = new MapModel();
            var parent = new GameObject();
            var mapView = parent.AddComponent<MapView>();

            //Add a child with a MarkerView component
            var child = new GameObject();
            var markerView = child.AddComponent<MarkerView>();
            child.transform.parent = parent.transform;

            //Add a MarkerModel model
            mapModel.MarkerModels.Add(new MarkerModel(new MarkerStateLocked()));

            Assert.That(markerView.Button.interactable, Is.True);

            var presenter = new MapPresenter(mapView, mapModel);

            Assert.That(markerView.Button.interactable, Is.False);
        }

        /// <summary>
        /// Presenter should reflect changes in the Model to the View.
        /// </summary>
        [Test]
        public void PresenterConnectsModelAndViewChange()
        {
            var mapModel = new MapModel();
            var parent = new GameObject();
            var mapView = parent.AddComponent<MapView>();

            //Add a child with a MarkerView component
            var child = new GameObject();
            var markerView = child.AddComponent<MarkerView>();
            child.transform.parent = parent.transform;

            //Add a MarkerModel model
            var markerModel = new MarkerModel(new MarkerStateLocked());
            mapModel.MarkerModels.Add(markerModel);

            Assert.That(markerView.Button.interactable, Is.True);

            var presenter = new MapPresenter(mapView, mapModel);

            Assert.That(markerView.Button.interactable, Is.False);

            markerModel.Unlock();
            Assert.That(markerView.Button.interactable, Is.True);

            markerModel.Complete();
            Assert.That(markerView.Button.interactable, Is.True);
            Assert.That(markerView.Button.colors.normalColor, Is.EqualTo(Color.yellow));
            Assert.That(markerView.Button.colors.highlightedColor, Is.EqualTo(Color.yellow));
        }

        /// <summary>
        /// Presenter should connect Model and View even if
        /// their children are added after the Presenter's creation.
        /// </summary>
        /// <remarks>Currently Presenter doesn't work that way.</remarks>
        [Test]
        public void PresenterConnectsModelAndViewLateAdd()
        {
            var mapModel = new MapModel();
            var parent = new GameObject();
            var mapView = parent.AddComponent<MapView>();

            var presenter = new MapPresenter(mapView, mapModel);

            //Add a child with a MarkerView component
            var child = new GameObject();
            var markerView = child.AddComponent<MarkerView>();
            child.transform.parent = parent.transform;

            //Add a MarkerModel model
            mapModel.MarkerModels.Add(new MarkerModel(new MarkerStateLocked()));

            //TODO: Button should become disabled
            Assert.That(markerView.Button.interactable, Is.True);
        }

        /// <summary>
        /// Presenter should see clicks in every button of its View's children.
        /// </summary>
        [Test]
        public void PresenterListensToAllButtons()
        {
            var mapModel = new MapModel();
            var parent = new GameObject();
            var mapView = parent.AddComponent<MapView>();

            //Add a child with a MarkerView component
            var child1 = new GameObject();
            child1.AddComponent<MarkerView>();
            child1.transform.parent = parent.transform;

            //Add a child with a MarkerView component
            var child2 = new GameObject();
            child2.AddComponent<MarkerView>();
            child2.transform.parent = parent.transform;

            //Add a MarkerModel model
            mapModel.MarkerModels.Add(new MarkerModel(new MarkerStateCompleted()));
            mapModel.MarkerModels.Add(new MarkerModel(new MarkerStateUnlocked()));

            var presenter = new MapPresenter(mapView, mapModel);

            int callCount = 0;
            presenter.OnButtonClickAsObservable.Subscribe(_ => callCount++);

            //Click twice on the first button
            mapView.MarkerViews[0].Button.onClick.Invoke();
            Assert.That(callCount, Is.EqualTo(1));

            mapView.MarkerViews[0].Button.onClick.Invoke();
            Assert.That(callCount, Is.EqualTo(2));

            //Click once on the second button
            mapView.MarkerViews[1].Button.onClick.Invoke();
            Assert.That(callCount, Is.EqualTo(3));
        }
    }
}
