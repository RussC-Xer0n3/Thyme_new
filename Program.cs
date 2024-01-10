using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Thyme_new
{
    class Program()
    {

        static void Main()
        {
            // Example usage
            NeuralNetwork neuralNetwork = new NeuralNetwork(8, 40, 8);

            TcpListener server = null;

            try
            {
                // Set the TcpListener on a specific port.
                int port = 123;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener is used to wait for a connection from a client.
                server = new TcpListener(localAddr, port);

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

                    // Buffer to store the response bytes.
                    byte[] data = new byte[256];
                    StringBuilder responseData = new StringBuilder();

                    int bytesRead;

                    while ((bytesRead = stream.Read(data, 0, data.Length)) != 0)
                    {
                        // Translate data bytes to a UTF-8 string.
                        string receivedData = Encoding.UTF8.GetString(data, 0, bytesRead);
                        responseData.Append(receivedData);
                    }

                    // Process the received data and convert it to a double array.
                    double[] processedData = ProcessSatelliteData(responseData.ToString());

                    // Convert the double array to a string for response (for simplicity).
                    string responseString = string.Join(",", processedData);

                    // Send the response back to the client.
                    byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);

                    // Training the neural network
                    AIHelpers ai = new AIHelpers();

                    for (int epoch = 0; epoch < 10000; epoch++)
                    {
                        for (int i = 0; i < responseBytes.Length; i++)
                        {
                            neuralNetwork.Train(processedData, responseBytes, ai.CalculateMDP(processedData));
                        }
                    }
                    stream.Write(responseBytes, 0, responseBytes.Length);

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

        public static double[] ProcessSatelliteData(string rawData)
        {
            // Replace this method with your actual data processing logic.
            // For simplicity, split the received string by comma and convert to double.
            string[] values = rawData.Split(',');
            double[] result = new double[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                double.TryParse(values[i], out result[i]);
            }

            return result;
        }
    }
}
