using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PrimS.Telnet;
using LiteGuard;

namespace NetManager
{
    class Program
    {
        static private string[] commandList = {"restart_wireless"};
        static volatile bool running = true;

        static void Main(string[] args)
        {
            // TODO: Check wifi continually

            // TODO: wifi is down, so connect to other band temporarily

            // REFACTOR: reset wifi on router
            // TODO: hide login details from code (remove hard coded values)
            bool result = ResetWirelessAsync("[IP-HERE]", "TELNET-PORT", login: "[LOGIN]", password: "P/W", timeoutMS: 1000).Result;
            while(running)
            {
            }
            Console.WriteLine((result ? "\nSuccessfully accessed router through Telnet." : "\nFailed to access router through Telnet.") + "\nNow terminating.\n");

            // TODO: disconnect from other band, and reconnect to the previously dropped
        }

        private static async Task WriteTelnetLineDelayed(Client telnetCl, string line, int delayWriteMS=1000)
        {
            System.Threading.Thread.Sleep(delayWriteMS);
            await telnetCl.WriteLine(line);
        }

        public static async Task<bool> ResetWirelessAsync(string ip, int port, string login="admin", string password="admin", int timeoutMS=1000)
        {
            bool result = false;

            Guard.AgainstNullArgument("login", login);
            Guard.AgainstNullArgument("password", password);

            Console.WriteLine("Attempting to connect...\n");
            try
            {
                using (Client telnetCl = new Client(ip, port, new System.Threading.CancellationToken()))
                {
                    if (telnetCl.IsConnected)
                    {
                        Console.WriteLine("Connection successful.\nAttempting to login...\n");

                        // TODO: prob a better way to do it... but cannot get it to work
                        //bool loginResult = await client.TryLoginAsync(username, password, timeoutMS);

                        // REFACTOR: Temporary way.... see above
                        await WriteTelnetLineDelayed(telnetCl, login);
                        await WriteTelnetLineDelayed(telnetCl, password);

                        string s = await telnetCl.TerminatedReadAsync(">", TimeSpan.FromMilliseconds(timeoutMS));
                        if (s.Contains("admin@RT-N53"))
                        {
                            Console.WriteLine("Login successful.\n");
                            Console.Write(s);

                            await WriteTelnetLineDelayed(telnetCl, "echo Restarting wireless on router...\n"+commandList[0]);
                            s = await telnetCl.TerminatedReadAsync(">", TimeSpan.FromMilliseconds(timeoutMS));
                            Console.WriteLine(s);

                            result = true;
                        }
                        else
                        {
                            Console.WriteLine("Login failed.");
                        }
                    }
                }
            } catch (Exception e)
            {
                if (e.GetBaseException().GetType() == typeof(System.Net.Sockets.SocketException))
                {
                    Console.WriteLine("Connection failed.\n");
                    
                }

                Console.WriteLine(e.StackTrace);
            }

            running = false;

            return result;
        }
    }
}
