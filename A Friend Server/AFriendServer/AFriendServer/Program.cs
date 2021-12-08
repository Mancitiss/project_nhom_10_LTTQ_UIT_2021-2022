﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using CryptSharp;
using Jil;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;

namespace AFriendServer
{
    internal partial class Program
    {
        static string s = "";
        static X509Certificate serverCertificate = new X509Certificate(Environment.GetEnvironmentVariable("certpath", EnvironmentVariableTarget.User), Environment.GetEnvironmentVariable("certpass", EnvironmentVariableTarget.User));

        static Dictionary<string, TcpClient> dictionary = new Dictionary<string, TcpClient>();
        static Dictionary<string, SslStream> streams = new Dictionary<string, SslStream>();
        //static Dictionary<string, Int64> bytes = new Dictionary<string, long>();
        static SqlConnection sql;
        static Random rand;

        //static Dictionary<string, int> byte_expected = new Dictionary<string, int>();
        //static Dictionary<string, bool> is_processing = new Dictionary<string, bool>();
        static Dictionary<string, bool> is_locked = new Dictionary<string, bool>();
        static Dictionary<string, int> loaded = new Dictionary<string, int>();

        static Thread main_thread, loop;
        static ManualResetEvent mainstop = new ManualResetEvent(true);

        public static Int64 NextInt64(Random rnd)
        {
            var buffer = new byte[sizeof(Int64)];
            rnd.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        public static void Main(string[] args)
        {
            main_thread = Thread.CurrentThread;
            try
            {
                Console.WriteLine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

                rand = new Random();

                using (sql = new SqlConnection(
                        "Data Source=" +
                        Environment.GetEnvironmentVariable("DBServer", EnvironmentVariableTarget.User) +
                        ";Initial Catalog=" +
                        Environment.GetEnvironmentVariable("DBicatalog", EnvironmentVariableTarget.User) +
                        ";User ID=" +
                        Environment.GetEnvironmentVariable("DBusername", EnvironmentVariableTarget.User) +
                        ";Password=" +
                        Environment.GetEnvironmentVariable("DBpassword", EnvironmentVariableTarget.User) +
                        ";MultipleActiveResultSets = true"
                        ))
                {
                    try
                    {

                        sql.Open();
                        Thread Clientloop = new Thread(new ParameterizedThreadStart(Client_Loop));
                        Clientloop.IsBackground = true;
                        Clientloop.Start();
                        loop = Clientloop;
                        ExecuteServer();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void clear(string id)
        {
            Console.WriteLine("{0} has quit", id);
            try
            {
                streams[id].Dispose();
            }
            catch (Exception e)
            {

            }
            try
            {
                dictionary[id].Dispose();
            }
            catch (Exception e)
            {

            }
            string str_id = id;
            streams.Remove(id);
            dictionary.Remove(id);
            //byte_expected.Remove(id);
            //is_processing.Remove(id);
            is_locked.Remove(id);
            loaded.Remove(id);
            //bytes.Remove(id);
            /*
            while (str_id[0] == '0' && str_id.Length > 1) str_id.Remove(0, 1);
            using (SqlCommand cmd = new SqlCommand("update top (1) account set state=0 where id=@id", sql))
            {
                cmd.Parameters.AddWithValue("@id", Int64.Parse(str_id));
                cmd.ExecuteNonQuery();
            }*/
        }

        private static void shutdown(KeyValuePair<string, TcpClient> item) 
        {
            Console.WriteLine("{0} has quit", item.Key);
            try
            {
                streams[item.Key].Dispose();
            } catch (Exception e)
            {

            }
            try
            {
                dictionary[item.Key].Dispose();
            } catch (Exception e)
            {

            }
            string str_id = item.Key;
            //bytes.Remove(item.Key);
            while (str_id[0] == '0' && str_id.Length > 1) str_id.Remove(0, 1);
            using (SqlCommand cmd = new SqlCommand("update top (1) account set state=0 where id=@id", sql))
            {
                cmd.Parameters.AddWithValue("@id", Int64.Parse(str_id));
                cmd.ExecuteNonQuery();
            }
            //clear(item.Key);
        }

        private static void exception_handler(KeyValuePair<string, TcpClient> item, string se)
        {
            if (se.Contains("open and available Connection"))
            {
                sql.Open();
            }
            else if (se.Contains("was forcibly closed"))
            {
                shutdown(item);
            }
        }

        /*
        private static void process_message(object obj)
        {

            KeyValuePair<string, TcpClient> item = (KeyValuePair<string, TcpClient>)obj;
            SslStream s = streams[item.Key];
            try
            {
                string data_string;
                if (SslStream_receive(s, byte_expected[item.Key], out data_string))// all data received, save to database, send to socket
                {
                    byte_expected[item.Key] = 0;
                    string receiver_id = data_string.Substring(0, 19);
                    data_string = data_string.Remove(0, 19);
                    //save to database start
                    string id1, id2;
                    if (item.Key.CompareTo(receiver_id) <= 0)
                    {
                        id1 = item.Key;
                        id2 = receiver_id;
                    } else
                    {
                        id1 = receiver_id;
                        id2 = item.Key;
                    }
                    string sqlmessage = data_string;
                    try
                    {
                        bool success = false;
                        DateTime now = DateTime.Now;
                        using (SqlCommand command = new SqlCommand("insert into message values (@id1, @id2, @n, @datetimenow, @sender, @message)", sql))
                        {
                            command.Parameters.AddWithValue("@id1", id1);
                            command.Parameters.AddWithValue("@id2", id2);
                            command.Parameters.AddWithValue("@n", rand.Next(-1000000000, 0));
                            command.Parameters.AddWithValue("@datetimenow", now);
                            command.Parameters.AddWithValue("@sender", item.Key == id2);
                            command.Parameters.AddWithValue("@message", sqlmessage);
                            if (command.ExecuteNonQuery() >= 1) success = true;
                        }
                        if (success)
                        {
                            using (SqlCommand another_command = new SqlCommand("select top 1 * from message where id1 = @id1 and id2 = @id2 and timesent = @timesent and sender = @sender", sql))
                            {
                                another_command.Parameters.AddWithValue("@id1", id1);
                                another_command.Parameters.AddWithValue("@id2", id2);
                                another_command.Parameters.AddWithValue("@timesent", now);
                                another_command.Parameters.AddWithValue("sender", item.Key == id2);
                                using (SqlDataReader reader = another_command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        MessageObject msgobj = new MessageObject(reader["id1"].ToString().PadLeft(19, '0'), reader["id2"].ToString().PadLeft(19, '0'), (Int64)reader["messagenumber"], (DateTime)reader["timesent"], (bool)reader["sender"], reader["message"].ToString());
                                        //data_string = data_string.Insert(0, item.Key);
                                        //send to socket start
                                        if (item.Key != receiver_id) Send_to_id(item.Key, msgobj);
                                        if (!Send_to_id(receiver_id, msgobj))
                                        {
                                            s.Write(Encoding.Unicode.GetBytes("0404"+receiver_id));
                                        } else
                                        {
                                            s.Write(Encoding.Unicode.GetBytes("2211"+receiver_id));
                                        }
                                        Console.WriteLine("Sent");
                                        //send to socket end
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        exception_handler(item, e.ToString());
                    }
                    //save to database end
                }
                else // data corrupted
                {
                    byte_expected[item.Key] = 0;
                    Console.WriteLine("Data Corrupted");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                exception_handler(item, e.ToString());
            }
            finally
            {
                try
                {
                    is_processing[item.Key] = false;
                    is_locked[item.Key] = false;
                } catch (Exception e)
                {
                    // do nothing
                }
            }
        }*/

        private static void Client_Loop(object obj)
        {
            while (true)
            {
                try
                {
                    foreach (var item in dictionary)
                    {
                        try
                        {
                            if (is_locked[item.Key]) continue;
                            //Console.WriteLine(item.Key + " is online");
                            if (item.Value.Connected)
                            {
                                //Console.WriteLine(item.Value.Available);
                                if (item.Value.Client.Poll(1, SelectMode.SelectRead)/* || byte_expected[item.Key]!=0*/)
                                {
                                    //bytes[item.Key] += item.Value.Available;
                                    if (!item.Value.Connected) // Something bad has happened, shut down
                                    {
                                        try
                                        {
                                            shutdown(item);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.ToString());
                                        }
                                    }
                                    else // There is data waiting to be read"
                                    {
                                        //if (is_locked[item.Key]) continue;
                                        is_locked[item.Key] = true;
                                        if (true /*byte_expected[item.Key] == 0*/)
                                        {
                                            try
                                            {
                                                //Console.WriteLine("new work created");
                                                Thread work = new Thread(new ParameterizedThreadStart(Receive_message));
                                                work.IsBackground = true;
                                                work.Start(item);
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.ToString());
                                            }
                                            /*
                                            finally
                                            {
                                                //is_locked[item.Key] = false;
                                            }*/
                                        }
                                        /*
                                        else if (!is_processing[item.Key])
                                        {
                                            try
                                            {
                                                is_processing[item.Key] = true;
                                                Thread process = new Thread(new ParameterizedThreadStart(process_message));
                                                process.IsBackground = true;
                                                process.Start(item);
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.ToString());
                                            }
                                            finally
                                            {
                                                //is_locked[item.Key] = false;
                                            }
                                        }*/
                                    }
                                }
                            }
                            else
                            {
                                clear(item.Key);
                            }
                        }
                        catch (Exception e)
                        {
                            /*
                            Console.WriteLine(e.ToString());
                            try 
                            {
                                exception_handler(item, e.ToString());
                            }
                            catch
                            {

                            }
                            finally
                            {
                                clear(item.Key);
                            }
                            */
                        } 
                    }
                }
                catch (Exception e)
                {
                    // do nothing
                }
            }
        }

        private static void ExecuteServer()
        {
            // Establish the local endpoint
            // for the socket. Dns.GetHostName
            // returns the name of the host
            // running the application.

            //Old Socket code below

            /*
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

            // Creation TCP/IP Socket using
            // Socket Class Constructor
            Socket listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);
            

            */ // Old Socket code above


            // new TcpClient code below

            TcpListener listener = new TcpListener(IPAddress.Any, 11111);
            listener.Start();

            Console.WriteLine("Server at: {0}", IPAddress.Any);
            // new TcpClient code above

            try
            {
                /*
                listener.Bind(localEndPoint);
                listener.Listen(1000);
                */

                while (true)
                {
                    Thread.Sleep(10);
                    TcpClient client = listener.AcceptTcpClient();

                    Console.WriteLine("Accepted Client");
                    try
                    {
                        Receive_from_socket_not_logged_in(client);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    client = null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static bool receive_ASCII_data_automatically(SslStream s, out string data)
        {
            if (SslStream_receive_ASCII(s, 2, out data))
            {
                int bytesize;
                if (Int32.TryParse(data, out bytesize))
                {
                    if (SslStream_receive_ASCII(s, bytesize, out data))
                    {
                        if (Int32.TryParse(data, out bytesize))
                        {
                            if (SslStream_receive_ASCII(s, bytesize, out data))
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

        private static bool receive_data_automatically(SslStream s, out string data)
        {
            if (SslStream_receive(s, 4, out data))
            {
                //Console.WriteLine("1:"+data);
                int bytesize;
                if (Int32.TryParse(data, out bytesize))
                {
                    bytesize = bytesize * 2;
                    if (SslStream_receive(s, bytesize, out data))
                    {
                        //Console.WriteLine("2:" + data);
                        if (Int32.TryParse(data, out bytesize))
                        {
                            if (SslStream_receive(s, bytesize, out data))
                            {
                                //Console.WriteLine("3:" + data);
                                return true;
                            }
                        }
                    }
                }
            }
            //Console.WriteLine("wrong data: " + data);
            data = "";
            return false;
        }

        private static bool SslStream_receive_ASCII(SslStream s, int byte_expected, out string data_string)
        {
            int total_byte_received = 0;
            byte[] data = new byte[byte_expected];
            int received_byte;
            do
            {
                received_byte = s.Read(data, total_byte_received, byte_expected);
                if (received_byte > 0)
                {
                    total_byte_received += received_byte;
                    byte_expected -= received_byte;
                }
                else break;
            } while (byte_expected > 0 && received_byte > 0);
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

        private static bool SslStream_receive(SslStream s, int byte_expected, out string data_string)
        {
            int total_byte_received = 0;
            byte[] data = new byte[byte_expected];
            int received_byte;
            do
            {
                received_byte = s.Read(data, total_byte_received, byte_expected);
                if (received_byte > 0)
                {
                    total_byte_received += received_byte;
                    byte_expected -= received_byte;
                }
                else break;
            } while (byte_expected > 0 && received_byte > 0);
            if (byte_expected == 0) // all data received
            {
                data_string = Encoding.Unicode.GetString(data, 0, total_byte_received);
                return true;
            }
            else // data corrupted
            {
                //Console.WriteLine("Corrupted: " + Encoding.Unicode.GetString(data, 0, total_byte_received));
                data_string = "";
                return false;
            }
        }

        private static string data_with_byte(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                string databyte = Encoding.Unicode.GetByteCount(data).ToString();
                return databyte.Length.ToString().PadLeft(2, '0') + databyte + data;
            }
            return "";
        }

        private static string data_with_ASCII_byte(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                string databyte = Encoding.ASCII.GetByteCount(data).ToString();
                return databyte.Length.ToString().PadLeft(2, '0') + databyte + data;
            }
            return "";
        }

        private static void Receive_message(object si)
        {
            Console.WriteLine("Work Started");
            KeyValuePair<string, TcpClient> item = (KeyValuePair<string, TcpClient>)si;
            SslStream s = streams[item.Key];
            try
            {
                string data;
                if (SslStream_receive(s, 8, out data))
                {
                    Console.WriteLine("Work: " + data);
                    if (data != null && data != "")
                    {
                        string instruction = data;
                        switch (instruction) 
                        {
                            case "6475":
                                {
                                    string receiver_id;
                                    if (SslStream_receive(s, 38, out receiver_id))
                                    {
                                        Console.WriteLine(receiver_id);
                                        string id1, id2;
                                        if (item.Key.CompareTo(receiver_id) <= 0)
                                        {
                                            id1 = item.Key;
                                            id2 = receiver_id;
                                        }
                                        else
                                        {
                                            id1 = receiver_id;
                                            id2 = item.Key;
                                        }
                                        if (receive_data_automatically(s, out data))
                                        {
                                            //Console.WriteLine(data);
                                            Int64 num;
                                            if (Int64.TryParse(data, out num))
                                            {
                                                if (num == 0)
                                                {
                                                    SqlCommand command = new SqlCommand("select top 1 count from friend where id1=@id1 and id2=@id2", sql);
                                                    command.Parameters.AddWithValue("@id1", id1);
                                                    command.Parameters.AddWithValue("@id2", id2);
                                                    using (SqlDataReader reader = command.ExecuteReader())
                                                    {
                                                        if (reader.Read())
                                                        {
                                                            num = (Int64)reader["count"];
                                                        }
                                                    }
                                                    //Console.WriteLine(num);
                                                    int i = 0;
                                                    List<MessageObject> messageObjects = new List<MessageObject>();
                                                    while (num > 0 && i < 50)
                                                    {
                                                        command = new SqlCommand("select top 1 * from message where id1=@id1 and id2=@id2 and messagenumber=@messagenumber", sql);
                                                        command.Parameters.AddWithValue("@id1", id1);
                                                        command.Parameters.AddWithValue("@id2", id2);
                                                        command.Parameters.AddWithValue("@messagenumber", num);
                                                        using (SqlDataReader reader = command.ExecuteReader())
                                                        {
                                                            if (reader.Read())
                                                            {
                                                                //Console.WriteLine((DateTime)reader["timesent"]);
                                                                MessageObject msgobj = new MessageObject(reader["id1"].ToString().PadLeft(19, '0'), reader["id2"].ToString().PadLeft(19, '0'), (Int64)reader["messagenumber"], (DateTime)reader["timesent"], (bool)reader["sender"], reader["message"].ToString());
                                                                messageObjects.Add(msgobj);
                                                            }
                                                        }
                                                        num = num - 1;
                                                        i = i + 1;
                                                    }
                                                    string datasend = JSON.Serialize<List<MessageObject>>(messageObjects);
                                                    string datasendbyte = Encoding.Unicode.GetByteCount(datasend).ToString();
                                                    s.Write(Encoding.Unicode.GetBytes("6475" + receiver_id + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
                                                    //Console.WriteLine("Old messages sent");
                                                    if (loaded.ContainsKey(item.Key))
                                                    {
                                                        if (loaded[item.Key] <= 0)
                                                        {
                                                            loaded.Remove(item.Key);
                                                        }
                                                        else if (loaded[item.Key] > 1)
                                                        {
                                                            loaded[item.Key] -= 1;
                                                        }
                                                        else if (loaded[item.Key] == 1)
                                                        {
                                                            s.Write(Encoding.Unicode.GetBytes("2411"));
                                                            loaded[item.Key] -= 1;
                                                            loaded.Remove(item.Key);
                                                        }
                                                    }
                                                }
                                                else if (num > 1)
                                                {
                                                    int i = 0;
                                                    SqlCommand command;
                                                    List<MessageObject> messageObjects = new List<MessageObject>();
                                                    while (num > 0 && i < 50)
                                                    {
                                                        command = new SqlCommand("select top 1 * from message where id1=@id1 and id2=@id2 and messagenumber=@messagenumber", sql);
                                                        command.Parameters.AddWithValue("@id1", id1);
                                                        command.Parameters.AddWithValue("@id2", id2);
                                                        command.Parameters.AddWithValue("@messagenumber", num);
                                                        using (SqlDataReader reader = command.ExecuteReader())
                                                        {
                                                            if (reader.Read())
                                                            {
                                                                MessageObject msgobj = new MessageObject(reader["id1"].ToString().PadLeft(19, '0'), reader["id2"].ToString().PadLeft(19, '0'), (Int64)reader["messagenumber"], (DateTime)reader["timesent"], (bool)reader["sender"], reader["message"].ToString());
                                                                messageObjects.Add(msgobj);
                                                            }
                                                        }
                                                        num = num - 1;
                                                        i = i + 1;
                                                    }
                                                    string datasend = JSON.Serialize<List<MessageObject>>(messageObjects);
                                                    string datasendbyte = Encoding.Unicode.GetByteCount(datasend).ToString();
                                                    s.Write(Encoding.Unicode.GetBytes("6475" + receiver_id + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
                                                    //Console.WriteLine("Old messages sent");
                                                }
                                            }
                                        }
                                    }

                                    break;
                                } // load message

                            case "1901":
                                {
                                    SslStream_receive(s, 4, out data);
                                    int bytesize = Int32.Parse(data) * 2;
                                    SslStream_receive(s, bytesize, out data);
                                    Int32.TryParse(data, out int temp);
                                    if (SslStream_receive(s, temp, out string data_string))// all data received, save to database, send to socket
                                    {
                                        string receiver_id = data_string.Substring(0, 19);
                                        data_string = data_string.Remove(0, 19);
                                        //save to database start
                                        string id1, id2;
                                        if (item.Key.CompareTo(receiver_id) <= 0)
                                        {
                                            id1 = item.Key;
                                            id2 = receiver_id;
                                        }
                                        else
                                        {
                                            id1 = receiver_id;
                                            id2 = item.Key;
                                        }
                                        string sqlmessage = data_string;
                                        try
                                        {
                                            bool success = false;
                                            DateTime now = DateTime.Now;
                                            using (SqlCommand command = new SqlCommand("insert into message values (@id1, @id2, @n, @datetimenow, @sender, @message)", sql))
                                            {
                                                command.Parameters.AddWithValue("@id1", id1);
                                                command.Parameters.AddWithValue("@id2", id2);
                                                command.Parameters.AddWithValue("@n", rand.Next(-1000000000, 0));
                                                command.Parameters.AddWithValue("@datetimenow", now);
                                                command.Parameters.AddWithValue("@sender", item.Key == id2);
                                                command.Parameters.AddWithValue("@message", sqlmessage);
                                                if (command.ExecuteNonQuery() >= 1) success = true;
                                            }
                                            if (success)
                                            {
                                                using (SqlCommand another_command = new SqlCommand("select top 1 * from message where id1 = @id1 and id2 = @id2 and timesent = @timesent and sender = @sender", sql))
                                                {
                                                    another_command.Parameters.AddWithValue("@id1", id1);
                                                    another_command.Parameters.AddWithValue("@id2", id2);
                                                    another_command.Parameters.AddWithValue("@timesent", now);
                                                    another_command.Parameters.AddWithValue("sender", item.Key == id2);
                                                    using (SqlDataReader reader = another_command.ExecuteReader())
                                                    {
                                                        if (reader.Read())
                                                        {
                                                            MessageObject msgobj = new MessageObject(reader["id1"].ToString().PadLeft(19, '0'), reader["id2"].ToString().PadLeft(19, '0'), (Int64)reader["messagenumber"], (DateTime)reader["timesent"], (bool)reader["sender"], reader["message"].ToString());
                                                            //data_string = data_string.Insert(0, item.Key);
                                                            //send to socket start
                                                            if (item.Key != receiver_id) Send_to_id(item.Key, msgobj);
                                                            if (!Send_to_id(receiver_id, msgobj))
                                                            {
                                                                s.Write(Encoding.Unicode.GetBytes("0404" + receiver_id));
                                                            }
                                                            else
                                                            {
                                                                s.Write(Encoding.Unicode.GetBytes("2211" + receiver_id));
                                                            }
                                                            Console.WriteLine("Sent");
                                                            //send to socket end
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.ToString());
                                            exception_handler(item, e.ToString());
                                        }
                                        //save to database end
                                    }
                                    else // data corrupted
                                    {
                                        Console.WriteLine("Data Corrupted");
                                    }
                                    /*
                                    byte_expected[item.Key] = temp;*/
                                    //Console.WriteLine("After Message:" + item.Value.Available);
                                    break;
                                } // handle message

                            case "1234":
                                {
                                    string receiver_id;
                                    if (SslStream_receive(s, 38, out receiver_id))
                                    {
                                        string id1, id2;
                                        if (item.Key.CompareTo(receiver_id) <= 0)
                                        {
                                            id1 = item.Key;
                                            id2 = receiver_id;
                                        }
                                        else
                                        {
                                            id1 = receiver_id;
                                            id2 = item.Key;
                                        }
                                        string boolstr;
                                        if (SslStream_receive(s, 2, out boolstr))
                                        {
                                            using (SqlCommand command = new SqlCommand("update top (1) seen set seen=@bool where id1=@id1 and id2=@id2", sql))
                                            {
                                                if (boolstr == "0")
                                                {
                                                    command.Parameters.AddWithValue("@bool", 0);
                                                }
                                                else
                                                {
                                                    command.Parameters.AddWithValue("@bool", 1);
                                                }
                                                command.Parameters.AddWithValue("@id1", id1);
                                                command.Parameters.AddWithValue("@id2", id2);
                                                command.ExecuteNonQuery();
                                            }
                                        }
                                    }

                                    break;
                                } // me seen

                            case "0708":
                                {
                                    string receiver_id;
                                    if (SslStream_receive(s, 38, out receiver_id))
                                    {
                                        string id1, id2;
                                        if (item.Key.CompareTo(receiver_id) <= 0)
                                        {
                                            id1 = item.Key;
                                            id2 = receiver_id;
                                        }
                                        else
                                        {
                                            id1 = receiver_id;
                                            id2 = item.Key;
                                        }
                                        string commandtext = "select top 1 seen from seen where id1=@id1 and id2=@id2";
                                        SqlCommand command = new SqlCommand(commandtext, sql);
                                        command.Parameters.AddWithValue("@id1", Int64.Parse(id1));
                                        command.Parameters.AddWithValue("@id2", Int64.Parse(id2));
                                        bool result = false;
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                result = (bool)reader[0];
                                            }
                                        }
                                        if (result)
                                        {
                                            s.Write(Encoding.Unicode.GetBytes("0708" + receiver_id + "1"));
                                        }
                                        else
                                        {
                                            s.Write(Encoding.Unicode.GetBytes("0708" + receiver_id + "0"));
                                        }
                                    }

                                    break;
                                } // load seen

                            case "2002":
                                {
                                    string receiver_id;
                                    if (SslStream_receive(s, 38, out receiver_id))
                                    {
                                        string id1, id2;
                                        if (item.Key.CompareTo(receiver_id) <= 0)
                                        {
                                            id1 = item.Key;
                                            id2 = receiver_id;
                                        }
                                        else
                                        {
                                            id1 = receiver_id;
                                            id2 = item.Key;
                                        }
                                        string messagenumberstring;
                                        if (receive_data_automatically(s, out messagenumberstring))
                                        {
                                            long messagenumber;
                                            if (long.TryParse(messagenumberstring, out messagenumber))
                                            {
                                                using (SqlCommand command = new SqlCommand("delete top (1) from message where id1=@id1 and id2=@id2 and messagenumber=@messagenumber", sql))
                                                {
                                                    command.Parameters.AddWithValue("@id1", id1);
                                                    command.Parameters.AddWithValue("@id2", id2);
                                                    command.Parameters.AddWithValue("messagenumber", messagenumber);
                                                    command.ExecuteNonQuery();
                                                }
                                            }
                                            if (dictionary.ContainsKey(receiver_id))
                                            {
                                                streams[receiver_id].Write(Encoding.Unicode.GetBytes("2002" + item.Key + data_with_byte(messagenumber.ToString())));
                                            }
                                        }
                                    }

                                    break;
                                } // delete message

                            case "2004":
                                shutdown(item);
                                break;
                            case "0609":
                                {
                                    if (SslStream_receive(s, 38, out data))
                                    {
                                        string commandtext = "select top 1 id, username, name, state from account where id=@id";
                                        SqlCommand command = new SqlCommand(commandtext, sql);
                                        command.Parameters.AddWithValue("@id", Int64.Parse(data));
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                string datasend = reader["id"].ToString().PadLeft(19, '0') + " " + reader["username"].ToString() + " " + reader["name"].ToString() + " " + reader["state"].ToString();
                                                string datasendbyte = Encoding.Unicode.GetByteCount(datasend).ToString();
                                                s.Write(Encoding.Unicode.GetBytes("1609" + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
                                            }
                                            else
                                            {
                                                s.Write(Encoding.Unicode.GetBytes("2609")); // info not found
                                            }
                                        }
                                    }

                                    break;
                                } // iplookup

                            case "0610":
                                {
                                    if (receive_data_automatically(s, out data))
                                    {
                                        string commandtext = "select top 1 id, username, name, state from account where username=@username";
                                        SqlCommand command = new SqlCommand(commandtext, sql);
                                        command.Parameters.AddWithValue("@username", data);
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                string datasend = reader["id"].ToString().PadLeft(19, '0') + " " + reader["username"].ToString() + " " + reader["name"].ToString() + " " + reader["state"].ToString();
                                                string datasendbyte = Encoding.Unicode.GetByteCount(datasend).ToString();
                                                s.Write(Encoding.Unicode.GetBytes("1609" + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
                                                //Console.WriteLine("Info sent");
                                            }
                                            else
                                            {
                                                s.Write(Encoding.Unicode.GetBytes("2609")); // info not found
                                            }
                                        }
                                    }

                                    break;
                                } //nameloopkpup

                            case "1060":
                                {
                                    string requested_id;
                                    if (SslStream_receive(s, 38, out requested_id))
                                    {
                                        SqlCommand command = new SqlCommand("select avatar from account where id=@id", sql);
                                        command.Parameters.AddWithValue("@id", requested_id);
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                if (reader[0].GetType() != typeof(DBNull))
                                                {
                                                    s.Write(Combine(Encoding.Unicode.GetBytes("1060" + requested_id), Encoding.ASCII.GetBytes(data_with_ASCII_byte(reader[0].ToString()))));
                                                    Console.WriteLine("Friend avatar sent!");
                                                }
                                            }
                                        }
                                    }

                                    break;
                                } // load friend's avatars

                            case "0601":
                                {
                                    string img_string;
                                    if (receive_ASCII_data_automatically(s, out img_string))
                                    {
                                        using (SqlCommand command = new SqlCommand("update top (1) account set avatar=@avatar where id=@id", sql))
                                        {
                                            command.Parameters.AddWithValue("@avatar", img_string);
                                            command.Parameters.AddWithValue("@id", Int64.Parse(item.Key));
                                            command.ExecuteNonQuery();
                                        }
                                    }

                                    break;
                                } // set avatar

                            case "4269":
                                {
                                    string opw;
                                    if (receive_data_automatically(s, out opw))
                                    {
                                        string pw;
                                        if (receive_data_automatically(s, out pw))
                                        {
                                            SqlCommand command = new SqlCommand("Select top 1 pw from account where id=@id", sql);
                                            Int64 longkey;
                                            if (Int64.TryParse(item.Key, out longkey))
                                            {
                                                command.Parameters.AddWithValue("@id", Int64.Parse(item.Key));
                                                using (SqlDataReader reader = command.ExecuteReader())
                                                {
                                                    if (reader.Read())
                                                    {
                                                        if (Crypter.CheckPassword(opw, reader["pw"].ToString()))
                                                        {
                                                            using (SqlCommand changepass = new SqlCommand("update top (1) account set pw = @pw where id = @id", sql))
                                                            {
                                                                changepass.Parameters.AddWithValue("@pw", Crypter.Blowfish.Crypt(pw));
                                                                changepass.Parameters.AddWithValue("@id", item.Key);
                                                                if (changepass.ExecuteNonQuery() == 1)
                                                                {
                                                                    s.Write(Encoding.Unicode.GetBytes("4269"));
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            s.Write(Encoding.Unicode.GetBytes("9624"));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    break;
                                } // change pass

                            case "1508":
                                {
                                    using (SqlCommand command = new SqlCommand("update top (1) account set private=1 where id=@id", sql))
                                    {
                                        command.Parameters.AddWithValue("@id", item.Key);
                                        command.ExecuteNonQuery();
                                    }
                                    break;
                                } // set private = true

                            case "0508":
                                {
                                    using (SqlCommand command = new SqlCommand("update top (1) account set private=0 where id=@id", sql))
                                    {
                                        command.Parameters.AddWithValue("@id", item.Key);
                                        command.ExecuteNonQuery();
                                    }
                                    break;
                                } // set private = false;

                            case "1012":
                                {
                                    string newname;
                                    if (receive_data_automatically(s, out newname))
                                    {
                                        using (SqlCommand changename = new SqlCommand("update top (1) account set name = @name where id = @id", sql))
                                        {
                                            changename.Parameters.AddWithValue("@name", newname);
                                            changename.Parameters.AddWithValue("@id", item.Key);
                                            if (changename.ExecuteNonQuery() == 1)
                                            {
                                                streams[item.Key].Write(Encoding.Unicode.GetBytes("1012"));
                                            }
                                        }
                                    }

                                    break;
                                } // user has changed their name

                            case "5859":
                                {
                                    string receiver_id;
                                    if (SslStream_receive(s, 38, out receiver_id))
                                    {
                                        string id1, id2;
                                        if (item.Key.CompareTo(receiver_id) <= 0)
                                        {
                                            id1 = item.Key;
                                            id2 = receiver_id;
                                        }
                                        else
                                        {
                                            id1 = receiver_id;
                                            id2 = item.Key;
                                        }
                                        using (SqlCommand command = new SqlCommand("delete top (1) from friend where id1=@id1 and id2=@id2", sql))
                                        {
                                            command.Parameters.AddWithValue("@id1", id1);
                                            command.Parameters.AddWithValue("@id2", id2);
                                            command.ExecuteNonQuery();
                                        }
                                        Thread thread = new Thread(() => Delete_conversation_thread(id1, id2));
                                        thread.IsBackground = true;
                                        thread.Start();
                                    }

                                    break;
                                } // delete conversation

                            default:
                                shutdown(item);
                                Console.WriteLine("Received strange signal, socket closed");
                                break;
                        }
                    }
                    else
                    {
                        shutdown(item);
                        Console.WriteLine("Received strange signal, socket closed (2)");
                    }
                    streams[item.Key] = s;
                    Console.WriteLine("Work finished");
                }
                else
                {
                    shutdown(item);
                    Console.WriteLine("Received strange signal, socket closed (3)");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Work quitted");
                exception_handler(item, e.ToString());
            }
            finally
            {
                try
                {
                    is_locked[item.Key] = false;
                } catch (Exception e)
                {
                    // do nothing
                }
            }
        }

        private static void Delete_conversation_thread(string id1, string id2)
        {
            using (SqlCommand command = new SqlCommand("delete from message where id1=@id1 and id2=@id2", sql))
            {
                command.Parameters.AddWithValue("@id1", id1);
                command.Parameters.AddWithValue("@id2", id2);
                command.ExecuteNonQuery();
            }
            using (SqlCommand command = new SqlCommand("delete from seen where id1=@id1 and id2=@id2", sql))
            {
                command.Parameters.AddWithValue("@id1", id1);
                command.Parameters.AddWithValue("@id2", id2);
                command.ExecuteNonQuery();
            }
        }

        private static bool Send_to_id(string id, MessageObject msgobj)
        {
            // do something
            bool success = false;
            if (dictionary.ContainsKey(id))
            {
                try
                {
                    string data = JSON.Serialize<MessageObject>(msgobj);
                    string data_string = Encoding.Unicode.GetByteCount(data).ToString();
                    streams[id].Write(Encoding.Unicode.GetBytes("1901" + data_string.Length.ToString().PadLeft(2, '0') + data_string + data));
                    streams[id].Flush();
                    success = true;
                }
                catch (Exception e)
                {
                    success = false;
                    Console.WriteLine(e.ToString());
                }
            }
            return success;
        }

        private static byte[] Combine(params byte[][] arrays)
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

        private static void Receive_from_socket_not_logged_in(TcpClient c)
        {
            // new client code below

            // A client has connected. Create the
            // SslStream using the client's network stream.
            SslStream sslStream = new SslStream(c.GetStream(), false);
            // Authenticate the server but don't require the client to authenticate.
            try
            {
                sslStream.AuthenticateAsServer(serverCertificate, clientCertificateRequired: false, checkCertificateRevocation: true);

                // Display the properties and settings for the authenticated stream.
                /*
                DisplaySecurityLevel(sslStream);
                DisplaySecurityServices(sslStream);
                DisplayCertificateInformation(sslStream);
                DisplayStreamProperties(sslStream);
                */
                // Set timeouts for the read and write to 5 seconds.

                SslStream_receive(sslStream, 8, out string data);
                Console.WriteLine("not logged in:"+data);
                // new client code above
                /*
                // do something
                byte[] bytes = new byte[s.ReceiveBufferSize];

                //read the identifier from client
                int numByte = s.Receive(bytes);

                string data = Encoding.Unicode.GetString(bytes,
                                           0, numByte);*/
                if (data != null && data != "")
                {
                    string instruction = data;

                    if (instruction == "0010") // 0010 = log in
                    {
                        receive_data_automatically(sslStream, out data);
                        string[] list_str = new string[2];
                        list_str[0] = data;
                        receive_data_automatically(sslStream, out data);
                        list_str[1] = data;
                        Console.WriteLine(list_str[0]);
                        Console.WriteLine(list_str[1]);
                        try
                        {
                            Console.WriteLine("Before avatar");
                            string commandtext = "select top 1 id, name, pw, avatar, private from account where username=@username";
                            SqlCommand command = new SqlCommand(commandtext, sql);
                            command.Parameters.AddWithValue("@username", list_str[0]);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                Console.WriteLine("After avatar");
                                if (reader.Read())
                                {
                                    //if (list_str[1] == reader["pw"].ToString())
                                    if (list_str[1] == reader["pw"].ToString() || Crypter.CheckPassword(list_str[1], reader["pw"].ToString()))
                                    {
                                        if (list_str[1] == reader["pw"].ToString())
                                        {
                                            using (SqlCommand changepass = new SqlCommand("update top (1) account set pw = @pw where id = @id", sql))
                                            {
                                                changepass.Parameters.AddWithValue("@pw", Crypter.Blowfish.Crypt(list_str[1]));
                                                changepass.Parameters.AddWithValue("@id", reader["id"].ToString());
                                                changepass.ExecuteNonQuery();
                                            }
                                        }
                                        string id = reader["id"].ToString();
                                        string str_id = id;
                                        while (id.Length < 19) id = '0' + id;

                                        string name = reader["name"].ToString();
                                        string namebyte = Encoding.Unicode.GetByteCount(name).ToString();

                                        sslStream.Write(Encoding.Unicode.GetBytes("0200"
                                            + id + namebyte.Length.ToString().PadLeft(2, '0') + namebyte + name + reader["private"].ToString()));
                                        Console.WriteLine("Before state");
                                        //state was here
                                        Console.WriteLine("Before dictionaries");
                                        try
                                        {
                                            try
                                            {
                                                is_locked.Add(id, true);
                                            }
                                            catch (Exception e)
                                            {
                                                is_locked[id] = true;
                                            }

                                            if (streams.ContainsKey(id))
                                            {
                                                try
                                                {
                                                    streams[id].Write(Encoding.Unicode.GetBytes("2004"));
                                                }
                                                catch
                                                {

                                                }
                                                try
                                                {
                                                    streams[id].Dispose();
                                                    streams[id] = sslStream;
                                                }
                                                catch (Exception ex)
                                                {
                                                    streams.Remove(id);
                                                    streams.Add(id, sslStream);
                                                }
                                            } else
                                            {
                                                streams.Add(id, sslStream);
                                            }
                                            if (dictionary.ContainsKey(id))
                                            {
                                                Console.WriteLine("another one");
                                                try
                                                {
                                                    dictionary[id].Dispose();
                                                    dictionary[id] = c;
                                                }catch (Exception ex)
                                                {
                                                    dictionary.Remove(id);
                                                    dictionary.Add(id, c);
                                                }
                                                
                                            } else
                                            {
                                                dictionary.Add(id, c);
                                            }
                                        
                                            //add the entry in the dictionary
                                            try
                                            {
                                                loaded.Add(id, 0);
                                            }
                                            catch (Exception e)
                                            {
                                                loaded[id] = 0;
                                            }
                                            /*
                                            try
                                            {
                                                byte_expected.Add(id, 0);
                                            }
                                            catch (Exception e)
                                            {
                                                byte_expected[id] = 0;
                                            }
                                            try
                                            {
                                                is_processing.Add(id, false);
                                            }
                                            catch (Exception e)
                                            {
                                                is_processing[id] = false;
                                            }*/
                                            Console.WriteLine("got id");

                                            Int64 id_int = (Int64)reader["id"];
                                            SqlCommand friendcommand = new SqlCommand("select id1, id2 from friend where id1=@id or id2=@id", sql);
                                            friendcommand.Parameters.AddWithValue("@id", id_int);
                                            using (SqlDataReader friendreader = friendcommand.ExecuteReader())
                                            {
                                                while (friendreader.Read())
                                                {
                                                    loaded[id] += 1;
                                                    Int64 friendid = (Int64)friendreader["id1"];
                                                    if (id_int == friendid) friendid = (Int64)friendreader["id2"];

                                                    string friendcommandtext = "select top 1 id, username, name, state from account where id=@id";
                                                    SqlCommand friendcommandget = new SqlCommand(friendcommandtext, sql);
                                                    friendcommandget.Parameters.AddWithValue("@id", friendid);
                                                    using (SqlDataReader readerget = friendcommandget.ExecuteReader())
                                                    {
                                                        if (readerget.Read())
                                                        {
                                                            string datasend = readerget["id"].ToString().PadLeft(19, '0') + " " + readerget["username"].ToString() + " " + readerget["name"].ToString() + " " + readerget["state"].ToString();
                                                            string datasendbyte = Encoding.Unicode.GetByteCount(datasend).ToString();
                                                            sslStream.Write(Encoding.Unicode.GetBytes("1609" + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
                                                        }
                                                    }
                                                }
                                            }
                                            if (loaded[id] == 0)
                                            {
                                                sslStream.Write(Encoding.Unicode.GetBytes("2411"));
                                                loaded.Remove(id);
                                            }
                                        
                                            if (reader["avatar"].GetType() != typeof(DBNull))
                                            {
                                                Console.WriteLine("Before get avatar");
                                                string tmp = reader["avatar"].ToString();
                                                string tmpbyte = Encoding.ASCII.GetByteCount(tmp).ToString();
                                                sslStream.Write(Combine(Encoding.Unicode.GetBytes("0601"), Encoding.ASCII.GetBytes(tmpbyte.Length.ToString().PadLeft(2, '0') + tmpbyte + tmp)));
                                            }
                                            using (SqlCommand cmd = new SqlCommand("update top (1) account set state=1 where id=@id", sql))
                                            {
                                                cmd.Parameters.AddWithValue("@id", str_id);
                                                cmd.ExecuteNonQuery();
                                            }
                                        } catch (Exception e)
                                        {
                                            Console.WriteLine(e.ToString());
                                            clear(str_id);
                                        }
                                        finally
                                        {
                                            is_locked[id] = false;
                                        }
                                        c = null;
                                        sslStream = null;
                                    }
                                    else
                                    {
                                        sslStream.Write(Encoding.Unicode.GetBytes("-200"));
                                        sslStream.Close();
                                        c.Close();
                                    }

                                }
                                else
                                {
                                    sslStream.Write(Encoding.Unicode.GetBytes("-200"));
                                    sslStream.Close();
                                    c.Close();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    else if (instruction == "0011") // 0011 = sign up 
                    {
                        try
                        {
                            receive_data_automatically(sslStream, out data);
                            string[] list_str = new string[2];
                            list_str[0] = data;
                            Console.WriteLine(data);
                            receive_data_automatically(sslStream, out data);
                            list_str[1] = data;
                            Console.WriteLine(data);
                            if (!check_existed_username(list_str[0]))
                            {
                                Int64 randomid = 0;
                                while (randomid <= 0 || check_existed_id(randomid))
                                {
                                    randomid = NextInt64(rand);
                                }
                                string id_string = randomid.ToString();
                                while (id_string.Length < 19) id_string = '0' + id_string;
                                using (SqlCommand command = new SqlCommand("insert into account values (@id, @username, @name, @pw, @state, @private, @number_of_contacts, @avatar)", sql))
                                {
                                    command.Parameters.AddWithValue("@id", id_string);
                                    command.Parameters.AddWithValue("@username", list_str[0]);
                                    command.Parameters.AddWithValue("@name", list_str[0]);
                                    command.Parameters.AddWithValue("@pw", Crypter.Blowfish.Crypt(list_str[1]));
                                    command.Parameters.AddWithValue("@state", 0);
                                    command.Parameters.AddWithValue("@private", 0);
                                    command.Parameters.AddWithValue("@number_of_contacts", 0);
                                    command.Parameters.AddWithValue("@avatar", DBNull.Value);
                                    command.ExecuteNonQuery();
                                }
                                sslStream.Write(Encoding.Unicode.GetBytes("1011")); // New account created

                            }
                            else
                            {
                                sslStream.Write(Encoding.Unicode.GetBytes("1111")); // Username exists
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        finally
                        {
                            sslStream.Close();
                            c.Close();
                        }
                    }

                }
            } 
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine("Authentication failed - closing the connection.");
                sslStream.Close();
                c.Close();
                return;
            }
        }

        private static bool check_existed_username(string v)
        {
            string commandtext = "select top 1 id from account where username=@username";
            SqlCommand command = new SqlCommand(commandtext, sql);
            command.Parameters.AddWithValue("@username", v);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return true;
                }
                else return false;
            }
        }

        private static bool check_existed_id(long randomid)
        {
            if (randomid > 0)
            {
                string commandtext = "select top 1 id from account where id=@id";
                SqlCommand command = new SqlCommand(commandtext, sql);
                command.Parameters.AddWithValue("@id", randomid);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    else return false;
                }
            }
            return true;
        }

        static void DisplaySecurityLevel(SslStream stream)
        {
            Console.WriteLine("Cipher: {0} strength {1}", stream.CipherAlgorithm, stream.CipherStrength);
            Console.WriteLine("Hash: {0} strength {1}", stream.HashAlgorithm, stream.HashStrength);
            Console.WriteLine("Key exchange: {0} strength {1}", stream.KeyExchangeAlgorithm, stream.KeyExchangeStrength);
            Console.WriteLine("Protocol: {0}", stream.SslProtocol);
        }
        static void DisplaySecurityServices(SslStream stream)
        {
            Console.WriteLine("Is authenticated: {0} as server? {1}", stream.IsAuthenticated, stream.IsServer);
            Console.WriteLine("IsSigned: {0}", stream.IsSigned);
            Console.WriteLine("Is Encrypted: {0}", stream.IsEncrypted);
        }
        static void DisplayStreamProperties(SslStream stream)
        {
            Console.WriteLine("Can read: {0}, write {1}", stream.CanRead, stream.CanWrite);
            Console.WriteLine("Can timeout: {0}", stream.CanTimeout);
        }
        static void DisplayCertificateInformation(SslStream stream)
        {
            Console.WriteLine("Certificate revocation list checked: {0}", stream.CheckCertRevocationStatus);

            X509Certificate localCertificate = stream.LocalCertificate;
            if (stream.LocalCertificate != null)
            {
                Console.WriteLine("Local cert was issued to {0} and is valid from {1} until {2}.",
                    localCertificate.Subject,
                    localCertificate.GetEffectiveDateString(),
                    localCertificate.GetExpirationDateString());
            }
            else
            {
                Console.WriteLine("Local certificate is null.");
            }
            // Display the properties of the client's certificate.
            X509Certificate remoteCertificate = stream.RemoteCertificate;
            if (stream.RemoteCertificate != null)
            {
                Console.WriteLine("Remote cert was issued to {0} and is valid from {1} until {2}.",
                    remoteCertificate.Subject,
                    remoteCertificate.GetEffectiveDateString(),
                    remoteCertificate.GetExpirationDateString());
            }
            else
            {
                Console.WriteLine("Remote certificate is null.");
            }
        }
    }
}
