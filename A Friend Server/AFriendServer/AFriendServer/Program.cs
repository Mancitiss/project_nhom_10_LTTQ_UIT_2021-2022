using System;
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

namespace AFriendServer
{
    class Program
    {
        static Dictionary<string, Socket> dictionary;
        static SqlConnection sql;
        static Random rand;

        static Dictionary<string, int> byte_expected = new Dictionary<string, int>();
        static Dictionary<string, bool> is_processing = new Dictionary<string, bool>();
        static Dictionary<string, bool> is_locked = new Dictionary<string, bool>();
        static Dictionary<string, int> loaded = new Dictionary<string, int>();

        public static Int64 NextInt64(Random rnd)
        {
            var buffer = new byte[sizeof(Int64)];
            rnd.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
                dictionary = new Dictionary<string, Socket>();

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

        private static void shutdown(KeyValuePair<string, Socket> item) 
        {
            Console.WriteLine("{0} has quit", item.Key);
            item.Value.Shutdown(SocketShutdown.Both);
            item.Value.Close();
            string str_id = item.Key;
            dictionary.Remove(item.Key);
            byte_expected.Remove(item.Key);
            is_processing.Remove(item.Key);
            is_locked.Remove(item.Key);
            loaded.Remove(item.Key);
            while (str_id[0] == '0' && str_id.Length > 1) str_id.Remove(0, 1);
            using (SqlCommand cmd = new SqlCommand("update top (1) account set state=0 where id=@id", sql))
            {
                cmd.Parameters.AddWithValue("@id", Int64.Parse(str_id));
                cmd.ExecuteNonQuery();
            }
        }

        private static void process_message(object obj)
        {

            KeyValuePair<string, Socket> item = (KeyValuePair<string, Socket>)obj;
            try
            {
                string data_string;
                if (Socket_receive(item.Value, byte_expected[item.Key], out data_string))// all data received, save to database, send to socket
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
                                            item.Value.Send(Encoding.Unicode.GetBytes("0404"+receiver_id));
                                        } else
                                        {
                                            item.Value.Send(Encoding.Unicode.GetBytes("2211"+receiver_id));
                                        }
                                        //send to socket end
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
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
            }
            finally
            {
                is_processing[item.Key] = false;
                is_locked[item.Key] = false;
            }
        }

