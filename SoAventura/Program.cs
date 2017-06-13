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
            
            cmds.Add(new Subscrever("1) Subscrever cliente."));//E
            cmds.Add(new PagarSubscricao("2) Pagar uma subscrição."));//F
            cmds.Add(new VerificarEstados("3) Atualizar Estados em relação a data corrente."));//G
            cmds.Add(new EnviarMailIntervalo("4) Enviar avisos por email a todos os clientes inscritos em eventos que se irão realizar num intervalo de tempo."));//H
            cmds.Add(new EventosCancelados("5) Listar a contagem dos eventos cancelados."));//I
            cmds.Add(new EventosDisponiveis("6) Listar todos os eventos com lugares disponíveis para um intervalo de datas."));//J
            cmds.Add(new FaturasPorAno("7) Obter os pagamentos realizados num dado ano com um intervalo de amostragem especificado."));//K
            cmds.Add(new DeleteEventos("8) Eliminar Eventos de um dado ano."));//1C
            int last = cmds.Count+1;
            cmds.Add(new ExitCmd(last+") Para fechar a aplicação."));
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
