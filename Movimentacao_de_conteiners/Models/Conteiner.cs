using System.Text.RegularExpressions;

namespace Movimentacao_de_conteiners.Models
{
    public class Conteiner
    {
        public Conteiner(int id, string cliente, string numeroConteiner, string tipo, int status, string categoria)
        {
            this.id = id;
            this.cliente = cliente;
            this.numeroConteiner = numeroConteiner;
            this.tipo = tipo;
            this.status = status;
            this.categoria = categoria;
        }

        public int id { get; set; }
        public string cliente { get; set; }
        public string numeroConteiner { get; set; }
        public string tipo { get; set; }
        public int status { get; set; }
        public string categoria { get; set; }

        public bool validate()
        {

            if(this.cliente == "") return false;

            if(!Regex.IsMatch(this.numeroConteiner, "[A-Z]{4}[0-9]{7}")) return false;

            string[] tipos = { "20", "40" };
            if(!tipos.Contains(this.tipo)) return false;

            string[] categorias = { "Importação", "Exportação" };
            if (!categorias.Contains(this.categoria)) return false;

            int[] status = { 0, 1 };
            if(!status.Contains(this.status)) return false;

            return true;
        }
    }
}
