using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoAventura
{
    public interface ICmd
    {
        void ExecuteEnt();//Entety Framework
        void Execute(string con);// ADO.NET
    }
}
