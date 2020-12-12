using System;
using System.IO.Ports;
using System.Timers;

namespace SerialTool_CSharp {
    public class Program {
        static SerialPort serialPort;
        static Byte[] packet;
        static int packetNumber;
        static int readBytes;

        /// <summary>
        /// serialPort 收到資料時的 Event Handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e) {
            SerialPort serialPort = (SerialPort)sender;
            packet = new Byte[serialPort.BytesToRead];
            packetNumber++;
            readBytes += serialPort.Read(packet, 0, packet.Length);
        }

        /// <summary>
        /// aTimer Event Handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTimedEvent(Object source, ElapsedEventArgs e) {
            try {
                if ((serialPort.IsOpen)
                 && (readBytes > 0)) {
                    Console.WriteLine($"從 {serialPort.PortName} 收到 {packetNumber} 筆封包，長度為 {readBytes} bytes");
                    serialPort.Write(packet, 0, readBytes);
                }
            } catch (ArgumentException ex) {
                Console.WriteLine(ex.Message);
            } finally {
                packetNumber = 0;
                readBytes = 0;
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