---
tags: Serial Port, C#
---
<!-- 使用黑色主題 -->
{%hackmd BkVfcTxlQ %}
<!-- 決定 CSS 樣板 -->
{%hackmd @aidan/inc_hackmd_css %}

# SerialPort (Console)
## <span class="Title">主題0. GetPortNames()</span>
### <span class="SubTitle">Lab0-1 取得本機的所有 COM Port 名稱</span>
```C#=
using System;
using System.IO.Ports;

namespace SerialTool_CSharp {
    class Program {
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
```
## <span class="Title">主題1. 傳送</span>
### <span class="SubTitle">Lab1-1 傳送字串</span>
```C#=
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
```
### <span class="SubTitle">Lab1-2 來探討 static </span>
<font color = "Red">以下程式是無法編譯</font>
```C#=
using System;
using System.IO.Ports;

namespace SerialTool_CSharp {
    public class Program {
        SerialPort serialPort;
        public static void Main() {
            serialPort = new SerialPort("COM3", 115200);
            serialPort.Open();
            serialPort.WriteLine($"From {serialPort.PortName}, Hello");
            serialPort.Close();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
```

### <span class="SubTitle">Lab1-3 傳送 Byte 陣列</span>
```C#=
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
```


### <span class="SubTitle">Lab1-4 每 500ms 傳送 Byte 陣列</span>
```C#=
using System;
using System.IO.Ports;

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
```
## <span class="Title">主題2. 使用 Thread </span>
### <span class="SubTitle">Lab2-1 導入 Thread()，可以在傳送的時候，輸入 ENTER 來結束程序</span>
```C#=
using System;
using System.IO.Ports;
using System.Threading;

namespace SerialTool_CSharp {
    public class Program {
        public static void transmitMsg() {
            SerialPort serialPort = new SerialPort("COM3", 115200);
            serialPort.Open();
            Byte[] packet = new Byte[] { 0x30, 0x31, 0x32, 0x33, 0x34 };
            DateTime dt1 = new DateTime();
            Console.Write("Transmitting");
            while (true) {
                if (new TimeSpan(DateTime.Now.Ticks - dt1.Ticks).TotalMilliseconds > 500) {
                    dt1 = DateTime.Now;
                    serialPort.Write(packet, 0, packet.Length);
                    Console.Write(".");
                }
            }
        }

        public static void Main() {
            var t = new Thread(transmitMsg);
            t.Start();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
            t.Abort();
        }
    }
}
```

### <span class="SubTitle">Lab2-2 Thread 開始前，可以指定 packet 內容進行傳送</span>
```C#=
using System;
using System.IO.Ports;
using System.Threading;

namespace SerialTool_CSharp {
    public class Program {
        public static void transmitMsg() {
            SerialPort serialPort = new SerialPort("COM3", 115200);
            serialPort.Open();
            Byte[] packet = new Byte[] { 0x30, 0x31, 0x32, 0x33, 0x34 };
            DateTime dt1 = new DateTime();
            Console.Write("Transmitting");
            while (true) {
                if (new TimeSpan(DateTime.Now.Ticks - dt1.Ticks).TotalMilliseconds > 500) {
                    dt1 = DateTime.Now;
                    serialPort.Write(packet, 0, packet.Length);
                    Console.Write(".");
                }
            }
        }

        public static void transmitWithMsg(Object msg) {
            SerialPort serialPort = new SerialPort("COM3", 115200);
            serialPort.Open();
            Byte[] packet = (Byte[])msg;
            DateTime dt1 = new DateTime();
            Console.Write("Transmitting");
            while (true) {
                if (new TimeSpan(DateTime.Now.Ticks - dt1.Ticks).TotalMilliseconds > 500) {
                    dt1 = DateTime.Now;
                    serialPort.Write(packet, 0, packet.Length);
                    Console.Write(".");
                }
            }
        }
        public static void Main() {
            var t = new Thread(transmitWithMsg);
            Byte[] packet = new Byte[] { 0x65, 0x66, 0x67, 0x68, 0x69 };
            t.Start(packet);

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
            t.Abort();
        }
    }
}

```

## <span class="Title">主題3. 讀取 </span>
### <span class="SubTitle">Lab3-1 讀取字串 </span>
```C#=
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
```

### <span class="SubTitle">Lab3-2 讀取 Byte 陣列 </span>
```C#=
using System;
using System.IO.Ports;

namespace SerialTool_CSharp {
    public class Program {
        public static void Main() {
            SerialPort serialPort = new SerialPort("COM3", 115200);
            serialPort.Open();
            Byte[] packet = new Byte[10];
            int readBytes = serialPort.Read(packet, 0, packet.Length);
            serialPort.Close();
            Console.WriteLine($"從 {serialPort.PortName} 收到");
            for (int i = 0; i < readBytes; i++) {
                Console.Write(String.Format("0x{0:X} ,", packet[i]));
            }
            Console.WriteLine();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
```

