using System;
using ComponentModel.Container;
using ComponentModel.Interfaces;
using ComponentModel.DTO;
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
            ResponseMethodDTO responseMethodDTO;
            int returnValue;
            
            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("TestUnidad1");
            responseMethodDTO = componentModel.Execute ("ReturnValue", new object[]{x}, false);
            Assert.AreEqual (responseMethodDTO.ExecutionSuccess, true);
            Assert.IsNotNull (responseMethodDTO.MethodResult);
            if (responseMethodDTO.ExecutionSuccess == true) {
                returnValue = (int) responseMethodDTO.MethodResult;    
                Assert.AreEqual (returnValue, x);
                //Al estar usando reflection, no creo que se devuelva la misma
                //posición de memoria.
                //Assert.AreNotSame (returnValue, x);
            }
        }

        [Test]
        public void ExecutionNonRedirectOverload () {
            char c = 'a';
            ResponseMethodDTO responseMethodDTO;
            char returnValue;

            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("TestUnidad1");
            responseMethodDTO = componentModel.Execute ("ReturnValue", new object[]{c}, false);
            Assert.AreEqual (responseMethodDTO.ExecutionSuccess, true);
            Assert.IsNotNull (responseMethodDTO.MethodResult);
            if (responseMethodDTO.ExecutionSuccess == true) {
                returnValue = (char) responseMethodDTO.MethodResult;
                Assert.AreEqual (returnValue, c);
            }
        }

        [Test]
        public void ExecuteRedirectNewView () {
            int x = 4;
            ResponseMethodDTO responseMethodDTO;

            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("TestUnidad1");
            responseMethodDTO = componentModel.Execute ("ReturnValue", new object[]{x});
            Assert.AreEqual (responseMethodDTO.ExecutionSuccess, true);
            Assert.IsNotNull (responseMethodDTO.MethodResult);
            Assert.AreEqual (responseMethodDTO.MethodResult, x);
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
            ResponseMethodDTO responseMethodDTO;
            
            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("TestUnidad1");
            responseMethodDTO = componentModel.Execute ("TestException", null, false);
            Assert.AreEqual (responseMethodDTO.ExecutionSuccess, false);
            Assert.IsNull (responseMethodDTO.MethodResult);
        }
    }
}
