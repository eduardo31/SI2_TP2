using SoAventura.Commands;
using SoAventura.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SoAventura
{

    class Program
    {
        private static List<ICmd> SetupCommands()
        {
            List<ICmd> cmds = new List<ICmd>();
            
            cmds.Add(new Subscrever("1) Subscrever cliente."));
            /*cmds.Add(new DeletePromoCmd(DELETE_PROMO));
            cmds.Add(new UpdatePromoCmd(UPDATE_PROMO));
            cmds.Add(new InsertRentFullClientCmd(INSERT_RENT_CLIENT));
            cmds.Add(new InsertRentCmd(INSERT_RENT));
            cmds.Add(new RemoveRentCmd(REMOVE_RENT));
            cmds.Add(new UpdateRentPriceCmd(UPDATE_RENTPRICE));
            cmds.Add(new ListFreeEquipDateCmd(LIST_FREE_EQUIP_DATE));
            cmds.Add(new ListEquipRentCmd(LIST_RENT));
            cmds.Add(new ExportRentCmd(XML_EXPORT_RENT));
            cmds.Add(new ExitCmd(EXIT_APP));*/
            return cmds;
        }
        static void Main(string[] args)
        {
            bool isAdo;
            List<ICmd> cmds = SetupCommands();
            Console.WriteLine("Welcome to SoAventura Console App!");

            Console.WriteLine("Insert letter A to ADO or E to Entity FrameWork approach.");
            string letter = Console.ReadLine();
            if (letter.Equals("A"))
                isAdo = true;
            else if (letter.Equals("E"))
                isAdo = false;
            else
            {
                Console.WriteLine("Invalid letter choosing ADO has default!");
                isAdo = true;
            }
            //Run(cmds, isAdo);

            ICmd cmd;
            while (true)
            {
                PrintCommands(cmds);
                int value = 0;
                try
                {
                    value = Convert.ToInt32(Console.ReadLine());

                }
                catch (FormatException)
                {
                    Console.WriteLine("Insert a number...");
                    continue;
                }
                if (value > cmds.Count || value <= 0)
                {
                    try
                    {
                        throw new MismatchedCommand("Invalid Command try another one....");
                    }
                    catch (MismatchedCommand e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }

                cmd = cmds[value - 1];

                string connectionString = ConfigurationManager.ConnectionStrings["ConnectString"].ConnectionString;
                Console.Clear();

                if (isAdo)
                    cmd.Execute(connectionString);
                else
                    cmd.ExecuteEnt();

                Console.WriteLine("\nClick to continue.");
                Console.ReadKey();
                Console.Clear();



            }

        }
        private static void PrintCommands(List<ICmd> cmds)
        {
            Console.WriteLine("Pick a command:");
            cmds.ForEach(Console.WriteLine);
        }

    }
}
