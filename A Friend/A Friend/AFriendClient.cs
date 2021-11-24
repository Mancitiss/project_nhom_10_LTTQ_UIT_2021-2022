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

        internal static string temp_name;

        public static Socket client;
        public static Account user;

        private static FormApplication UIForm;

        Random rand = new Random();


        internal static void change_name()
        {
            if (!string.IsNullOrEmpty(temp_name))
            {
                user.name = temp_name;
            }
            temp_name = null;
        }

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
                                        string data_string;
                                        if (Socket_receive(byte_expected, out data_string))// all data received, send to UI
                                        {
                                            byte_expected = 0;
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
                                                    if (!msgobj.sender)
                                                        UIForm.Invoke(UIForm.turnContactActiveStateDelegate, new object[] { msgobj.id1, (byte)1 });
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
                                                    if (msgobj.sender)
                                                        UIForm.Invoke(UIForm.turnContactActiveStateDelegate, new object[] { msgobj.id2, (byte)1 });
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

        internal static string data_with_byte(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                string databyte = Encoding.Unicode.GetByteCount(data).ToString();
                return databyte.Length.ToString().PadLeft(2, '0') + databyte + data;
            }
            return "";
        }

        private static bool receive_data_automatically(out string data)
        {
            if (Socket_receive(4, out data))
            {
                int bytesize;
                if (Int32.TryParse(data, out bytesize))
                {
                    bytesize = bytesize * 2;
                    if (Socket_receive(bytesize, out data))
                    {
                        if (Int32.TryParse(data, out bytesize))
                        {
                            if (Socket_receive(bytesize, out data))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            data = "";
            return false;
        }

        private static bool Socket_receive(int byte_expected, out string data_string) 
        {
            int total_byte_received = 0;
            byte[] data = new byte[byte_expected];
            int received_byte;
            Console.WriteLine("Expected: {0}", byte_expected);
            do
            {
                received_byte = client.Receive(data, total_byte_received, byte_expected, SocketFlags.None);
                if (received_byte > 0)
                {
                    total_byte_received += received_byte;
                    byte_expected -= received_byte;
                }
                else break;
            } while (byte_expected > 0 &&  received_byte > 0);
            Console.WriteLine("Received: {0}",total_byte_received );
            if (byte_expected == 0) // all data received
            {
                data_string = Encoding.Unicode.GetString(data, 0, total_byte_received);
                return true;
            } else // data corrupted
            {
                data_string = "";
                return false;
            }
        }

        private static void Receive_from_id(Socket self)
        {
            try
            {
                string data;
                if (Socket_receive(8, out data))
                {
                    instruction = data;
                    Console.WriteLine(data);
                    if (instruction == "6475") 
                    {
                        string panelid;
                        if (Socket_receive(38, out panelid))
                        {
                            Console.WriteLine(panelid);
                            string objectdatastring;
                            if (receive_data_automatically(out objectdatastring))
                            {
                                Console.WriteLine("Old messages have come");
                                List<MessageObject> messageObjects = JSON.Deserialize<List<MessageObject>>(objectdatastring);
                                UIForm.panelChats[panelid].Invoke(UIForm.panelChats[panelid].LoadMessageDelegate, new object[] { messageObjects });
                                Console.WriteLine("Message Loaded");
                            }
                        }
                    }
                    else if (instruction == "2211") // 2211 = this id is online
                    {
                        Console.WriteLine("This person is online");
                        string online_id;
                        if (Socket_receive(38, out online_id))
                        {
                            Console.WriteLine(online_id);
                            UIForm.Invoke(UIForm.turnContactActiveStateDelegate, new object[] { online_id, (byte)1 });
                        }
                    }
                    else if (instruction == "0404") //0404 = this id is offline, don't worry about your nudes, they are stored *not so securely* on the server :)
                    {
                        Console.WriteLine("This person is not online");
                        string offline_id;
                        if (Socket_receive(38, out offline_id))
                        {
                            Console.WriteLine(offline_id);
                            UIForm.Invoke(UIForm.turnContactActiveStateDelegate, new object[] { offline_id, (byte)0 });
                        }
                    }
                    else if (instruction == "1901")
                    { // 1901 = message received
                        if (Socket_receive(4, out data))
                        {
                            int bytesize;
                            if (Int32.TryParse(data, out bytesize))
                            {
                                bytesize = bytesize * 2;
                                if (Socket_receive(bytesize, out data)) byte_expected = Int32.Parse(data);
                            }
                        }
                    }
                    else if (instruction == "1609")
                    {

                        string data_found;
                        if (receive_data_automatically(out data_found))
                        {
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
                                UIForm.formAddContact.Invoke(UIForm.formAddContact.changeWarningLabelDelegate, new object[] { "New contact added!", Color.FromArgb(143, 228, 185) }); 
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
                    }
                    else if (instruction == "2609")
                    {
                        Console.WriteLine("No such account exists");
                        UIForm.formAddContact.Invoke(UIForm.formAddContact.changeWarningLabelDelegate, new object[] { "That user name doesn't eixst!", Color.Red }); 
                        first_message = null;
                        first_message_sender = String.Empty;
                    }
                    else if (instruction == "0200")
                    { // 0200 = logged in successfully
                        user = new Account();
                        if (Socket_receive(38, out data)) user.id = data;
                        receive_data_automatically(out user.name);
                        user.state = 1;
                    }
                    else if (instruction == "-200") // -200 = logged in failed
                    {
                        Console.WriteLine("Thong tin dang nhap bi sai");

                    }
                    else if (instruction == "1011") // 1011 = New account created successfully
                    {
                        Console.WriteLine("Tao tai khoan thanh cong");
                    }
                    else if (instruction == "1111") // 1111 = Username exists
                    {
                        Console.WriteLine("Ten tai khoan da ton tai");
                    }
                    else if (instruction == "2004") // 2004 = loggin from another deive
                    {
                        Console.WriteLine("You are logged in from another device, you will be logged out");
                        user.state = 0;
                    }
                    else if (instruction == "4269")
                    {
                        Console.WriteLine("Password changed successfully!");
                    }
                    else if (instruction == "9624")
                    {
                        Console.WriteLine("Old Password is not correct!!");
                    }
                    else if (instruction == "1012")
                    {
                        Console.WriteLine("Name changed!");
                        change_name();
                        //MessageBox.Show("What a beautiful name!");
                        //if name not change then it is your internet connection problem
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
