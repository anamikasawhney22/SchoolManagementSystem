﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Design
{
    public partial class Addnewstudent : System.Web.UI.Page
    {
        static string GetConnectionString()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            return ConnectionString;
        }
        static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillState();

            }
            using (SqlConnection con = GetSqlConnection())
            {
                using (SqlCommand com = new SqlCommand("select * from Board", con))
                {
                    con.Open();
                    SqlDataReader read = com.ExecuteReader();
                    Board.DataSource = read;
                    Board.DataTextField = "Boardname";
                    Board.DataValueField = "Boardid";
                    Board.DataBind();
                    con.Close();
                }
            }

        }

        protected void State_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCity();
        }
        void FillState()
        {
            using (SqlConnection con = GetSqlConnection())
            {
                using (SqlCommand com = new SqlCommand("Select * from States", con))
                {
                    con.Open();
                    SqlDataReader read = com.ExecuteReader();
                    State.DataSource = read;
                    State.DataTextField = "Statename";
                    State.DataValueField = "Stateid";
                    State.DataBind();
                    con.Close();
                }
            }
        }


        void FillCity()
        {
            Response.Write(State.SelectedValue);
            using (SqlConnection con = GetSqlConnection())
            {
                using (SqlCommand com = new SqlCommand("Select * from City where Stateid=@Stateid", con))
                {
                    com.Parameters.AddWithValue("@Stateid", State.SelectedValue);
                    con.Open();
                    SqlDataReader read = com.ExecuteReader();
                    City.DataSource = read;
                    City.DataTextField = "Cityname";
                    City.DataValueField = "Cityid";
                    City.DataBind();
                    con.Close();
                }
            }
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            string in_query = "insert into School(Schoolname, Address, Stateid,Cityid,Boardid,Pincode,Phoneno,DateOfRegistration,Zone) Values(@Schoolname,@Address,@Stateid,@Cityid,@Boardid,@Pincode,@Phoneno,@DateOfRegistration, @Zone)";
            using (SqlConnection con = GetSqlConnection())
            {
                using (SqlCommand com = new SqlCommand(in_query))
                {
                    com.Parameters.AddWithValue("@Schoolname", txtSchoolName.Text);
                    com.Parameters.AddWithValue("@Address", TxtAddress.Text);
                    com.Parameters.AddWithValue("@Stateid", Convert.ToInt32(State.Text));
                    com.Parameters.AddWithValue("@Cityid", Convert.ToInt32(City.Text));
                    com.Parameters.AddWithValue("@Boardid", Convert.ToInt32(Board.Text));
                    com.Parameters.AddWithValue("@Pincode", Convert.ToInt64(TxtPincode.Text));
                    com.Parameters.AddWithValue("@Phoneno", TxtPhone.Text);
                    com.Parameters.AddWithValue("@DateOfRegistration", Convert.ToDateTime(Calender.Text));
                    com.Parameters.AddWithValue("@Zone", Zone.Text);
                    com.Connection = con;
                    con.Open();
                    if (com.ExecuteNonQuery() > 0)
                    {
                        Response.Write("Record Inserted Successfully");
                    }
                    con.Close();
                }


            }
        }

        protected void Calender_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtSchoolName_TextChanged(object sender, EventArgs e)
        {

            using (SqlConnection con = GetSqlConnection())
            {
                using (SqlCommand com = new SqlCommand("select * from School where Schoolname=@Schoolname", con))
                {
                    com.Parameters.AddWithValue("@Schoolname", txtSchoolName.Text);
                    con.Open();

                    SqlDataAdapter ada = new SqlDataAdapter();
                    ada.SelectCommand = com;
                    DataSet ds = new DataSet();
                    ada.Fill(ds, "School");
                    DataTable dt = ds.Tables["School"];
                    if (dt.Rows.Count < 0)
                    {
                        Response.Write("Record Already Exit!");

                    }
                    else
                    {
                        Panel1.Visible = true;
                        Panel2.Visible = true;
                        Panel3.Visible = true;
                        Panel4.Visible = true;
                        Panel5.Visible = true;
                        Button1.Visible = true;
                      
                    }
                }
            }
        }
    }
}
