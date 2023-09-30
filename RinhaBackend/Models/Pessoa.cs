namespace RinhaBackend.Models
{
    public class Pessoa
    {
        public Pessoa()
        { }
        public Pessoa(string apelido, string nome, List<string> stack, DateTime nascimento)
        {
            Id = Guid.NewGuid();
            Apelido = apelido;
            Nome = nome;
            Stack = string.Join(",", stack);
            Nascimento = nascimento;
            SearchTerms = $"{Apelido} {Nome} {Stack}";
        }

        public Guid Id { get; set; }
        public string Apelido { get; set; }
        public string Nome { get; set; }
        public string Stack { get; set; }
        public DateTime Nascimento { get; set; }
        public string SearchTerms { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Apelido)
                && Apelido.Length <= 32
                && !string.IsNullOrWhiteSpace(Nome)
                && Nome.Length <= 100
                && Nascimento != default;
        }
    }
}