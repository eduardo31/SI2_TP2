using SoAventura.Tools;
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
    class EventosCancelados : ICmd
    {
        public readonly string Description;
        public EventosCancelados(string desc) {
            Description=desc;
        }
        public override string ToString()
        {
            return Description;
        }


        public void Execute(string conLink)
        {
            DateTime inicio, fim;
            List<string> prms = new List<string>();
            prms = InfoGetter(prms);

            try
            {
                inicio = Convert.ToDateTime(prms[0]);
                fim = Convert.ToDateTime(prms[1]);

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
                            cmd.CommandText = "select * from dbo.EventosCancelados(@INICIO,@FIM)";
                            cmd.Parameters.Add("@INICIO", SqlDbType.Date).Value = inicio;
                            cmd.Parameters.Add("@FIM", SqlDbType.Date).Value = fim;
                        
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                List<string> columnNames = TablePrinter.Getcolumns(dr).ToList();

                                TablePrinter.PrintTable(columnNames);

                                while (dr.Read())
                                {
                                    Console.Write(Convert.ToInt32(dr[columnNames[0]]) + " - ");
                                    Console.Write(Convert.ToInt32(dr[columnNames[1]]) + " - ");
                                    Console.Write(Convert.ToInt32(dr[columnNames[2]]) + " - ");
                                    Console.Write(Convert.ToInt32(dr[columnNames[3]]) + " - ");
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    tran.Commit();
                    con.Close();
                }
            }

        public void ExecuteEnt()
        {
            DateTime inicio, fim;
            List<string> prms = new List<string>();
            prms = InfoGetter(prms);

            try
            {
                inicio = Convert.ToDateTime(prms[0]);
                fim = Convert.ToDateTime(prms[1]);

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
                    var CanceledEvents = ctx.EventosCancelados(inicio, fim).ToList();
                    ctx.SaveChanges();

                    var colunas = typeof(EventosCancelados_Result).GetProperties().Select(prop => prop.Name).ToList();

                    TablePrinter.PrintTable(colunas);

                    CanceledEvents.ForEach(eq => Console.WriteLine(eq.CanoagemCancelados + " - " + eq.EscaladaCancelados + " - " + eq.CiclismoCancelados + " - " + eq.TrailCancelados));

                    Console.WriteLine("Listagem dos eventos cancelados.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    return;
                }

            }

        }
        private List<string> InfoGetter(List<string> prms)
        {
            Console.WriteLine("Eventos cancelados entre:");

            Console.WriteLine("Data de inicio (DD/MM/YYYY):");
            prms.Add(Console.ReadLine());

            Console.WriteLine("Data de fim (DD/MM/YYYY):");
            prms.Add(Console.ReadLine());

            return prms;
        }


    }
}
