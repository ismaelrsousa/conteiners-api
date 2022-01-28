using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movimentacao_de_conteiners.Models;
using System.Data.SqlClient;
using System.Net;

namespace Movimentacao_de_conteiners.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConteinerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConteinerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public List<Conteiner> Get()
        {
            string query = "select * from dbo.conteiner order by cd_conteiner DESC";

            string sqlDataSource = _configuration.GetConnectionString("DatabaseConnection");

            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                List<Conteiner> conteinersList = new List<Conteiner>();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        Conteiner conteiner = new Conteiner(
                            Convert.ToInt32(reader["cd_conteiner"]),
                            Convert.ToString(reader["nm_cliente"]),
                            Convert.ToString(reader["nr_conteiner"]),
                            Convert.ToString(reader["nm_tipo"]),
                            Convert.ToInt32(reader["nm_status"]),
                            Convert.ToString(reader["nm_categoria"])
                        );

                        conteinersList.Add(conteiner);
                    }
                }

                reader.Close();
                con.Close();

                return conteinersList;
            }
        }

        [HttpPost]
        public HttpResponseMessage Post(Conteiner conteiner)
        {
            if (conteiner.validate())
            {
                string query = $"insert into dbo.conteiner values ('{conteiner.cliente}', '{conteiner.numeroConteiner}', '{conteiner.tipo}', {conteiner.status}, '{conteiner.categoria}')";

                string sqlDataSource = _configuration.GetConnectionString("DatabaseConnection");

                try
                {
                    using (SqlConnection con = new SqlConnection(sqlDataSource))
                    {
                        SqlCommand cmd = new SqlCommand(query, con);

                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        reader.Close();
                        con.Close();

                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            } else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        public HttpResponseMessage Put(Conteiner conteiner)
        {
            if (conteiner.validate())
            {
                string query = @$"update dbo.conteiner set 
                                    nm_cliente = '{conteiner.cliente}',
                                    nr_conteiner = '{conteiner.numeroConteiner}',
                                    nm_tipo = '{conteiner.tipo}',
                                    nm_status = {conteiner.status},
                                    nm_categoria = '{conteiner.categoria}'
                                where cd_conteiner = {conteiner.id}";

                string sqlDataSource = _configuration.GetConnectionString("DatabaseConnection");

                try
                {
                    using (SqlConnection con = new SqlConnection(sqlDataSource))
                    {
                        SqlCommand cmd = new SqlCommand(query, con);

                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        reader.Close();
                        con.Close();

                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            } else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            string query = $"delete from dbo.conteiner where cd_conteiner = {id}";

            string sqlDataSource = _configuration.GetConnectionString("DatabaseConnection");

            try
            {
                using (SqlConnection con = new SqlConnection(sqlDataSource))
                {
                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    reader.Close();
                    con.Close();

                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
