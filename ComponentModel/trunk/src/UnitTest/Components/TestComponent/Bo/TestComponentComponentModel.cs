using System;
using ComponentModel;

namespace UnitTest.Components.TestComponent {
    [Component ("TestUnidad1", "UnitTest.Components.TestComponent.Exceptions.TestComponentExceptionManager")]
    public sealed class TestComponentComponentModel : DefaultComponentModel {
        
        [ComponentMethod ("ResponseReturnValue","UnitTest.Components.TestComponent.Form.MainTestComponentForm")]
        public int ReturnValue (int x) {
            return x;
        }

    }
}