        private static void Client_Loop(object obj)
        {
            while (true)
            {
                try
                {
                    foreach (var item in dictionary)
                    {
                        if (is_locked[item.Key]) continue;
                        try
                        {
                            //Console.WriteLine(item.Key + " is online");
                            if (item.Value.Connected)
                            {
                                if (item.Value.Poll(1, SelectMode.SelectRead))
                                {
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
                                        if (is_locked[item.Key]) continue;
                                        is_locked[item.Key] = true;
                                        if (byte_expected[item.Key] == 0)
                                        {
                                            try
                                            {
                                                Console.WriteLine("new work created");
                                                Thread work = new Thread(new ParameterizedThreadStart(Receive_message));
                                                work.IsBackground = true;
                                                work.Start(item);
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.ToString());
                                            }
                                            finally
                                            {
                                                //is_locked[item.Key] = false;
                                            }
                                        }
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
                                        }
                                        
                                    }
                                }
                            }
                            else
                            {
                                shutdown(item);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
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

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

            // Creation TCP/IP Socket using
            // Socket Class Constructor
            Socket listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Server at: {0}", ipAddr);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(1000);
                while (true)
                {
                    Thread.Sleep(10);
                    Socket client = listener.Accept();

                    Console.WriteLine("Accepted Client");
                    try
                    {
                        Receive_from_socket_not_logged_in(client);
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

        private static bool receive_ASCII_data_automatically(Socket s, out string data)
        {
            if (Socket_receive_ASCII(s, 2, out data))
            {
                int bytesize;
                if (Int32.TryParse(data, out bytesize))
                {
                    if (Socket_receive_ASCII(s, bytesize, out data))
                    {
                        if (Int32.TryParse(data, out bytesize))
                        {
                            if (Socket_receive_ASCII(s, bytesize, out data))
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

        private static bool receive_data_automatically(Socket s, out string data)
        {
            if (Socket_receive(s, 4, out data))
            {
                int bytesize;
                if (Int32.TryParse(data, out bytesize))
                {
                    bytesize = bytesize * 2;
                    if (Socket_receive(s, bytesize, out data))
                    {
                        if (Int32.TryParse(data, out bytesize))
                        {
                            if (Socket_receive(s, bytesize, out data))
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

        private static bool Socket_receive_ASCII(Socket s, int byte_expected, out string data_string)
        {
            int total_byte_received = 0;
            byte[] data = new byte[byte_expected];
            int received_byte;
            do
            {
                received_byte = s.Receive(data, total_byte_received, byte_expected, SocketFlags.None);
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

        private static bool Socket_receive(Socket s, int byte_expected, out string data_string)
        {
            int total_byte_received = 0;
            byte[] data = new byte[byte_expected];
            int received_byte;
            do
            {
                received_byte = s.Receive(data, total_byte_received, byte_expected, SocketFlags.None);
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

        private static void Receive_message(object si)
        {
            KeyValuePair<string, Socket> item = (KeyValuePair<string, Socket>)si;
            Socket s = item.Value;
            try
            {
                string data;
                if (Socket_receive(s, 8, out data))
                {
                    Console.WriteLine("Work: " + data);
                    if (data != null && data != "")
                    {
                        string instruction = data;
                        if (instruction == "6475") // load messages
                        {
                            string receiver_id;
                            if (Socket_receive(s, 38, out receiver_id))
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
                                    Console.WriteLine(data);
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
                                            Console.WriteLine(num);
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
                                            s.Send(Encoding.Unicode.GetBytes("6475" + receiver_id + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
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
                                                    s.Send(Encoding.Unicode.GetBytes("2411"));
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
                                            s.Send(Encoding.Unicode.GetBytes("6475" + receiver_id + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
                                            //Console.WriteLine("Old messages sent");
                                        }
                                    }
                                }
                            }
                        } // load message
                        else if (instruction == "1901") // message handlings
                        {
                            Socket_receive(s, 4, out data);
                            int bytesize = Int32.Parse(data) * 2;
                            Socket_receive(s, bytesize, out data);
                            Int32.TryParse(data, out int temp);
                            byte_expected[item.Key] = temp;
                        } // handle message
                        else if (instruction == "1234")
                        {
                            string receiver_id;
                            if (Socket_receive(s, 38, out receiver_id))
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
                                if (Socket_receive(s, 2, out boolstr))
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
                        } // me seen
                        else if (instruction == "0708") // load seen
                        {
                            string receiver_id;
                            if (Socket_receive(s, 38, out receiver_id))
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
                                    s.Send(Encoding.Unicode.GetBytes("0708" + receiver_id + "1"));
                                }
                                else
                                {
                                    s.Send(Encoding.Unicode.GetBytes("0708" + receiver_id + "0"));
                                }
                            }
                        } // load seen
                        else if (instruction == "2002") // delete message
                        {
                            string receiver_id;
                            if (Socket_receive(s, 38, out receiver_id))
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
                                        dictionary[receiver_id].Send(Encoding.Unicode.GetBytes("2002"+item.Key+data_with_byte(messagenumber.ToString())));
                                    }
                                }
                            }
                        }
                        else if (instruction == "2004") // offline (quit)
                        {
                            shutdown(item);
                        }
                        else if (instruction == "0609") // lookup sb's info using id
                        {
                            if (Socket_receive(s, 38, out data))
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
                                        s.Send(Encoding.Unicode.GetBytes("1609" + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
                                    }
                                    else
                                    {
                                        s.Send(Encoding.Unicode.GetBytes("2609")); // info not found
                                    }
                                }
                            }
                        }
                        else if (instruction == "0610") // lookup sb's info using username
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
                                        s.Send(Encoding.Unicode.GetBytes("1609" + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
                                        Console.WriteLine("Info sent");
                                    }
                                    else
                                    {
                                        s.Send(Encoding.Unicode.GetBytes("2609")); // info not found
                                    }
                                }
                            }
                        }
                        else if (instruction == "0601")
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
                        }
                        else if (instruction == "4269")
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
                                                            s.Send(Encoding.Unicode.GetBytes("4269"));
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    s.Send(Encoding.Unicode.GetBytes("9624"));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (instruction == "1012")
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
                                        s.Send(Encoding.Unicode.GetBytes("1012"));
                                    }
                                }
                            }
                        }
                        else
                        {
                            shutdown(item);
                            Console.WriteLine("Received strange signal, socket closed");
                        }
                    }
                    else
                    {
                        shutdown(item);
                        Console.WriteLine("Received strange signal, socket closed (2)");
                    }
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
            }
            finally
            {
                is_locked[item.Key] = false;
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
                    dictionary[id].Send(Encoding.Unicode.GetBytes("1901" + data_string.Length.ToString().PadLeft(2, '0') + data_string + data));
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

        private static bool Receive_from_socket_not_logged_in(Socket s)
        {
            // do something
            bool quit = false;
            byte[] bytes = new byte[s.ReceiveBufferSize];

            //read the identifier from client
            int numByte = s.Receive(bytes);

            string data = Encoding.Unicode.GetString(bytes,
                                       0, numByte);
            if (data != null && data != "")
            {
                string instruction = data.Substring(0, 4);

                if (instruction == "0010") // 0010 = log in
                {
                    string[] list_str = data.Remove(0, 4).Split(' ');
                    Console.WriteLine(list_str[0]);
                    Console.WriteLine(list_str[1]);
                    try
                    {
                        Console.WriteLine("Before avatar");
                        string commandtext = "select top 1 id, name, pw, avatar from account where username=@username";
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

                                    s.Send(Encoding.Unicode.GetBytes("0200"
                                        + id + namebyte.Length.ToString().PadLeft(2, '0') + namebyte + name));
                                    Console.WriteLine("Before state");
                                    //state was here
                                    Console.WriteLine("Before dictionaries");
                                    try
                                    {
                                        if (dictionary.ContainsKey(id))
                                        {
                                            dictionary[id].Send(Encoding.Unicode.GetBytes("2004"));
                                            dictionary[id].Shutdown(SocketShutdown.Both);
                                            dictionary[id].Close();
                                            dictionary.Remove(id);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.ToString());
                                    }

                                    try
                                    {
                                        //add the entry in the dictionary
                                        try
                                        {
                                            loaded.Add(id, 0);
                                        }
                                        catch (Exception e)
                                        {
                                            loaded[id] = 0;
                                        }
                                        try
                                        {
                                            byte_expected.Add(id, 0);
                                        } catch (Exception e)
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
                                        }
                                        try
                                        {
                                            is_locked.Add(id, false);
                                        }
                                        catch (Exception e) 
                                        {
                                            is_locked[id] = false;
                                        }
                                        dictionary.Add(id, s);
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
                                                        s.Send(Encoding.Unicode.GetBytes("1609" + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
                                                    }
                                                }
                                            }
                                        }
                                        if (loaded[id] == 0)
                                        {
                                            s.Send(Encoding.Unicode.GetBytes("2411"));
                                            loaded.Remove(id);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.ToString());
                                    }
                                    /*
                                    while (thread.ThreadState == ThreadState.Running)
                                    {
                                        Console.WriteLine("Im still running");
                                    }*/
                                    if (reader["avatar"].GetType() != typeof(DBNull))
                                    {
                                        Console.WriteLine("Before get avatar");
                                        string tmp = reader["avatar"].ToString();
                                        string tmpbyte = Encoding.ASCII.GetByteCount(tmp).ToString();
                                        s.Send(Combine(Encoding.Unicode.GetBytes("0601"), Encoding.ASCII.GetBytes(tmpbyte.Length.ToString().PadLeft(2, '0') + tmpbyte + tmp)));
                                    }
                                    using (SqlCommand cmd = new SqlCommand("update top (1) account set state=1 where id=@id", sql))
                                    {
                                        cmd.Parameters.AddWithValue("@id", str_id);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    s.Send(Encoding.Unicode.GetBytes("-200"));
                                    s.Shutdown(SocketShutdown.Both);
                                    s.Close();
                                }

                            }
                            else
                            {
                                s.Send(Encoding.Unicode.GetBytes("-200"));
                                s.Shutdown(SocketShutdown.Both);
                                s.Close();
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
                        string[] list_str = data.Remove(0, 4).Split(' ');
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
                            s.Send(Encoding.Unicode.GetBytes("1011")); // New account created

                        }
                        else
                        {
                            s.Send(Encoding.Unicode.GetBytes("1111")); // Username exists
                        }
                        s.Shutdown(SocketShutdown.Both);
                        s.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }

            }
            return quit;
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
    }
}
