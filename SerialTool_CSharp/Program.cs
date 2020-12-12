using System;
using System.IO.Ports;
using System.Timers;

namespace SerialTool_CSharp {
    public class Program {
        static SerialPort serialPort;
        /// <summary>
        /// serialPort 收到資料時的 Event Handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e) {
            SerialPort serialPort = (SerialPort)sender;
            Byte[] buffer = new Byte[serialPort.BytesToRead];
            int readBytes = serialPort.Read(buffer, 0, buffer.Length);

            Console.WriteLine($"從 {serialPort.PortName} 收到");
            for (int i = 0; i < readBytes; i++) {
                Console.Write(String.Format("0x{0:X} ,", buffer[i]));
            }
            Console.WriteLine();
        }

        /// <summary>
        /// aTimer Event Handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTimedEvent(Object source, ElapsedEventArgs e) {
            if (serialPort.IsOpen) {
                Byte[] packet = new Byte[] { 0x30, 0x31, 0x32, 0x33, 0x34 };
                serialPort.Write(packet, 0, packet.Length);
            }
        }

        public static void Main() {
            Timer aTimer = new System.Timers.Timer(500);  // 建立1個 500ms 的 timer.
            aTimer.Elapsed += OnTimedEvent;         // timer 時間到的時候，
            aTimer.AutoReset = true;                // 自動重置 timer
            aTimer.Enabled = true;                  // 啟動 timer

            serialPort = new SerialPort("COM3", 115200);
            serialPort.DataReceived += DataReceivedEventHandler;
            serialPort.Open();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
            serialPort.Close();
        }
    }
}