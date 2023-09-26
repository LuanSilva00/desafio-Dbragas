using Dbragas.Interfaces;
using Dbragas.Repositories;
using DBragas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data.Common;
using Dbragas.Models;
using OfficeOpenXml;

namespace Dbragas.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        public readonly IClientRepository _clientRepository;
        public readonly ITypeClientRepository _typeClientRepository;
        private readonly IWebHostEnvironment _environment;

        public ClientsController(IClientRepository clientsRepository, ITypeClientRepository typeClientRepository, IWebHostEnvironment environment)
        {
            _clientRepository = clientsRepository;
            _typeClientRepository = typeClientRepository;
            _environment = environment;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddClient([FromBody] ClientCreateDto clientCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingClientByEmail = await _clientRepository.GetByEmail(clientCreateDto.Email);

                if (existingClientByEmail != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
                }

                var typeClient = await _typeClientRepository.GetByName(clientCreateDto.TypeClient);

                if (typeClient == null)
                {
                    var typeClientCreateDto = new TypeClientCreateDto
                    {
                        Name = clientCreateDto.TypeClient
                    };

                    typeClient = new TypeClients
                    {
                        Id = Guid.NewGuid(),
                        Name = typeClientCreateDto.Name,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    _typeClientRepository.Add(typeClient);

                    if (!await _typeClientRepository.SaveAllAsync())
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
                    }
                }

                var newClient = new Clients
                {
                    Id = Guid.NewGuid(),
                    Name = clientCreateDto.Name,
                    Email = clientCreateDto.Email,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    TypeClientId = (Guid)typeClient.Id
                };

                _clientRepository.Add(newClient);

                if (await _clientRepository.SaveAllAsync())
                {
                    return Ok("Cliente salvo com sucesso");
                }
            }
            catch (DbException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                var client = await _clientRepository.GetById(id);
                if (client != null)
                {
                    return Ok(client);
                }
                else
                {
                    return NotFound("Cliente não encontrado");
                    ;
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");

            }
        }
        [HttpGet("findAll")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Clients>>> GetClients()
        {
            var clients = await _clientRepository.GetAllAsync();
            if (clients != null)
            {
                return NotFound("Clientes não encontrados");
            }
            return Ok(clients);
        }
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult> PatchClient(Guid id, ClientPatchDTO ClientPatchDTO)
        {
            var existingClient = await _clientRepository.GetById(id);

            if (existingClient == null)
            {
                return NotFound("User not found");
            }

            existingClient.Name = ClientPatchDTO.Name;
            existingClient.Email = ClientPatchDTO.Email;
            existingClient.UpdatedAt = DateTime.UtcNow;

            if (await _clientRepository.SaveAllAsync())
            {
                return Ok("User updated successfully");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteClient(Guid id)
        {
            var client = await _clientRepository.GetById(id);
            if (client == null)
            {
                return NotFound("Cliente não encontrado");
            }
            _clientRepository.Delete(client);
            if (await _clientRepository.SaveAllAsync())
            {
                return Ok("Cliente deletado com sucesso");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error: An error occurred while processing the request.");

        }
        [HttpPost("import")]
        [Authorize]
        public async Task<ActionResult> ImportClients(IFormFile file)
        {
            try
            {
                if (file == null || file.Length <= 0)
                {
                    return BadRequest("O arquivo está vazio.");
                }

                if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
                {
                    return BadRequest("Por favor, envie um arquivo XLSX válido.");
                }

                bool newClientsCreated = false; 

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var workbook = package.Workbook;
                        if (workbook.Worksheets.Count > 0)
                        {
                            var worksheet = workbook.Worksheets[0];
                            var rowIndex = 2;

                            while (worksheet.Cells[rowIndex, 1].Value != null)
                            {
                                var name = worksheet.Cells[rowIndex, 1].Value.ToString();
                                var email = worksheet.Cells[rowIndex, 2].Value.ToString();
                                var typeClientName = worksheet.Cells[rowIndex, 3].Value.ToString();

                                
                                var existingClient = await _clientRepository.GetByEmail(email);

                                if (existingClient == null)
                                {
                                    
                                    var existingTypeClient = await _typeClientRepository.GetByName(typeClientName);

                                    if (existingTypeClient == null)
                                    {
                                        existingTypeClient = new TypeClients
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = typeClientName,
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            IsActive = true
                                        };

                                        _typeClientRepository.Add(existingTypeClient);

                                        if (!await _typeClientRepository.SaveAllAsync())
                                        {
                                            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao salvar o tipo de cliente.");
                                        }
                                    }

                                    var newClient = new Clients
                                    {
                                        Id = Guid.NewGuid(),
                                        Name = name,
                                        Email = email,
                                        CreatedAt = DateTime.UtcNow,
                                        UpdatedAt = DateTime.UtcNow,
                                        IsActive = true,
                                        TypeClientId = (Guid)existingTypeClient.Id
                                    };

                                    _clientRepository.Add(newClient);
                                    newClientsCreated = true;
                                }

                                rowIndex++;
                            }
                        }
                    }
                }

                if (await _clientRepository.SaveAllAsync())
                {
                    if (newClientsCreated)
                    {
                        return Ok("Clientes importados com sucesso.");
                    }
                    else
                    {
                        return Ok("Nenhum novo cliente foi criado durante a importação.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {ex.Message}");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
        }

    }
}
