
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TDRevision.Models;
using TDRevision.Models.DTO;
using TDRevision.Models.Repository;

namespace TDRevision.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommandeController(IDataRepository<Commande,int,string> _dataRepository, IMapper  _mapper) : ControllerBase
    {

        /// <summary>
        /// Recupere un commande par son ID et le retourne
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ActionName("GetByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Commande>> GetCommande(int id)
        {

            var result = await _dataRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        /// <summary>
        /// Recupere tous les commandes et les retourne
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CommandeDTO>>> GetCommandes()
        {
            var list = await _dataRepository.GetAllAsync();
            return _mapper.Map<List<CommandeDTO>>(list);

        }


        /// <summary>
        /// Recupere un commande par son nom et le retourne
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet("{str}")]
        [ActionName("GetByString")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Commande>> GetCommandeByString(string str)
        {
            var commande = await _dataRepository.GetByKeyAsync(str);
            if (commande == null)
            {
                return NotFound();
            }
            return commande;
        }


        /// <summary>
        /// Ajoute un commande avec un commande dans le body et retourne le commande ajouté
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Commande>> Postcommande([FromBody] Commande commande)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _dataRepository.AddAsync(commande);

            return CreatedAtAction("GetByID", new { id = commande.IdCommande }, commande);
        }   

        /// <summary>
        /// Supprimer un commande par son ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ActionName("Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Deletecommande(int id)
        {
            ActionResult<Commande> commande = await _dataRepository.GetByIdAsync(id);
            if (commande.Value == null)
            {
                return NotFound();
            }
            await _dataRepository.DeleteAsync(commande.Value);
            return NoContent();
        }


        /// <summary>
        /// Modifie un commande par son ID et un commande depuis le body
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commande"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ActionName("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Putcommande(int id, [FromBody] Commande commande)
        {
            if (id != commande.IdCommande)
            {
                return BadRequest();
            }

            ActionResult<Commande?> commToUpdate = await _dataRepository.GetByIdAsync(id);

            if (commToUpdate.Value == null)
            {
                return NotFound();
            }

            await _dataRepository.UpdateAsync(commToUpdate.Value, commande);
            return NoContent();
        }


    }
}
