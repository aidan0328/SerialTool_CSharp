using System;
using System.IO.Ports;

namespace SerialTool_CSharp {
    public class Program {
        public static void Main() {
            SerialPort serialPort = new SerialPort("COM3", 115200);
            serialPort.Open();
            Byte[] packet = new Byte[] { 0x30, 0x31, 0x32, 0x33, 0x34 };
            serialPort.Write(packet, 0, packet.Length);
            serialPort.Close();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
