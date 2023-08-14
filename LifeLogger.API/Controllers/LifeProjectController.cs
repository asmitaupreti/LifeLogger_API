using LifeLogger.DataAccess.Data;
using LifeLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifeLogger.API.Controllers
{
    [Route("api/LifeProject")]
    [ApiController]
    public class LifeProjectController:Controller
    {
        private readonly ApplicationDbContext _db;
        public LifeProjectController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllLifeEvents(string id) 
        { 
            try
            {
                IEnumerable<LifeProject> lifeProjects = await _db.LifeProjects.Where(u => u.UserID == id).ToListAsync();
                if(lifeProjects!= null && lifeProjects.Count()> 0)
                {
                    return Ok(lifeProjects);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
            
        }

        [HttpGet("{id}/{projectId:int}")]
        public async Task<IActionResult> GetLifeEvent(string id, int projectId ) 
        { 
             try
            {
                LifeProject lifeProject = await _db.LifeProjects.Where(u => u.UserID == id && u.ProjectId == projectId).FirstOrDefaultAsync();
                if(lifeProject!= null)
                {
                   
                    return Ok(lifeProject);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (System.Exception)
            {

                return BadRequest();
            }; 
        }

        [HttpPost]
        public async Task<IActionResult> CreateLifeEvent([FromBody] LifeProject lifeProject) 
        { 
             try
            {
               
                if(lifeProject!= null)
                {

                     var user = await _db.ApplicationUsers.Where( u=> u.Id ==lifeProject.UserID).FirstOrDefaultAsync();
                    if(user== null)
                        return BadRequest();

                    await _db.LifeProjects.AddAsync(lifeProject);
                    await _db.SaveChangesAsync();
                    return Ok(lifeProject);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (System.Exception)
            {

                return BadRequest();
            }; 
        }

        [HttpPut("{id}/{projectId:int}")]
        public async Task<IActionResult> EditLifeEvent(string id, int projectId ,[FromBody] LifeProject lifeProject )
         { 
            try
            {
                LifeProject project = await _db.LifeProjects.Where(u => u.UserID == id && u.ProjectId == projectId).FirstOrDefaultAsync();
                if(project!= null && projectId==lifeProject.ProjectId)
                {
                    project.Title = lifeProject.Title;
                    project.Description = lifeProject.Description;
                    project.IsPublic = lifeProject.IsPublic;
                    project.StartTime = DateTime.Now;
                    project.EndTime = DateTime.Now;
                    project.UpdatedAt = DateTime.Now;
                    project.Location = lifeProject.Location;
                    _db.LifeProjects.Update(project);
                    await _db.SaveChangesAsync();
                    return Ok(project);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (System.Exception)
            {

                return BadRequest();
            }; 
         }

        [HttpDelete("{id}/{projectId:int}")]
        public async Task<IActionResult> DeleteLifeEvent(string id, int projectId ) 
        { 
            try
            {
                LifeProject events = await _db.LifeProjects.Where(u => u.UserID == id && u.ProjectId == projectId).FirstOrDefaultAsync();
                if(events!= null)
                {
                    _db.LifeProjects.Remove(events);
                    await _db.SaveChangesAsync();
                    return Ok(events);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (System.Exception)
            {

                return BadRequest();
            }; 
        }
    }
}