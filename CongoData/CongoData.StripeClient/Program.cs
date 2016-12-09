using CongoData.StripeClient.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongoData.StripeClient {
    public class Program {
        static void Main(string[] args) {
            ProductsProgram productsCmd = new ProductsProgram();

            string command = string.Empty;
            int line = 1;

            do {
                Console.Write(string.Format("[{0,3}] > ", line));
                command = Console.ReadLine().Trim('\n');

                string[] commandParts = command.Split(' ');

                if (commandParts[0] == "products") {
                    productsCmd.Execute(commandParts);
                }
                else if (commandParts[0] != "exit") {
                    Console.WriteLine(string.Format("\tUnknown command '{0}'.", commandParts[0]));
                }

                line++;
            } while (command.ToLowerInvariant() != "exit");
        }
    }
}
