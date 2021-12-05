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
using System.IO;

namespace A_Friend 
{
    class AFriendClient : Form
    {
        //private static IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        private static IPAddress ipAddr = IPAddress.Any;
        private static string instruction;
        private static MessageObject first_message = null;
        private static string first_message_sender = null;

        private static int byte_expected = 0;

        internal static string temp_name = null;
        internal static string img_string = null;

        public static Socket client;
        public static Account user;

        private static FormApplication UIForm;

        private static void Logout()
        {
            first_message = null;
            first_message_sender = null;
            byte_expected = 0;
            temp_name = null;
            img_string = null;
            user = null;
            client.Send(Encoding.Unicode.GetBytes("2004"));
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        internal static void Change_name()
        {
            if (!string.IsNullOrEmpty(temp_name))
            {
                user.name = temp_name;
            }
            temp_name = null;
        }

        public static void ExecuteClient()
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
                                        if (Socket_receive(byte_expected, out string data_string))// all data received, send to UI
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
                Logout();
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

        internal static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
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

        internal static string data_with_ASCII_byte(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                string databyte = Encoding.ASCII.GetByteCount(data).ToString();
                return databyte.Length.ToString().PadLeft(2, '0') + databyte + data;
            }
            return "";
        }

        private static bool receive_data_automatically(out string data)
        {
            if (Socket_receive(4, out data))
            {
                if (Int32.TryParse(data, out int bytesize))
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

        private static bool receive_ASCII_data_automatically(out string data)
        {
            if (Socket_receive_ASCII(2, out data))
            {
                if (Int32.TryParse(data, out int bytesize))
                {
                    if (Socket_receive_ASCII(bytesize, out data))
                    {
                        if (Int32.TryParse(data, out bytesize))
                        {
                            if (Socket_receive_ASCII(bytesize, out data))
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

        private static bool Socket_receive_ASCII(int byte_expected, out string data_string)
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
            } while (byte_expected > 0 && received_byte > 0);
            Console.WriteLine("Received: {0}", total_byte_received);
            if (byte_expected == 0) // all data received
            {
                data_string = Encoding.ASCII.GetString(data, 0, total_byte_received);
                return true;
            }
            else // data corrupted
            {
                data_string = "";
                return false;
            }
        }

        public static string ImageToString(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            Image im = Image.FromFile(path);
            MemoryStream ms = new MemoryStream();
            im.Save(ms, im.RawFormat);
            byte[] array = ms.ToArray();
            return Convert.ToBase64String(array);
        }
        public static Image StringToImage(string imageString)
        {

            if (imageString == null)
                throw new ArgumentNullException("imageString");
            byte[] array = Convert.FromBase64String(imageString);
            Image image = Image.FromStream(new MemoryStream(array));
            return image;
        }

        private static void Receive_from_id(Socket self)
        {
            try
            {
                if (Socket_receive(8, out string data))
                {
                    instruction = data;
                    Console.WriteLine(data);
                    switch (instruction)
                    {
                        case "-200": // -200 = logged in failed
                            {
                                Console.WriteLine("Thong tin dang nhap bi sai");

                            } // logged in failed
                            break;
                        case "0200": // logged in successfully
                            { // 0200 = logged in successfully
                                user = new Account();
                                if (Socket_receive(38, out data)) user.id = data;
                                receive_data_automatically(out user.name);
                                user.state = 1;
                            } // successfully logged in
                            break;
                        case "0404": //0404 = this id is offline, don't worry about your nudes, they are stored *not so securely* on the server :)
                            {
                                Console.WriteLine("This person is not online");
                                if (Socket_receive(38, out string offline_id))
                                {
                                    Console.WriteLine(offline_id);
                                    UIForm.Invoke(UIForm.turnContactActiveStateDelegate, new object[] { offline_id, (byte)0 });
                                }
                            } // this id is offline
                            break;
                        case "0601": // avatar received, not loaded
                            {

                                if (receive_ASCII_data_automatically(out img_string))
                                {
                                    //user.avatar = StringToImage(img_string);
                                    Console.WriteLine("Image received");
                                }
                            } // avatar received, not loaded
                            break;
                        case "0708": // me seen
                            {
                                if (Socket_receive(38, out string panelid))
                                {
                                    if (Socket_receive(2, out string boolstr))
                                    {
                                        if (boolstr == "0" && Program.mainform.panelChats[panelid].IsLastMessageFromYou())
                                        {
                                            Program.mainform.contactItems[panelid].Unread = false;
                                        }
                                        else if (boolstr == "0" && !Program.mainform.panelChats[panelid].IsLastMessageFromYou())
                                        {
                                            Program.mainform.contactItems[panelid].Unread = true;
                                        }
                                        else
                                        {
                                            Program.mainform.contactItems[panelid].Unread = false;
                                        }
                                    }
                                }
                            } // me seen 
                            break;
                        case "1011": // 1011 = New account created successfully
                            {
                                Console.WriteLine("New account created");
                            } // New account created successfully
                            break;
                        case "1012": // name changed successfully
                            {
                                Console.WriteLine("Name changed!");
                                Change_name();
                                UIForm.formSettings.Invoke(UIForm.formSettings.changeSettingsWarning, new object[] { "Name changed successfully!", Color.FromArgb(37, 75, 133) });
                                //MessageBox.Show("What a beautiful name!");
                                //if name not change then it is your internet connection problem
                            } // successfully changed your name to a different one
                            break;
                        case "1060": // load friend's avatars 
                            {
                                if (Socket_receive(38, out string panelid))
                                {
                                    if (receive_ASCII_data_automatically(out string friend_avatar))
                                    {
                                        // friend_avatar is now a base64 image
                                        // should check if user exists first and give them their avatar after
                                        Console.WriteLine("Friend avatar received");
                                        if (!string.IsNullOrEmpty(friend_avatar))
                                        {
                                            byte[] array = Convert.FromBase64String(friend_avatar);
                                            Image image = Image.FromStream(new MemoryStream(array));
                                            UIForm.Invoke(UIForm.set_avatar_delegate, new object[] { panelid, image });
                                        }

                                        // finish
                                    }
                                }
                            } // load friend's avatars
                            break;
                        case "1111": // 1111 = Username exists
                            {
                                Console.WriteLine("This username is already in use");
                            } // username is already in use
                            break;
                        case "1609": // add contact
                            {

                                if (receive_data_automatically(out string data_found))
                                {
                                    List<string> found = data_found.Split(' ').ToList<string>();
                                    Console.WriteLine(string.Join(" ", found));
                                    string name = "";
                                    for (int i = 2; i < found.Count - 1; i++)
                                    {
                                        name += found[i] + ' ';
                                    }
                                    name = name.Trim();
                                    Console.WriteLine("I even reached here");
                                    if (Byte.TryParse(found[found.Count - 1], out byte state))
                                    {
                                        UIForm.formAddContact.Invoke(UIForm.formAddContact.changeWarningLabelDelegate, new object[] { "New contact added!", Color.FromArgb(143, 228, 185) });
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

                                        client.Send(Encoding.Unicode.GetBytes("1060" + found[0]));
                                    }
                                    else
                                    {
                                        Console.WriteLine("Data Corrupted");
                                        System.Windows.Forms.MessageBox.Show("that username doesn't exist!");
                                    }
                                }
                            } // add contact
                            break;
                        case "1901": // message received
                            { // 1901 = message received
                                if (Socket_receive(4, out data))
                                {
                                    if (Int32.TryParse(data, out int bytesize))
                                    {
                                        bytesize = bytesize * 2;
                                        if (Socket_receive(bytesize, out data)) byte_expected = Int32.Parse(data);
                                    }
                                }
                            } // message received
                            break;
                        case "2002": // message deleted
                            {
                                if (Socket_receive(38, out string panelid))
                                {
                                    if (receive_data_automatically(out string messagenumber_int))
                                    {
                                        if (UIForm.panelChats.ContainsKey(panelid))
                                        {
                                            UIForm.panelChats[panelid].Invoke(UIForm.panelChats[panelid].RemoveMessage_Invoke, new object[] { messagenumber_int });
                                        }
                                    }
                                }
                            } // message deleted
                            break;
                        case "2004": // 2004 = log in from another device
                            {
                                Console.WriteLine("You are logged in from another device, you will be logged out");
                                user.state = 0;
                            } // logged in from another device, will log out
                            break;
                        case "2211": // 2211 = this id is online
                            {
                                Console.WriteLine("This person is online");
                                if (Socket_receive(38, out string online_id))
                                {
                                    Console.WriteLine(online_id);
                                    UIForm.Invoke(UIForm.turnContactActiveStateDelegate, new object[] { online_id, (byte)1 });
                                }
                            }
                            break; // this id is online
                        case "2411": // sort contact list
                            {
                                UIForm.Invoke(UIForm.sort_contact_item_delegate);
                            } // sort contact list
                            break;
                        case "2609": // add contact failed
                            {
                                Console.WriteLine("No such account exists");
                                UIForm.formAddContact.Invoke(UIForm.formAddContact.changeWarningLabelDelegate, new object[] { "That username doesn't eixst!", Color.Red });
                                first_message = null;
                                first_message_sender = String.Empty;
                            } // add contact failed
                            break;
                        case "4269": // password changed successfully
                            {
                                Console.WriteLine("Password changed successfully!");
                                UIForm.formSettings.Invoke(UIForm.formSettings.changeSettingsWarning, new object[] { "Password changed successfully!", Color.FromArgb(143, 228, 185) });
                            } // successfully changed password
                            break;
                        case "6475":
                            // load messages
                            {
                                if (Socket_receive(38, out string panelid))
                                {
                                    Console.WriteLine(panelid);
                                    if (receive_data_automatically(out string objectdatastring))
                                    {
                                        Console.WriteLine("Old messages have come");
                                        List<MessageObject> messageObjects = JSON.Deserialize<List<MessageObject>>(objectdatastring);
                                        UIForm.panelChats[panelid].Invoke(UIForm.panelChats[panelid].LoadMessageDelegate, new object[] { messageObjects });
                                        Console.WriteLine("Message Loaded");
                                        self.Send(Encoding.Unicode.GetBytes("0708" + panelid));
                                    }
                                }
                            } // load messages
                            break;
                        case "9624": // old password is incorrect
                            {
                                Console.WriteLine("Old Password is not correct!!");
                                UIForm.formSettings.Invoke(UIForm.formSettings.changeSettingsWarning, new object[] { "Current password is incorrect!", Color.FromArgb(213, 54, 41) });

                            } // password is incorrect
                            break;
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
