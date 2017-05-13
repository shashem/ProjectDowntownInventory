using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ProjectDowntownInventory
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                tbl.Visible = false;
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM I_Inventory"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            DataTable dt = new DataTable();
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            gvUsers.DataSource = dt;
                            gvUsers.DataBind();
                        }
                    }
                }
            }
            if(Session["Auth"] == (object)"1")
            {
                tbl.Visible = true;
                Auth.Visible = false;
            }
        }
        [WebMethod]
        [ScriptMethod]
        public static void addInventory(AddItem it)
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("EXEC P_ADD_ITEM @ITEM"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ITEM", it.ITEM);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        public class AddItem
        {
            public string ITEM { get; set; }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            var _auth = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EXEC P_AUTH_USER @Username, @Password", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                    using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adap.Fill(dt);
                        _auth = dt.Rows[0][0].ToString();
                    }
                    if(_auth == "1")
                    {
                        Auth.Visible = false;
                        tbl.Visible = true;
                        Session["Auth"] = (object)"1";
                    }
                }
            }
        }

        protected void SubmitItemAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EXEC P_ADD_ITEM @ITEM", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ITEM", ItemAddTxt.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Redirect(Request.RawUrl);

                }
            }
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EXEC P_DELETE_ITEM @ID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@ID", SqlDbType.Int);
                    var te = gvUsers.Rows[e.RowIndex].Cells[0].Text;
                    cmd.Parameters["@ID"].Value = te;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Redirect(Request.RawUrl);
                }
            }
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            foreach(GridViewRow row in gvUsers.Rows)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("EXEC P_EDIT_ITEM @ID, @QUANTITY, @UNITS,  @ORDER_MORE", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@ID", SqlDbType.Int);
                        string id = row.Cells[0].Text;
                        cmd.Parameters["@ID"].Value = id;
                        TextBox qty = (row.Cells[2].FindControl("txtRowQTY") as TextBox);
                        cmd.Parameters.AddWithValue("@QUANTITY", qty.Text);
                        TextBox unt = (row.Cells[3].FindControl("txtRowUNITS") as TextBox);
                        cmd.Parameters.AddWithValue("@UNITS", unt.Text);
                        cmd.Parameters.Add("@ORDER_MORE", SqlDbType.Int);
                        CheckBox chkRow = (row.Cells[4].FindControl("chkRow") as CheckBox);
                        int te = 0;
                        if(chkRow.Checked) { te = 1; }
                        cmd.Parameters["@ORDER_MORE"].Value = te;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            SaveSuccessMsg.Visible = true;
            Response.Redirect(Request.RawUrl);

        }
    }
}