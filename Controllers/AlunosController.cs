using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlunoWeb.Models;
using AlunoWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AlunosController : ControllerBase
    {
      private IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AlunoWeb.Models.Aluno>), 200)]
        //[ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 500)]
        public async Task<ActionResult<IAsyncEnumerable<AlunoWeb.Models.Aluno>>> GetAlunos()
        {
            try
            {
                var alunos = await _alunoService.GetAlunos();
                return Ok(alunos);
            }
            catch
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }
        [HttpGet("AlunoPorNome")]
        public async Task<ActionResult<AlunoWeb.Models.Aluno>> 
        GetAlunosByNome([FromQuery] string nome)
        {
            try
            {
                var alunos = await _alunoService.GetAlunosByNome(nome);
                if(alunos == null )
                
                    return NotFound($"Não existe aluno com o nome {nome}");
                    return Ok(alunos);
                
                
            }
            catch
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }
        [HttpGet("{id:int}", Name = "GetAluno")]
        public async Task<ActionResult<AlunoWeb.Models.Aluno>> GetAluno(int Id)
        {
            try
            {
                var aluno = await _alunoService.GetAluno(Id);
                if (aluno == null)
                {
                    return NotFound($"Não existe aluno com o ID {Id}");
                }
                return Ok(aluno);
            }
            catch
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }
        [HttpPost]
        public async Task<ActionResult> CreateAluno(AlunoWeb.Models.Aluno aluno)
        {
            try
            {
                await _alunoService.CreateAluno(aluno);
                return CreatedAtRoute(nameof(GetAluno),
                    new { id = aluno.Id }, aluno);
            }
            catch
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAluno(int id, AlunoWeb.Models.Aluno aluno)
        {
            try
            {
                if(aluno.Id == id)
                {
                await _alunoService.UpdateAluno(aluno);
                //return NoContent();
                return Ok($"Aluno com ID {id} atualizado com sucesso");
                }
                else
                {
                return BadRequest("Dados inconsistentes");
                }
         
            }
            catch
            {
              return  BadRequest("Erro interno do servidor");
            }
            }
            [HttpDelete("{id:int}")]
            public async Task<ActionResult> DeleteAluno(int id)
            {
                try
                {
                    var aluno = await _alunoService.GetAluno(id);
                    if (aluno == null)
                    {
                        return NotFound($"Não existe aluno com o ID {id}");
                    }
                    await _alunoService.DeleteAluno(aluno);
                    return Ok($"Aluno com ID {id} deletado com sucesso");
                }
                catch
                {
                    return StatusCode(500, "Erro interno do servidor");
                }
            }
        }


    }
    
