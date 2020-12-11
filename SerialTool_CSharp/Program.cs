using System;
using System.IO.Ports;

namespace SerialTool_CSharp {
    public class Program {
        public static void Main() {
            string[] ports = SerialPort.GetPortNames();

            foreach (string p in ports) {
                Console.WriteLine(p);
            }

            //Console.WriteLine("Press ENTER to exit.");
            //Console.ReadLine();
        }
    }
}
