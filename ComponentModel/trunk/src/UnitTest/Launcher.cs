using System;
using NUnit.Framework;
using NUnit.Core;
using UnitTest.Components.TestComponent;

namespace UnitTest {
    /*
     *  Contendrá la suite para realizar los tests de unidad.
     */
    public sealed class Launcher {
        [Suite]
        public static TestSuite Suite {
            get {
                TestSuite suite = new TestSuite("All Tests");
                suite.Add (new DefaultContainerUnit ());
                suite.Add (new TestComponentUnit ());
                return suite;
            }
        }
        
    }
}
