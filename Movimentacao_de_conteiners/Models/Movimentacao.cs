namespace Movimentacao_de_conteiners.Models
{
    public class Movimentacao
    {
        public Movimentacao()
        {
        }

        public int id { get; set; }
        public string tipo { get; set; }
        public DateTime inicio { get; set; }
        public DateTime fim { get; set; }
        public int conteinerId { get; set; }
        public Conteiner? conteiner { get; set; }

        public bool validate()
        {
            string[] tipos = { "Embarque", "Descarga", "Gate in", "Gate out", "Reposicionamento", "Pesagem", "Scanner" };

            if (!tipos.Contains(this.tipo)) return false;

            if (this.fim < this.inicio) return false;

            return true;
        }
    }
}
