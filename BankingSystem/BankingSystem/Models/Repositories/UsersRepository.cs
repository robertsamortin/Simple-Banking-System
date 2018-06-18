using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Models.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        //Change connection to the database
        string connectionString = "Server =.\\SQLEXPRESS01;Database=Banking;Integrated Security = true;";
        SqlTransaction sqlTrans = null;

        public void AddUser(Users usr)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    sqlTrans = con.BeginTransaction();
                    SqlCommand cmd = new SqlCommand("spUserInsert", con, sqlTrans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@LoginName", usr.LoginName);
                    cmd.Parameters.AddWithValue("@AccountNumber", usr.AccountNumber);
                    cmd.Parameters.AddWithValue("@Password", usr.Password);
                    cmd.Parameters.AddWithValue("@Balance", usr.Balance);
                    cmd.Parameters.AddWithValue("@CreatedDate", usr.CreatedDate);

                    cmd.ExecuteNonQuery();
                    sqlTrans.Commit();
                }
                catch
                {
                    if (sqlTrans != null)
                    {
                        sqlTrans.Rollback();
                    }
                }
                finally
                {
                    con.Close();
                }
                
            }
        }

        public bool CheckLoginName(string LoginName)
        {
            bool result = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUserCheckLoginName", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LoginName", LoginName);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }


                con.Close();
            }
            return result;
        }

        public Users GetUserByAccountNumber(string AccountNumber)
        {
            Users usr = new Users();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUserGetByAccountNumber", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AccountNumber", AccountNumber);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    usr.ID = Convert.ToInt32(rdr["ID"]);
                    usr.LoginName = rdr["LoginName"].ToString();
                    usr.AccountNumber = rdr["AccountNumber"].ToString();
                    usr.Password = rdr["Password"].ToString();
                    usr.Balance = double.Parse(string.Format("{0:N2}", rdr["Balance"].ToString()));
                    usr.CreatedDate = DateTime.Parse(rdr["CreatedDate"].ToString());
                }
                con.Close();
            }
            return usr;
        }

        public List<UserTransactions> GetUserTransactionsByAccountNumber(string AccountNumber)
        {
            List<UserTransactions> lstTrans = new List<UserTransactions>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUserTransactionsGetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AccountNumber", AccountNumber);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UserTransactions usr = new UserTransactions();

                    usr.ID = Convert.ToInt32(rdr["ID"]);
                    usr.AccountNumber = rdr["AccountNumber"].ToString();
                    usr.Amount = double.Parse(rdr["Amount"].ToString());
                    usr.RunningBalance = double.Parse(rdr["RunningBalance"].ToString());
                    usr.TransType = rdr["TransType"].ToString();
                    usr.TransDate = DateTime.Parse(rdr["TransDate"].ToString());
                    usr.TransBy = rdr["TransBy"].ToString();
                    usr.Balance = double.Parse(string.Format("{0:N2}", rdr["Balance"].ToString()));

                    lstTrans.Add(usr);
                }
                con.Close();
            }
            return lstTrans;
        }

        public void InsertUserTransactions(UserTransactions usrTrans)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    sqlTrans = con.BeginTransaction();
                    SqlCommand cmd = new SqlCommand("spUserTransactionsInsert", con, sqlTrans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AccountNumber", usrTrans.AccountNumber);
                    cmd.Parameters.AddWithValue("@Amount", usrTrans.Amount);
                    cmd.Parameters.AddWithValue("@TransType", usrTrans.TransType);
                    cmd.Parameters.AddWithValue("@TransDate", usrTrans.TransDate);
                    cmd.Parameters.AddWithValue("@TransBy", usrTrans.TransBy);

                    cmd.ExecuteNonQuery();
                    sqlTrans.Commit();
                }
                catch
                {
                    if (sqlTrans != null)
                    {
                        sqlTrans.Rollback();
                    }
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public Users UserLogin(string LoginName, string Password)
        {
            Users usr = new Users();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spUserLogin", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@LoginName", LoginName);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        usr.ID = Convert.ToInt32(rdr["ID"]);
                        usr.LoginName = rdr["LoginName"].ToString();
                        usr.AccountNumber = rdr["AccountNumber"].ToString();
                        usr.Password = rdr["Password"].ToString();
                        usr.Balance = double.Parse(rdr["Balance"].ToString());
                        usr.CreatedDate = DateTime.Parse(rdr["CreatedDate"].ToString());
                    }
                    con.Close();
                    return usr;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }
           
        }

        public bool CheckBalance(string AccountNumber, double CurrBalance)
        {
            double result = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spUserCheckBalanceIfEqual", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AccountNumber", AccountNumber);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        result = double.Parse(rdr["Balance"].ToString());
                    }
                    else
                    {
                        result = 0;
                    }

                    //con.Close();
                    if (result != CurrBalance)
                        return false;
                    return true;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }
                
            }
            
        }
    }
}
