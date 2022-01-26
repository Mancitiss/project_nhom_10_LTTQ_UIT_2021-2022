﻿using System;
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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Configuration;
using System.Collections.Concurrent;

namespace A_Friend 
{
    class AFriendClient
    {
        //private static IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        //private static IPAddress ipAddr = IPAddress.Any;
        private static string instruction;

        //private static MessageObject first_message = null;
        //private static string first_message_sender = null;

        private static Dictionary<string, List<MessageObject>> first = new Dictionary<string, List<MessageObject>>();


        internal static string temp_name = null;
        internal static string img_string = null;

        public static TcpClient client;
        internal static SslStream stream;
        public static Account user;

        public static bool loginResult = true;

        private static int workeradded = 0;
        internal static ConcurrentQueue<byte[]> commands = new ConcurrentQueue<byte[]>();

        internal static void Queue_command(byte[] command)
        {
            commands.Enqueue(command);
            Ping();
            Add_worker_thread();
        }

        private static void Add_worker_thread()
        {
            Console.WriteLine("worker: {0}", workeradded);
            if (0 == Interlocked.Exchange(ref workeradded, 1))
            {
                try
                {
                    ThreadPool.QueueUserWorkItem(Send_commands);
                } catch (NotSupportedException nse)
                {
                    Console.WriteLine(nse.ToString());
                    try
                    {
                        Interlocked.Exchange(ref workeradded, 0);
                    }
                    catch { }
                }
            }
        }

        private static void Send_commands(object state)
        {
            Console.WriteLine("start sending command");
            try
            {
                while (!commands.IsEmpty) 
                    if (commands.TryDequeue(out byte[] command))
                    {
                        Console.WriteLine("Sending command...");
                        stream.Write(command);
                        Console.WriteLine("Command sent");
                        Ping();
                    }
                Ping();
                Interlocked.Exchange(ref workeradded, 0);
                Console.WriteLine("Worker reset!");
            }
            catch (Exception se)
            {
                Console.WriteLine(se.ToString());
                //throw se;
            }
        }
        private static void Logout()
        {
            //first_message = null;
            //first_message_sender = null;
            temp_name = null;
            img_string = null;
            Ping();
            user = new Account();
            user.state = 0;
            stream.Dispose();
            client.Dispose();
            GC.Collect();
        }

        internal static void Change_name()
        {
            if (!string.IsNullOrEmpty(temp_name))
            {
                user.name = temp_name;
            }
            temp_name = null;
        }


