using System;
using System.IO.Ports;

namespace SerialTool_CSharp {
    class Program {
        SerialPort serialPort;
        static void Main(string[] args) {
            string[] ports = SerialPort.GetPortNames();

            foreach (string p in ports) {
                Console.WriteLine(p);
            }

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
