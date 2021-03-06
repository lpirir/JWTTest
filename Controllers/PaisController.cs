﻿using JWTTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pais/politica
        // Solo usuarios que cumplan con la política de autorización (Claims correspondientes en JWT) accesan a este recurso
        [Authorize (Policy = "ShouldBeOnlyEmployee")]
        [HttpGet("politica")]
        public async Task<ActionResult<IEnumerable<Pais>>> GetPaisPolitica()
        {
            return await _context.Paises.ToListAsync();
        }

        // GET: api/Pais/anonimo
        // Cualquier usuario puede entrar aun si no esta autenticado (no JWT)
        [AllowAnonymous]
        [HttpGet("anonimo")]
        public async Task<ActionResult<IEnumerable<Pais>>> GetPaisAnonimo()
        {
            return await _context.Paises.ToListAsync();
        }

        // GET: api/Pais/autenticado
        // Solo un usurio Autenticado puede pasar (Cualquier JWT)
        [Authorize]
        [HttpGet("autenticado")]
        public async Task<ActionResult<IEnumerable<Pais>>> GetPaisAuteuticado()
        {
            return await _context.Paises.ToListAsync();
        }

        // GET: api/Pais/autenticado
        // Solo un usurio un Claim de tipo Rol = Administrador puede acceder a este recurso
        [Authorize (Roles = "Administrador")]
        [HttpGet("rol")]
        public async Task<ActionResult<IEnumerable<Pais>>> GetPaisRol()
        {
            return await _context.Paises.ToListAsync();
        }

        // GET: api/Pais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pais>> GetPais(Guid id)
        {
            var pais = await _context.Paises.FindAsync(id);

            if (pais == null)
            {
                return NotFound();
            }

            return pais;
        }

        // PUT: api/Pais/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPais(Guid id, Pais pais)
        {
            if (id != pais.Id)
            {
                return BadRequest();
            }

            _context.Entry(pais).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaisExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pais
        [HttpPost]
        public async Task<ActionResult<Pais>> PostPais(Pais pais)
        {
            _context.Paises.Add(pais);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPais", new { id = pais.Id }, pais);
        }

        // DELETE: api/Pais/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pais>> DeletePais(Guid id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null)
            {
                return NotFound();
            }

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();

            return pais;
        }

        private bool PaisExists(Guid id)
        {
            return _context.Paises.Any(e => e.Id == id);
        }
    }
}