        public static async void ExecuteClient()
        {
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
                            if (client.Client.Available > 0)
                            {
                                if (!client.Connected)
                                {
                                    // Something bad has happened, shut down
                                    try
                                    {
                                        Logout();
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.ToString());
                                    }
                                }
                                else
                                {
                                    // There is data waiting to be read"
                                    Receive_from_id(client);
                                }
                            }
                        }
                        else
                        {
                            Logout();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    await Task.Delay(70);
                }
                Logout();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        internal static void Ping()
        {
            try
            {
                string server_address = ConfigurationManager.AppSettings.Get("sever_address");
                using (var client_2 = new TcpClient(server_address, Convert.ToInt16(ConfigurationManager.AppSettings.Get("port"))))
                {
                    using (var stream_2 = new SslStream(
                        client_2.GetStream(),
                        false,
                        new RemoteCertificateValidationCallback(ValidateServerCertificate),
                        null
                        ))
                    {
                        try
                        {
                            stream_2.AuthenticateAsClient(server_address);
                        }
                        catch (AuthenticationException e)
                        {
                            Console.WriteLine("Exception: {0}", e.Message);
                            if (e.InnerException != null)
                            {
                                Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                            }
                            Console.WriteLine("Authentication failed - closing the connection.");
                            stream_2.Close();
                            client_2.Close();
                        }
                        stream_2.Write(Combine(Encoding.Unicode.GetBytes("0012"), Encoding.ASCII.GetBytes(user.id)));
                        Console.WriteLine("Pinged");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static bool Logged_in(string tk, string mk)
        {
            string server_address = ConfigurationManager.AppSettings.Get("sever_address");
            client = new TcpClient(server_address, Convert.ToInt16(ConfigurationManager.AppSettings.Get("port")));
            stream = new SslStream(
                client.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null
                );
            try
            {
                stream.AuthenticateAsClient(server_address);
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine("Authentication failed - closing the connection.");
                stream.Close();
                client.Close();
                return false;
            }
            try
            {
                Queue_command(Encoding.Unicode.GetBytes("0010" + data_with_byte(tk) + data_with_byte(mk))); //0010 = log in
                Receive_from_id(client);
                if (user == null)
                {
                    Queue_command(Encoding.Unicode.GetBytes("2004")); // 2004 = stop client
                    Ping();
                    stream.Close();
                    client.Close();
                    return false;
                }
                FormApplication.currentID = user.id;
                if (!loginResult)
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public static void Send_to_id(string id, string myid, string str)
        {
            if (myid.Length != 19 || id.Length != 19)
            {
                //if (myid.Length != 19) Console.WriteLine("Wrong user ID");
                //else Console.WriteLine("Wrong receiver ID");
                return;
            }
            string sent_message = id + str; // 1901 = send message // original was "1901" + id + myid + str;
            string data_string = Encoding.Unicode.GetByteCount(sent_message).ToString();
            try
            {
                Queue_command(Encoding.Unicode.GetBytes("1901"+data_string.Length.ToString().PadLeft(2, '0')+data_string+sent_message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        internal static byte[] Combine(params byte[][] arrays)
        {
            Console.WriteLine("Start combining");
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                Console.WriteLine("Finish block copy");
                offset += array.Length;
            }
            Console.WriteLine("Finish Combining");
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
            if (Stream_receive(4, out data))
            {
                if (Int32.TryParse(data, out int bytesize))
                {
                    bytesize = bytesize * 2;
                    if (Stream_receive(bytesize, out data))
                    {
                        if (Int32.TryParse(data, out bytesize))
                        {
                            if (Stream_receive(bytesize, out data))
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
            if (Stream_receive_ASCII(2, out data))
            {
                if (Int32.TryParse(data, out int bytesize))
                {
                    if (Stream_receive_ASCII(bytesize, out data))
                    {
                        if (Int32.TryParse(data, out bytesize))
                        {
                            if (Stream_receive_ASCII(bytesize, out data))
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

        private static bool Stream_receive(int byte_expected, out string data_string) 
        {
            try
            {
                int total_byte_received = 0;
                byte[] data = new byte[byte_expected];
                int received_byte;
                Console.WriteLine("Expected: {0}", byte_expected);
                do
                {
                    received_byte = stream.Read(data, total_byte_received, byte_expected);
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
                    data_string = Encoding.Unicode.GetString(data, 0, total_byte_received);
                    return true;
                }
                else // data corrupted
                {
                    data_string = "";

                    return false;
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
        }

        private static bool Stream_receive_ASCII(int byte_expected, out string data_string)
        {
            int total_byte_received = 0;
            byte[] data = new byte[byte_expected];
            int received_byte;
            Console.WriteLine("Expected: {0}", byte_expected);
            do
            {
                received_byte = stream.Read(data, total_byte_received, byte_expected);
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

        private static /*async*/ async void Receive_from_id(TcpClient self)
        {
            try
            {
                if (Stream_receive(8, out string data))
                {
                    instruction = data;
                    Console.WriteLine(data);
                    switch (instruction)
                    {
                        case "-200": // -200 = logged in failed
                            {
                                Console.WriteLine("Thong tin dang nhap bi sai");
                                loginResult = false;
                            } // logged in failed
                            break;
                        case "0200": // logged in successfully
                            { // 0200 = logged in successfully
                                user = new Account();
                                if (Stream_receive(38, out data)) user.id = data;
                                receive_data_automatically(out user.name);
                                if (Stream_receive(10, out string priv))
                                {
                                    Console.WriteLine(priv);
                                    user.priv = bool.Parse(priv);
                                }
                                Console.WriteLine(user.priv);
                                user.state = 1;
                                // set initial private option (on or off) from here
                                if (Program.mainform != null)
                                { 
                                    if (Program.mainform.formSettings != null && Program.mainform.formSettings.IsHandleCreated)
                                        Program.mainform.formSettings.Invoke(Program.mainform.formSettings.changeIncognitoMode, new object[] { user.priv });
                                }
                                // or not, if you don't have to

                                //
                            } // successfully logged in
                            break;
                        case "0404": //0404 = this id is offline, don't worry about your nudes, they are stored *not so securely* on the server :)
                            {
                                Console.WriteLine("This person is not online");
                                if (Stream_receive(38, out string offline_id))
                                {
                                    Console.WriteLine(offline_id);
                                    Program.mainform.Invoke(Program.mainform.turnContactActiveStateDelegate, new object[] { offline_id, (byte)0 });
                                }
                            } // this id is offline
                            break;
                        case "0601": // avatar received, not loaded
                            {
                                //await Task.Delay(100);
                                
                                if (receive_ASCII_data_automatically(out img_string))
                                {
                                    //user.avatar = StringToImage(img_string);
                                    Console.WriteLine("My avatar received");
                                }
                                //int h = 1;
                                //while (!Program.mainform.formSettings.IsHandleCreated) { Console.WriteLine(h++); await Task.Delay(1000); }
                                //Program.mainform.formLoading.Invoke(Program.mainform.formLoading.Show_progress_delegate, new object[] { 0 });
                                Console.WriteLine("My Avatar finished");
                            } // avatar received, not loaded
                            break;
                        case "0708": // me seen
                            {
                                if (Stream_receive(38, out string panelid))
                                {
                                    if (Stream_receive(2, out string boolstr))
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
                                Program.mainform.formSettings.Invoke(Program.mainform.formSettings.changeSettingsWarning, new object[] { "Name changed successfully!", Color.FromArgb(37, 75, 133) });
                                //MessageBox.Show("What a beautiful name!");
                                //if name not change then it is your internet connection problem
                            } // successfully changed your name to a different one
                            break;
                        case "1060": // load friend's avatars 
                            {
                                //await Task.Delay(100);
                                if (Stream_receive(38, out string panelid))
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
                                            Program.mainform.Invoke(Program.mainform.set_avatar_delegate, new object[] { panelid, image });
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
                                        try
                                        {
                                            Program.mainform.formAddContact.Invoke(Program.mainform.formAddContact.changeWarningLabelDelegate, new object[] { "New contact added!", Color.FromArgb(143, 228, 185) });
                                            Program.mainform.Invoke(Program.mainform.addContactItemDelegate, new object[] { new Account(found[1], name, found[0], state) });
                                            Console.WriteLine("New Contact Added");
                                            if (first.ContainsKey(found[0]))
                                            {
                                                foreach (var msgobj in first[found[0]])
                                                {
                                                    Program.mainform.panelChats[found[0]].Invoke(Program.mainform.panelChats[found[0]].AddMessageDelegate, new object[] { msgobj });
                                                }
                                                first.Remove(found[0]);
                                            }
                                        }catch (Exception e)
                                        {
                                            Console.WriteLine(e.ToString());
                                            throw e;
                                        }
                                        /*
                                        if ((first_message_sender != "") && (first_message_sender != null) && (first_message_sender != String.Empty))
                                        {
                                            Program.mainform.panelChats[first_message_sender].Invoke(Program.mainform.panelChats[first_message_sender].AddMessageDelegate, new object[] { first_message });
                                            first_message_sender = String.Empty;
                                            first_message = null;
                                        }
                                        */
                                        /*
                                        Program.mainform.panelChats[found[0]].Invoke(Program.mainform.panelChats[found[0]].AddMessageDelegate, new object[] { data, true });
                                        Console.WriteLine("Message Received");*/

                                        Queue_command(Encoding.Unicode.GetBytes("1060" + found[0]));
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
                                if (Stream_receive(4, out data))
                                {
                                    if (Int32.TryParse(data, out int bytesize))
                                    {
                                        bytesize = bytesize * 2;
                                        if (Stream_receive(bytesize, out data)) 
                                        {
                                            if (Stream_receive(Int32.Parse(data), out string data_string))// all data received, send to UI
                                            {
                                                Console.WriteLine("Data Received");
                                                MessageObject msgobj = JSON.Deserialize<MessageObject>(data_string);
                                                string sender = msgobj.id1;
                                                if (msgobj.sender) sender = msgobj.id2;
                                                //Console.WriteLine("{0}: {1}", sender, msgobj.message);
                                                if (user.id == msgobj.id2) //if me = user2 add user1
                                                {
                                                    if (Program.mainform.Is_this_person_added(msgobj.id1))
                                                    {
                                                        Program.mainform.panelChats[msgobj.id1].Invoke(Program.mainform.panelChats[msgobj.id1].AddMessageDelegate, new object[] { msgobj });
                                                        Console.WriteLine("data added");
                                                        //Console.WriteLine(msgobj.message);
                                                        if (!msgobj.sender)
                                                            Program.mainform.Invoke(Program.mainform.turnContactActiveStateDelegate, new object[] { msgobj.id1, (byte)1 });
                                                    }
                                                    else
                                                    {
                                                        if (first.ContainsKey(sender))
                                                        {
                                                            first[sender].Add(msgobj);
                                                        } 
                                                        else
                                                        {
                                                            first.Add(sender, new List<MessageObject>() { msgobj });
                                                        }
                                                        Console.WriteLine("Ask for info");
                                                        Queue_command(Encoding.Unicode.GetBytes("0609" + sender));
                                                    }
                                                }
                                                else if (user.id == msgobj.id1) // if me = user1 add user2
                                                {
                                                    if (Program.mainform.Is_this_person_added(msgobj.id2))
                                                    {
                                                        Program.mainform.panelChats[msgobj.id2].Invoke(Program.mainform.panelChats[msgobj.id2].AddMessageDelegate, new object[] { msgobj });
                                                        Console.WriteLine("data added");
                                                        //Console.WriteLine(msgobj.message);
                                                        if (msgobj.sender)
                                                            Program.mainform.Invoke(Program.mainform.turnContactActiveStateDelegate, new object[] { msgobj.id2, (byte)1 });
                                                    }
                                                    else
                                                    {
                                                        if (first.ContainsKey(sender))
                                                        {
                                                            first[sender].Add(msgobj);
                                                        }
                                                        else
                                                        {
                                                            first.Add(sender, new List<MessageObject>() { msgobj });
                                                        }
                                                        Console.WriteLine("Ask for info");
                                                        Queue_command(Encoding.Unicode.GetBytes("0609" + sender));
                                                    }
                                                }
                                            }
                                            else // data corrupted
                                            {
                                                Console.WriteLine("Data Corrupted");
                                            }
                                        }
                                    }
                                }
                            } // message received
                            break;
                        case "2002": // message deleted
                            {
                                if (Stream_receive(38, out string panelid))
                                {
                                    if (receive_data_automatically(out string messagenumber))
                                    {
                                        if (Program.mainform.panelChats.ContainsKey(panelid) && long.TryParse(messagenumber, out long messagenumber_long))
                                        {
                                            Console.WriteLine("deleting: {0}", messagenumber_long);
                                            Program.mainform.panelChats[panelid].Invoke(Program.mainform.panelChats[panelid].RemoveMessage_Invoke, new object[] { messagenumber_long });
                                        }
                                    }
                                }
                            } // message deleted
                            break;
                        case "2004": // 2004 = log in from another device
                            {
                                Console.WriteLine("You are logged in from another device, you will be logged out");
                                user.state = 0;
                                Program.mainform.Invoke(Program.mainform.show_login_delegate);
                            } // logged in from another device, will log out
                            break;
                        case "2211": // 2211 = this id is online
                            {
                                Console.WriteLine("This person is online");
                                if (Stream_receive(38, out string online_id))
                                {
                                    Console.WriteLine(online_id);
                                    Program.mainform.Invoke(Program.mainform.turnContactActiveStateDelegate, new object[] { online_id, (byte)1 });
                                }
                            }
                            break; // this id is online
                        case "2411": // sort contact list
                            {
                                Program.mainform.formLoading.Invoke(Program.mainform.formLoading.Show_progress_delegate, new object[] { 100 });
                                Program.mainform.Invoke(Program.mainform.sort_contact_item_delegate);
                            } // sort contact list
                            break;
                        case "2609": // add contact failed
                            {
                                Console.WriteLine("No such account exists");
                                Program.mainform.formAddContact.Invoke(Program.mainform.formAddContact.changeWarningLabelDelegate, new object[] { "That username doesn't eixst!", Color.Red });
                                //first_message = null;
                                //first_message_sender = String.Empty;
                            } // add contact failed
                            break;
                        case "4269": // password changed successfully
                            {
                                Console.WriteLine("Password changed successfully!");
                                Program.mainform.formSettings.Invoke(Program.mainform.formSettings.changeSettingsWarning, new object[] { "Password changed successfully!", Color.FromArgb(143, 228, 185) });
                            } // successfully changed password
                            break;
                        case "6475":
                            // load messages
                            {
                                if (Stream_receive(38, out string panelid))
                                {
                                    //await Task.Delay(100);
                                    Console.WriteLine(panelid);
                                    if (receive_data_automatically(out string objectdatastring))
                                    {
                                        Console.WriteLine("Old messages have come");
                                        List<MessageObject> messageObjects = JSON.Deserialize<List<MessageObject>>(objectdatastring);
                                        try
                                        {
                                            Program.mainform.panelChats[panelid].Invoke(Program.mainform.panelChats[panelid].LoadMessageDelegate, new object[] { messageObjects });
                                        }catch(Exception asd)
                                        {
                                            Console.WriteLine(asd.ToString());
                                            if (asd.InnerException != null)
                                                Console.WriteLine(asd.InnerException.ToString());
                                        }
                                        Console.WriteLine("Message Loaded");
                                        Queue_command(Encoding.Unicode.GetBytes("0708" + panelid));
                                    }
                                }
                            } // load messages
                            break;
                        case "9624": // old password is incorrect
                            {
                                Console.WriteLine("Old Password is not correct!!");
                                Program.mainform.formSettings.Invoke(Program.mainform.formSettings.changeSettingsWarning, new object[] { "Current password is incorrect!", Color.FromArgb(213, 54, 41) });

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

                client = new TcpClient("mancitiss.duckdns.org", 11111);
                stream = new SslStream(
                    client.GetStream(),
                    false,
                    new RemoteCertificateValidationCallback(ValidateServerCertificate),
                    null
                    );
                try
                {
                    Console.WriteLine("Try authenticate");
                    stream.AuthenticateAsClient("mancitiss.duckdns.org");
                }
                catch (AuthenticationException e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                    if (e.InnerException != null)
                    {
                        Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                    }
                    Console.WriteLine("Authentication failed - closing the connection.");
                    stream.Close();
                    client.Close();
                    return false;
                }
                Console.WriteLine("Success");
                Queue_command(Encoding.Unicode.GetBytes("0011" + data_with_byte(tk) + data_with_byte(mk))); //0011 = sign up

                Receive_from_id(client);
                if (instruction == "1011")
                {
                    success = true;
                }
                else if (instruction == "1111")
                {

                }
                Console.WriteLine(instruction);
                Queue_command(Encoding.Unicode.GetBytes("2004"));
                stream.Close();
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
