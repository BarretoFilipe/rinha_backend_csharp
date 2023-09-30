using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RinhaBackend.Dtos;
using RinhaBackend.Models;

namespace RinhaBackend.Persistences
{
    public class PessoaRepo
    {
        private NpgsqlConnection GetConnection()
            => new NpgsqlConnection("Host=localhost; Database=rinha; Username=postgres; Password=200912es; Integrated Security=true; Pooling=true;");

        public async Task CreateDatabase()
        {
            using (var conn = new NpgsqlConnection("Host=localhost; Database=postgres; Username=postgres; Password=200912es; Integrated Security=true; Pooling=true;"))
            {
                await conn.ExecuteAsync(@"DROP DATABASE IF EXISTS rinha WITH (FORCE);
                        CREATE DATABASE rinha;");
            }

            using (var conn = GetConnection())
            {
                var sql = @$"CREATE TABLE IF NOT EXISTS pessoas(
                                id UUID PRIMARY KEY NOT NULL,
                                apelido VARCHAR(32) unique NOT NULL,
                                nome VARCHAR(100) NOT NULL,
                                nascimento DATE NOT NULL,
                                stack TEXT NULL,
                                searchterms TEXT NULL
                            );";

                await conn.ExecuteAsync(sql);
            }
        }

        public async Task Save(Pessoa pessoa)
        {
            using (var conn = GetConnection())
            {
                //stub talvez testar sem await e ver performance
                var sql = @"INSERT INTO pessoas (id, apelido, nome, stack, nascimento, searchTerms)
                    VALUES(@id, @apelido, @nome, @stack, @nascimento, @searchTerms)";
                await conn.ExecuteAsync(sql, pessoa);
            }
        }

        public async Task<PessoaDto> GetById(Guid Id)
        {
            using (var conn = GetConnection())
            {
                //stub Pode ser gargalo aqui
                var pessoa = await conn.QueryFirstOrDefaultAsync<Pessoa>($"SELECT * FROM pessoas WHERE id = '{Id}';");

                if (pessoa == null)
                    return null;

                return new PessoaDto()
                {
                    Id = pessoa.Id,
                    Apelido = pessoa.Apelido,
                    Nome = pessoa.Nome,
                    Nascimento = pessoa.Nascimento,
                    Stack = pessoa.Stack?.Split(",").ToList()
                };
            }
        }

        public async Task<IList<PessoaDto>> GetTerm([FromQuery] string t)
        {
            using (var conn = GetConnection())
            {
                //stub Pode ser gargalo aqui
                var result = await conn.QueryAsync<Pessoa>($"SELECT * FROM pessoas WHERE searchterms LIKE '%{t}%' LIMIT 50;");
                return result.Select(pessoa => new PessoaDto()
                {
                    Id = pessoa.Id,
                    Apelido = pessoa.Apelido,
                    Nome = pessoa.Nome,
                    Nascimento = pessoa.Nascimento,
                    Stack = pessoa.Stack?.Split(",").ToList()
                }).ToList();
            }
        }

        public async Task<int> Count()
        {
            using (var conn = GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<int>($"SELECT COUNT(id) FROM pessoas;");
            }
        }
    }
}