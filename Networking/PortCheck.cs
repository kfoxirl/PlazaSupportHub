using PlazaConnectivityChecker.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PlazaConnectivityChecker.Networking
{
    class PortCheck
    {

        public void CheckPort(PortTestResults portTestResults)
        {
            
            PortCheck pc = new PortCheck();
            string server = "216.58.219.206";
            foreach (PortTestResult p in portTestResults)
            {
                try
                {
                    if (p.Protocol == "TCP")
                    {
                        try
                        {
                            try
                            {
                                p.PortAvailable = pc.TcpIsListening(server, p.PortNumber);


                            }

                            catch
                            {
                                p.PortAvailable = false;

                            }

                            finally
                            {
                                p.Loaded = true;
                            }
                        }

                        catch (Exception ex)
                        {

                        }

                    }

                    else if (p.Protocol == "UDP")
                    {
                        try
                        {

                            try
                            {
                                pc.TcpIsListening(server, p.PortNumber);
                                p.PortAvailable = true;

                            }

                            catch
                            {
                                p.PortAvailable = false;

                            }


                            finally
                            {
                                p.Loaded = true;
                            }
                        }

                        catch (Exception ex)
                        {

                        }
                    }
                }

                catch (Exception ex)
                {

                }

                finally
                {

                }
            }




        }

        public bool TcpIsListening(string server, int port)
        {
            using (TcpClient client = new TcpClient())
            {
                try
                {
                   var result = client.BeginConnect(server, port,null,null);
                   var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3));
                   client.EndConnect(result);
                }
                catch (SocketException)
                {
                    return false;
                }
                
                client.Close();
                return true;
            }
        }

        public bool UdpIsListening(string server, int port)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress serverAddr = IPAddress.Parse(server);
            IPEndPoint endPoint = new IPEndPoint(serverAddr, port);
            var timeToWait = TimeSpan.FromSeconds(2);

            string text = "Hello this a connection test from a Syngo.Plaza client to test if port is available";
            byte[] send_buffer = Encoding.ASCII.GetBytes(text);

            using (UdpClient client = new UdpClient())
            {
                try
                {
                    client.Connect(server, port);
                    try
                    {
                        sock.SendTo(send_buffer, endPoint);
                        var asyncResult = client.BeginReceive(null, null);
                        asyncResult.AsyncWaitHandle.WaitOne(timeToWait);
                        if (asyncResult.IsCompleted)
                        {
                            try
                            {
                                IPEndPoint remoteEP = null;
                                byte[] receivedData = client.EndReceive(asyncResult, ref remoteEP);
                                // EndReceive worked and we have received data and remote endpoint
                            }
                            catch (Exception ex)
                            {
                                // EndReceive failed and we ended up here
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        return false;
                    }
                }
                catch (SocketException)
                {
                    return false;
                }
                client.Close();
                return true;
            }
        }
    }
}
