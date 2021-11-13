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

namespace AFriendServer
{
    /*
    class Account
    {
        public string id;
        public string username;
        public int state;
        private string password;

        public Account(string id, string username, string password)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.state = 0;
        }

        public bool check_pass(string pass)
        {
            if (pass == password)
            {
                return true;
            }
            return false;
        }
    }
    */

    class Program
    {
        static Dictionary<string, Socket> dictionary;
        static SqlConnection sql;
        static Random rand;

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
                            //Console.WriteLine(item.Key + " is online");
                            if (item.Value.Connected)
                            {
                                if (item.Value.Poll(1, SelectMode.SelectRead))
                                {
                                    if (!item.Value.Connected)
                                    {
                                        // Something bad has happened, shut down
                                        Console.WriteLine("{0} has quit", item.Key);
                                        try
                                        {
                                            item.Value.Shutdown(SocketShutdown.Both);
                                            item.Value.Close();
                                            string str_id = item.Key;
                                            dictionary.Remove(item.Key);
                                            while (str_id[0] == '0' && str_id.Length > 1) str_id.Remove(0, 1);
                                            using (SqlCommand cmd = new SqlCommand("update top (1) account set state=0 where id=@id", sql))
                                            {
                                                cmd.Parameters.AddWithValue("@id", Int64.Parse(str_id));
                                                cmd.ExecuteNonQuery();
                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.ToString());
                                        }
                                    }
                                    else
                                    {
                                        // There is data waiting to be read"
                                        Console.WriteLine("new work created");
                                        Thread work = new Thread(new ParameterizedThreadStart(Receive_message));
                                        work.IsBackground = true;
                                        work.Start(item);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine(item.Key + " has quit");
                                item.Value.Shutdown(SocketShutdown.Both);
                                item.Value.Close();
                                string str_id = item.Key;
                                dictionary.Remove(item.Key);
                                while (str_id[0] == '0' && str_id.Length > 1) str_id.Remove(0, 1);
                                using (SqlCommand cmd = new SqlCommand("update top (1) account set state=0 where id=@id", sql))
                                {
                                    cmd.Parameters.AddWithValue("@id", Int64.Parse(str_id));
                                    cmd.ExecuteNonQuery();
                                }
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
        /*
        private static void ClientHandler(object obj)
        {
            string id = (string)obj;
            Socket client = dictionary[id];
            
            try
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(1000);
                        if (Receive_message(client))
                        {
                            Console.WriteLine("{0} has quit", id);
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} has quit", id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            try
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                dictionary.Remove(id);
                string str_id = id;
                while (str_id[0] == '0' && str_id.Length > 1) str_id.Remove(0, 1);
                using (SqlCommand cmd = new SqlCommand("update account set state=0 where id='" + str_id + "'", sql))
                {
                    cmd.ExecuteNonQuery();
                }

            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            if (id != null)
            {
                if (dictionary.ContainsKey(id))
                {
                    dictionary.Remove(id);
                }
            }
            try 
            {
                return;
            }
            catch (Exception e) 
            { 
                Console.WriteLine(e.ToString()); 
            }
        }*/

        private static void Receive_message(object si)
        {
            try
            {
                KeyValuePair<string, Socket> item = (KeyValuePair<string, Socket>)si;
                Socket s = item.Value;

                byte[] bytes = new byte[s.ReceiveBufferSize];

                //read the identifier from client
                int numByte = s.Receive(bytes);

                string data = Encoding.Unicode.GetString(bytes,
                                           0, numByte);
                if (data != null && data != "")
                {
                    string instruction = data.Substring(0, 4);

                    if (instruction == "1901")
                    {
                        string receiver_id = data.Substring(4, 19);
                        data = data.Remove(4, 19);
                        if (!Send_to_id(receiver_id, data))
                        {
                            s.Send(Encoding.Unicode.GetBytes("0404This person is not online"));
                        }
                    }
                    else if (instruction == "2004")
                    {
                        s.Shutdown(SocketShutdown.Both);
                        s.Close();
                        string str_id = item.Key;
                        dictionary.Remove(item.Key);
                        while (str_id[0] == '0' && str_id.Length > 1) str_id.Remove(0, 1);
                        using (SqlCommand cmd = new SqlCommand("update top (1) account set state=0 where id=@id", sql))
                        {
                            cmd.Parameters.AddWithValue("@id", Int64.Parse(str_id));
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (instruction == "0609")
                    {
                        data = data.Remove(0, 4);
                        string commandtext = "select top 1 id, username, name, state from account where id=@id";
                        SqlCommand command = new SqlCommand(commandtext, sql);
                        command.Parameters.AddWithValue("@id", Int64.Parse(data));
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                s.Send(Encoding.Unicode.GetBytes("1609" + reader["id"].ToString().PadLeft(19, '0') + " " + reader["username"].ToString() + " " + reader["name"].ToString() + " " + reader["state"].ToString()));
                            }
                            else
                            {
                                s.Send(Encoding.Unicode.GetBytes("2609"));
                            }
                        }
                    }
                    else if (instruction == "0610")
                    {
                        data = data.Remove(0, 4);
                        string commandtext = "select top 1 id, username, name, state from account where username=@username";
                        SqlCommand command = new SqlCommand(commandtext, sql);
                        command.Parameters.AddWithValue("@username", data);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                s.Send(Encoding.Unicode.GetBytes("1609" + reader["id"].ToString().PadLeft(19, '0') + " " + reader["username"].ToString() + " " + reader["name"].ToString() + " " + reader["state"].ToString()));
                                Console.WriteLine("Info sent");
                            }
                            else
                            {
                                s.Send(Encoding.Unicode.GetBytes("2609"));
                            }
                        }
                    }
                }
                Console.WriteLine("Work finished");
            }
            catch (Exception e)
            {
                Console.WriteLine("Work quitted");
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
                    dictionary[id].Send(Encoding.Unicode.GetBytes(data));
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
                                if (list_str[1] == reader["pw"].ToString())
                                {
                                    string id = reader["id"].ToString();
                                    string str_id = id;
                                    while (id.Length < 19) id = '0' + id;

                                    s.Send(Encoding.Unicode.GetBytes("0200"
                                        + id + reader["username"].ToString()));

                                    using (SqlCommand cmd = new SqlCommand("update top (1) account set state=1 where id=@id", sql))
                                    {
                                        cmd.Parameters.AddWithValue("@id", str_id);
                                        cmd.ExecuteNonQuery();
                                    }

                                    try
                                    {
                                        s.Send(Encoding.Unicode.GetBytes("19010000000000000000000You are connected"));


                                        if (dictionary.ContainsKey(id))
                                        {
                                            dictionary[id].Send(Encoding.Unicode.GetBytes("19010000000000000000000You are logged in from another device. You will be logged out."));
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
                            using (SqlCommand command = new SqlCommand("insert into account values (@id, @username, @name, @pw, @state, @private)", sql))
                            {
                                command.Parameters.AddWithValue("@id", id_string);
                                command.Parameters.AddWithValue("@username", list_str[0]);
                                command.Parameters.AddWithValue("@name", list_str[0]);
                                command.Parameters.AddWithValue("@pw", list_str[1]);
                                command.Parameters.AddWithValue("@state", 0);
                                command.Parameters.AddWithValue("@private", 0);
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
