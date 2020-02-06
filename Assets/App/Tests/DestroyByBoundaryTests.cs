using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using SpaceShooterPlus;

namespace SpaceShooterPlusTests
{
    [TestFixture]
    public class DestroyByBoundaryTests
    {
        [UnityTest]
        public IEnumerator DestroyByBoundaryWorks()
        {
            var boundary = new GameObject();
            var boundaryCollider = boundary.AddComponent<BoxCollider>();
            boundaryCollider.isTrigger = true;
            boundary.AddComponent<DestroyByBoundary>();

            var gameObject = new GameObject();
            var gameObjectCollider = gameObject.AddComponent<BoxCollider>();
            gameObjectCollider.isTrigger = false;
            gameObject.AddComponent<Rigidbody>();

            yield return new WaitForFixedUpdate();
            Assert.That(gameObject, Is.Not.Null);

            gameObject.transform.position = new Vector3(1, 1, 1);
            yield return new WaitForFixedUpdate();
            Assert.That(gameObject, Is.Not.Null);

            gameObject.transform.position = new Vector3(2, 2, 2);
            yield return new WaitForFixedUpdate();
            Assert.That(gameObject, Is.Not.Null);

            gameObject.transform.position = new Vector3(3, 3, 3);
            yield return new WaitForFixedUpdate();
            Assert.That(gameObject == null, Is.True);
        }
    }
}
