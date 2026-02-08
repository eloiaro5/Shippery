using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Shippery.Models.Basic;
using Shippery.Models.Database;

namespace Shippery.Models.Resources
{
    public class DatabaseDelegate
    {
        #region Static Requests
        public static bool Init(string connection)
        {
            return InitCurrencies(connection) && InitCities(connection);
        }

        public static bool InitCurrencies(string connection)
        {
            MySqlConnection conn = new MySqlConnection(connection);
            int n = 0;
            conn.Open();
            try
            {
                string sql = "SELECT * FROM currency ORDER BY code";
                MySqlCommand cmd = new MySqlCommand(sql, new DatabaseDelegate(ConfigurationHelper.GetMySQLConnectionString()).GetSqlConnection());

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        n++;
                        Iterator i = new Iterator();
                        if (!dr.IsDBNull(1))
                        {
                            Currency.currencies.Add(new Currency(dr.GetString(i.Iterate()), dr.GetString(i.Iterate()), dr.GetChar(i.Iterate())));
                        }
                    }
                }
                return n > 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool InitCities(string connection)
        {
            MySqlConnection conn = new MySqlConnection(connection);
            int n = 0;
            conn.Open();
            try
            {
                string sql = "SELECT * FROM city ORDER BY id";
                MySqlCommand cmd = new MySqlCommand(sql, new DatabaseDelegate(ConfigurationHelper.GetMySQLConnectionString()).GetSqlConnection());

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        n++;
                        Iterator i = new Iterator();
                        Enum.TryParse(dr.GetString(9), out CityType type);
                        City.cities.Add(new City(
                            dr.GetInt32(i.Iterate()),
                            dr.GetString(i.Iterate()),
                            dr.GetString(i.Iterate()),
                            dr.GetDouble(i.Iterate()),
                            dr.GetDouble(i.Iterate()),
                            dr.GetString(i.Iterate()),
                            dr.GetString(i.Iterate()),
                            dr.GetString(i.Iterate()),
                            dr.GetString(i.Iterate()),
                            type,
                            dr.GetInt32(i.Iterate(2))
                            ));
                    }
                }
                return n > 0;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        protected string connection;

        public DatabaseDelegate(string connection) { this.connection = connection; }

        public MySqlConnection GetSqlConnection(bool opened = true)
        {
            MySqlConnection conn = new MySqlConnection(connection);
            if (opened) conn.Open();
            return conn;
        }

