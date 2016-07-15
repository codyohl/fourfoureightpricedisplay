using System;
using System.IO;
using System.IO.Pipes;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;

namespace PriceDisplayForwarder
{
    public class PipeServer
    {
        public static void Main()
        {
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.In, 1);
            
            while(true)
            {
                // Wait for a client to connect
                pipeServer.WaitForConnection();
                try
                {
                    // Read the request from the client.
                    StreamString ss = new StreamString(pipeServer);
                    string contents = ss.ReadString();

                    ParseAndSet(contents);
                }
                catch (IOException e)
                {
                    Console.WriteLine("ERROR: {0}", e.Message);
                }

                pipeServer.Disconnect();
            }

            //pipeServer.Close();
        }

        private static void ParseAndSet(string pair)
        {
            Match m;
            if ((m = (new Regex("(\\d+)*,*(\\d+)")).Match(pair)).Success)
            {
                SetRow(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
            }
        }

        public static void SetRow(int r, int val)
        {
            // Writes contents to serial.
            for (int i = 0; i < 2; i++)
            {
                var port = new SerialPort("COM4", 9600);
                port.Open();
                byte high = (byte)(val / 256);
                byte low = (byte)(val % 256);
                byte row = Convert.ToByte(r);
                byte[] buffer = new byte[] { high, low, row };
                port.Write(buffer, 0, 3);
                port.Close();
            }

            Console.WriteLine($"Row:{r}, Val:{val}");


        }
    }

    // Defines the data protocol for reading and writing strings on our stream
    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            int len;
            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }
    }
}
