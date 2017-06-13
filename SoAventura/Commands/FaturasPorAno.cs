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
    class FaturasPorAno : ICmd
    {
        public readonly string Description;
        public FaturasPorAno(string desc) {
            Description=desc;
        }
        public override string ToString()
        {
            return Description;
        }


        public void Execute(string conLink)
        {
            int ano,MONTANTEMINIMO, MONTANTEMAXIMO;
            List<int> prms = new List<int>();
            prms = InfoGetter(prms);

            try
            {
                ano = prms[0];
                MONTANTEMINIMO= prms[1];
                MONTANTEMAXIMO = prms[2];
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
                            cmd.CommandText = "select * from dbo.FaturasPorAno(@ANO,@MONTANTEMINIMO,@MONTANTEMAXIMO)";
                            cmd.Parameters.Add("@ANO", SqlDbType.Int).Value = ano;
                            cmd.Parameters.Add("@MONTANTEMINIMO", SqlDbType.SmallMoney).Value = MONTANTEMINIMO;
                            cmd.Parameters.Add("@MONTANTEMAXIMO", SqlDbType.SmallMoney).Value = MONTANTEMAXIMO;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                List<string> columnNames = TablePrinter.Getcolumns(dr).ToList();

                                TablePrinter.PrintTable(columnNames);

                                while (dr.Read())
                                {
                                    Console.Write(Convert.ToInt32(dr[columnNames[0]]) + " - "+ 
                                        Convert.ToInt32(dr[columnNames[1]]) + " - " +
                                        Convert.ToInt32(dr[columnNames[2]]) + " - " +
                                        Convert.ToInt32(dr[columnNames[3]]));
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
            int ano, MONTANTEMINIMO, MONTANTEMAXIMO;
            List<int> prms = new List<int>();
            prms = InfoGetter(prms);

            try
            {
                ano = prms[0];
                MONTANTEMINIMO = prms[1];
                MONTANTEMAXIMO = prms[2];
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
                    var CanceledEvents = ctx.FaturasPorAno(ano,MONTANTEMINIMO,MONTANTEMAXIMO).ToList();
                    ctx.SaveChanges();

                    var colunas = typeof(EventosDisponiveis_Result).GetProperties().Select(prop => prop.Name).ToList();

                    TablePrinter.PrintTable(colunas);

                    CanceledEvents.ForEach(eq => Console.WriteLine(eq.Id_Evento + " - " + eq.ano + " - " + eq.NIF + " - " + eq.montante));

                    Console.WriteLine("Listagem dos eventos Disponiveis.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    return;
                }

            }

        }
        private List<int> InfoGetter(List<int> prms)
        {
            Console.WriteLine("FaturasPorAno:");
            Console.WriteLine("Ano:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("Montante Minimo:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("Montante Maximo:");
            prms.Add(Convert.ToInt32(Console.ReadLine()));
            return prms;
        }


    }
}
