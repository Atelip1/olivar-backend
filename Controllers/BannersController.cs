using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.Models;
using OlivarBackend.DTOs;

namespace OlivarBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BannersController : ControllerBase
{
    private readonly RestauranteDbContext _context;

    public BannersController(RestauranteDbContext context)
    {
        _context = context;
    }

    // GET: api/Banners
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BannerDTO>>> GetBanners()
    {
        var banners = await _context.Banners
            .Select(b => new BannerDTO
            {
                BannerId = b.BannerId,
                Titulo = b.Titulo,
                ImagenUrl = b.ImagenUrl,
                FechaInicio = b.FechaInicio,
                FechaFin = b.FechaFin,
                Activo = b.Activo
            })
            .ToListAsync();

        return Ok(banners);
    }

    // GET: api/Banners/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BannerDTO>> GetBanner(int id)
    {
        var banner = await _context.Banners.FindAsync(id);

        if (banner == null)
            return NotFound();

        var dto = new BannerDTO
        {
            BannerId = banner.BannerId,
            Titulo = banner.Titulo,
            ImagenUrl = banner.ImagenUrl,
            FechaInicio = banner.FechaInicio,
            FechaFin = banner.FechaFin,
            Activo = banner.Activo
        };

        return Ok(dto);
    }

    // POST: api/Banners
    [HttpPost]
    public async Task<ActionResult<Banner>> PostBanner(BannerDTO dto)
    {
        var banner = new Banner
        {
            Titulo = dto.Titulo,
            ImagenUrl = dto.ImagenUrl,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            Activo = dto.Activo ?? true
        };

        _context.Banners.Add(banner);
        await _context.SaveChangesAsync();

        dto.BannerId = banner.BannerId;

        return CreatedAtAction(nameof(GetBanner), new { id = banner.BannerId }, dto);
    }

    // PUT: api/Banners/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBanner(int id, BannerDTO dto)
    {
        if (id != dto.BannerId)
            return BadRequest();

        var banner = await _context.Banners.FindAsync(id);

        if (banner == null)
            return NotFound();

        banner.Titulo = dto.Titulo;
        banner.ImagenUrl = dto.ImagenUrl;
        banner.FechaInicio = dto.FechaInicio;
        banner.FechaFin = dto.FechaFin;
        banner.Activo = dto.Activo;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Banners/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBanner(int id)
    {
        var banner = await _context.Banners.FindAsync(id);

        if (banner == null)
            return NotFound();

        _context.Banners.Remove(banner);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Banner eliminado correctamente." });
    }
}
