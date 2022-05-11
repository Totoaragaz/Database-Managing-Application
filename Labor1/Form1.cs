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
using System.Configuration;

namespace Labor1
{
    public partial class Form1 : Form
    {
        private string parentPrimaryKeyValue;
        private string childPrimaryKeyValue;
        private string connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        private string parentTableName = ConfigurationManager.AppSettings.Get("parentTable");
        private string childTableName = ConfigurationManager.AppSettings.Get("childTable");
        private DataTable parentTable;
        private DataTable childTable;
        private string parentPrimaryKey;
        private string childPrimaryKey;
        private string childForeignKey;
        private int parentPrimaryKeyColumnNr;
        private int childPrimaryKeyColumnNr;
        private List<Label> labels = new List<Label>();
        private List<TextBox> textBoxes = new List<TextBox>();

        public Form1()
        {
            InitializeComponent();
        }

        public void makeParentTableColumns()
        {
            DataTable dataTable = new DataTable();

            string queryString = "select top 1 * from " + parentTableName;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dataTable.Columns.Add(reader.GetName(i));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
            parentTable = dataTable;
        }

        public void addParentTableRows()
        {
            string queryString = "select * from " + parentTableName;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DataRow row = parentTable.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader[i];
                        }
                        parentTable.Rows.Add(row);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }

        public void makeChildTableColumns()
        {
            string queryString = "select top 1 * from " + childTableName;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        childTable.Columns.Add(reader.GetName(i));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }

