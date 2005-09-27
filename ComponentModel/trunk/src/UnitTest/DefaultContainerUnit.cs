using System;
using ComponentModel.Container;
using ComponentModel.Interfaces;
using ComponentModel.Exceptions;
using NUnit.Framework;

namespace UnitTest {
    /*
     * Batería de tests para este componente.
     */
    [TestFixture]
    public sealed class DefaultContainerUnit {

        [SetUp]
        public void SetUp () {
        }

        [TearDown]
        public void TearDown () {
        }

        /*A partir de aqui escribiremos los tests*/

        [Test]
        public void InitContainer () {
            Assert.IsNotNull (DefaultContainer.Instance);
        }

        [Test]
        public void GetComponentByName () {
            Assert.IsNotNull (DefaultContainer.Instance.GetComponentByName ("TestUnidad1"));
        }

        [Test]
        [ExpectedException (typeof (ComponentNotFoundException))]
        public void FailGetComponentByName () {
            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("NoName");
            Assert.IsNull (componentModel);
        }
    }
}
