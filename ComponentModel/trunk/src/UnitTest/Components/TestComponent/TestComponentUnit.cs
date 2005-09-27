using System;
using ComponentModel.Container;
using ComponentModel.Interfaces;
using ComponentModel.VO;
using NUnit.Framework;

namespace UnitTest.Components.TestComponent {
    /*
     * Batería de tests para este componente.
     */
    [TestFixture]
    public sealed class TestComponentUnit {

        [SetUp]
        public void SetUp () {
        }

        [TearDown]
        public void TearDown () {
        }

        /*A partir de aqui escribiremos los tests*/

        [Test]
        public void GetComponentFromContainer () {
            Assert.IsNotNull (DefaultContainer.Instance.GetComponentByName ("TestUnidad1"));
        }

        [Test]
        public void ExecutionNonRedirect () {
            int x = 4;
            ResponseMethodVO responseMethodVO;
            int returnValue;
            
            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("TestUnidad1");
            responseMethodVO = componentModel.Execute ("ReturnValue", new object[]{x}, false);
            Assert.AreEqual (responseMethodVO.ExecutionSuccess, true);
            Assert.IsNotNull (responseMethodVO.MethodResult);
            if (responseMethodVO.ExecutionSuccess == true) {
                returnValue = (int) responseMethodVO.MethodResult;    
                Assert.AreEqual (returnValue, x);
                //Al estar usando reflection, no creo que se devuelva la misma
                //posición de memoria.
                //Assert.AreNotSame (returnValue, x);
            }
        }
    }
}
