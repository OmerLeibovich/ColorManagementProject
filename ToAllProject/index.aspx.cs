using Npgsql; // PostgreSQL database driver for .NET
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using ToAllProject.Modules;

public partial class index : Page
{
    // Connection string property — reads PostgreSQL connection from Web.config
    private string ConnStr =>
        ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

    // List that will store all color records fetched from the database
    public List<ColorDto> list = new List<ColorDto>();

    // SQL query to get all colors ordered by display_order
    private string sql = @"SELECT id, name, display_order, price, description
                           FROM public.""Colors""
                           ORDER BY display_order ASC;";

    // Runs on every page load
    protected void Page_Load(object sender, EventArgs e)
    {
        // Only load the data when it's the first request (not a postback)
        if (!IsPostBack)
        {
            loadPage();
        }
    }

    /// <summary>
    /// Loads the color list from the database into the 'list' field.
    /// </summary>
    private void loadPage()
    {
        try
        {
            using (var conn = new NpgsqlConnection(ConnStr))
            {
                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        using (var r = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (r.Read())
                            {
                                list.Add(new ColorDto
                                {
                                    Id = r.GetInt32(r.GetOrdinal("id")),
                                    Name = r.GetString(r.GetOrdinal("name")),
                                    DisplayOrder = r.GetInt32(r.GetOrdinal("display_order")),
                                    Price = r.GetFloat(r.GetOrdinal("price")),
                                    Description = r.GetString(r.GetOrdinal("description"))
                                });
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Show error message in red text
            Response.Write("<p style='color:red'> שגיאה: " + Server.HtmlEncode(ex.Message) + "</p>");
        }
    }

    // ========================
    // Web Methods (AJAX APIs)
    // ========================

    /// Adds a new color to the database if the display order is unique.
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static object AddNewColor(ColorDto color)
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                // Check if the display order already exists
                string checkSql = @"SELECT COUNT(*) FROM public.""Colors"" WHERE display_order = @display_order;";
                using (var checkCmd = new NpgsqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("display_order", color.DisplayOrder);
                    var exists = (long)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        return new { success = false, message = "סדר זה כבר קיים במערכת" };
                    }
                }

                // Insert the new color
                string insertSql = @"INSERT INTO public.""Colors"" (name, display_order, price, description) 
                                     VALUES (@name, @display_order, @price, @description);";

                using (var cmd = new NpgsqlCommand(insertSql, conn))
                {
                    cmd.Parameters.AddWithValue("name", color.Name);
                    cmd.Parameters.AddWithValue("display_order", color.DisplayOrder);
                    cmd.Parameters.AddWithValue("price", color.Price);
                    cmd.Parameters.AddWithValue("description", color.Description);
                    cmd.ExecuteNonQuery();
                }
            }

            return new { success = true, message = $"צבע {color.Name} נוסף בהצלחה" };
        }
        catch (Exception ex)
        {
            return new { success = false, message = "שגיאה: " + ex.Message };
        }
    }

    /// Updates an existing color's order, price, and description by name.
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static object UpdateColor(ColorDto color)
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                // Check if the color exists by name
                string checkSqlName = @"SELECT COUNT(*) FROM public.""Colors"" WHERE name = @name";
                using (var checkCmd = new NpgsqlCommand(checkSqlName, conn))
                {
                    checkCmd.Parameters.AddWithValue("name", color.Name);
                    long count = (long)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        return new { success = false, message = " הצבע לא קיים במערכת" };
                    }
                }

                // Update color data
                string updateSql = @"UPDATE public.""Colors"" 
                                     SET display_order = @display_order,
                                         price = @price,
                                         description = @description
                                     WHERE name = @name";

                using (var updateCmd = new NpgsqlCommand(updateSql, conn))
                {
                    updateCmd.Parameters.AddWithValue("display_order", color.DisplayOrder);
                    updateCmd.Parameters.AddWithValue("price", color.Price);
                    updateCmd.Parameters.AddWithValue("description", color.Description);
                    updateCmd.Parameters.AddWithValue("name", color.Name);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return new { success = true, message = $"הצבע {color.Name} עודכן בהצלחה" };
                    }
                    else
                    {
                        return new { success = false, message = " העדכון נכשל" };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return new { success = false, message = " שגיאה: " + ex.Message };
        }
    }

    /// Deletes a color from the database by ID.
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static object DeleteColor(int id)
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                string colorName = null;

                // Get the color name before deletion (for message purposes)
                using (var cmdGet = new NpgsqlCommand(@"SELECT name FROM public.""Colors"" WHERE id = @id", conn))
                {
                    cmdGet.Parameters.AddWithValue("id", id);
                    var result = cmdGet.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        colorName = result.ToString();
                    }
                }

                // Delete the color
                string deleteSql = @"DELETE FROM public.""Colors"" WHERE id = @id;";
                using (var cmd = new NpgsqlCommand(deleteSql, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return new { success = true, message = $" הצבע {colorName} נמחק בהצלחה" };
                    }
                    else
                    {
                        return new { success = false, message = $" צבע עם ID {id} לא נמצא" };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return new { success = false, message = " שגיאה: " + ex.Message };
        }
    }


    /// Saves a new display order for multiple colors.
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public static object SaveOrder(List<OrderItem> items)
    {
        if (items == null || items.Count == 0)
            return new { success = false, message = "אין נתונים לשמירה." };

        var connStr = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

        try
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                using (var cmd = new NpgsqlCommand(
                    @"UPDATE public.""Colors"" SET display_order = @order WHERE id = @id;", conn, tx))
                {
                    cmd.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
                    cmd.Parameters.Add("order", NpgsqlTypes.NpgsqlDbType.Integer);

                    // Loop through all items and update their order
                    foreach (var it in items)
                    {
                        cmd.Parameters["id"].Value = it.Id;
                        cmd.Parameters["order"].Value = it.DisplayOrder;
                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit(); // Commit all updates as one transaction
                }
            }
            return new { success = true, message = "הסדר החדש נשמר." };
        }
        catch (Exception ex)
        {
            return new { success = false, message = "שגיאה בשמירת הסדר: " + ex.Message };
        }
    }
}
