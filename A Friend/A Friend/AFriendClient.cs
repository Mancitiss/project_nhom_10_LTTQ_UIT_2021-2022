using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Jil;

namespace A_Friend
{
    class AFriendClient
    {
        private static IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        private static IPAddress ipAddr = IPAddress.Any;
        private static string instruction;
        private static MessageObject first_message = null;
        private static string first_message_sender = null;

        private static int byte_expected = 0;


        public static Socket client;
        public static Account user;

        private static FormApplication UIForm;

        Random rand = new Random();

        public static void ExecuteClient(object obj)
        {
            UIForm = Program.mainform;
            try
            {
                while (user.state == 1 || user.state == 2) // while self.state == online or fake-offline
                {
                    //Console.WriteLine("In loop");
                    //Receive_from_id(client);
                    try
                    {
                        //Console.WriteLine(item.Key + " is online");
                        if (client.Connected)
                        {
                            if (client.Poll(1, SelectMode.SelectRead))
                            {
                                if (!client.Connected)
                                {
                                    // Something bad has happened, shut down
                                    try
                                    {
                                        client.Shutdown(SocketShutdown.Both);
                                        client.Close();
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.ToString());
                                    }
                                }
                                else
                                {
                                    // There is data waiting to be read"
                                    if (byte_expected == 0)
                                    {
                                        Receive_from_id(client);
                                    }
                                    else
                                    {
                                        int total_byte_received = 0;
                                        byte[] data = new Byte[byte_expected];
                                        int receivedbyte = client.Receive(data);
                                        if (receivedbyte > 0)
                                        {
                                            total_byte_received += receivedbyte;
                                            byte_expected -= receivedbyte;
                                        }
                                        while (byte_expected > 0 && receivedbyte > 0)
                                        {
                                            receivedbyte = client.Receive(data, total_byte_received, byte_expected, SocketFlags.None); 
                                            if (receivedbyte > 0) 
                                            { 
                                                total_byte_received += receivedbyte; 
                                                byte_expected -= receivedbyte; 
                                            } 
                                            else break;
                                        }
                                        if (byte_expected == 0)// all data received, send to UI
                                        {
                                            string data_string = Encoding.Unicode.GetString(data, 0, total_byte_received);
                                            Console.WriteLine("Data Received");
                                            MessageObject msgobj = JSON.Deserialize<MessageObject>(data_string);
                                            string sender = msgobj.id1;
                                            if (msgobj.sender) sender = msgobj.id2;
                                            //Console.WriteLine("{0}: {1}", sender, msgobj.message);
                                            if (user.id == msgobj.id2) //if me = user2 add user1
                                            {
                                                if (Program.mainform.Is_this_person_added(msgobj.id1))
                                                {
                                                    UIForm.panelChats[msgobj.id1].Invoke(UIForm.panelChats[msgobj.id1].AddMessageDelegate, new object[] { msgobj });
                                                    Console.WriteLine("data added");
                                                    Console.WriteLine(msgobj.message);
                                                }
                                                else
                                                {
                                                    first_message_sender = sender;
                                                    first_message = msgobj;
                                                    Console.WriteLine("Ask for info");
                                                    client.Send(Encoding.Unicode.GetBytes("0609" + sender));
                                                }
                                            } 
                                            else if (user.id == msgobj.id1) // if me = user1 add user2
                                            {
                                                if (Program.mainform.Is_this_person_added(msgobj.id2))
                                                {
                                                    UIForm.panelChats[msgobj.id2].Invoke(UIForm.panelChats[msgobj.id2].AddMessageDelegate, new object[] { msgobj });
                                                    Console.WriteLine("data added");
                                                    Console.WriteLine(msgobj.message);
                                                }
                                                else
                                                {
                                                    first_message_sender = sender;
                                                    first_message = msgobj;
                                                    Console.WriteLine("Ask for info");
                                                    client.Send(Encoding.Unicode.GetBytes("0609" + sender));
                                                }
                                            }
                                        }
                                        else // data corrupted
                                        {
                                            byte_expected = 0;
                                            Console.WriteLine("Data Corrupted");
                                        } 
                                    }
                                }
                            }
                        }
                        else
                        {
                            client.Shutdown(SocketShutdown.Both);
                            client.Close();
                            user.state = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
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
                FormApplication.currentID = user.id;
                return true;
            }
            catch (Exception e)
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
            string sent_message = id + str; // 1901 = send message // original was "1901" + id + myid + str;
            string data_string = Encoding.Unicode.GetByteCount(sent_message).ToString();
            try
            {
                self.Send(Encoding.Unicode.GetBytes("1901"+data_string.Length.ToString().PadLeft(2, '0')+data_string+sent_message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private static void Receive_from_id(Socket self)
        {
            try
            {
                byte[] bytes = new Byte[8];
                int numByte = self.Receive(bytes);
                string data = Encoding.Unicode.GetString(bytes, 0, numByte);
                if (data != null && data != "")
                {
                    instruction = data;
                    if (instruction == "1901")
                    { // 1901 = message received
                        bytes = new byte[4];
                        numByte = self.Receive(bytes, 4, SocketFlags.None);
                        data = Encoding.Unicode.GetString(bytes, 0, numByte);
                        int bytezize = Int32.Parse(data) * 2;
                        bytes = new byte[bytezize];
                        numByte = self.Receive(bytes, bytezize, SocketFlags.None);
                        data = Encoding.Unicode.GetString(bytes, 0, numByte);
                        byte_expected = Int32.Parse(data);
                    }
                    else
                    if (instruction == "1609")
                    {
                        bytes = new byte[4];
                        numByte = self.Receive(bytes, 4, SocketFlags.None);
                        data = Encoding.Unicode.GetString(bytes, 0, numByte);
                        int bytezize = Int32.Parse(data) * 2;
                        bytes = new byte[bytezize];
                        numByte = self.Receive(bytes, bytezize, SocketFlags.None);
                        data = Encoding.Unicode.GetString(bytes, 0, numByte);
                        bytezize = Int32.Parse(data);
                        bytes = new byte[bytezize];
                        numByte = self.Receive(bytes, bytezize, SocketFlags.None);
                        data = Encoding.Unicode.GetString(bytes, 0, numByte);
                        string data_found = data;
                        List<string> found = data_found.Split(' ').ToList<string>();
                        Console.WriteLine(string.Join(" ", found));
                        string name = "";
                        for (int i = 2; i < found.Count - 1; i++)
                        {
                            name += found[i] + ' ';
                        }
                        name = name.Trim();
                        Byte state;
                        Console.WriteLine("I even reached here");
                        if (Byte.TryParse(found[found.Count - 1], out state))
                        {
                            UIForm.Invoke(UIForm.addContactItemDelegate, new object[] { new Account(found[1], name, found[0], state) });
                            Console.WriteLine("New Contact Added");
                            if ((first_message_sender != "") && (first_message_sender != null) && (first_message_sender != String.Empty))
                            {
                                UIForm.panelChats[first_message_sender].Invoke(UIForm.panelChats[first_message_sender].AddMessageDelegate, new object[] { first_message });
                                first_message_sender = String.Empty;
                                first_message = null;
                            }
                            /*
                            UIForm.panelChats[found[0]].Invoke(UIForm.panelChats[found[0]].AddMessageDelegate, new object[] { data, true });
                            Console.WriteLine("Message Received");*/
                        }
                        else
                        {
                            Console.WriteLine("Data Corrupted");
                            System.Windows.Forms.MessageBox.Show("that username doesn't exist!");
                        }
                    }
                    else
                    if (instruction == "2609")
                    {
                        Console.WriteLine("No such account exists");
                        first_message = null;
                        first_message_sender = String.Empty;
                    }
                    else
                    if (instruction == "0404") //0404 = error
                    {
                        Console.WriteLine("This person is not online");
                    }
                    else
                    if (instruction == "0200")
                    { // 0200 = logged in successfully
                        user = new Account();
                        bytes = new byte[38];
                        numByte = self.Receive(bytes, 38, SocketFlags.None);
                        data = Encoding.Unicode.GetString(bytes, 0, numByte);
                        user.id = data;
                        bytes = new byte[4];
                        numByte = self.Receive(bytes, 4, SocketFlags.None);
                        data = Encoding.Unicode.GetString(bytes, 0, numByte);
                        Console.WriteLine(data);
                        int bytezize = Int32.Parse(data)*2;
                        bytes = new byte[bytezize];
                        numByte = self.Receive(bytes, bytezize, SocketFlags.None);
                        data = Encoding.Unicode.GetString(bytes, 0, numByte);
                        Console.WriteLine(data);
                        bytezize = Int32.Parse(data);
                        bytes = new byte[bytezize];
                        numByte = self.Receive(bytes, bytezize, SocketFlags.None);
                        data = Encoding.Unicode.GetString(bytes, 0, numByte);
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
                    else
                    if (instruction == "2004") // 2004 = loggin from another deive
                    {
                        Console.WriteLine("You are logged in from another device, you will be logged out");
                        user.state = 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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
                }
                else if (instruction == "1111")
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
