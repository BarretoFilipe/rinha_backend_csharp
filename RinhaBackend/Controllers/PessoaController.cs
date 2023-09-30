using Microsoft.AspNetCore.Mvc;
using RinhaBackend.Dtos;
using RinhaBackend.Models;
using RinhaBackend.Persistences;

namespace RinhaBackend.Controllers
{
    [ApiController]
    public class PessoaController : ControllerBase
    {
        public readonly PessoaRepo _pessoaRepo;

        public PessoaController(ILogger<PessoaController> logger)
        {
            _pessoaRepo = new();
        }

        [HttpPost]
        [Route("pessoas")]
        public async Task<ActionResult<Pessoa>> Post([FromBody] PessoaCommand pessoaCommand)
        {
            var pessoa = new Pessoa(pessoaCommand.Apelido, pessoaCommand.Nome, pessoaCommand.Stack, pessoaCommand.Nascimento);

            if (!pessoa.IsValid())
                return UnprocessableEntity();

            await _pessoaRepo.Save(pessoa);

            //stub Pode ser gargalo aqui
            return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, pessoa);
        }

        [HttpGet]
        [Route("pessoas/{id:guid}")]
        public async Task<ActionResult<PessoaDto>> GetById(Guid Id)
        {
            var pessoa = await _pessoaRepo.GetById(Id);
            return pessoa == null
                ? NotFound()
                : Ok(pessoa);
        }

        [HttpGet]
        [Route("pessoas")]
        public async Task<ActionResult<IList<PessoaDto>>> GetTerm([FromQuery] string t)
        {
            if (string.IsNullOrWhiteSpace(t))
            {
                await Task.CompletedTask;
                return BadRequest();
            }

            return Ok(await _pessoaRepo.GetTerm(t));
        }

        [HttpGet]
        [Route("contagem-pessoas")]
        public async Task<ActionResult> Get()
        {
            return Ok(await _pessoaRepo.Count());
        }
    }
}