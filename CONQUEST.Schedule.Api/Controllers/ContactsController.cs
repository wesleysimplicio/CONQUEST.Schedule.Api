using CONQUEST.Schedule.Api.Domain.Interfaces;
using CONQUEST.Schedule.Api.Domain.Models;
using CONQUEST.Schedule.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace CONQUEST.Schedule.Api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class ContactsController : Controller
    {
        private readonly IContactBusiness _contactBusiness;
        private readonly ILogger _logger;

        public ContactsController(
             IContactBusiness contactBusiness,
             ILogger<ContactsController> logger
            )
        {
            this._contactBusiness = contactBusiness;
            this._logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(this._contactBusiness.Get());
            }
            catch (Exception ex)
            {
                string error = "Não foi possível realizar a busca de contatos";
                this._logger.LogError(ex, error);
                return BadRequest(new ErrorItem(1, error));
            }
        }

        [HttpGet("{Code}")]
        public IActionResult GetById(string Code)
        {
            try
            {
                return Ok(this._contactBusiness.GetById(Code));
            }
            catch (Exception ex)
            {
                string error = $"Não foi possível realizar a busca do contato: {Code}";
                this._logger.LogError(ex, error);
                return BadRequest(new ErrorItem(2, error));
            }
        }

        // POST: api/Contacts
        [HttpPost]
        public IActionResult Post([FromBody]Contact contact)
        {
            try
            {
                var resul = this._contactBusiness.Insert(contact);
                if (resul)
                {
                    return Ok();
                }
                else
                {
                    _logger.LogDebug("Não foi possível inserir");
                    return new BadRequestResult();
                }

            }
            catch (Exception ex)
            {
                string error = $"Não foi possível inserir";
                this._logger.LogError(ex, error);
                return BadRequest(new ErrorItem(2, error));
            }
        }

        // PUT: api/Contacts/5
        [HttpPut("{Code}")]
        public IActionResult Put(string Code, [FromBody]Contact contact)
        {
            try
            {
                contact.Code = Code;
                var resul = this._contactBusiness.Update(contact);
                if (resul)
                {
                    return Ok();
                }
                else
                {
                    _logger.LogDebug("Não foi possível atualizar contato : " + Code);
                    return new BadRequestResult();
                }

            }
            catch (Exception ex)
            {
                string error = $"Não foi possível inserir";
                this._logger.LogError(ex, error);
                return BadRequest(new ErrorItem(2, error));
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{Code}")]
        public IActionResult Delete(string Code)
        {
            try
            {
                _logger.LogDebug("Exclusão do contato: " + Code);
                var ok = this._contactBusiness.Delete(Code);
                if (ok)
                {
                    return new NoContentResult();
                }
                else
                {
                    _logger.LogDebug("Contato não encontrado");
                    return new NotFoundResult();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Erro ao excluir contato");
                return new BadRequestResult();
            }
        }

    }
}
