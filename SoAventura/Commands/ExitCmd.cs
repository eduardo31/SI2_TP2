using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoAventura.Commands
{
    class ExitCmd : ICmd
    {
        public readonly string Description;
        public ExitCmd(string desc) {
            Description=desc;
        }
        public override string ToString()
        {
            return Description;
        }


        public void Execute(string conLink)
        {
            Environment.Exit(0);
        }

        public void ExecuteEnt()
        {
            Environment.Exit(0);
        }
    }
}