### <span class="SubTitle">Lab3-3 使用 DataReceived <font color =#ff0>event</font> 讀取 Byte 陣列 </span>
<font color = "Red">以下程式是無法編譯</font>
```C#=
using System;
using System.IO.Ports;

namespace SerialTool_CSharp {
    public class Program {
        private static void DataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e) {
            SerialPort serialPort = (SerialPort)sender;
            Byte[] packet = new Byte[serialPort.BytesToRead];
            int readBytes = serialPort.Read(packet, 0, packet.Length);

            Console.WriteLine($"從 {serialPort.PortName} 收到");
            for (int i = 0; i < readBytes; i++) {
                Console.Write(String.Format("0x{0:X} ,", packet[i]));
            }
            Console.WriteLine();
        }
        public static void Main() {
            SerialPort serialPort = new SerialPort("COM3", 1一個ㄉㄜ15200);
            serialPort.DataReceived += DataReceivedEventHandler;
            serialPort.Open();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
            serialPort.Close();
        }
    }
}
```
:::spoiler 補充說明



註冊事件
![](https://i.imgur.com/BbmYdWl.png)
<span class="Figure">(Fig.3-1)</span>

SerialPort 的 DataReceived 是一個 **SerialDataReceivedEventHandler** 型別的事件
![](https://i.imgur.com/O36y7gp.png)
<span class="Figure">(Fig.3-2)</span>

**SerialDataReceivedEventHandler** 是一種 <font color =#ff0>delegate</font> 型別
![](https://i.imgur.com/EwR1ylb.png)
<span class="Figure">(Fig. 3-3)</span>

:::

## <span class="Title">主題4. 傳送與接收同時發生(全雙工) </span>
### <span class="SubTitle">Lab4-1 使用 System.Timers.Timer  </span>
<font color = "Red">以下程式只能傳送，卻無法接收</font>
```C#=
using System;
using System.IO.Ports;
using System.Timers;

namespace SerialTool_CSharp {
    public class Program {

        /// <summary>
        /// serialPort 收到資料時的 Event Handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e) {
            SerialPort serialPort = (SerialPort)sender;
            Byte[] packet = new Byte[serialPort.BytesToRead];
            int readBytes = serialPort.Read(packet, 0, packet.Length);

            Console.WriteLine($"從 {serialPort.PortName} 收到");
            for (int i = 0; i < readBytes; i++) {
                Console.Write(String.Format("0x{0:X} ,", packet[i]));
            }
            Console.WriteLine();
        }

        /// <summary>
        /// aTimer Event Handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTimedEvent(Object source, ElapsedEventArgs e) {
            SerialPort serialPort = new SerialPort("COM3", 115200);
            serialPort.DataReceived += DataReceivedEventHandler;
            serialPort.Open();

            if (serialPort.IsOpen) {
                Byte[] packet = new Byte[] { 0x30, 0x31, 0x32, 0x33, 0x34 };
                serialPort.Write(packet, 0, packet.Length);
            }
            serialPort.Close();
        }

        public static void Main() {
            Timer aTimer = new System.Timers.Timer(500);  // 建立1個 500ms 的 timer.
            aTimer.Elapsed += OnTimedEvent;         // timer 時間到的時候，
            aTimer.AutoReset = true;                // 自動重置 timer
            aTimer.Enabled = true;                  // 啟動 timer

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
```

### <span class="SubTitle">Lab4-2 收到的來自於 Access Port 的封包，就立即回傳給 Access Port (Echo Test)</span>
```C#=
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
            if ((serialPort.IsOpen)
             && (readBytes > 0)) {
                Console.WriteLine($"從 {serialPort.PortName} 收到 {packetNumber} 筆封包，長度為 {readBytes} bytes");
                serialPort.Write(packet, 0, readBytes);
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
```
:warning: 當 Access Port 傳送非常快，會出現以下的訊息
![](https://i.imgur.com/GH8cEKp.gif)

![](https://i.imgur.com/WBoRBIv.png)
<span class="Figure">(Fig. 4-1)</span>

### <span class="SubTitle">Lab4-3 用 try,catch 處理 run time error(ArgumentException) </span>
```C#=
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
                    packetNumber = 0;
                    readBytes = 0;
                }
            } catch (ArgumentException ex) {
                Console.WriteLine(ex.Message);
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
```
:interrobang: Access Port 已經停下來發送，但為什麼 Console 還是顯示收到封包呢?如下圖
![](https://i.imgur.com/RqEcxK8.gif)
<span class="Figure">(Fig. 4-2)</span>

<span class="Practice">:memo:練習1. Console 該如正確的停下來呢?如下圖
![](https://i.imgur.com/6RUsbSq.gif)
<span class="Figure">(Fig. 4-3)</span>

### <span class="SubTitle">Lab4-4 顯示出收到的封包筆數以及總 byte 數</span>
```C#=
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
            if ((serialPort.IsOpen)
             && (readBytes > 0)) {
                Console.WriteLine($"從 {serialPort.PortName} 收到 {packetNumber} 筆封包，長度為 {readBytes} bytes");
                //serialPort.Write(packet, 0, readBytes);
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
```
:interrobang: 當 Access Port (傳送端)是每 50ms 傳送一筆，會出現以下的結果
![](https://i.imgur.com/BLX2srY.png)
<span class="Figure">(Fig. 4-4)</span>


<span class="Practice">:memo:練習2. Access Port (傳送端)還是每 50ms 傳送一筆要，那該如何接收到正確的封包數，如下圖</span>
![](https://i.imgur.com/AxLE3Me.png)
<span class="Figure">(Fig. 4-5)</span>
### 