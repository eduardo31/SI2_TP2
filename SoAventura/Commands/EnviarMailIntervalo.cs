using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoAventura.Commands
{
    class EnviarMailIntervalo : ICmd
    {
        public readonly string Description;
        public EnviarMailIntervalo(string desc) {
            Description=desc;
        }
        public override string ToString()
        {
            return Description;
        }


        public void Execute(string conLink)
        {
            int intervalo;
            List<int> prms = new List<int>();
            prms = InfoGetter(prms);

            try
            {
                intervalo = prms[0];
            }
            catch (FormatException)
            {
                Console.WriteLine("Alguns parametros estavam errados.");
                return;
            }
            using (SqlConnection con = new SqlConnection(conLink))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {

                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "EnviarMailIntervalo";

                        cmd.Parameters.Add("@Id_Evento", SqlDbType.Int).Value = intervalo;
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("Emails Enviados.");
                    }
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    Console.WriteLine(e.Message);
                    return;
                }
                tran.Commit();
                con.Close();
            }
        }

        public void ExecuteEnt()
        {
            int intervalo;
            List<int> prms = new List<int>();
            prms = InfoGetter(prms);

            try
            {
                intervalo = prms[0];
            }
            catch (FormatException)
            {
                Console.WriteLine("Alguns parametros estavam errados.");
                return;
            }
            using (var ctx = new SoAventuraEntities())
            {
                try
                {
                    ctx.EnviarMailIntervalo(intervalo);
                    ctx.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    return;
                }

            }

            Console.WriteLine("Emails Enviados.");
        }
        private List<int> InfoGetter(List<int> prms)
        {
            Console.WriteLine("Avisar clientes.");

            Console.WriteLine("Intervalo em dias:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));
            return prms;
        }


    }
}
