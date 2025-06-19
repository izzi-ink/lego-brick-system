using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LegoBrickApi.Models;

namespace LegoBrickApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BricksController : ControllerBase
    {
        private readonly BrickContext _context;

        public BricksController(BrickContext context)
        {
            _context = context;
        }

        // GET: api/Bricks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brick>>> GetBricks()
        {
            return await _context.Bricks.ToListAsync();
        }

        // GET: api/Bricks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brick>> GetBrick(int id)
        {
            var brick = await _context.Bricks.FindAsync(id);

            if (brick == null)
            {
                return NotFound();
            }

            return brick;
        }

        // POST: api/Bricks
        [HttpPost]
        public async Task<ActionResult<Brick>> PostBrick(Brick brick)
        {
            // Set the creation date automatically
            brick.DateCreated = DateTime.UtcNow;
            
            // Add the brick to the database
            _context.Bricks.Add(brick);
            await _context.SaveChangesAsync();

            // Return the created brick with its new ID
            return CreatedAtAction("GetBrick", new { id = brick.Id }, brick);
        }

        // PUT: api/Bricks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrick(int id, Brick brick)
        {
            if (id != brick.Id)
            {
                return BadRequest("ID mismatch");
            }

            // Get the existing brick from database
            var existingBrick = await _context.Bricks.FindAsync(id);
            if (existingBrick == null)
            {
                return NotFound();
            }

            // Update only the editable fields
            existingBrick.ServiceName = brick.ServiceName;
            existingBrick.ProjectName = brick.ProjectName;
            existingBrick.Description = brick.Description;
            existingBrick.DataConsumed = brick.DataConsumed;
            existingBrick.DataProduced = brick.DataProduced;
            existingBrick.Tags = brick.Tags;
            existingBrick.CreatorName = brick.CreatorName;
            // Keep original DateCreated

            _context.Entry(existingBrick).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrickExists(id))
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

        // DELETE: api/Bricks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrick(int id)
        {
            var brick = await _context.Bricks.FindAsync(id);
            if (brick == null)
            {
                return NotFound();
            }

            _context.Bricks.Remove(brick);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BrickExists(int id)
        {
            return _context.Bricks.Any(e => e.Id == id);
        }
    }
}