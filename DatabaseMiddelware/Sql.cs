using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DatabaseMiddelware
{
    public class Sql
    {
        /*  Attr    */
        private String ConnectionString = "";
        private SqlConnection Connection;
        private bool IS_OK = false;
        private int IntType = 0;
        private String NotEqulEx = "VlauesAndAttributesAreNotEqualsException";
        private int TOP = -1;
        //private String sql = "";
        //private String err = "Error : -- > " ;
        /************/
        public Sql(String connString)
        {
            try
            {
            this.ConnectionString = connString;
            Connection = new SqlConnection(ConnectionString);
            this.IS_OK = ConnectionState();
            }
            catch (Exception e)
            {
                throw new Exception("Error : " + e.Message);
            }
        }
        public Sql(SqlConnection co)
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
                SqlCommand command = new SqlCommand(sql, this.Connection);
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
                SqlCommand command = new SqlCommand(sql, this.Connection);
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
                SqlCommand command = new SqlCommand(sql, Connection);
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
        public DataTable SelectStar(String tableName )
        {
            String sql = "";
            try
            {
                if (this.TOP > -1)
                {
                    sql = "SELECT TOP " + this.TOP + " * FROM " + tableName;
                }
                else
                {
                    sql = "SELECT * FROM " + tableName;
                }
                
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                return (ds.Tables[0].Rows.Count == 0) ? null : ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message + "SQL : " + sql);
            }
        }

        public DataTable SelectStar(String tableName, String Attr, object Value)
        {
            String sql = "";
            try
            {
                if (this.TOP > -1)
                {
                    sql = "SELECT TOP " + this.TOP + " * FROM " + tableName + " WHERE " + Attr + " = " + setSql(Value);
                }
                else
                {
                    sql = "SELECT * FROM " + tableName + " WHERE " + Attr + " = " + setSql(Value);
                }
                
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                return (ds.Tables[0].Rows.Count == 0) ? null : ds.Tables[0];
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
                if (this.TOP > -1)
                {
                    sql = "SELECT TOP " + this.TOP + " * FROM " + tableName + " WHERE ";
                }
                else
                {
                    sql = "SELECT * FROM " + tableName + " WHERE ";
                }
                
                for (int i = 0; i < count; i++)
                {
                    sql += Attrs[i] + " = " + setSql(Value[i]) + " AND ";
                }
                sql = sql.Substring(0, sql.Length - 4);
                //
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                return /*(ds.Tables[0].Rows.Count == 0) ? null :*/ ds.Tables[0];
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
                SqlDataAdapter adp = new SqlDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                return (ds.Tables[0].Rows.Count == 0) ? null : ds.Tables[0];
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
                SqlDataAdapter adp = new SqlDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                return (ds.Tables[0].Rows.Count == 0) ? null : ds.Tables[0];
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
                SqlDataAdapter adp = new SqlDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    if (ds.Tables[0].Rows.Count == 0){return null;}else{ return ds.Tables[0];}
                
                }
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
                SqlDataAdapter adp = new SqlDataAdapter(sql, Connection);
                adp.Fill(ds);
                //
                return (ds.Tables[0].Rows.Count == 0) ? null : ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message + " SQL : " + sql);
            }
        }

        public void Update(String tableName, String[] Attr, object[] Values,String att , object v)
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
                sql += " WHERE " + att + " = " + setSql(v);
                SqlCommand comand = new SqlCommand(sql, Connection);
                Connection.Open();
                comand.ExecuteNonQuery();
                Connection.Close();
            }catch{
                throw new Exception("Prlobem in update : " + sql);
            }

        }
        //**
        public DataTable quetyWithReturn(String query)
        {
            String sql = query;
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter(sql, Connection);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return (ds.Tables[0].Rows.Count == 0) ? null : ds.Tables[0];
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
                SqlCommand command = new SqlCommand(sql, Connection);
                Connection.Open();
                command.ExecuteNonQuery();
                Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message + " SQL : " + sql);
            }
        }
        //
        
        public Sql SelectTop(int TOP)
        {
            this.TOP = TOP;
            return this;
        }

    }
}