        public void addChildTableRows()
        {
            string queryString = "select * from " + childTableName;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DataRow row = childTable.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader[i];
                        }
                        childTable.Rows.Add(row);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }

        private string findParentPrimaryKeyConstraintName()
        {
            string constraintName = "";

            string queryString = "select CONSTRAINT_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS "
                + "where TABLE_NAME = '" + parentTableName + "' AND CONSTRAINT_TYPE = 'PRIMARY KEY'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    constraintName = reader.GetValue(0).ToString();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
            return constraintName;
        }

        private void findParentPrimaryKeyColumn()
        {
            string queryString = "select COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                "where CONSTRAINT_NAME = '" + findParentPrimaryKeyConstraintName() + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    parentPrimaryKey = reader.GetValue(0).ToString();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }

        private string findChildForeignKeyConstraint()
        {
            string constraintName = "";

            string queryString = "select CONSTRAINT_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS "
                + "where TABLE_NAME = '" + childTableName + "' AND CONSTRAINT_TYPE = 'FOREIGN KEY'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        constraintName = reader.GetValue(0).ToString();
                        if (constraintName == "FK_" + childTableName + "_" + parentTableName) break;
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
            return constraintName;
        }

        private void findChildForeignKeyColumn()
        {
            string queryString = "select COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE "+
                "where CONSTRAINT_NAME = '" + findChildForeignKeyConstraint() + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    childForeignKey=reader.GetValue(0).ToString();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }

        private string findChildPrimaryKeyConstraintName()
        {
            string constraintName = "";

            string queryString = "select CONSTRAINT_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS "
                + "where TABLE_NAME = '" + childTableName + "' AND CONSTRAINT_TYPE = 'PRIMARY KEY'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    constraintName = reader.GetValue(0).ToString();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
            return constraintName;
        }

        private void findChildPrimaryKeyColumn()
        {
            string queryString = "select COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                "where CONSTRAINT_NAME = '" + findChildPrimaryKeyConstraintName() + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    childPrimaryKey = reader.GetValue(0).ToString();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }

        public int findParentPrimaryKeyColumnNr()
        {
            foreach (DataColumn col in parentTable.Columns)
            {
                if (col.ColumnName == parentPrimaryKey)
                    return col.Ordinal;
            }
            return -1;
        }

        public int findChildPrimaryKeyColumnNr()
        {
            foreach (DataColumn col in childTable.Columns)
            {
                if (col.ColumnName == childPrimaryKey)
                    return col.Ordinal;
            }
            return -1;
        }

        public void generateTextBoxes()
        {
            foreach (DataColumn col in childTable.Columns)
            {
                Label label = new Label();
                label.Text = col.ColumnName;
                labels.Add(label);
                labels[col.Ordinal].Location = new Point(620,200 + 30*col.Ordinal);
                labels[col.Ordinal].Visible = true;

                Controls.Add(labels[col.Ordinal]);

                TextBox textBox = new TextBox();
                textBox.Size = new Size(200, 20);
                textBoxes.Add(textBox);
                textBoxes[col.Ordinal].Location= new Point(720,200 +30*col.Ordinal);
                textBoxes[col.Ordinal].Visible = true;

                Controls.Add(textBoxes[col.Ordinal]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {   
            makeParentTableColumns();
            addParentTableRows();
            findParentPrimaryKeyColumn();
            findChildPrimaryKeyColumn();
            findChildForeignKeyColumn();
            parentPrimaryKeyColumnNr =findParentPrimaryKeyColumnNr();
            dataGridView1.DataSource = parentTable;
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            childPrimaryKeyValue = dataGridView2.Rows[e.RowIndex].Cells[childPrimaryKeyColumnNr].Value.ToString();
            foreach (DataColumn col in childTable.Columns)
            {
                textBoxes[col.Ordinal].Text = dataGridView2.Rows[e.RowIndex].Cells[col.Ordinal].Value.ToString(); ;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string queryString = "SELECT * FROM " + childTableName + " WHERE " 
                + childForeignKey + "=@p";

            try
            {
                parentPrimaryKeyValue = dataGridView1.Rows[e.RowIndex].Cells[parentPrimaryKeyColumnNr].Value.ToString();
                string paramValue = parentPrimaryKeyValue;


                using (SqlConnection connection =
                new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@p", paramValue);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        childTable = new DataTable();
                        makeChildTableColumns();
                        findChildPrimaryKeyColumnNr();

                        while (reader.Read())
                        {
                            DataRow row = childTable.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader[i];
                            }
                            childTable.Rows.Add(row);
                        }

                        dataGridView2.DataSource = childTable;

                        reader.Close();

                        generateTextBoxes();

                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tryInsertWithoutPKey()
        {
            string queryString = "insert into " + childTableName + " values (";
            foreach (DataColumn col in childTable.Columns)
            {
                if (col.ColumnName != childPrimaryKey)
                {
                    queryString = queryString + "@" + col.ColumnName + ",";
                }
            }
            queryString = queryString.Remove(queryString.Length - 1);
            queryString = queryString + ")";

                using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                foreach (DataColumn col in childTable.Columns)
                {
                    if (col.ColumnName != childPrimaryKey)
                    {
                        if (textBoxes[col.Ordinal].Text != "")
                            command.Parameters.AddWithValue("@" + col.ColumnName, textBoxes[col.Ordinal].Text);
                        else
                            command.Parameters.AddWithValue("@" + col.ColumnName, Convert.DBNull);
                    }
                }
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    SuccessLabel.Visible = true;
                    FailLabel.Visible = false;
                    IdExistsLabel.Visible = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    SuccessLabel.Visible = false;
                    FailLabel.Visible = true;
                }
                Console.ReadLine();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //INSERT
            string queryString = "insert into " + childTableName + " values (";
            foreach (DataColumn col in childTable.Columns)
            {
                queryString = queryString + "@" + col.ColumnName + ",";
            }
            queryString = queryString.Remove(queryString.Length - 1);
            queryString = queryString + ")";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                foreach (DataColumn col in childTable.Columns)
                {
                    if (textBoxes[col.Ordinal].Text != "")
                        command.Parameters.AddWithValue("@" + col.ColumnName, textBoxes[col.Ordinal].Text);
                    else
                        command.Parameters.AddWithValue("@" + col.ColumnName, Convert.DBNull);
                }

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    SuccessLabel.Visible = true;
                    FailLabel.Visible = false;
                    IdExistsLabel.Visible = false;
                }
                catch (Exception ex)
                {
                    if (ex is SqlException) IdExistsLabel.Visible = true;
                    Console.WriteLine(ex.Message);
                    SuccessLabel.Visible = false;
                    FailLabel.Visible= true;
                    tryInsertWithoutPKey();
                }
                Console.ReadLine();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //UPDATE

            string queryString = "update " + childTableName + " set "; //name=@n, fid=@f, email=@e, telefonnr=@t where id=@i";
            foreach (DataColumn col in childTable.Columns)
            {
                if (col.ColumnName != childPrimaryKey)
                {
                    queryString = queryString + col.ColumnName + "=@" + col.ColumnName + ",";
                }
            }
            queryString = queryString.Remove(queryString.Length - 1);
            queryString = queryString + " where " + childPrimaryKey + "=@" + childPrimaryKey;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);
                foreach (DataColumn col in childTable.Columns)
                {
                    command.Parameters.AddWithValue("@" + col.ColumnName, textBoxes[col.Ordinal].Text);
                }

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    SuccessLabel.Visible = true;
                    FailLabel.Visible = false;
                    IdExistsLabel.Visible = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    SuccessLabel.Visible = false;
                    FailLabel.Visible = true;
                }
                Console.ReadLine();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //DELETE

            string queryString = "delete " + childTableName + " where " + childPrimaryKey + "=@" + childPrimaryKey;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@" + childPrimaryKey, childPrimaryKeyValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    SuccessLabel.Visible = true;
                    FailLabel.Visible = false;
                    IdExistsLabel.Visible = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    SuccessLabel.Visible = false;
                    FailLabel.Visible = true;
                }
                Console.ReadLine();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
