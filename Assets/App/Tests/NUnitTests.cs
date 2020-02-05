using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

namespace SpaceShooterPlusTests
{
    [TestFixture]
    internal class NUnitTests
    {
        [Test]
        public void TestEqual()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void TestNotEqual()
        {
            Assert.AreNotEqual(1, 2);
        }

        [UnityTest]
        public IEnumerator GameObject_WithRigidBody_WillBeAffectedByPhysics()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<Rigidbody>();

            var originalPosition = gameObject.transform.position.y;
            yield return new WaitForFixedUpdate();
            var updatedPosition = gameObject.transform.position.y;

            Assert.AreNotEqual(originalPosition, updatedPosition);
        }
    }
}
