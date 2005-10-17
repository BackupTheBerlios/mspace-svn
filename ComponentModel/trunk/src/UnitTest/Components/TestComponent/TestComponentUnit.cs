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
            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("TestUnidad1");
            componentModel.SetProperty ("Entero", 0);
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

        [Test]
        public void ExecutionNonRedirectOverload () {
            char c = 'a';
            ResponseMethodVO responseMethodVO;
            char returnValue;

            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("TestUnidad1");
            responseMethodVO = componentModel.Execute ("ReturnValue", new object[]{c}, false);
            Assert.AreEqual (responseMethodVO.ExecutionSuccess, true);
            Assert.IsNotNull (responseMethodVO.MethodResult);
            if (responseMethodVO.ExecutionSuccess == true) {
                returnValue = (char) responseMethodVO.MethodResult;
                Assert.AreEqual (returnValue, c);
            }
        }

        [Test]
        public void ExecuteRedirectNewView () {
            int x = 4;
            ResponseMethodVO responseMethodVO;

            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("TestUnidad1");
            responseMethodVO = componentModel.Execute ("ReturnValue", new object[]{x});
            Assert.AreEqual (responseMethodVO.ExecutionSuccess, true);
            Assert.IsNotNull (responseMethodVO.MethodResult);
            Assert.AreEqual (responseMethodVO.MethodResult, x);
            Assert.IsTrue (componentModel.ViewHandlerCollection.Count != 0);
        }

        [Test]
        public void GetProperty () {
            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("TestUnidad1");
            Assert.IsNotNull (componentModel.GetProperty ("Entero"));
            Assert.AreEqual ((int) componentModel.GetProperty ("Entero"), 0); 
        }

        [Test]
        public void SetProperty () {
            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("TestUnidad1");
            componentModel.SetProperty ("Entero", 5);
            Assert.IsNotNull (componentModel.GetProperty ("Entero"));
            Assert.AreEqual ((int) componentModel.GetProperty ("Entero"), 5);
        }

        [Test]
        public void Exception () {
            ResponseMethodVO responseMethodVO;
            
            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("TestUnidad1");
            responseMethodVO = componentModel.Execute ("TestException", null, false);
            Assert.AreEqual (responseMethodVO.ExecutionSuccess, false);
            Assert.IsNull (responseMethodVO.MethodResult);
        }
    }
}
