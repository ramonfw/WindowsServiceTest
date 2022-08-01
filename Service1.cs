using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SQLite;

namespace WcfServiceLibraryTest
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
    public class Service1 : IService1
    {
        private const String ConnectionString= "..\\..\\..\\DataBasesSQLite\\Data Source=chinook.db; Version = 3; New = True; Compress = True; ";
        private string ErrStr;

        public string GetLastErrorStr()
        {
            return ErrStr;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        private SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection(ConnectionString);
            // Open the connection:
            try
            {
                sqlite_conn.Open();
                ErrStr = "None";
            }
            catch (Exception ex)
            {
                ErrStr = "Error opening connection";
            }
            return sqlite_conn;
        }


        public string InsertEmployee( String pLastName, String pFirstName, String pTitle, 
                                    String pReportsTo, String pBirthDate, String pHireDate, String pAddress, 
                                    String pCity, String pState, String pCountry, 
                                    String pPostalCode, String pPhone, String pFax, String pEmail)
        {
            SQLiteConnection conn= CreateConnection();

            SQLiteCommand sqlite_cmd;
            String[] ArrCampos = { "LastName", "FirstName", "Title", "ReportsTo", "BirthDate", "HireDate", "Address", "City", "State", "Country", "PostalCode", "Phone", "Fax", "Email" };
            String[] pArrDatosEmployee = {pLastName, pFirstName, pTitle, pReportsTo, 
                                         pBirthDate, pHireDate, pAddress, pCity, pState, 
                                         pCountry, pPostalCode, pPhone, pFax, pEmail };
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO employees ";
            String Columnas = "(LastName,FirstName,Title,ReportsTo,BirthDate,HireDate,Address, " +
                "City, State, Country,PostalCode,Phone,Fax,Email)";
            String Valores = "(?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

            sqlite_cmd.CommandText += Columnas + " VALUES " + Valores;
            string CadenSQL = sqlite_cmd.CommandText;

            for (int i = 0; i < ArrCampos.Length; i++)
            {
                sqlite_cmd.Parameters.AddWithValue(ArrCampos[i], pArrDatosEmployee[i]);
            }

            int rowsAffected = sqlite_cmd.ExecuteNonQuery();

            conn.Close();

            if (rowsAffected > 0)
                ErrStr = "None";
            else
                ErrStr = "Error: Inserting record data failure";

            return ErrStr;
        }


    }
}
