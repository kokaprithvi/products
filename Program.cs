// See https://aka.ms/new-console-template for more information


using System;
using System.Data;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Npgsql;

class Sample
{
    static void Main(string[] args)
    {
        // Connect to a PostgreSQL database
        NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1:5432;User Id=postgres; " +
           "Password=Prithvi123;Database=prods;");
        conn.Open();

        // Define a query returning a single row result set
        NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM PRODUCT", conn);
        
        
        


        // Execute the query and obtain the value of the first column of the first row
        //Int64 count = (Int64)command.ExecuteScalar();
        NpgsqlDataReader reader = command.ExecuteReader();

        DataTable dt = new DataTable();
        dt.Load(reader);

       
        NpgsqlCommand customer = new NpgsqlCommand("SELECT * FROM customer", conn);
        NpgsqlDataReader read = customer.ExecuteReader();
        DataTable customer_table = new DataTable();
        customer_table.Load(read);

        results7(dt);
        results20(customer_table);

        

        conn.Close();
    }

    static void print_results(DataTable data)
    {
        Console.WriteLine();
        Dictionary<string, int> colWidths = new Dictionary<string, int>();

        foreach (DataColumn col in data.Columns)
        {
            Console.Write(col.ColumnName);
            var maxLabelSize = data.Rows.OfType<DataRow>()
                    .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                    .OrderByDescending(m => m).FirstOrDefault();

            colWidths.Add(col.ColumnName, maxLabelSize);
            for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 14; i++) Console.Write(" ");
        }

        Console.WriteLine();

        foreach (DataRow dataRow in data.Rows)
        {
            for (int j = 0; j < dataRow.ItemArray.Length; j++)
            {
                Console.Write(dataRow.ItemArray[j]);
                for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 14; i++) Console.Write(" ");
            }
            Console.WriteLine();
        }
    }





    public static void results7(DataTable data)
    {

        foreach (DataRow dataRow in data.Rows)
        {
            if ((Int16)dataRow["prod_quantity"] >= 12 && ((Int16)dataRow["prod_quantity"] <= 30)) {

                Console.WriteLine(dataRow["prod_ID"] + " " + (string)dataRow["prod_desc"] + " " + dataRow["prod_quantity"]);
            }
        }
    }

    public static void results20(DataTable data)
    {
     var results = data.AsEnumerable().GroupBy(r =>
        {
            int rep_id;
            int.TryParse(r.Field<string>("rep_id"), out rep_id);
       

            return rep_id;
        })
        .Select(g => new
        {
            rep_id = g.Key,
            total_balance = g.Sum(r => r.Field<decimal>("cust_balance"))
        });

        var sortedResults = results.Where(r => r.total_balance > 12000 && r.rep_id > 0)
            .OrderBy(r => r.rep_id);

        foreach(var result in sortedResults)
        {
            Console.WriteLine("Rep ID: {0}, Total Balance: {1}", result.rep_id, result.total_balance);
        }
    }
 }

    




