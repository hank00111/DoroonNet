using GMap.NET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoroonNet.DoroonDB
{
    class TableListData
    {
        public string TblName;  
    }

    class TableInfoData
    {
        public int cid;
        public string Name;
    }

    class FlightListData
    {
        public int FlightID;
        public string linkid;
        public long start_datetimestamp;
    }

    class FlightListDatasb
    {
        public Int32 ID { get; set; }
        public string linkid { get; set; }
        public string start_datetimestamp { get; set; }
    }

    class FlightPathData
    {
        public int FlightDatasID;
        public int FlightID;
        public double latitude;
        public double longitude;
        public float altitude;
        public float groundspeed;
        public int heading;
        public long datetimestamp;
        public float X, Y, Z, X1, Y1, Z1, X2, Y2, Z2;
    }
    public class DBFlightData
    {
        public int FlightID { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public float altitude { get; set; }
        public float groundspeed { get; set; }
        public int heading { get; set; }
        public long datetimestamp { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float Z1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float Z2 { get; set; }

    }

    class FlightPathDataCSV
    {
        public int FlightDatasID { get; set; }
        public int FlightID { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public float altitude { get; set; }
        public float groundspeed { get; set; }
        public int heading { get; set; }
        public string datetimestamp { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float Z1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float Z2 { get; set; }

    }

    class DoroonSQLLiteDB
    {
        SQLiteConnection sqlite_conn;
        public bool IsRun = false;
        public delegate List<PointLatLng> DelegateFlightDataGPS( int ID);
        public static DelegateFlightDataGPS DelegateFlightDataGPSObj;
        public DoroonSQLLiteDB()
        {
            CreateConnection();
            DelegateFlightDataGPSObj = GetFlightDataGPS;
        }

        void CreateConnection()
        {
            sqlite_conn = new SQLiteConnection("Data Source=DoroonNetDB.db; Version = 3; New = True; Compress = True; Cache=Shared; ");//Synchronous=off
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                sqlite_conn = null;
                Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "new SQLiteConnection", null), ex);
            }
        }

        public void CreateTables()
        {
            SQLiteCommand sqlite_cmd;
            string Createsql;

            if (CheckTableExists("Flights") == false)
            {
                sqlite_cmd = sqlite_conn.CreateCommand();
                Createsql = "CREATE TABLE Flights (FlightID INTEGER PRIMARY KEY, linkid varchar(256), start_datetimestamp long NOT NULL)";
                sqlite_cmd.CommandText = Createsql;
                try
                {
                    sqlite_cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteNonQuery", sqlite_cmd), ex);
                    throw ex;
                }
            }

            if (CheckTableExists("FlightDatas") == false)
            {
                sqlite_cmd = sqlite_conn.CreateCommand();
                Createsql = "CREATE TABLE FlightDatas (FlightDatasID INTEGER PRIMARY KEY, FlightID integer NOT NULL, latitude double, longitude double, altitude float, groundspeed float, heading int32, datetimestamp long,  " +
                    "X float, Y float, Z float, X1 float,Y1 float, Z1 float, X2 float, Y2 float, Z2 float,";
                Createsql += "FOREIGN KEY (FlightID) REFERENCES Flights(FlightID) )";
                sqlite_cmd.CommandText = Createsql;
                try
                {
                    sqlite_cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteNonQuery", sqlite_cmd), ex);
                    throw ex;
                }
            }
        }

        private string GetDBQueryExtraInfo(string strCallerName, string strQueryType, SQLiteCommand sqlite_cmd)
        {
            string sAdtlInfo = strCallerName + " " + strQueryType;
            if (sqlite_cmd != null)
            {
                sAdtlInfo += ": " + sqlite_cmd.CommandText.ToString();
                foreach (SQLiteParameter p in sqlite_cmd.Parameters)
                {
                    string isQuted = (p.Value is string) ? "'" : "";
                    sAdtlInfo += "\n " + p.ParameterName.ToString() + " = " + isQuted + p.Value.ToString() + isQuted;
                }
            }
            return sAdtlInfo;
        }

        private bool CheckTableExists(String tblName)
        {
            SQLiteCommand sqlite_cmd;
            bool bRetVal = false;

            string Selectsql;
            sqlite_cmd = sqlite_conn.CreateCommand();
            Selectsql = "SELECT name FROM sqlite_master WHERE type ='table' and name = @tblName";
            sqlite_cmd.CommandText = Selectsql;
            sqlite_cmd.Parameters.AddWithValue("@tblName", tblName);
            try
            {
                SQLiteDataReader r = sqlite_cmd.ExecuteReader();

                while (r.Read())
                    bRetVal = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteReader", sqlite_cmd), ex);
                throw ex;
            }

            return (bRetVal);
        }

        #region Write
        public int WriteFlight(string linkid)
        {
            SQLiteCommand sqlite_cmd;
            SQLiteTransaction transaction = null;
            string sqlStr;            
            long FlightID;

            sqlite_cmd = sqlite_conn.CreateCommand();
            transaction = sqlite_conn.BeginTransaction();
            sqlStr = "Insert into Flights (linkid, start_datetimestamp) VALUES (@linkid, @start_datetimestamp)";
            sqlite_cmd.CommandText = sqlStr;
            sqlite_cmd.Parameters.AddWithValue("@linkid", linkid);
            sqlite_cmd.Parameters.AddWithValue("@start_datetimestamp", DateTime.Now.Ticks);
            try
            {
                sqlite_cmd.ExecuteNonQuery();
                FlightID = sqlite_conn.LastInsertRowId;
                transaction.Commit();
            } 
            catch (Exception ex)
            {
                Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteNonQuery", sqlite_cmd), ex);
                throw ex;
            }
      

            return Convert.ToInt32(FlightID);
        }
        //int i = 0;
        public void WriteFlightPoint(long pk, double latitude, double longitude, float altitude, float groundspeed, int heading, float X, float Y, float Z, float X1, float Y1, float Z1, float X2, float Y2, float Z2)
        {
            //SQLiteCommand sqlite_cmd;
            
            string Insertsql;
            //long FlightID;
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            using (SQLiteCommand connection = new SQLiteCommand("Data Source=(local);Initial Catalog=Northwind;Integrated Security=SSPI;"))
            {
                //connection.Open();
                SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                Insertsql = "Insert into FlightDatas (FlightID, latitude, longitude, altitude, groundspeed, heading, datetimestamp" +
                                ", X, Y, Z, X1, Y1, Z1, X2, Y2, Z2) " +
                                "VALUES (@FlightID, @latitude, @longitude, @altitude, @groundspeed, @heading, @datetimestamp" +
                                ", @X, @Y, @Z, @X1, @Y1, @Z1, @X2, @Y2, @Z2)";
                sqlite_cmd.CommandText = Insertsql;
                sqlite_cmd.Parameters.AddWithValue("@FlightID", pk);
                sqlite_cmd.Parameters.AddWithValue("@latitude", latitude);
                sqlite_cmd.Parameters.AddWithValue("@longitude", longitude);
                sqlite_cmd.Parameters.AddWithValue("@altitude", altitude);
                sqlite_cmd.Parameters.AddWithValue("@groundspeed", groundspeed);
                sqlite_cmd.Parameters.AddWithValue("@heading", heading);
                sqlite_cmd.Parameters.AddWithValue("@datetimestamp", DateTime.Now.Ticks);
                sqlite_cmd.Parameters.AddWithValue("@X", X);
                sqlite_cmd.Parameters.AddWithValue("@Y", Y);
                sqlite_cmd.Parameters.AddWithValue("@Z", Z);
                sqlite_cmd.Parameters.AddWithValue("@X1", X1);
                sqlite_cmd.Parameters.AddWithValue("@Y1", Y1);
                sqlite_cmd.Parameters.AddWithValue("@Z1", Z1);
                sqlite_cmd.Parameters.AddWithValue("@X2", X2);
                sqlite_cmd.Parameters.AddWithValue("@Y2", Y2);
                sqlite_cmd.Parameters.AddWithValue("@Z2", Z2);
                try
                {
                    var into = sqlite_cmd.ExecuteNonQuery();
                    //transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteNonQuery", sqlite_cmd), ex);
                    //transaction.Rollback();
                    throw ex;
                }
            }
            //SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            //SQLiteTransaction transaction = sqlite_conn.BeginTransaction();
            

            //FlightID = sqlite_conn.LastInsertRowId;
            //transaction.Commit();
            //sqlite_cmd.Dispose();
            //return Convert.ToInt32(FlightID);
            //sw.Stop();            
            //Console.WriteLine($"{i} Duration={sw.ElapsedMilliseconds:n0}ms");
            //i += 1;
        }

        public Task WriteFlightPointV2(Dictionary<string, DBFlightData> IdicFlightData)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string Insertsql;


            using (var transaction = sqlite_conn.BeginTransaction())
            {
                using (var command = sqlite_conn.CreateCommand())
                {
                    try
                    {
                        foreach (var v in IdicFlightData.Values.ToList())
                        {
                            Insertsql =
    "Insert into FlightDatas (FlightID, latitude, longitude, altitude, groundspeed, heading, datetimestamp" + ", X, Y, Z, X1, Y1, Z1, X2, Y2, Z2) " + "VALUES (@FlightID, @latitude, @longitude, @altitude, @groundspeed, @heading, @datetimestamp" +
", @X, @Y, @Z, @X1, @Y1, @Z1, @X2, @Y2, @Z2)";
                            command.CommandText = Insertsql;
                            command.Parameters.AddWithValue("@FlightID", v.FlightID);
                            command.Parameters.AddWithValue("@latitude", v.latitude);
                            command.Parameters.AddWithValue("@longitude", v.longitude);
                            command.Parameters.AddWithValue("@altitude", v.altitude);
                            command.Parameters.AddWithValue("@groundspeed", v.groundspeed);
                            command.Parameters.AddWithValue("@heading", v.heading);
                            command.Parameters.AddWithValue("@datetimestamp", v.datetimestamp);
                            command.Parameters.AddWithValue("@X", v.X);
                            command.Parameters.AddWithValue("@Y", v.Y);
                            command.Parameters.AddWithValue("@Z", v.Z);
                            command.Parameters.AddWithValue("@X1", v.X1);
                            command.Parameters.AddWithValue("@Y1", v.Y1);
                            command.Parameters.AddWithValue("@Z1", v.Z1);
                            command.Parameters.AddWithValue("@X2", v.X2);
                            command.Parameters.AddWithValue("@Y2", v.Y2);
                            command.Parameters.AddWithValue("@Z2", v.Z2);

                            command.CommandText = Insertsql;
                            command.ExecuteNonQuery();
                        }
                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;
                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Minutes, ts.Seconds, ts.Milliseconds, ts.Ticks);

                        Console.WriteLine("[DBinfo]RunTime:{0} Total:{1} Date:{2}", elapsedTime, IdicFlightData.Count, DateTime.Now.ToString("HH:mm:ss:fff"));
                        //transaction.Commit();
                        IsRun = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteNonQuery", command), ex);
                        //transaction.Rollback();
                        throw ex;
                    }
                }

                transaction.Commit();
            }

            return Task.CompletedTask;
        }

        #endregion

        #region GET
        public List<FlightListData> GetFlightID()
        {
            List<FlightListData> FlightList = new List<FlightListData>();
            SQLiteCommand sqlite_cmd;
            string Selectsql;
            sqlite_cmd = sqlite_conn.CreateCommand();
            Selectsql = "SELECT FlightID, linkid, start_datetimestamp FROM Flights ORDER BY FlightID ASC";
            sqlite_cmd.CommandText = Selectsql;
            try
            {
                SQLiteDataReader r = sqlite_cmd.ExecuteReader();

                while (r.Read())
                {
                    FlightListData data = new FlightListData
                    {
                        FlightID = r.GetInt32(0),
                        linkid = r.GetString(1)
                    };
                    //data.start_flight_timestamp = r.GetInt64(2);
                    FlightList.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteReader", sqlite_cmd), ex);
                throw ex;
            }

            return FlightList;
        }

        public List<FlightListData> GetFlightList()
        {
            List<FlightListData> FlightList = new List<FlightListData>();
            SQLiteCommand sqlite_cmd;
            string Selectsql;
            sqlite_cmd = sqlite_conn.CreateCommand();
            Selectsql = "SELECT FlightID, linkid, start_datetimestamp FROM Flights ORDER BY FlightID ASC";
            sqlite_cmd.CommandText = Selectsql;
            try
            {
                SQLiteDataReader r = sqlite_cmd.ExecuteReader();

                while (r.Read())
                {
                    FlightListData data = new FlightListData
                    {
                        FlightID = r.GetInt32(0),
                        linkid = r.GetString(1),
                        start_datetimestamp = r.GetInt64(2)
                    };
                    FlightList.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteReader", sqlite_cmd), ex);
                throw ex;
            }

            return FlightList;
        }

        public List<FlightPathData> GetFlightData(int WhereID)
        {
            List<FlightPathData> FlightData = new List<FlightPathData>();
            SQLiteCommand sqlite_cmd;
            string Selectsql;
            sqlite_cmd = sqlite_conn.CreateCommand();
            Selectsql = $"SELECT * FROM FlightDatas WHERE FlightID={WhereID}";
            sqlite_cmd.CommandText = Selectsql;
            try
            {
                SQLiteDataReader r = sqlite_cmd.ExecuteReader();

                while (r.Read())
                {
                    FlightPathData data = new FlightPathData
                    {
                        FlightDatasID = r.GetInt32(0),
                        FlightID = r.GetInt32(1),
                        latitude = r.GetDouble(2),
                        longitude = r.GetDouble(3),
                        altitude = r.GetFloat(4),
                        groundspeed = r.GetFloat(5),
                        heading = r.GetInt32(6),
                        datetimestamp = r.GetInt64(7),
                        X = r.GetFloat(8),
                        Y = r.GetFloat(9),
                        Z = r.GetFloat(10),
                        X1 = r.GetFloat(11),
                        Y1 = r.GetFloat(12),
                        Z1 = r.GetFloat(13),
                        X2 = r.GetFloat(14),
                        Y2 = r.GetFloat(15),
                        Z2 = r.GetFloat(16)

                    };
                    FlightData.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteReader", sqlite_cmd), ex);
                throw ex;
            }

            return FlightData;
        }

        public List<FlightPathDataCSV> GetFlightDataCSV(int WhereID)
        {
            List<FlightPathDataCSV> FlightData = new List<FlightPathDataCSV>();
            SQLiteCommand sqlite_cmd;
            string Selectsql;
            sqlite_cmd = sqlite_conn.CreateCommand();
            Selectsql = $"SELECT * FROM FlightDatas WHERE FlightID={WhereID}";
            sqlite_cmd.CommandText = Selectsql;
            try
            {
                SQLiteDataReader r = sqlite_cmd.ExecuteReader();

                while (r.Read())
                {
                    FlightPathDataCSV data = new FlightPathDataCSV
                    {
                        FlightDatasID = r.GetInt32(0),
                        FlightID = r.GetInt32(1),
                        latitude = r.GetDouble(2),
                        longitude = r.GetDouble(3),
                        altitude = r.GetFloat(4),
                        groundspeed = r.GetFloat(5),
                        heading = r.GetInt32(6),
                        datetimestamp = new DateTime(r.GetInt64(7)).ToString(),
                        X = r.GetFloat(8),
                        Y = r.GetFloat(9),
                        Z = r.GetFloat(10),
                        X1 = r.GetFloat(11),
                        Y1 = r.GetFloat(12),
                        Z1 = r.GetFloat(13),
                        X2 = r.GetFloat(14),
                        Y2 = r.GetFloat(15),
                        Z2 = r.GetFloat(16)
                    };
                    FlightData.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteReader", sqlite_cmd), ex);
                throw ex;
            }

            return FlightData;
        }

        public List<PointLatLng> GetFlightDataGPS(int WhereID)
        {
            List<PointLatLng> FlightDataGPS = new List<PointLatLng>();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand(); 
            string Selectsql;
            Selectsql = $"SELECT latitude, longitude FROM FlightDatas WHERE FlightID={WhereID} ORDER BY FlightDatasID ASC";
            sqlite_cmd.CommandText = Selectsql;
            try
            {
                SQLiteDataReader r = sqlite_cmd.ExecuteReader();

                while (r.Read())
                {
                    PointLatLng data = new PointLatLng
                    {
                        Lat = r.GetDouble(0),
                        Lng = r.GetDouble(1)         
                    };
                    FlightDataGPS.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteReader", sqlite_cmd), ex);
                throw ex;
            }
            return FlightDataGPS;
        }

        public List<TableListData> GetAllTables()
        {
            List<TableListData> TableList = new List<TableListData>();
            SQLiteCommand sqlite_cmd;
            string Selectsql;
            sqlite_cmd = sqlite_conn.CreateCommand();
            Selectsql = "SELECT name FROM sqlite_master WHERE type ='table'";
            sqlite_cmd.CommandText = Selectsql;
            try
            {
                SQLiteDataReader r = sqlite_cmd.ExecuteReader();

                while (r.Read())
                {
                    TableListData data = new TableListData
                    {
                        TblName = r.GetString(0)
                    };
                    TableList.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteReader", sqlite_cmd), ex);
                throw ex;
            }

            return TableList;
        }
        
        public List<TableInfoData> GetTableInfo(string SelTableName)
        {
            List<TableInfoData> TableList = new List<TableInfoData>();
            SQLiteCommand sqlite_cmd;
            string Selectsql;
            sqlite_cmd = sqlite_conn.CreateCommand();
            Selectsql = $"PRAGMA table_info([{SelTableName}])";
            sqlite_cmd.CommandText = Selectsql;
            try
            {
                SQLiteDataReader r = sqlite_cmd.ExecuteReader();

                while (r.Read())
                {
                    TableInfoData data = new TableInfoData
                    {
                        cid = r.GetInt32(0),
                        Name = r.GetString(1)
                    };
                    TableList.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetDBQueryExtraInfo(System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteReader", sqlite_cmd), ex);
                throw ex;
            }

            return TableList;
        }



        #endregion
    }
}
