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
    class PagarSubscricao : ICmd
    {
        public readonly string Description;
        public PagarSubscricao(string desc) {
            Description=desc;
        }
        public override string ToString()
        {
            return Description;
        }


        public void Execute(string conLink)
        {
            int IDevento, ano, Nif,montante;
            List<int> prms = new List<int>();
            prms = InfoGetter(prms);

            try
            {
                IDevento = prms[0];
                ano = prms[1];
                montante = prms[2];
                Nif = prms[3];

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
                        cmd.CommandText = "PagarSubscricao";

                        cmd.Parameters.Add("@Id_Evento", SqlDbType.Int).Value = IDevento;
                        cmd.Parameters.Add("@ano", SqlDbType.Int).Value = ano;
                        var Fac_ID = cmd.Parameters.Add("@Id_Factura", SqlDbType.Int);
                        cmd.Parameters.Add("@montante", SqlDbType.Int).Value = ano;
                        cmd.Parameters.Add("@NIF", SqlDbType.Int).Value = Nif;

                        
                        Fac_ID.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        Console.WriteLine("Subscrição paga com sucesso com o id {0}", (int)Fac_ID.Value);
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
            int IDevento, ano, Nif, montante;
            List<int> prms = new List<int>();
            prms = InfoGetter(prms);

            try
            {
                IDevento = prms[0];
                ano = prms[1];
                montante = prms[2];
                Nif = prms[3];

            }
            catch (FormatException)
            {
                Console.WriteLine("Alguns parametros estavam errados.");
                return;
            }

            var Fac_ID = new ObjectParameter("Id_Factura", typeof(Int32));
            using (var ctx = new SoAventuraEntities())
            {
                try
                {
                    ctx.PagarSubscricao(IDevento, ano,Fac_ID,montante, Nif);
                    ctx.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    return;
                }

            }

            Console.WriteLine("Subscrição paga com sucesso com o id {0}", (int)Fac_ID.Value);
        }
        private List<int> InfoGetter(List<int> prms)
        {
            Console.WriteLine("Pagar uma Subscrição.");

            Console.WriteLine("Seleccionar evento:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));

            Console.WriteLine("Seleccionar ano do evento:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));

            Console.WriteLine("Seleccionar o montante:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));

            Console.WriteLine("Nif do cliente a adicionar:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));

            return prms;
        }


    }
}
