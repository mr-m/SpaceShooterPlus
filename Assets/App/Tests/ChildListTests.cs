using System.Linq;
using NUnit.Framework;
using SpaceShooterPlus;
using SpaceShooterPlus.MarkerMvp;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceShooterPlusTests
{
    internal class ChildListTests
    {
        [Test]
        public void ChildListTest()
        {
            var parent = new GameObject();
            var list = parent.AddComponent<ChildList>();
            Assert.That(list.Children.Count, Is.EqualTo(0));

            //Add children
            var child1 = new GameObject();
            child1.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(1));

            var child2 = new GameObject();
            child2.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(2));

            //Remove children
            child1.transform.parent = null;
            Assert.That(list.Children.Count, Is.EqualTo(1));

            child2.transform.parent = null;
            Assert.That(list.Children.Count, Is.EqualTo(0));
        }

        [Test]
        public void ChildListIsNotObservable()
        {
            var parent = new GameObject();
            var list = parent.AddComponent<ChildList>();

            int callCount = 0;

            list.Children.ObserveEveryValueChanged(_ => callCount++);
            Assert.That(list.Children.Count, Is.EqualTo(0));
            Assert.That(callCount, Is.EqualTo(0));

            //Add a child, callCount is not changing
            var child = new GameObject();
            child.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(1));
            Assert.That(callCount, Is.EqualTo(0));

            //Add a child, callCount is not changing
            child.transform.parent = null;
            Assert.That(list.Children.Count, Is.EqualTo(0));
            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void ChildListReactiveObserveAdd()
        {
            var parent = new GameObject();
            var list = parent.AddComponent<ChildListReactive>();

            int callCount = 0;

            list.Children.ObserveAdd().Subscribe(_ => callCount++);
            Assert.That(list.Children.Count, Is.EqualTo(0));
            Assert.That(callCount, Is.EqualTo(0));

            //Add children, callCount is changing
            var child1 = new GameObject();
            child1.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(1));
            Assert.That(callCount, Is.EqualTo(1));

            var child2 = new GameObject();
            child2.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(2));
            Assert.That(callCount, Is.EqualTo(2));
        }

        [Test]
        public void ChildListReactiveObserveRemove()
        {
            var parent = new GameObject();
            var list = parent.AddComponent<ChildListReactive>();

            int callCount = 0;

            list.Children.ObserveRemove().Subscribe(_ => callCount++);
            Assert.That(list.Children.Count, Is.EqualTo(0));
            Assert.That(callCount, Is.EqualTo(0));

            //Add children, callCount should not be changing
            var child1 = new GameObject();
            child1.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(1));
            Assert.That(callCount, Is.EqualTo(0));

            var child2 = new GameObject();
            child2.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(2));
            Assert.That(callCount, Is.EqualTo(0));

            //Remove children, callCount is changing
            child1.transform.parent = null;
            Assert.That(list.Children.Count, Is.EqualTo(1));
            Assert.That(callCount, Is.EqualTo(1));

            child2.transform.parent = null;
            Assert.That(list.Children.Count, Is.EqualTo(0));
            Assert.That(callCount, Is.EqualTo(2));
        }

        [Test]
        public void ChildListReactiveObserveComponent()
        {
            var parent = new GameObject();
            var list = parent.AddComponent<ChildListReactive>();

            int callCount = 0;

            list.Children
                .ObserveAdd()
                .Where(x => x.Value.GetComponent<MarkerView>())
                .Subscribe(_ => callCount++);

            Assert.That(list.Children.Count, Is.EqualTo(0));
            Assert.That(callCount, Is.EqualTo(0));

            //Add a child with a MarkerView component
            var child1 = new GameObject();
            child1.AddComponent<MarkerView>();
            child1.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(1));
            Assert.That(callCount, Is.EqualTo(1));

            //Add a child without required a MarkerView component
            var child2 = new GameObject();
            child2.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(2));
            Assert.That(callCount, Is.EqualTo(1));

            //Add a child with a MarkerView component
            var child3 = new GameObject();
            child3.AddComponent<MarkerView>();
            child3.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(3));
            Assert.That(callCount, Is.EqualTo(2));
        }

        [Test]
        public void ChildListReactiveObserveComponentLateAdd()
        {
            var parent = new GameObject();
            var list = parent.AddComponent<ChildListReactive>();

            int callCount = 0;

            list.Children
                .ObserveAdd()
                .Where(x => x.Value.GetComponent<MarkerView>())
                .Subscribe(_ => callCount++);

            Assert.That(list.Children.Count, Is.EqualTo(0));
            Assert.That(callCount, Is.EqualTo(0));

            //Add a child with a MarkerView component
            var child1 = new GameObject();
            child1.AddComponent<MarkerView>();
            child1.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(1));
            Assert.That(callCount, Is.EqualTo(1));

            //Add a child without a required MarkerView component
            var child2 = new GameObject();
            child2.transform.parent = parent.transform;
            Assert.That(list.Children.Count, Is.EqualTo(2));
            Assert.That(callCount, Is.EqualTo(1));

            //TODO: 'callCount' should be increasing
            //Add MarkerView component to a child without it
            child2.AddComponent<MarkerView>();
            Assert.That(list.Children.Count, Is.EqualTo(2));
            Assert.That(callCount, Is.EqualTo(1));
        }
    }
}
