
using LifeLogger.DataAccess.Data;
using LifeLogger.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LifeLogger.API.Controllers
{
    [Route("api/Tag")]
    [ApiController]
    public class TagController:Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public TagController(ApplicationDbContext db,UserManager<ApplicationUser> userManager )
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            try
            {
                IEnumerable<Tag> tagList =  await _db.Tags.ToListAsync();
                return Ok(tagList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception GetAllTags:{ex}");
                return BadRequest();
                
            }
           
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTag(int id)
        {
            try
            {
                Tag tag = await _db.Tags.Where( u=> u.TagID ==id).FirstOrDefaultAsync();
                return Ok(tag);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception GetTag:{ex}");
                return BadRequest();
                
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody]Tag tag)
        {
            try
            {
                var res = await _db.Tags.FirstOrDefaultAsync(u => u.TagName == tag.TagName);
                if(res!= null)
                    return BadRequest();
                else
                {
                    if(tag !=null){
                        await _db.Tags.AddAsync( tag);
                        await _db.SaveChangesAsync();
                        return Ok(tag);
                    } 
                   else{
                        return BadRequest();
                     }
                    
                    
                    
                } 
             }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception CreateTag:{ex}");
                return BadRequest();
                
            }
                
         }

        
            
        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditTag(int id, [FromBody]Tag tag)
        {
            try
            {

                if(id != tag.TagID)
                {
                    return BadRequest();
                }

                var tagToUpdate = await _db.Tags.Where(u => u.TagID == id).FirstOrDefaultAsync();
                
                if(tagToUpdate == null)
                    return BadRequest();
                else
                {
                    tagToUpdate.TagName =  tag.TagName;
                    var result = _db.Tags.Update(tagToUpdate);
                    await _db.SaveChangesAsync();
                    return Ok();
                } 
             }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception EditTag:{ex}");
                return BadRequest();
                
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tagToDelete = await _db.Tags.Where(u => u.TagID == id).FirstOrDefaultAsync();
            if(tagToDelete!= null)
                {
                    _db.Tags.Remove(tagToDelete);
                    await _db.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
        }
    }
}