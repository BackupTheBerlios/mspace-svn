using NUnit.Framework;
using Vim.Components.VimBuffer;
using System;

namespace Vim.Test {
    [TestFixture]
    public class VimBufferStrategyTest {
        IVimBufferStrategy vimBufferStrategy;
        string text;
        
        [SetUp]
        public void Init () {
            vimBufferStrategy = new GapVimBufferStrategy ();
            text = "TestingBuffer";
            vimBufferStrategy.SetContent(text);
        }
        [TearDown]    
        public void Clean () {
            vimBufferStrategy = null;
            text = String.Empty;
        }

        [Test]
        public void TestLength () {
            Assert.AreEqual (vimBufferStrategy.Length, text.Length);
        }
    
        [Test]
        public void TestItemAt () {
            for (int i = 0; i < vimBufferStrategy.Length; i++) {
                Assert.AreEqual (vimBufferStrategy[i], text[i]);
            }
        }

        [Test]
        public void TestInsert () {
            string textInserted = "TextoInsertado";
            vimBufferStrategy.Insert (0, textInserted);
            text = textInserted + text;
            TestLength ();
            TestItemAt ();
        }

        [Test]
        public void TestInsert1 () {
            string textInserted = "TextoInsertado";
            vimBufferStrategy.Insert (vimBufferStrategy.Length, textInserted);
            text = text + textInserted;
            TestLength ();
            TestItemAt ();
        }
            
    }
}
