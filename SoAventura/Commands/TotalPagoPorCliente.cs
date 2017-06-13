using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoAventura.Commands
{
    /*
     Obter o total pago por cliente relativo a eventos de um dado tipo num intervalo
        de datas especificado, sem usar qualquer procedimento armazenado;
         */
    class TotalPagoPorCliente : ICmd
    {
        //receber nif
        //tipo evento
        //receber data inicio e data fim
        public readonly string Description;
        public TotalPagoPorCliente(string desc)
        {
            Description = desc;
        }
        public override string ToString()
        {
            return Description;
        }


        public void Execute(string conLink)
        {
            int Nif;
            DateTime min, max;
            string tipo;
            List<string> prms = new List<string>();
            prms = InfoGetter(prms);

            try
            {
                Nif = Convert.ToInt32(prms[0]);
                min= Convert.ToDateTime(prms[1]);
                max = Convert.ToDateTime(prms[2]);
                tipo = prms[3];
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
                        cmd.CommandText = "set @Montante = (select SUM(montante) as Montante from dbo.Fatura,dbo." + tipo +
                            " where NIF = @Nif and data_pagamento<@MaxData and data_pagamento>@MinData and dbo.Fatura.Id_Evento=dbo."+tipo+".Id_Evento)";

                        cmd.Parameters.Add("@Nif", SqlDbType.Int).Value = Nif;
                        cmd.Parameters.Add("@MaxData", SqlDbType.Date).Value = max;
                        cmd.Parameters.Add("@MinData", SqlDbType.Date).Value = min;

                        var returnParameter = cmd.Parameters.Add("@Montante", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        int montante = (int)returnParameter.Value;
                        Console.WriteLine("Montante:{0}",montante);
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
            int Nif;
            DateTime min, max;
            string tipo;
            List<string> prms = new List<string>();
            prms = InfoGetter(prms);

            try
            {
                Nif = Convert.ToInt32(prms[0]);
                min = Convert.ToDateTime(prms[1]);
                max = Convert.ToDateTime(prms[2]);
                tipo = prms[3];
            }
            catch (FormatException)
            {
                Console.WriteLine("Alguns parametros estavam errados.");
                return;
            }

            int montante;
            using (var ctx = new SoAventuraEntities())
            {
                montante = ctx.Database.ExecuteSqlCommand("return select SUM(montante) from dbo.Fatura, dbo." + tipo +
                            " where NIF = {0} and data_pagamento< {1} and data_pagamento> {2} and dbo.Fatura.Id_Evento=dbo." + tipo + ".Id_Evento", Nif,max,min);

                ctx.SaveChanges();

            }
            Console.WriteLine("Montante:{0}", montante);
        }
        private List<string> InfoGetter(List<string> prms)
        {
            Console.WriteLine("Apagar eventos de um dado ano.");

            Console.WriteLine("NIF do cliente:");
            prms.Add(Console.ReadLine());
            Console.WriteLine("Data inicio (DD/MM/YYYY):");
            prms.Add(Console.ReadLine());
            Console.WriteLine("Data fim (DD/MM/YYYY):");
            prms.Add(Console.ReadLine());
            Console.WriteLine("Tipo de evento(escalada,ciclismo,canoagem,trail):");
            prms.Add(Console.ReadLine());

            return prms;
        }

    }
}
