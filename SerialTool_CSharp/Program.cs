using System;
using System.IO.Ports;

namespace SerialTool_CSharp {
    public class Program {
        public static void Main() {
            SerialPort serialPort = new SerialPort("COM3", 115200);
            serialPort.Open();
            string str = serialPort.ReadLine();
            serialPort.Close();
            Console.WriteLine($"從 {serialPort.PortName} 收到 {str}");

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}