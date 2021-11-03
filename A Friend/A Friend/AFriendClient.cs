using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace A_Friend
{
    class AFriendClient
    {
        private static IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        private static IPAddress ipAddr = IPAddress.Any;
        private static string instruction;

        public static Socket client;
        public static Account user;

        public static void ExecuteClient(object obj)
        {
            try
            {
                //Send_to_id(client, "0000000000000000002", "0000000000000000001", "alo"); How to send message
                while (user.state == 1 || user.state == 2) // while self.state == online or fake-offline
                {
                    Receive_from_id(client);
                }
                user = null;
                client.Send(Encoding.Unicode.GetBytes("2004"));
                client.Shutdown(SocketShutdown.Both);
                client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static bool Logged_in(string tk, string mk)
        {
            client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect("mancitiss.ddns.net", 11111);
                client.Send(Encoding.Unicode.GetBytes("0010" + tk + " " + mk)); //0010 = log in
                Receive_from_id(client);
                if (user == null)
                {
                    client.Send(Encoding.Unicode.GetBytes("2004")); // 2004 = stop client
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    return false;
                }
                return true;
            } catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public static void Send_to_id(Socket self, string id, string myid, string str)
        {
            // do something
            if (myid.Length != 19 || id.Length != 19)
            {
                if (myid.Length != 19) Console.WriteLine("Wrong user ID");
                else Console.WriteLine("Wrong receiver ID");
                return;
            }
            string sent_message = "1901" + id + myid + str; // 1901 = send message
            try
            {
                self.Send(Encoding.Unicode.GetBytes(sent_message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private static void Receive_from_id(Socket self)
        {
            byte[] bytes = new Byte[self.ReceiveBufferSize];
            int numByte = self.Receive(bytes);
            string data = Encoding.Unicode.GetString(bytes, 0, numByte);
            if (data != null && data != "")
            {
                instruction = data.Substring(0, 4);
                data = data.Remove(0, 4);
                if (instruction == "1901")
                { // 1901 = message received
                    string sender = data.Substring(0, 19);
                    data = data.Remove(0, 19);
                    Console.WriteLine("{0}: {1}", sender, data);
                }
                else
                if (instruction == "0404") //0404 = error
                {
                    Console.WriteLine(data);
                }
                else
                if (instruction == "0200")
                { // 0200 = logged in successfully
                    user = new Account();
                    user.id = data.Substring(0, 19);
                    data = data.Remove(0, 19);
                    user.username = data;
                    user.state = 1;
                }
                else
                if (instruction == "-200") // -200 = logged in failed
                {
                    Console.WriteLine("Thong tin dang nhap bi sai");

                }
                else
                if (instruction == "1011") // 1011 = New account created successfully
                {
                    Console.WriteLine("Tao tai khoan thanh cong");
                }
                else
                if (instruction == "1111") // 1111 = Username exists
                {
                    Console.WriteLine("Ten tai khoan da ton tai");
                }
            }
        }

        public static bool Signed_up(string tk, string mk)
        {
            try
            {
                bool success = false;

                client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                client.Connect("mancitiss.ddns.net", 11111);
                client.Send(Encoding.Unicode.GetBytes("0011" + tk + " " + mk)); //0011 = sign up
                Receive_from_id(client);
                if (instruction == "1011")
                {
                    success = true;
                } else if (instruction == "1111")
                {

                }
                Console.WriteLine(instruction);
                client.Send(Encoding.Unicode.GetBytes("2004"));
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                return success;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }
    }
}
