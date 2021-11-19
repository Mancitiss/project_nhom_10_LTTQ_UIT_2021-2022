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

namespace AFriendServer
{
    class Program
    {
        static Dictionary<string, Socket> dictionary;
        static SqlConnection sql;
        static Random rand;

        static DateTime datetime = new DateTime();

        static Dictionary<string, int> byte_expected = new Dictionary<string, int>();
        static Dictionary<string, bool> is_processing = new Dictionary<string, bool>();
        static Dictionary<string, bool> is_locked = new Dictionary<string, bool>();

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
                int total_byte_received = 0;
                byte[] data = new Byte[byte_expected[item.Key]];
                int receivedbyte = item.Value.Receive(data);
                if (receivedbyte > 0)
                {
                    total_byte_received += receivedbyte;
                    byte_expected[item.Key] -= receivedbyte;
                }
                while (byte_expected[item.Key] > 0 && receivedbyte > 0)
                {
                    receivedbyte = item.Value.Receive(data, total_byte_received, byte_expected[item.Key], SocketFlags.None);
                    Console.WriteLine("Received bytes:" + receivedbyte.ToString());
                    if (receivedbyte > 0)
                    {
                        total_byte_received += receivedbyte;
                        byte_expected[item.Key] -= receivedbyte;
                    }
                    else break;
                }
                if (byte_expected[item.Key] == 0)// all data received, save to database, send to socket
                {
                    string data_string = Encoding.Unicode.GetString(data, 0, total_byte_received);
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
                    var sqlthread = new Thread(() =>
                    {
                        Console.WriteLine("sql message thread is running");
                        bool finish = false;
                        while (!finish)
                        {
                            try
                            {
                                using (SqlCommand command = new SqlCommand("insert into message values (@id1, @id2, @n, @datetimenow, @sender, @message)", sql))
                                {
                                    command.Parameters.AddWithValue("@id1", id1);
                                    command.Parameters.AddWithValue("@id2", id2);
                                    command.Parameters.AddWithValue("@n", rand.Next(-1000000000, 0));
                                    command.Parameters.AddWithValue("@datetimenow", DateTime.Now);
                                    command.Parameters.AddWithValue("@sender", item.Key == id2);
                                    command.Parameters.AddWithValue("@message", sqlmessage);
                                    if (command.ExecuteNonQuery() >= 1) finish = true;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    });
                    sqlthread.IsBackground = true;
                    sqlthread.Start();
                    //save to database end

                    data_string = data_string.Insert(0, item.Key);

                    //send to socket start
                    if (!Send_to_id(receiver_id, data_string))
                    {
                        item.Value.Send(Encoding.Unicode.GetBytes("0404"));
                    }
                    //send to socket end
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
        private static void Receive_message(object si)
        {
            KeyValuePair<string, Socket> item = (KeyValuePair<string, Socket>)si;
            Socket s = item.Value;
            try
            {
                byte[] bytes = new byte[8];

                //read the identifier from client
                int numByte = s.Receive(bytes, 8, SocketFlags.None);

                if (numByte != 0)
                {
                    string data = Encoding.Unicode.GetString(bytes,
                                               0, numByte);
                    Console.WriteLine("Work: " + data);
                    if (data != null && data != "")
                    {
                        string instruction = data;

                        if (instruction == "1901") // message handlings
                        {
                            bytes = new byte[4];
                            numByte = s.Receive(bytes, 4, SocketFlags.None);
                            data = Encoding.Unicode.GetString(bytes, 0, numByte);
                            int bytezize = Int32.Parse(data)*2;
                            bytes = new byte[bytezize];
                            numByte = s.Receive(bytes, bytezize, SocketFlags.None);
                            data = Encoding.Unicode.GetString(bytes, 0, numByte);
                            Int32.TryParse(data, out int temp);
                            byte_expected[item.Key] = temp;
                        }
                        else if (instruction == "2004") // offline (quit)
                        {
                            shutdown(item);
                        }
                        else if (instruction == "0609") // lookup sb's info using id
                        {
                            bytes = new byte[38];
                            numByte = s.Receive(bytes, 38, SocketFlags.None);
                            data = Encoding.Unicode.GetString(bytes, 0, numByte);
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
                        else if (instruction == "0610") // lookup sb's info using username
                        {
                            bytes = new byte[4];
                            numByte = s.Receive(bytes, 4, SocketFlags.None);
                            data = Encoding.Unicode.GetString(bytes, 0, numByte);
                            int bytezize = Int32.Parse(data) * 2;
                            bytes = new byte[bytezize];
                            numByte = s.Receive(bytes, bytezize, SocketFlags.None);
                            data = Encoding.Unicode.GetString(bytes, 0, numByte);
                            bytezize = Int32.Parse(data) ;
                            bytes = new byte[bytezize];
                            numByte = s.Receive(bytes, bytezize, SocketFlags.None);
                            data = Encoding.Unicode.GetString(bytes, 0, numByte);
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
                } else
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

        private static void Send_to_socket(Socket s, string str)
        {
            // do something

        }

        private static bool Send_to_id(string id, string data)
        {
            // do something
            bool success = false;
            if (dictionary.ContainsKey(id))
            {
                try
                {
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
                        string commandtext = "select top 1 id, username, pw from account where username=@username";
                        SqlCommand command = new SqlCommand(commandtext, sql);
                        command.Parameters.AddWithValue("@username", list_str[0]);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
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

                                    string username = reader["username"].ToString();
                                    string usernamebyte = Encoding.Unicode.GetByteCount(username).ToString();

                                    s.Send(Encoding.Unicode.GetBytes("0200"
                                        + id + usernamebyte.Length.ToString().PadLeft(2, '0') + usernamebyte + username));

                                    using (SqlCommand cmd = new SqlCommand("update top (1) account set state=1 where id=@id", sql))
                                    {
                                        cmd.Parameters.AddWithValue("@id", str_id);
                                        cmd.ExecuteNonQuery();
                                    }

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
                            using (SqlCommand command = new SqlCommand("insert into account values (@id, @username, @name, @pw, @state, @private, @number_of_contacts)", sql))
                            {
                                command.Parameters.AddWithValue("@id", id_string);
                                command.Parameters.AddWithValue("@username", list_str[0]);
                                command.Parameters.AddWithValue("@name", list_str[0]);
                                command.Parameters.AddWithValue("@pw", Crypter.Blowfish.Crypt(list_str[1]));
                                command.Parameters.AddWithValue("@state", 0);
                                command.Parameters.AddWithValue("@private", 0);
                                command.Parameters.AddWithValue("@number_of_contacts", 0);
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
