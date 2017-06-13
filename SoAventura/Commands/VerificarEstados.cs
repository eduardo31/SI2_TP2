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
    class VerificarEstados : ICmd
    {
        public readonly string Description;
        public VerificarEstados(string desc) {
            Description=desc;
        }
        public override string ToString()
        {
            return Description;
        }


        public void Execute(string conLink)
        {
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
                        cmd.CommandText = "VerificarEstados";

                        cmd.ExecuteNonQuery();

                        Console.WriteLine("Estados atualizados!");
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
            using (var ctx = new SoAventuraEntities())
            {
                try
                {
                    ctx.VerificarEstados();
                    ctx.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    return;
                }

            }

            Console.WriteLine("Estados atualizados!");
        }
    }
}
