using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainfuckInterpreter {
    class Program {

        static void Main(string[] args) {

            try {
                var cmp = new Interpreter();

                string order = "+++++++++[>++++++++>+++++++++++>+++++<<<-]>.>++.+++++++..+++.>-------------.<<+++++++++++++++.>.+++.------.--------.>+..";
                string res = cmp.Operate(order, Console.In);

                Console.WriteLine(res);

            } catch (Exception ex) {
                Console.WriteLine(ex);
            
            } finally {
                Console.ReadLine();
            }
        }

    }



}
