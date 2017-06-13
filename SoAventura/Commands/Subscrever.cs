using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoAventura.Commands
{
    class Subscrever : ICmd
    {
        public readonly string Description;
        public Subscrever(string desc) {
            Description=desc;
        }
        public override string ToString()
        {
            return Description;
        }


        public void Execute(string conLink)
        {
            int IDevento, ano, Nif;
            List<int> prms = new List<int>();
            prms = InfoGetter(prms);

            try
            {
                IDevento = prms[0];
                ano = prms[1];
                Nif = prms[2];
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
                        cmd.CommandText = "SubscreverClienteEvento";

                        cmd.Parameters.Add("@Id_Evento", SqlDbType.Int).Value = IDevento;
                        cmd.Parameters.Add("@ano", SqlDbType.Int).Value = ano;
                        cmd.Parameters.Add("@NIF", SqlDbType.Int).Value = Nif;

                        cmd.ExecuteNonQuery();

                        Console.WriteLine("Subscrição efectuada com sucesso !");
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
            int IDevento, ano, Nif;
            List<int> prms = new List<int>();
            prms = InfoGetter(prms);

            try
            {
                IDevento = prms[0];
                ano = prms[1];
                Nif = prms[2];
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
                    ctx.SubscreverClienteEvento(IDevento, ano, Nif);
                    ctx.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    return;
                }

            }

            Console.WriteLine("Subscrição efectuada com sucesso !");
        }
        private List<int> InfoGetter(List<int> prms)
        {
            Console.WriteLine("Subscrever um cliente a um Evento.");

            Console.WriteLine("Seleccionar evento:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));

            Console.WriteLine("Seleccionar ano do evento:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));

            Console.WriteLine("Nif do cliente a adicionar:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));

            return prms;
        }


    }
}
