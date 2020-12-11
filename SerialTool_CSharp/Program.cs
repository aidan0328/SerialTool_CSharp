using System;
using System.IO.Ports;

namespace SerialTool_CSharp {
    public class Program {
        public static void Main() {
            SerialPort serialPort = new SerialPort("COM3", 115200);
            serialPort.Open();
            serialPort.WriteLine($"From {serialPort.PortName}, Hello");
            serialPort.Close();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
