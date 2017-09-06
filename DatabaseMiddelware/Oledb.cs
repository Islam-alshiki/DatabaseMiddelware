using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace DatabaseMiddelware
{
    public class Oledb
    {
        /*  Attr    */
        private String ConnectionString = "";
        private OleDbConnection Connection;
        private bool IS_OK = false;
        private int IntType = 0;
        private String NotEqulEx = "VlauesAndAttributesAreNotEqualsException";
        //private String sql = "";
        //private String err = "Error : -- > " ;
        /************/
        public Oledb(String connString)
        {
            try
            {
            this.ConnectionString = connString;
            Connection = new OleDbConnection(ConnectionString);
            this.IS_OK = ConnectionState();
            }
            catch (Exception e)
            {
                throw new Exception("Error : " + e.Message);
            }
        }
        public Oledb(OleDbConnection co)
        {
            try
            {
            this.Connection = co;
            this.IS_OK = ConnectionState();

            }
            catch (Exception e)
            {
                throw new Exception("Error : " + e.Message);
            }
        }
        public bool ConnectionState()
        {
            try
            {
                this.Connection.Open();
                if (this.Connection.State == System.Data.ConnectionState.Open)
                {
                    this.Connection.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }catch{
                return false;
            }
        }
        //      **DONE

        private String setSql(object v){
           
            if (v.GetType().Equals(IntType.GetType()))
                    {
                        return "" + v;
                    }
                    else
                    {
                       return "'" + v + "'";
                    }
        }
        public void Insert(String tableName , String[] Attrs , Object[] Values)
        {
            String sql = "";
            try
            {
                if (!(Attrs.Length == Values.Length))
                {
                    throw new Exception(NotEqulEx);
                }
                //
                sql = "INSERT INTO " + tableName + " (";
                foreach (String attr in Attrs)
                {
                    sql += attr + ",";    
                }
                sql = sql.Substring(0,sql.Length - 1);
                sql += ") Values (";
                foreach (Object value in Values)
                {
                    sql += setSql(value) + ",";
                }
                sql = sql.Substring(0,sql.Length - 1);
                sql += ");";
                //
                OleDbCommand command = new OleDbCommand(sql, this.Connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Error : " + e.Message + "SQL: " + sql);
            }
            //
       }//
        public void Insert(String tableName, Object[] Values)
        {
            String sql = "";
            try
            {
                //
                sql = "INSERT INTO " + tableName + " Values (";
                foreach (object value in Values)
                {
                    sql += setSql(value) + ",";
                }
                sql = sql.Substring(0, sql.Length - 1);
                sql += ");";
                //
                OleDbCommand command = new OleDbCommand(sql, this.Connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Error : " + e.Message + " SQL : " + sql);
            }
            //
        }
       //       **Done
        public void Delete(String tableName, String Attr , object Value)
        {
            String sql = "";
            try
            {
                sql = "DELETE FROM " + tableName + " WHERE " + Attr + " = " + setSql(Value);
                OleDbCommand command = new OleDbCommand(sql, Connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Error : " + e.Message + "SQL: " + sql);
            }
        }
        //*************************************
        public DataTable SelectStar(String tableName)
        {
            String sql = "";
            try
            {
                sql = "SELECT * FROM " + tableName ;
                DataSet ds = new DataSet();
                OleDbDataAdapter adp = new OleDbDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message + "SQL : " + sql);
            }
        }

        public DataTable SelectStar(String tableName, String Attr, String Value)
        {
            String sql = "";
            try
            {
                sql = "SELECT * FROM " + tableName + " WHERE " + Attr + " = " + setSql(Value);
                DataSet ds = new DataSet();
                OleDbDataAdapter adp = new OleDbDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message + " SQL : " + sql);
            }
        }
        //************** DONE
        public DataTable SelectStar(String tableName, String[] Attrs, object[] Value)
        {
            String sql = "";
            try
            {
                if (!(Attrs.Length == Value.Length))
                {
                    throw new Exception(NotEqulEx);
                }
                int count = Attrs.Length;
                sql = "SELECT * FROM " + tableName + " WHERE ";
                for (int i = 0; i < count; i++)
                {
                    sql += Attrs[i] + " = " + setSql(Value[i]) + " AND ";
                }
                sql = sql.Substring(0, sql.Length - 4);
                //
                DataSet ds = new DataSet();
                OleDbDataAdapter adp = new OleDbDataAdapter(sql, Connection);
                adp.Fill(ds);
                //

                return ds.Tables[0];
            }
            catch(Exception ex)
            {
                throw new Exception("Error : " + ex.Message + " SQL: " + sql);
            }
        }
        //************************************* DONE
        public DataTable Select(String tableName,String[] Attrs)
        {
            String sql = "";
            try
            {
                sql = "SELECT ";
                foreach (String Att in Attrs)
                {
                    sql += Att + ",";
                }
                sql = sql.Substring(0, sql.Length - 1);
                sql += " FROM " + tableName;
                DataSet ds = new DataSet();
                OleDbDataAdapter adp = new OleDbDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message + " SQL: " + sql);

            }
        }
        // dONE ////////////////////*********************************************000000000000000000000000000000000000
        public DataTable Select(String tableName,String[] Attrs, String Attr, String Value)
        {
            String sql = "";
            try
            {
                sql = "SELECT ";
                foreach (String Att in Attrs)
                {
                    sql += Attr + ",";
                }
                sql = sql.Substring(0, sql.Length - 1);
                sql += " FROM " + tableName + " WHERE " + Attr + " = " + setSql(Value);
                DataSet ds = new DataSet();
                OleDbDataAdapter adp = new OleDbDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message + " SQL: " + sql);
            }
        }
        // DONE
        public DataTable Select(String tableName,String[] Attrs, String[] Attr, String[] Value)
        {
            String sql = "";
            try
            {
                int IntType = 0;
                if (!(Attrs.Length == Value.Length))
                {
                    throw new Exception("");
                }
                int count = Attrs.Length;
                sql = "SELECT ";
                foreach (String Att in Attrs)
                {
                    sql += Attr + ",";
                }
                sql = sql.Substring(0, sql.Length - 1);
                sql += " FROM " + tableName + " WHERE ";
                for (int i = 0; i < count; i++)
                {
                    sql += Attrs[i] + " = ";
                    if (Value[i].GetType().Equals(IntType.GetType()))
                    {
                        sql += Value[i] + " AND ";
                    }
                    else
                    {
                        sql += "'" + Value[i] + "'" + " AND ";
                    }
                }
                sql = sql.Substring(0, sql.Length - 4);
                //
                DataSet ds = new DataSet();
                OleDbDataAdapter adp = new OleDbDataAdapter(sql, Connection);
                adp.Fill(ds);
                //

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message + " SQL : " + sql);
            }
        }
        //      DONE
        public DataTable Select(String sqlStatment)
        {
            String sql = "";
            try
            {
                sql = sqlStatment;
                DataSet ds = new DataSet();
                OleDbDataAdapter adp = new OleDbDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message + " SQL : " + sql);
            }
        }

        public void Update(String tableName, String[] Attr, object[] Values)
        {
            String sql = "";
            try
            {
                if (!(Attr.Length == Values.Length))
                {
                    throw new Exception("");
                }
                int count = Values.Length;
                sql = "UPDATE " + tableName + " SET ";
                for (int i = 0; i < count; i++)
                {
                    sql += Attr[i] + " = ";
                    if (Values[i].GetType().Equals(IntType.GetType()))
                    {
                        sql += Values[i];
                    }
                    else
                    {
                        sql += "'" + Values[i] + "'";
                    }
                    sql += ",";
                }
                sql = sql.Substring(0, sql.Length - 1);
                //
                OleDbCommand comand = new OleDbCommand(sql, Connection);
                Connection.Open();
                comand.ExecuteNonQuery();
                Connection.Close();
            }catch{
                throw new Exception("Prlobem in update : " + sql);
            }

        }

        public DataTable quetyWithReturn(String query)
        {
            String sql = query;
            try
            {
                OleDbDataAdapter adp = new OleDbDataAdapter(sql, Connection);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message + " SQL : " + sql);
            }
        }

        public void quetyWithNoReturn(String query)
        {
            String sql = query;
            try
            {
                OleDbCommand command = new OleDbCommand(sql, Connection);
                Connection.Open();
                command.ExecuteNonQuery();
                Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message + " SQL : " + sql);
            }
        }

    }
}
