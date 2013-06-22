using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainfuckInterpreter {
    /// <summary>
    /// Brainfuckのオペレータ一覧
    /// </summary>
    public enum Operator {
        None,
        IncPtr,
        DecPtr,
        IncVal,
        DecVal,
        Output,
        Input,
        JumpFwd,
        JumpBack,
    }


    /// <summary>
    /// インタプリタクラス
    /// </summary>
    public class Interpreter {

        const int MemorySize = 1024;

        byte[] memory = new byte[MemorySize];

        int pointer = 0;

        List<byte> outputBytes = new List<byte>();

        /// <summary>
        /// 変換処理を実行します。
        /// </summary>
        /// <param name="operators">オペレータを配列で指定します。</param>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Operate(Operator[] operators, TextReader input) {

            outputBytes.Clear();

            int cursor = 0;

            while (cursor < operators.Length) {
                operate(operators, ref cursor, input);
            }

            return Encoding.ASCII.GetString(outputBytes.ToArray());
        }

        /// <summary>
        /// 変換処理を実行します。
        /// </summary>
        /// <param name="operators">オペレータを文字列で指定します。</param>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Operate(string operators, TextReader input) {
            var ores = ToOperators(operators).ToArray();
            return Operate(ores, input);
        }


        /// <summary>
        /// 文字列で指定されたオペレータを<see cref="BrainfuckInterpreter.Operator"/>の値に置換して
        /// 順に列挙します。
        /// </summary>
        /// <param name="operatorText"></param>
        /// <returns></returns>
        public static IEnumerable<Operator> ToOperators(string operatorText) {

            foreach (var chr in operatorText) {
                switch (chr) {
                    case '>':
                        yield return Operator.IncPtr;
                        break;
                    case '<':
                        yield return Operator.DecPtr;
                        break;
                    case '+':
                        yield return Operator.IncVal;
                        break;
                    case '-':
                        yield return Operator.DecVal;
                        break;
                    case '[':
                        yield return Operator.JumpFwd;
                        break;
                    case ']':
                        yield return Operator.JumpBack;
                        break;
                    case ',':
                        yield return Operator.Input;
                        break;
                    case '.':
                        yield return Operator.Output;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

        }


        /// <summary>
        /// 変換の実処理
        /// </summary>
        /// <param name="oprs"></param>
        /// <param name="cursor"></param>
        /// <param name="input"></param>
        private void operate(Operator[] oprs, ref int cursor, TextReader input) {
            var opr = oprs[cursor];
            switch (opr) {
                case Operator.IncPtr:
                    pointer++;
                    checkPointerLocation();
                    cursor++;
                    break;

                case Operator.DecPtr:
                    pointer--;
                    checkPointerLocation();
                    cursor++;
                    break;

                case Operator.IncVal:
                    memory[pointer]++;
                    cursor++;
                    break;

                case Operator.DecVal:
                    memory[pointer]--;
                    cursor++;
                    break;

                case Operator.Output:
                    outputBytes.Add(memory[pointer]);
                    cursor++;
                    break;

                case Operator.Input:
                    memory[pointer] = byte.Parse(input.ReadLine());
                    cursor++;
                    break;

                case Operator.JumpFwd:
                    if (memory[pointer] == 0) {
                        cursor = JumpFwdCursor(cursor, oprs);
                    } else {
                        cursor++;
                    }
                    break;

                case Operator.JumpBack:
                    if (memory[pointer] != 0) {
                        cursor = JumpBackCursor(cursor, oprs);
                    } else {
                        cursor++;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void checkPointerLocation() {
            if (pointer < 0 || MemorySize <= pointer) throw new InvalidOperationException("ポインタ領域オーバー：" + pointer);
        }


        public static int JumpFwdCursor(int before, Operator[] operators) {

            int opeCnt = 1;

            while (true) {
                if (operators.Length <= before) throw new IndexOutOfRangeException();

                var next = operators[++before];
                if (next == Operator.JumpBack) {
                    opeCnt--;
                    if (opeCnt == 0) return ++before;
                }

                if (next == Operator.JumpFwd) opeCnt++;
            }
        }


        public static int JumpBackCursor(int before, Operator[] operators) {

            int opeCnt = 1;

            while (true) {
                if (before < 0) throw new IndexOutOfRangeException();

                var next = operators[--before];
                if (next == Operator.JumpFwd) {
                    opeCnt--;
                    if (opeCnt == 0) return ++before;
                }

                if (next == Operator.JumpBack) opeCnt++;
            }

        }
    }
}
