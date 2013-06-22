using BrainfuckInterpreter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainfuckInterpreterTests
{
    [TestFixture]
    public class InterpreterTest
    {
        [TestCase("+++++++++++++++++++++++++++++++++.", "!")]
        public void 入力のない命令を実行する(string operatorText, string expected) {


            var cmp = new Interpreter();
            var res = cmp.Operate(operatorText, null);

            Assert.AreEqual(expected, res);
            
        }


        [TestCase("+[+-]+", 1, 5)]
        [TestCase("+[[]]+", 1, 5)]
        [TestCase("[[][]]++", 0, 6)]
        public void カーソルの前方ジャンプ先を取得する(string operatorText, int before, int expected) {
            int res = Interpreter.JumpFwdCursor(before, Interpreter.ToOperators(operatorText).ToArray());

            Assert.AreEqual(expected, res);
        }

        [TestCase("+[+-]+", 4, 2)]
        [TestCase("+[[]]+", 4, 2)]
        [TestCase("[[][]]++", 5, 1)]
        public void カーソルの後方ジャンプ先を取得する(string operatorText, int before, int expected) {
            int res = Interpreter.JumpBackCursor(before, Interpreter.ToOperators(operatorText).ToArray());

            Assert.AreEqual(expected, res);
        }

    }
}
