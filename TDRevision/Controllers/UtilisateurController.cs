
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
    public class UtilisateurController(IDataRepository<Utilisateur,int,string> _dataRepository, IMapper  _mapper) : ControllerBase
    {

        /// <summary>
        /// Recupere un produit par son ID et le retourne
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ActionName("GetByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateur(int id)
        {

            var result = await _dataRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        /// <summary>
        /// Recupere tous les produits et les retourne
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UtilisateurDTO>>> GetUtilisateurs()
        {
            var list = await _dataRepository.GetAllAsync();
            return _mapper.Map<List<UtilisateurDTO>>(list);

        }

        /// <summary>
        /// Ajoute un produit avec un produit dans le body et retourne le produit ajouté
        /// </summary>
        /// <param name="Utilisateur"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Utilisateur>> PostProduit([FromBody] Utilisateur Utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _dataRepository.AddAsync(Utilisateur);

            return CreatedAtAction("GetByID", new { id = Utilisateur.IdUtilisateur }, Utilisateur);
        }   

        /// <summary>
        /// Supprimer un produit par son ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ActionName("Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            ActionResult<Utilisateur> produit = await _dataRepository.GetByIdAsync(id);
            if (produit.Value == null)
            {
                return NotFound();
            }
            await _dataRepository.DeleteAsync(produit.Value);
            return NoContent();
        }


        /// <summary>
        /// Modifie un produit par son ID et un produit depuis le body
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Utilisateur"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ActionName("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutProduit(int id, [FromBody] Utilisateur Utilisateur)
        {
            if (id != Utilisateur.IdUtilisateur)
            {
                return BadRequest();
            }

            ActionResult<Utilisateur?> commToUpdate = await _dataRepository.GetByIdAsync(id);

            if (commToUpdate.Value == null)
            {
                return NotFound();
            }

            await _dataRepository.UpdateAsync(commToUpdate.Value, Utilisateur);
            return NoContent();
        }


    }
}
