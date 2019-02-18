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
        static void Main(string[] args)
        {
            int port = 0x0;
            string ip = "ip-here";

            using (Client client = new Client(ip, port, new System.Threading.CancellationToken()))
            {
                if (client.IsConnected)
                {
                    Console.WriteLine("Connected.");
                //    (await client.TryLoginAsync("username", "password", TimeoutMs)).Should().Be(true);
                //client.WriteLine("show statistic wan2");
                //string s = await client.TerminatedReadAsync(">", TimeSpan.FromMilliseconds(TimeoutMs));
                //s.Should().Contain(">");
                //s.Should().Contain("WAN2");
                //Regex regEx = new Regex("(?!WAN2 total TX: )([0-9.]*)(?! GB ,RX: )([0-9.]*)(?= GB)");
                //regEx.IsMatch(s).Should().Be(true);
                //MatchCollection matches = regEx.Matches(s);
                //decimal tx = decimal.Parse(matches[0].Value);
                //decimal rx = decimal.Parse(matches[1].Value);
                //(tx + rx).Should().BeLessThan(50);
                }
            }
        }
    }
}
