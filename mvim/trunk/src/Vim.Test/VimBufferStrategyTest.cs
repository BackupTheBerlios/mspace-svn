using NUnit.Framework;
using Vim.Components.VimBuffer;
using System;

namespace Vim.Test {
    [TestFixture]
    public class VimBufferStrategyTest {
        IVimBufferStrategy vimBufferStrategy;
        string text;
        
        public VimBufferStrategyTest () {
        }
        
        [SetUp]
        public void Init () {
            vimBufferStrategy = new GapVimBufferStrategy ();
            text = "TestingBufferALot of Large AND extrangeaseras \t yeahhhh \n";
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

        [Test]
        public void TestInsert2 () {
            string textInserted = "TexToINsertado\n";
            vimBufferStrategy.Insert (vimBufferStrategy.Length / 2, textInserted);
            text = text.Insert (text.Length / 2, textInserted);
            TestLength ();
            TestItemAt ();
        }
       
        [Test]
        public void TestRemove () {
            int charsToRemove = 4;
            int oldLength = vimBufferStrategy.Length;
            vimBufferStrategy.Remove (0, charsToRemove);
            Assert.AreEqual (vimBufferStrategy.Length, oldLength - charsToRemove );
            text = text.Remove (0, charsToRemove);
            TestLength ();
            TestItemAt ();
        }

        [Test]
        public void TestReplace () {
            string textForReplace = "REEMPLAZANDO !!";
            int charsToReplace = 4;
            string auxString = "";
            vimBufferStrategy.Replace (0, charsToReplace, textForReplace);
            auxString = text.Substring (0, charsToReplace);
            text = text.Replace (auxString, textForReplace);
            TestLength ();
            TestItemAt ();
        }

        [Test]
        public void TextGetText () {
            string auxString = vimBufferStrategy.GetText (0,5);
            string auxString1 = text.Substring (0,5);
            Assert.AreEqual (auxString.Length, auxString1.Length);
            for (int i = 0; i < auxString.Length; i++) 
                Assert.AreEqual (auxString[i], auxString1[i]);
        }
    }
}
