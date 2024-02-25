namespace ApiBank.Web.Models
{
    public class Conta
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateTime? Date { get; private set; }
        public string Type { get; private set; }      

        public Conta(string name, DateTime? date, string type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Nome é obrigatório.");
            if (date == null || date == DateTime.MinValue || date == DateTime.MaxValue)
                throw new Exception("Data inválida.");
            if (string.IsNullOrWhiteSpace(type))
                throw new Exception("Tipo é obrigatório.");
            
            Name = name;
            Date = date;
            Type = type;
        }

        public override string ToString() => $"{Name} ({Date})";        
    }
}
