using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using SpaceShooterPlus.LevelMvp;

namespace SpaceShooterPlusTests
{
    [TestFixture]
    public class LevelModelTests
    {
        [Test]
        public void LevelModelCreatedProperly()
        {
            var levelModel = new LevelModel(3);

            Assert.That(levelModel.Health, Is.Not.Null);
            Assert.That(levelModel.IsDead, Is.Not.Null);

            Assert.That(levelModel.Health.Value, Is.EqualTo(3));
            Assert.That(levelModel.IsDead.Value, Is.EqualTo(false));
        }

        [Test]
        public void LevelModelChangesProperly()
        {
            var levelModel = new LevelModel(3);
            Assert.That(levelModel.IsDead.Value, Is.EqualTo(false));

            levelModel.Health.Value -= 2;
            Assert.That(levelModel.IsDead.Value, Is.EqualTo(false));

            levelModel.Health.Value -= 1;
            Assert.That(levelModel.IsDead.Value, Is.EqualTo(true));
        }

        [Test]
        public void LevelModelSerializedProperly()
        {
            var oldLevelModel = new LevelModel(3);

            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, oldLevelModel);

            stream.Seek(0, SeekOrigin.Begin);

            var newLevelModel = (LevelModel)formatter.Deserialize(stream);
            stream.Close();

            Assert.That(oldLevelModel, Is.Not.Null);
            Assert.That(newLevelModel, Is.Not.Null);
            Assert.That(oldLevelModel.Health.Value, Is.EqualTo(newLevelModel.Health.Value));
            Assert.That(oldLevelModel.IsDead.Value, Is.EqualTo(newLevelModel.IsDead.Value));
        }
    }
}
