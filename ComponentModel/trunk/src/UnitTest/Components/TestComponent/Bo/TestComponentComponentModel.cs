using System;
using ComponentModel;

namespace UnitTest.Components.TestComponent {
    [Component ("TestUnidad1", "UnitTest.Components.TestComponent.Exceptions.TestComponentExceptionManager")]
    public sealed class TestComponentComponentModel : DefaultComponentModel {
        private int entero;

        public int Entero {
            get {return entero;}
            set {entero = value;}
        }
        
        [ComponentMethod ("UnitTest.Components.TestComponent.Form.MainTestComponentForm", "ResponseReturnValue")]
        public int ReturnValue (int x) {
            return x;
        }

        [ComponentMethod ("UnitTest.Components.TestComponent.Form.MainTestComponentForm", "ResponseReturnValue")]
        public char ReturnValue (char obj) {
            return obj;
        }

        [ComponentMethod ("UnitTest.Components.TestComponent.Form.MainTestComponentForm", "ResponseTestException")]
        public void TestException () {
            throw new Exception ("Testing exception Manager, and flags");
        }

        [ComponentMethod ("UnitTest.Components.TestComponent.Form.MainTestComponentForm", "ResponseNoResponse")]
        public void NoResponse () {
        }

    }
}
