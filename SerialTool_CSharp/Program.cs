using System;
using System.IO.Ports;

namespace SerialTool_CSharp {
    public class Program {
        public static void Main() {
            SerialPort serialPort = new SerialPort("COM3", 115200);
            serialPort.Open();
            Byte[] buffer = new Byte[10];
            int readBytes = serialPort.Read(buffer, 0, buffer.Length);
            serialPort.Close();

            Console.WriteLine($"從 {serialPort.PortName} 收到");
            for (int i = 0; i < readBytes; i++) {
                Console.Write(String.Format("0x{0:X} ,", buffer[i]));
            }
            Console.WriteLine();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}