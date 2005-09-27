using System;
using ComponentModel.Container;
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
    }
}
