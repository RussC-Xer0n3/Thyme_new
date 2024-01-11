using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Thyme_new
{
    class Program()
    {

        static void TCPserver(IPAddress localAddr, int port)
        {
            // TcpListener is used to wait for a connection from a client.

            TcpListener server = new TcpListener(localAddr, port);

            try
            {
                // Start listening for client requests.
                server.Start();
                Console.WriteLine($"Server listening on port {port}");

                while (true)
                {
                    // Accept the TcpClient connection.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine($"Client connected: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");

                    // Get the client stream.
                    NetworkStream stream = client.GetStream();

                    // Read one byte from the client.
                    int byteRead = stream.ReadByte();
                    Console.WriteLine($"Received Byte: {byteRead}");

                    // Process the received byte (replace this with your actual processing logic).
                    // ...

                    // Send a response back (for simplicity, echo the byte back).
                    stream.WriteByte((byte)byteRead);

                    // Close the connection.
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
            finally
            {
                // Stop listening for new clients.
                server?.Stop();
            }

            Console.WriteLine("Server stopped.");
        }

        static void TCPClient(NeuralNetwork neuralNetwork, string localAddr, int port)
        {
            TcpClient client = null;

            try
            {
                client = new TcpClient(localAddr, port);
                // Get the client stream.
                NetworkStream stream = client.GetStream();

                // Send one byte to the server (for simplicity, send the byte 255).
                stream.WriteByte(255);

                Console.WriteLine("Byte sent successfully.");

                // Read the response byte from the server.
                int responseByte = stream.ReadByte();
                Console.WriteLine($"Received Byte from Server: {responseByte}");

                // Process the received data and convert it to a double array.
                double[] processedData = ProcessSatelliteData(responseByte.ToString());
                double[] targetData = ProcessSatelliteData(responseByte.ToString());
                Console.WriteLine($"input size {processedData.Length}");
                Console.WriteLine($"input size {targetData.Length}");

                // Convert the double array to a string for response (for simplicity).
                string responseString = string.Join(",", processedData);

                // Send the response back to the client.
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);
                Console.WriteLine($"target size {responseBytes.Length}");

                Console.WriteLine($"Received Byte from Server: {responseBytes}");
                Console.WriteLine($"Received Byte from Server as processed to double: {processedData}");
                Console.WriteLine($"Received Byte from Server as double target data: {targetData}");

                // Training the neural network
                AIHelpers ai = new AIHelpers();

                for (int epoch = 0; epoch < 2; epoch++)
                {
                    Console.WriteLine("Training started.....");
                    neuralNetwork.Train(processedData, targetData, ai.CalculateMDP(processedData));
                    Console.WriteLine($"Epoch N=: {epoch}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
            finally
            {
                client?.Close();
            }
        }
        static void Main()
        {
            // Process the received byte (replace this with your actual processing logic).
            NeuralNetwork neuralNetwork = new NeuralNetwork(1, 3, 1);

            // Example usage

            // Set the TcpListener on a specific port.
            int port = 12345;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            string address = "127.0.0.1";
            
            // Connect to the server.
            TCPserver(localAddr, port);
            TCPClient(neuralNetwork, address, port);
            
            Console.WriteLine("Server stopped.");
            
        }

        public static double[] ProcessSatelliteData(string rawData)
        {
            // Replace this method with your actual data processing logic.
            // For simplicity, split the received string by comma and convert to double.
            string[] values = rawData.Split(',');
            double[] result = new double[values.Length];
            
            Console.WriteLine("ProcessSatelliteData in use");


            for (int i = 0; i < values.Length; i++)
            {
                double.TryParse(values[i], out result[i]);
            }

            return result;
        }
    }
}
