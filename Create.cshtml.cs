using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore2.Pages.Client
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        } 

        public void OnPost()
        {
            clientInfo.name= Request.Form["name"];  
            clientInfo.email= Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address= Request.Form["address"];    
             
            if(clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "All the fields are required.";
                return;
            }
            //save data into db 
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore2;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO client" +
                        "(name,email,phone,address) VALUES " +
                        "(@name,@email,@phone,@address);";

                    using (SqlCommand command =new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
						command.Parameters.AddWithValue("@email", clientInfo.email);
						command.Parameters.AddWithValue("@phone", clientInfo.phone);
						command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
					}
                }
                            
             }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";

            successMessage="New client added correctly.";

            Response.Redirect("/Client/Index");

        }
    }
}
