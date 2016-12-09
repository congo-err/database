using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongoData.StripeClient.Programs {
    public abstract class IProgram {
        public void Execute(string[] commandParts) {
            for (int c = 1; c < commandParts.Length; c++) {
                string flag = commandParts[c];

                int nArgs = NumberOfArgs(flag);
                string[] args;

                if (nArgs > 0) {
                    args = new string[nArgs];

                    for (int i = 0; i < nArgs; i++) {
                        args[i] = commandParts[c + i];
                    }

                    ExecuteFlag(flag, args);
                }

                ExecuteFlag(flag, null);

                c += nArgs;
            }

            ExecuteCommand();
        }

        public abstract int NumberOfArgs(string flag);

        public abstract void ExecuteFlag(string flag, string[] args);

        public abstract void ExecuteCommand();
    }
}
