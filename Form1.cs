using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

//
//Netflix Database Application
//
//Author: Adolfo Moreno
//U. of Illinois, Chicago
//CS341, Fall 2015
//Homework 8
//

namespace WindowsFormsApplication1
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    // #1. Display All Moview
    private void button1_Click(object sender, EventArgs e)
    {
      this.listBox1.Items.Clear();

      string filename, version, connectionInfo;
      SqlConnection db;

      version = "v11.0";
      filename = "netflix.mdf";

      connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename =|DataDirectory|\{1};Integrated Security = True;", version, filename);

      db = new SqlConnection(connectionInfo);
      db.Open();

      string sql = string.Format("SELECT MovieName, MovieID FROM Movies ORDER BY MovieName ASC;");

      SqlCommand cmd = new SqlCommand();
      cmd.Connection = db;
      SqlDataAdapter adapter = new SqlDataAdapter(cmd);
      DataSet ds = new DataSet();

      cmd.CommandText = sql;
      adapter.Fill(ds);

      foreach (DataRow row in ds.Tables["TABLE"].Rows)
      {
        string msg = string.Format("{0}: {1}", Convert.ToInt32(row["MovieID"]), row["MovieName"]);
        this.listBox1.Items.Add(msg);
      }

      db.Close();
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    //#2. Display all Users
    private void button2_Click(object sender, EventArgs e)
    {
      this.listBox1.Items.Clear();
      string filename, version, connectionInfo;
      SqlConnection db;

      version = "v11.0";
      filename = "netflix.mdf";

      connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename =|DataDirectory|\{1};Integrated Security = True;", version, filename);

      db = new SqlConnection(connectionInfo);
      db.Open();

      string sql = string.Format("SELECT UserName, UserID, Occupation FROM Users ORDER BY UserName ASC;");

      SqlCommand cmd = new SqlCommand();
      cmd.Connection = db;
      SqlDataAdapter adapter = new SqlDataAdapter(cmd);
      DataSet ds = new DataSet();

      cmd.CommandText = sql;
      adapter.Fill(ds);

      foreach (DataRow row in ds.Tables["TABLE"].Rows)
      {
        string msg = string.Format("{0}: {1} {2}", Convert.ToInt32(row["UserID"]), row["UserName"], row["Occupation"]);
        this.listBox1.Items.Add(msg);
      }

      db.Close();
    }

    //#3. Get all reviews from a specific Movie
    private void button3_Click(object sender, EventArgs e)
    {
      this.listBox1.Items.Clear();

      int userInput = Convert.ToInt32(this.textBox1.Text);

      //Open a database connection
      string filename, version, connectionInfo;
      SqlConnection db;

      version = "v11.0";
      filename = "netflix.mdf";

      connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename =|DataDirectory|\{1};Integrated Security = True;", version, filename);

      db = new SqlConnection(connectionInfo);
      db.Open();


      string sqlCheck = string.Format("SELECT COUNT(Movies.MovieID) FROM Movies WHERE MovieID = {0};", userInput);
      object result2;

      SqlCommand cmdCheck = new SqlCommand();

      cmdCheck.Connection = db;

      SqlDataAdapter adapter3 = new SqlDataAdapter(cmdCheck);

      DataSet ds3 = new DataSet();

      cmdCheck.CommandText = sqlCheck;

      adapter3.Fill(ds3);

      result2 = cmdCheck.ExecuteScalar();

      int tempMovieID = Convert.ToInt32(result2);

      if (tempMovieID == 1)
      {

        string sql = string.Format("SELECT * FROM Movies INNER JOIN ( SELECT MovieID, Rating, UserID FROM Reviews WHERE MovieID = {0}) AS T ON T.MovieID = Movies.MovieID ORDER BY T.Rating DESC, T.UserID ASC ;", userInput);
        string sql2 = string.Format("SELECT MovieName FROM Movies WHERE MovieID = {0}", userInput);
        object result;

        SqlCommand cmd = new SqlCommand();
        SqlCommand cmd2 = new SqlCommand();

        cmd.Connection = db;
        cmd2.Connection = db;

        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2);

        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();


        cmd.CommandText = sql;
        cmd2.CommandText = sql2;

        adapter.Fill(ds);
        adapter.Fill(ds2);

        result = cmd2.ExecuteScalar();
        string message = string.Format("Movie: {0}", result);
        this.listBox1.Items.Add(message);

        foreach (DataRow row in ds.Tables["TABLE"].Rows)
        {
          string msg = string.Format("{0}: {1}", Convert.ToInt32(row["UserID"]), Convert.ToInt32(row["Rating"]));
          this.listBox1.Items.Add(msg);
        }

      }
      else
      {
        MessageBox.Show("ERROR: Movie not found. Please try a different Movie ID.");
      }
  
      db.Close();

    }

    //#10. Top N Reviewers
    private void button6_Click(object sender, EventArgs e)
    {

      this.listBox1.Items.Clear();
      int userInput = Convert.ToInt32(this.textBox5.Text);

      
      string filename, version, connectionInfo;
      SqlConnection db;

      version = "v11.0";
      filename = "netflix.mdf";

      connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename =|DataDirectory|\{1};Integrated Security = True;", version, filename);

      db = new SqlConnection(connectionInfo);
      db.Open();

      string sql = string.Format("SELECT TOP {0} Users.UserName, COUNT(Reviews.UserID) AS NumberReviews FROM Reviews LEFT JOIN Users ON Reviews.UserID = Users.UserID GROUP BY UserName ORDER BY NumberReviews DESC, UserName ASC;", userInput);

      SqlCommand cmd = new SqlCommand();
      cmd.Connection = db;
      SqlDataAdapter adapter = new SqlDataAdapter(cmd);
      DataSet ds = new DataSet();

      cmd.CommandText = sql;
      adapter.Fill(ds);

      foreach (DataRow row in ds.Tables["TABLE"].Rows)
      {
        string msg = string.Format("{0}: {1}", row["UserName"], Convert.ToInt32(row["NumberReviews"]));
        this.listBox1.Items.Add(msg);
      }

      db.Close();
    }


    //#9 TOP N REVIEWS
    private void button7_Click(object sender, EventArgs e)
    {
      this.listBox1.Items.Clear();
      int userInput = Convert.ToInt32(this.textBox5.Text);


      string filename, version, connectionInfo;
      SqlConnection db;

      version = "v11.0";
      filename = "netflix.mdf";

      connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename =|DataDirectory|\{1};Integrated Security = True;", version, filename);

      db = new SqlConnection(connectionInfo);
      db.Open();

      string sql = string.Format("SELECT TOP {0} Movies.MovieName, COUNT(Reviews.MovieID) AS NumberReviews FROM Reviews LEFT JOIN Movies ON Reviews.MovieID = Movies.MovieID GROUP BY MovieName ORDER BY NumberReviews DESC, MovieName ASC;", userInput);

      SqlCommand cmd = new SqlCommand();
      cmd.Connection = db;
      SqlDataAdapter adapter = new SqlDataAdapter(cmd);
      DataSet ds = new DataSet();

      cmd.CommandText = sql;
      adapter.Fill(ds);

      foreach (DataRow row in ds.Tables["TABLE"].Rows)
      {
        string msg = string.Format("{0}: {1}", row["MovieName"], Convert.ToInt32(row["NumberReviews"]));
        this.listBox1.Items.Add(msg);
      }

      db.Close();
    }

    //#4. Get All Reviews by User
    private void button8_Click(object sender, EventArgs e)
    {
      this.listBox1.Items.Clear();

      int userInput = Convert.ToInt32(this.textBox2.Text);

      //Open a database connection
      string filename, version, connectionInfo;
      SqlConnection db;

      version = "v11.0";
      filename = "netflix.mdf";

      connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename =|DataDirectory|\{1};Integrated Security = True;", version, filename);

      db = new SqlConnection(connectionInfo);
      db.Open();

      string sqlCheck = string.Format("SELECT COUNT(Users.UserID) FROM Users WHERE UserID = {0};", userInput);
      object result2;

      SqlCommand cmdCheck = new SqlCommand();

      cmdCheck.Connection = db;

      SqlDataAdapter adapter3 = new SqlDataAdapter(cmdCheck);

      DataSet ds3 = new DataSet();

      cmdCheck.CommandText = sqlCheck;

      adapter3.Fill(ds3);

      result2 = cmdCheck.ExecuteScalar();

      int tempUserID = Convert.ToInt32(result2);

      if (tempUserID == 1)
      {

      string sql = string.Format("SELECT  * FROM Movies INNER JOIN ( SELECT MovieID, Rating FROM Reviews WHERE UserID = {0}) AS T ON T.MovieID = Movies.MovieID ORDER BY Movies.MovieName ASC;", userInput);
      string sql2 = string.Format("SELECT UserName FROM Users WHERE UserID = {0};", userInput);
      object result;

      SqlCommand cmd = new SqlCommand();
      SqlCommand cmdName = new SqlCommand();
      cmd.Connection = db;
      cmdName.Connection = db;
      SqlDataAdapter adapter = new SqlDataAdapter(cmd);
      SqlDataAdapter adapter2 = new SqlDataAdapter(cmdName);
      DataSet ds = new DataSet();
      DataSet ds2 = new DataSet();

      cmd.CommandText = sql;
      cmdName.CommandText = sql2;

      adapter.Fill(ds);
      adapter2.Fill(ds2);

      result = cmdName.ExecuteScalar();

      string message = string.Format("User Name: {0}", result);

      this.listBox1.Items.Add(message);
      foreach (DataRow row in ds.Tables["TABLE"].Rows)
      {
        string msg = string.Format("{0}: {1}", row["MovieName"], Convert.ToInt32(row["Rating"]));
        this.listBox1.Items.Add(msg);
      }

      }
      else
      {
        MessageBox.Show("ERROR: User not found. Please try a different User ID");
      }

      db.Close();
    }

    //#5. Insert New Rating 
    private void button9_Click(object sender, EventArgs e)
    {
      int userInput3 = 0;

      this.listBox1.Items.Clear();
      if (this.radioButton1.Checked)
      {
        userInput3 = 1;
      }
      else if (this.radioButton2.Checked)
      {
        userInput3 = 2;

      }
      else if (this.radioButton3.Checked)
      {
        userInput3 = 3;

      }
      else if (this.radioButton4.Checked)
      {
        userInput3 = 4;

      }
      else if (this.radioButton5.Checked)
      {
        userInput3 = 5;

      }

      int userInput = Convert.ToInt32(this.textBox3.Text);
      int userInput2 = Convert.ToInt32(this.textBox4.Text);

      //Open a database connection
      string filename, version, connectionInfo;
      SqlConnection db;

      version = "v11.0";
      filename = "netflix.mdf";

      connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename =|DataDirectory|\{1};Integrated Security = True;", version, filename);

      db = new SqlConnection(connectionInfo);

      db.Open();
      string sql2 = string.Format("SELECT COUNT(Users.UserID) FROM Users WHERE UserID = {0};", userInput);
      string sql3 = string.Format("SELECT COUNT(Movies.MovieID) FROM Movies WHERE MovieID = {0};", userInput2);

      string sql = string.Format("INSERT INTO Reviews (Reviews.UserID, Reviews.MovieID, Reviews.Rating) VALUES ({0}, {1}, {2});", userInput, userInput2, userInput3);
      object result;
      object result2;

      SqlCommand cmd2 = new SqlCommand();
      SqlCommand cmd3 = new SqlCommand();


      cmd2.Connection = db;
      cmd3.Connection = db;


      SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2);
      SqlDataAdapter adapter3 = new SqlDataAdapter(cmd3);

      DataSet ds2 = new DataSet();
      DataSet ds3 = new DataSet();

      cmd2.CommandText = sql2;
      cmd3.CommandText = sql3;

      adapter2.Fill(ds2);
      adapter3.Fill(ds3);

      result = cmd2.ExecuteScalar();
      int tempUser = Convert.ToInt32(result);

      string message = string.Format("{0}", result);
      //MessageBox.Show(message);

      result2 = cmd3.ExecuteScalar();
      int tempMovie = Convert.ToInt32(result);
      string message2 = string.Format(" Movie ID: {0}", result2);
      //MessageBox.Show(message2);

      if(tempUser == 1 && tempMovie == 1)
      {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = db;
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        cmd.CommandText = sql;
        adapter.Fill(ds);
        MessageBox.Show("Succesfully added your review. Thank You!");
      }
      else
      {
        MessageBox.Show("ERROR: User or Movie Not Found In DataBase. Try Again.");

      }

      db.Close();

    }

    //#6. Get Average Rating For A Movie
    private void button4_Click(object sender, EventArgs e)
    {
      this.listBox1.Items.Clear();

      int userInput = Convert.ToInt32(this.textBox1.Text);

      //Open a database connection
      string filename, version, connectionInfo;
      SqlConnection db;

      version = "v11.0";
      filename = "netflix.mdf";

      connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename =|DataDirectory|\{1};Integrated Security = True;", version, filename);

      db = new SqlConnection(connectionInfo);
      db.Open();

      string sqlCheck = string.Format("SELECT COUNT(Movies.MovieID) FROM Movies WHERE MovieID = {0};", userInput);
      object result2;

      SqlCommand cmdCheck = new SqlCommand();

      cmdCheck.Connection = db;

      SqlDataAdapter adapter3 = new SqlDataAdapter(cmdCheck);

      DataSet ds3 = new DataSet();

      cmdCheck.CommandText = sqlCheck;

      adapter3.Fill(ds3);

      result2 = cmdCheck.ExecuteScalar();

      int tempMovieID = Convert.ToInt32(result2);

      if (tempMovieID == 1)
      {

        string sql = string.Format("SELECT Reviews.MovieID, ROUND(AVG(CAST(Rating AS Float)),4) AS AverageRating FROM Reviews WHERE Reviews.MovieID = {0} GROUP BY MovieID;", userInput);
        string sql2 = string.Format("SELECT MovieName FROM Movies WHERE MovieID = {0};", userInput);
        object result;

        SqlCommand cmd = new SqlCommand();
        SqlCommand cmdName = new SqlCommand();

        cmd.Connection = db;
        cmdName.Connection = db;

        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        SqlDataAdapter adapter2 = new SqlDataAdapter(cmdName);

        cmd.CommandText = sql;
        cmdName.CommandText = sql2;

        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();

        adapter.Fill(ds);
        adapter2.Fill(ds2);

        result = cmdName.ExecuteScalar();

        string message = string.Format("Movie: {0}", result);

        this.listBox1.Items.Add(message);

        foreach (DataRow row in ds.Tables["TABLE"].Rows)
        {
          string msg = string.Format("Average Rating: {0}", Convert.ToDouble(row["AverageRating"]));
          this.listBox1.Items.Add(msg);
        }

      }
      else
      {
        MessageBox.Show("ERROR: Movie not found. Please try a different ID");
      }

      db.Close();

    }

    //#8. Top N Movies by average
    private void button10_Click(object sender, EventArgs e)
    {
      this.listBox1.Items.Clear();
      int userInput = Convert.ToInt32(this.textBox5.Text);

      string filename, version, connectionInfo;
      SqlConnection db;

      version = "v11.0";
      filename = "netflix.mdf";

      connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename =|DataDirectory|\{1};Integrated Security = True;", version, filename);

      db = new SqlConnection(connectionInfo);
      db.Open();

      string sql = string.Format("SELECT TOP {0} Movies.MovieName, Temp.AvgRating FROM Movies INNER JOIN( SELECT Reviews.MovieID, ROUND(AVG(CAST(Reviews.Rating AS FLOAT)),4) as AvgRating FROM Reviews GROUP BY REVIEWS.MovieID)AS Temp ON Temp.MovieID = Movies.MovieID ORDER BY Temp.AvgRating DESC, Movies.MovieName ASC;", userInput);

      SqlCommand cmd = new SqlCommand();
      cmd.Connection = db;
      SqlDataAdapter adapter = new SqlDataAdapter(cmd);
      DataSet ds = new DataSet();

      cmd.CommandText = sql;
      adapter.Fill(ds);

      foreach (DataRow row in ds.Tables["TABLE"].Rows)
      {
        string msg = string.Format("{0}: {1}", row["MovieName"], Convert.ToDouble(row["AvgRating"]));
        this.listBox1.Items.Add(msg);
      }

      db.Close();

    }
    //#7. Summary of Reviews
    private void button5_Click(object sender, EventArgs e)
    {
      this.listBox1.Items.Clear();

      int userInput = Convert.ToInt32(this.textBox1.Text);

      //Open a database connection
      string filename, version, connectionInfo;
      SqlConnection db;

      version = "v11.0";
      filename = "netflix.mdf";

      connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename =|DataDirectory|\{1};Integrated Security = True;", version, filename);

      db = new SqlConnection(connectionInfo);
      db.Open();

      string sqlCheck = string.Format("SELECT COUNT(Movies.MovieID) FROM Movies WHERE MovieID = {0};", userInput);

      string sql = string.Format("SELECT COUNT(Rating) AS ReviewCount FROM Reviews WHERE MovieID = {0} AND Rating = 5;", userInput);
      string sql2 = string.Format("SELECT COUNT(Rating) AS ReviewCount FROM Reviews WHERE MovieID = {0} AND Rating = 4;", userInput);
      string sql3 = string.Format("SELECT COUNT(Rating) AS ReviewCount FROM Reviews WHERE MovieID = {0} AND Rating = 3;", userInput);
      string sql4 = string.Format("SELECT COUNT(Rating) AS ReviewCount FROM Reviews WHERE MovieID = {0} AND Rating = 2;", userInput);
      string sql5 = string.Format("SELECT COUNT(Rating) AS ReviewCount FROM Reviews WHERE MovieID = {0} AND Rating = 1;", userInput);
      string sql6 = string.Format("SELECT COUNT(Rating) AS ReviewCount FROM Reviews WHERE MovieID = {0};", userInput);
      string sqlName = string.Format("SELECT Movies.MovieName FROM Movies WHERE MovieID = {0};", userInput);

      object resultCheck;

      SqlCommand cmdCheck = new SqlCommand();

      cmdCheck.Connection = db;

      SqlDataAdapter adapterCheck = new SqlDataAdapter(cmdCheck);

      DataSet dsCheck = new DataSet();

      cmdCheck.CommandText = sqlCheck;

      adapterCheck.Fill(dsCheck);

      resultCheck = cmdCheck.ExecuteScalar();

      int tempMovieID = Convert.ToInt32(resultCheck);

      if (tempMovieID == 1)
      {
        object result;
        object result2;
        object result3;
        object result4;
        object result5;
        object result6;
        object resultName;

        SqlCommand cmd = new SqlCommand();
        SqlCommand cmdName = new SqlCommand();
        SqlCommand cmd2 = new SqlCommand();
        SqlCommand cmd3 = new SqlCommand();
        SqlCommand cmd4 = new SqlCommand();
        SqlCommand cmd5 = new SqlCommand();
        SqlCommand cmdMovie = new SqlCommand();

        

        cmd.Connection = db;
        cmdName.Connection = db;
        cmd2.Connection = db;
        cmd3.Connection = db;
        cmd4.Connection = db;
        cmd5.Connection = db;
        cmdMovie.Connection = db;

        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        SqlDataAdapter adapter2 = new SqlDataAdapter(cmdName);
        SqlDataAdapter adapter3 = new SqlDataAdapter(cmd2);
        SqlDataAdapter adapter4 = new SqlDataAdapter(cmd3);
        SqlDataAdapter adapter5 = new SqlDataAdapter(cmd4);
        SqlDataAdapter adapter6 = new SqlDataAdapter(cmd5);
        SqlDataAdapter adapter7 = new SqlDataAdapter(cmdMovie);



        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();
        DataSet ds3 = new DataSet();
        DataSet ds4 = new DataSet();
        DataSet ds5 = new DataSet();
        DataSet ds6 = new DataSet();
        DataSet ds7 = new DataSet();

        cmd.CommandText = sql;
        cmdName.CommandText = sql2;
        cmd2.CommandText = sql3;
        cmd3.CommandText = sql4;
        cmd4.CommandText = sql5;
        cmd5.CommandText = sql6;
        cmdMovie.CommandText = sqlName;

        adapter.Fill(ds);
        adapter2.Fill(ds2);
        adapter3.Fill(ds3);
        adapter4.Fill(ds4);
        adapter5.Fill(ds5);
        adapter6.Fill(ds6);
        adapter7.Fill(ds7);


        result = cmd.ExecuteScalar();
        result2 = cmdName.ExecuteScalar();
        result3 = cmd2.ExecuteScalar();
        result4 = cmd3.ExecuteScalar();
        result5 = cmd4.ExecuteScalar();
        result6 = cmd5.ExecuteScalar();
        resultName = cmdMovie.ExecuteScalar();

        string messageName = string.Format("Movie: {0}", resultName);
        this.listBox1.Items.Add(messageName);

        string message = string.Format("#5: {0}", result);
        this.listBox1.Items.Add(message);

        string message2 = string.Format("#4: {0}", result2);
        this.listBox1.Items.Add(message2);

        string message3 = string.Format("#3: {0}", result3);
        this.listBox1.Items.Add(message3);

        string message4 = string.Format("#2: {0}", result4);
        this.listBox1.Items.Add(message4);

        string message5 = string.Format("#1: {0}", result5);
        this.listBox1.Items.Add(message5);

        string message6 = string.Format("Total Reviews: {0}", result6);
        this.listBox1.Items.Add(message6);

      }
      else
      {
        MessageBox.Show("ERROR: Movie not Found. Please Try a different Movie ID");
      }

      db.Close();
    }

  }
}
