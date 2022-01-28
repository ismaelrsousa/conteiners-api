using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movimentacao_de_conteiners.Models;
using System.Data.SqlClient;
using System.Net;

namespace Movimentacao_de_conteiners.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MovimentacaoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public List<Movimentacao> Get()
        {
            string query = "select * from dbo.movimentacao m inner join dbo.conteiner c on m.cd_conteiner = c.cd_conteiner order by cd_movimentacao DESC";

            string sqlDataSource = _configuration.GetConnectionString("DatabaseConnection");

            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                List<Movimentacao> movimentacaoList = new List<Movimentacao>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Movimentacao movimentacao = new Movimentacao();

                        movimentacao.id = Convert.ToInt32(reader["cd_movimentacao"]);
                        movimentacao.tipo = Convert.ToString(reader["nm_tipoMovimentacao"]);
                        movimentacao.inicio = Convert.ToDateTime(reader["dt_inicio"]);
                        movimentacao.fim = Convert.ToDateTime(reader["dt_fim"]);
                        movimentacao.conteinerId = Convert.ToInt32(reader["cd_conteiner"]);

                        Conteiner conteiner = new Conteiner(
                            Convert.ToInt32(reader["cd_conteiner"]),
                            Convert.ToString(reader["nm_cliente"]),
                            Convert.ToString(reader["nr_conteiner"]),
                            Convert.ToString(reader["nm_tipo"]),
                            Convert.ToInt32(reader["nm_status"]),
                            Convert.ToString(reader["nm_categoria"])
                        );

                        movimentacao.conteiner = conteiner;

                        movimentacaoList.Add(movimentacao);
                    }
                }

                reader.Close();
                con.Close();

                return movimentacaoList;
            }
        }

        [HttpPost]
        public HttpResponseMessage Post(Movimentacao movimentacao)
        {
            if (movimentacao.validate())
            {
                string query = $"insert into dbo.movimentacao values ('{movimentacao.tipo}', '{movimentacao.inicio}', '{movimentacao.fim}', {movimentacao.conteinerId})";

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
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        public HttpResponseMessage Put(Movimentacao movimentacao)
        {
            if (movimentacao.validate())
            {
                string query = @$"update dbo.movimentacao set 
                                    nm_tipoMovimentacao = '{movimentacao.tipo}',
                                    dt_inicio = '{movimentacao.inicio}',
                                    dt_fim = '{movimentacao.fim}',
                                    cd_conteiner = {movimentacao.conteinerId}
                                where cd_movimentacao = {movimentacao.id}";

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
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            string query = $"delete from dbo.movimentacao where cd_movimentacao = {id}";

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
