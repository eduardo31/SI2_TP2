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
    c. Apagar todas as ocorrências de eventos (e respetivas subscrições e pagamentos)
    de um dado ano, sem usar qualquer procedimento armazenado; 
    */
    class DeleteEventos : ICmd
    {
        public readonly string Description;
        public DeleteEventos(string desc)
        {
            Description = desc;
        }
        public override string ToString()
        {
            return Description;
        }


        public void Execute(string conLink)
        {
            int ano;
            List<int> prms = new List<int>();
            prms = InfoGetter(prms);

            try
            {
                ano = prms[0];
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
                        cmd.CommandText = "delete from  dbo.Fatura where ano = @ANO " +
                            "delete from  dbo.Subscrição where ano = @ANO " +
                            "delete from  dbo.trail where ano = @ANO " +
                            "delete from  dbo.ciclismo where ano = @ANO " +
                            "delete from  dbo.escalada where ano = @ANO " +
                            "delete from  dbo.canoagem where ano = @ANO " +
                            "delete from  dbo.Evento_Desportivo where ano = @ANO ";

                        cmd.Parameters.Add("@ANO", SqlDbType.Int).Value = ano;

                        cmd.ExecuteNonQuery();

                        Console.WriteLine("Eventos apagados com sucesso !");                        
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
            int ano;
            List<int> prms = new List<int>();
            prms = InfoGetter(prms);

            try
            {
                ano = prms[0];
            }
            catch (FormatException)
            {
                Console.WriteLine("Alguns parametros estavam errados.");
                return;
            }


            using (var ctx = new SoAventuraEntities())
            {
                
                ctx.Database.ExecuteSqlCommand("delete from  dbo.Fatura where ano = {0} " +
                            "delete from  dbo.Subscrição where ano = {0} " +
                            "delete from  dbo.trail where ano = {0} " +
                            "delete from  dbo.ciclismo where ano = {0} " +
                            "delete from  dbo.escalada where ano = {0} " +
                            "delete from  dbo.canoagem where ano = {0} " +
                            "delete from  dbo.Evento_Desportivo where ano = {0} ",ano);
                ctx.SaveChanges();
                }
                Console.WriteLine("Eventos apagados com sucesso !");
        }
        private List<int> InfoGetter(List<int> prms)
        {
            Console.WriteLine("Apagar eventos de um dado ano.");

            Console.WriteLine("Seleccionar ano:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));

            return prms;
        }


    }
}
