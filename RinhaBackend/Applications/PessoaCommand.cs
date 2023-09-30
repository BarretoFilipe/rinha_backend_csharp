namespace RinhaBackend.Dtos
{
    public class PessoaCommand
    {
        public string Apelido { get; set; }
        public string Nome { get; set; }
        public DateTime Nascimento { get; set; }
        public List<string> Stack { get; set; }
    }
}