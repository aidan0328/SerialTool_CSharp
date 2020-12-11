using System;
using System.IO.Ports;
using System.Globalization;

namespace SerialTool_CSharp {
    public class Program {
        public static void Main() {

            SerialPort serialPort = new SerialPort("COM3", 115200);
            serialPort.Open();
            Byte[] packet = new Byte[] { 0x30, 0x31, 0x32, 0x33, 0x34 };
            DateTime dt1 = new DateTime();
            while (true) {
                if (new TimeSpan(DateTime.Now.Ticks - dt1.Ticks).TotalMilliseconds > 500) {
                    dt1 = DateTime.Now;
                    serialPort.Write(packet, 0, packet.Length);
                }
            }
            serialPort.Close();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