        #region Quick Requests
        public string GetSessionUsername(string sessionId)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                string sql = "SELECT user_username FROM session WHERE session = @session";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@session", sessionId);
                return cmd.ExecuteScalar() is null ? "" : cmd.ExecuteScalar().ToString();
            }
            finally
            {
                conn.Close();
            }
        }
        public bool GetValid(string identifier, bool isSession = true)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                if (isSession)
                {
                    string sql = "SELECT valid FROM user WHERE username = (SELECT user_username FROM session WHERE session = @session)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@session", identifier);
                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
                else
                {
                    string sql = "SELECT valid FROM user WHERE username = @username";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", identifier);
                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Precise Requests
        public bool ConfirmAccount(int code)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                string sql = "UPDATE user SET valid = 1 WHERE username = (SELECT user_username FROM mail_confirmation WHERE type = 1 AND code = @code)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@code", code);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    sql = "DELETE FROM mail_confirmation WHERE type = 1 AND code = @code";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@code", code);
                    if (cmd.ExecuteNonQuery() == 1) return true;
                    else throw new Exception("An error ocurred while deleting the confirmation mail on confirming an account.");
                }
                else return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool DeleteSession(string id, bool all = false)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                MySqlCommand cmd;
                string sql;
                if (all)
                {
                    sql = "UPDATE session SET expired = 1 WHERE user_username = (SELECT user_username FROM session WHERE session = @session)";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@session", id);
                    if (cmd.ExecuteNonQuery() <= 0) throw new Exception("{DatabaseDelegate} An error ocurred while expiring sessions");

                    sql = "DELETE FROM session WHERE session = @session";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@session", id);
                    return cmd.ExecuteNonQuery() == 1;
                }
                else
                {
                    sql = "DELETE FROM session WHERE session = @session";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@session", id);
                    return cmd.ExecuteNonQuery() == 1;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        public bool CheckPassword(string identifier, string password, bool isSession = true)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                if (isSession)
                {
                    string sql = "SELECT valid FROM user WHERE username = (SELECT user_username FROM session WHERE session = @identifier) AND password = @password";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@identifier", identifier);
                    cmd.Parameters.AddWithValue("@password", password);
                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
                else
                {
                    string sql = "SELECT valid FROM user WHERE username = @identifier AND password = @password";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@identifier", identifier);
                    cmd.Parameters.AddWithValue("@password", password);
                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            finally
            {
                conn.Close();
            }
        }
        public bool CheckPassword(User u) => CheckPassword(u.Username, u.Password, false);
        public bool UpdateUser(User user, string sessionID, string password)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                string sql = "UPDATE user SET password = @newPassword, image = @image, description = @description, vehicleDescription = @vehicleDescription, plateNumber = @plateNumber, bond = @bond, parental = @parental, cardPayer = @cardPayer WHERE username = (SELECT user_username FROM session WHERE session = @session) AND password = @password";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                if (string.IsNullOrEmpty(user.Password)) cmd.CommandText = cmd.CommandText.Replace("password = @newPassword, ", "");
                else cmd.Parameters.AddWithValue("@newPassword", user.Password);
                cmd.Parameters.AddWithValue("@image", user.Image);
                cmd.Parameters.AddWithValue("@description", user.Description);
                cmd.Parameters.AddWithValue("@vehicleDescription", user.VehicleDescription);
                cmd.Parameters.AddWithValue("@plateNumber", user.Plate);
                cmd.Parameters.AddWithValue("@bond", user.PaysBond);
                cmd.Parameters.AddWithValue("@parental", user.Parental);
                cmd.Parameters.AddWithValue("@cardPayer", user.CardPayer);
                cmd.Parameters.AddWithValue("@session", sessionID);
                cmd.Parameters.AddWithValue("@password", password);
                return cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Model Getters
        public User GetUser(string username)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                Iterator i = new Iterator();
                User u = new User();
                string sql = "SELECT * FROM user WHERE username = @username";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", username);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        u.Username = dr.GetString(i.Iterate());
                        u.Mail = dr.GetString(i.Iterate(2));
                        u.Image = dr.GetString(i.Iterate());
                        u.Description = dr.IsDBNull(i.Iterate()) ? "" : dr.GetString(i.Position);
                        u.VehicleDescription = dr.IsDBNull(i.Iterate()) ? "" : dr.GetString(i.Position);
                        u.Plate = dr.GetString(i.Iterate());
                        u.Currency = Currency.GetCurrency(dr.GetString(i.Iterate()));
                        u.PaysBond = dr.GetBoolean(i.Iterate());
                        u.Parental = dr.GetBoolean(i.Iterate());
                        u.CardPayer = dr.GetBoolean(i.Iterate());
                        u.SendNewSession = dr.GetBoolean(i.Iterate());
                        u.SendPromotion = dr.GetBoolean(i.Iterate());
                    }
                }
                return u;
            }
            finally
            {
                conn.Close();
            }
        }

        public Trip GetTrip(int id)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                Iterator i = new Iterator();
                Trip t = new Trip();
                string sql = "SELECT * FROM trip WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        t.Id = dr.GetInt32(i.Iterate());
                        t.User = GetUser(dr.GetString(i.Iterate()));
                        t.SourceCity = City.GetCityById(dr.GetInt32(i.Iterate()));
                        t.SourceStreet = dr.GetString(i.Iterate());
                        t.DestinationCity = City.GetCityById(dr.GetInt32(i.Iterate()));
                        t.DestinationStreet = dr.GetString(i.Iterate());
                        t.Price = dr.GetFloat(i.Iterate());
                        t.Currency = Currency.GetCurrency(dr.GetString(i.Iterate()));
                        t.Date = dr.GetDateTime(i.Iterate());
                        t.TravelTime = dr.GetInt32(i.Iterate());
                        t.Description = dr.IsDBNull(i.Iterate()) ? "" : dr.GetString(i.Position);
                        t.NeedsCard = dr.GetBoolean(i.Iterate());
                        t.PersonsAllowed = dr.GetBoolean(i.Iterate());
                        t.Full = dr.GetBoolean(i.Iterate());
                    }
                }

                return t;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Trip> GetTrip(string username)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                Iterator i = new Iterator();
                List<Trip> ts = new List<Trip>();
                string sql = "SELECT * FROM trip WHERE user_username = @user_username";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user_username", username);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        i.Reset();
                        Trip t = new Trip();
                        t.Id = dr.GetInt32(i.Iterate());
                        t.User = GetUser(dr.GetString(i.Iterate()));
                        t.SourceCity = City.GetCityById(dr.GetInt32(i.Iterate()));
                        t.SourceStreet = dr.GetString(i.Iterate());
                        t.DestinationCity = City.GetCityById(dr.GetInt32(i.Iterate()));
                        t.DestinationStreet = dr.GetString(i.Iterate());
                        t.Price = dr.GetFloat(i.Iterate());
                        t.Currency = Currency.GetCurrency(dr.GetString(i.Iterate()));
                        t.Date = dr.GetDateTime(i.Iterate());
                        t.TravelTime = dr.GetInt32(i.Iterate());
                        t.Description = dr.IsDBNull(i.Iterate()) ? "" : dr.GetString(i.Position);
                        t.NeedsCard = dr.GetBoolean(i.Iterate());
                        t.PersonsAllowed = dr.GetBoolean(i.Iterate());
                        t.Full = dr.GetBoolean(i.Iterate());
                        ts.Add(t);
                    }
                }

                return ts;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Trip> GetTrip(string source, string destination, Price min, Price max, DateTime after, DateTime before, string order, string username = "")
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                Iterator i = new Iterator();
                string sql;
                if (!string.IsNullOrEmpty(username)) sql = "SELECT t.*, u.bond FROM trip t JOIN user u WHERE t.user_username = u.username AND t.user_username <> @user_username AND t.full = 0 AND t.id NOT IN (SELECT trip_id FROM reserve WHERE user_username = @user_username) AND sourceCity = @sourceCity AND destinationCity = @destinationCity AND price BETWEEN @min AND @max AND t.currencyCode = @currency AND date BETWEEN @after AND @before ORDER BY " + order;// + " LIMIT " + (navigator.Iteration * maximumDivisorElements["searcher"]) + "," + maximumDivisorElements["searcher"];
                else sql = "SELECT t.*, u.bond FROM trip t JOIN user u WHERE t.user_username = u.username AND t.full = 0 AND sourceCity = @sourceCity AND destinationCity = @destinationCity AND price BETWEEN @min AND @max AND t.currencyCode = @currency AND date BETWEEN @after AND @before ORDER BY " + order;// + " LIMIT " + (navigator.Iteration * maximumDivisorElements["searcher"]) + "," + maximumDivisorElements["searcher"];

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                if (string.IsNullOrEmpty(source)) cmd.CommandText = cmd.CommandText.Replace("AND sourceCity = @sourceCity ", "");
                else cmd.Parameters.AddWithValue("@sourceCity", City.GetCityByJSON(source).Id);
                if (string.IsNullOrEmpty(destination)) cmd.CommandText = cmd.CommandText.Replace("AND destinationCity = @destinationCity ", "");
                else cmd.Parameters.AddWithValue("@destinationCity", City.GetCityByJSON(destination).Id);
                cmd.Parameters.AddWithValue("@min", min.Cost);
                cmd.Parameters.AddWithValue("@max", max.Cost);
                cmd.Parameters.AddWithValue("@currency", max.Currency.Code);
                cmd.Parameters.AddWithValue("@after", after.ToString("yyyy/MM/dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@before", before.ToString("yyyy/MM/dd HH:mm:ss"));
                if (!string.IsNullOrEmpty(username)) cmd.Parameters.AddWithValue("@user_username", username);

                List<Trip> ts = new List<Trip>();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        i.Reset();
                        Trip t = new Trip();

                        t.Id = dr.GetInt32(i.Iterate());
                        t.User = GetUser(dr.GetString(i.Iterate()));
                        t.SourceCity = City.GetCityById(dr.GetInt32(i.Iterate()));
                        t.SourceStreet = dr.GetString(i.Iterate());
                        t.DestinationCity = City.GetCityById(dr.GetInt32(i.Iterate()));
                        t.DestinationStreet = dr.GetString(i.Iterate());
                        t.Price = dr.GetFloat(i.Iterate());
                        t.Currency = Currency.GetCurrency(dr.GetString(i.Iterate()));
                        t.Date = dr.GetDateTime(i.Iterate());
                        t.TravelTime = dr.GetInt32(i.Iterate());
                        t.Description = dr.IsDBNull(i.Iterate()) ? "" : dr.GetString(i.Position);
                        t.NeedsCard = dr.GetBoolean(i.Iterate());
                        t.PersonsAllowed = dr.GetBoolean(i.Iterate());
                        t.Full = dr.GetBoolean(i.Iterate());
                        ts.Add(t);
                    }
                }

                return ts;
            }
            finally
            {
                conn.Close();
            }
        }

        public Reserve GetReserve(int trip_id, string username)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                Iterator i = new Iterator();
                Reserve r = new Reserve();
                string sql = "SELECT * FROM reserve WHERE trip_id = @trip_id AND user_username = @user_username";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@trip_id", trip_id);
                cmd.Parameters.AddWithValue("@user_username", username);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        r.Trip = GetTrip(dr.GetInt32(i.Iterate()));
                        r.Bond = dr.GetInt32(i.Iterate(2));
                        r.Description = dr.IsDBNull(3) ? "" : dr.GetString(3);
                        //if (r.Trip.User.Parental) r.Description = dr.IsDBNull(3) ? "" : Controllers.MainController.ChangeBannedWords(dr.GetString(3));
                        //else r.Description = dr.IsDBNull(3) ? "" : dr.GetString(3);
                        r.ByCard = dr.GetBoolean(4);
                    }
                }
                return r;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Reserve> GetReserve(int trip_id)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                Iterator i = new Iterator();
                List<Reserve> rs = new List<Reserve>();
                string sql = "SELECT * FROM reserve WHERE trip_id = @trip_id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@trip_id", trip_id);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        i.Reset();
                        Reserve r = new Reserve();
                        r.Trip = GetTrip(dr.GetInt32(i.Iterate()));
                        r.Bond = dr.GetInt32(i.Iterate(2));
                        r.Description = dr.IsDBNull(3) ? "" : dr.GetString(3);
                        //if (r.Trip.User.Parental) r.Description = dr.IsDBNull(3) ? "" : Controllers.MainController.ChangeBannedWords(dr.GetString(3));
                        //else r.Description = dr.IsDBNull(3) ? "" : dr.GetString(3);
                        r.ByCard = dr.GetBoolean(4);
                        rs.Add(r);
                    }
                }
                return rs;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Reserve> GetReserve(string username)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                Iterator i = new Iterator();
                List<Reserve> rs = new List<Reserve>();
                string sql = "SELECT * FROM reserve WHERE user_username = @user_username";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user_username", username);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        i.Reset();
                        Reserve r = new Reserve();
                        r.Trip = GetTrip(dr.GetInt32(i.Iterate()));
                        r.Bond = dr.GetInt32(i.Iterate(2));
                        r.Description = dr.IsDBNull(3) ? "" : dr.GetString(3);
                        //if (r.Trip.User.Parental) r.Description = dr.IsDBNull(3) ? "" : Controllers.MainController.ChangeBannedWords(dr.GetString(3));
                        //else r.Description = dr.IsDBNull(3) ? "" : dr.GetString(3);
                        r.ByCard = dr.GetBoolean(4);
                        rs.Add(r);
                    }
                }
                return rs;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool? GetTripReserved(int trip_id, string username)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                object r;
                string sql = "SELECT id FROM reserve WHERE trip_id = @trip_id AND user_username = @user_username";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@trip_id", trip_id);
                cmd.Parameters.AddWithValue("@user_username", username);
                r = cmd.ExecuteScalar();
                if (r is null) return null;
                else return Convert.ToBoolean(r);
            }
            finally
            {
                conn.Close();
            }
        }

        public UserValoration GetValoration(int id)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                Iterator i = new Iterator();
                UserValoration v = new UserValoration();
                string sql = "SELECT * FROM uservaloration WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        v.Id = dr.GetInt32(i.Iterate());
                        v.User = GetUser(dr.GetString(i.Iterate()));
                        v.Qualification = dr.GetInt32(i.Iterate());
                        v.Comment = dr.GetString(i.Iterate());
                        v.Date = dr.GetDateTime(i.Iterate());
                        v.AsCostumer = dr.GetBoolean(i.Iterate());
                    }
                }

                return v;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<UserValoration> GetValoration(string username)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                Iterator i = new Iterator();
                List<UserValoration> vs = new List<UserValoration>();
                string sql = "SELECT * FROM uservaloration WHERE user_username = @username";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", username);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        i.Reset();
                        UserValoration v = new UserValoration();
                        v.Id = dr.GetInt32(i.Iterate());
                        v.User = GetUser(dr.GetString(i.Iterate()));
                        v.Qualification = dr.GetInt32(i.Iterate());
                        v.Comment = dr.GetString(i.Iterate());
                        v.Date = dr.GetDateTime(i.Iterate());
                        v.AsCostumer = dr.GetBoolean(i.Iterate());
                        vs.Add(v);
                    }
                }

                return vs;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<User> GetActiveChats(string user)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                List<User> users = new List<User>();

                string sql = "SELECT DISTINCT REPLACE(CONCAT(from_user,to_user),@user_username, '') FROM message WHERE (from_user = @user_username OR to_user = @user_username) AND to_hide = 0 ORDER BY date";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user_username", user);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read()) users.Add(GetUser(dr.GetString(0)));
                }

                return users;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Message> GetConversationMessages(string from, string to, int top = 0, bool parental = false, bool bidirectional = true)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                List<Message> messages = new List<Message>();
                string sql;

                if (bidirectional) sql = "SELECT id, to_user, message, date, watched FROM message WHERE (from_user = @from_user AND to_user = @to_user AND from_hide = 0) OR (from_user = @to_user AND to_user = @from_user AND to_hide = 0) ORDER BY date";
                else sql = "SELECT id, from_user, to_user, message, date, watched FROM message WHERE from_user = @from_user AND to_user = @to_user AND from_hide = 0 ORDER BY date";
                if (top > 0) sql += " LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@from_user", from);
                cmd.Parameters.AddWithValue("@to_user", to);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (bidirectional)
                        {
                            messages.Add(new Message(GetUser(dr.GetString(1)), dr.GetString(2), dr.GetDateTime(3), dr.GetBoolean(4)));
                            //if (parental) messages.Add(new Message(GetUser(dr.GetString(1)), Controllers.MainController.ChangeBannedWords(dr.GetString(2)), dr.GetDateTime(3), dr.GetBoolean(4)));
                            //else messages.Add(new Message(GetUser(dr.GetString(1)), dr.GetString(2), dr.GetDateTime(3), dr.GetBoolean(4)));
                        }
                        else
                        {
                            messages.Add(new Message(GetUser(dr.GetString(1)), GetUser(dr.GetString(2)), dr.GetString(3), dr.GetDateTime(4), dr.GetBoolean(5)));
                            //if (parental) messages.Add(new Message(GetUser(dr.GetString(1)), GetUser(dr.GetString(2)), Controllers.MainController.ChangeBannedWords(dr.GetString(3)), dr.GetDateTime(4), dr.GetBoolean(5)));
                            //else messages.Add(new Message(GetUser(dr.GetString(1)), GetUser(dr.GetString(2)), dr.GetString(3), dr.GetDateTime(4), dr.GetBoolean(5)));
                        }
                    }
                }

                return messages;
            }
            finally
            {
                conn.Close();
            }
        }

        public int NumberOfMessages(string from, string to, bool bidirectional = true)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                string sql;
                if (bidirectional) sql = "SELECT COUNT(*) FROM message WHERE (from_user = @from_user AND to_user = @to_user) OR (from_user = @to_user AND to_user = @from_user)";
                else sql = "SELECT COUNT(*) FROM message WHERE from_user = @from_user AND to_user = @to_user";

                MySqlCommand cmd1 = new MySqlCommand(sql, conn);
                cmd1.Parameters.AddWithValue("@from_user", from);
                cmd1.Parameters.AddWithValue("@to_user", to);
                return Convert.ToInt32(cmd1.ExecuteScalar());
            }
            finally
            {
                conn.Close();
            }
        }

        public int NumberOfMessages(User from, User to, bool bidirectional = true) => NumberOfMessages(from.Username, to.Username, bidirectional);

        public Dictionary<string, int> NumberOfMessages(string user, bool bidirectional = true)
        {
            Dictionary<string, int> number = new Dictionary<string, int>();
            
            List<User> users = GetActiveChats(user);
            foreach (User u in users) number.Add(u.Username, NumberOfMessages(user, u.Username, bidirectional));
            return number;
        }

        public Dictionary<string, int> NumberOfMessages(User user, bool bidirectional = true) => NumberOfMessages(user.Username, bidirectional);
        #endregion

        #region Model Setters
        public bool AddUser(User u)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                if (CheckUserUsernameExists(u.Username)) throw new DatabaseUserUsernameDuplicatedException();
                if (CheckUserEmailExists(u.Mail)) throw new DatabaseUserEmailDuplicatedException();

                string sql = "INSERT INTO user(username, password, mail, currencyCode) VALUES (@username,@password,@mail,@currencyCode)";
                MySqlCommand cmd = new MySqlCommand(sql, GetSqlConnection());
                cmd.Parameters.AddWithValue("@username", u.Username);
                cmd.Parameters.AddWithValue("@password", u.Password);
                cmd.Parameters.AddWithValue("@mail", u.Mail);
                cmd.Parameters.AddWithValue("@currencyCode", u.Currency.Code);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    int code = GetRandomCode;
                    sql = "INSERT INTO mail_confirmation(user_username, type, code) VALUES (@username,1,@code)";
                    cmd = new MySqlCommand(sql, GetSqlConnection());
                    cmd.Parameters.AddWithValue("@username", u.Username);
                    cmd.Parameters.AddWithValue("@code", code);
                    if (cmd.ExecuteNonQuery() == 1) return true;
                    else throw new Exception("An error ocurred while adding the mail confirmation to the database.");
                }
                else throw new Exception("An error ocurred while adding the new user to the database");
            }
            finally
            {
                conn.Close();
            }
        }

        public bool ResetUser(string username, string mail)
        {
            string pas = GetRandomCode.ToString();
            string sql = "UPDATE user SET password = @password WHERE username = @username AND mail = @mail";
            MySqlCommand cmd = new MySqlCommand(sql, GetSqlConnection());
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", User.ToSha256(pas));
            cmd.Parameters.AddWithValue("@mail", mail);
            return cmd.ExecuteNonQuery() == 1;
        }

        public bool ResetUser(User u, string mail) => ResetUser(u.Username, mail);
        public bool ResetUser(User u) => ResetUser(u.Username, u.Mail);
        #endregion

        #region Model Checkers
        public bool CheckUserExists(User u)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                return CheckUserUsernameExists(u.Username) || CheckUserEmailExists(u.Mail);
            }
            finally
            {
                conn.Close();
            }
        }
        public bool CheckUserUsernameExists(string u)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                string sql = "SELECT valid FROM user WHERE username = @username";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", u);
                return !(cmd.ExecuteScalar() is null);
            }
            finally
            {
                conn.Close();
            }
        }
        public bool CheckUserEmailExists(string mail)
        {
            MySqlConnection conn = GetSqlConnection();
            try
            {
                string sql = "SELECT valid FROM user WHERE mail = @mail";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mail", mail);
                return !(cmd.ExecuteScalar() is null);
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Helper Methods
        int GetRandomCode
        {
            get
            {
                Random rnd = new Random();
                return Convert.ToInt32(1000000 + rnd.NextDouble() * 9000000);
            }
        }
        #endregion
    }

    #region Exceptions
    public class DatabaseUserUsernameDuplicatedException : Exception { }
    public class DatabaseUserEmailDuplicatedException : Exception { }
    #endregion
}
