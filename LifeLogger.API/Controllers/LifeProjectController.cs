using AutoMapper;
using LifeLogger.DataAccess.Data;
using LifeLogger.Models;
using LifeLogger.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifeLogger.API.Controllers
{
    [Route("api/LifeProject")]
    [ApiController]
    [Authorize]
    public class LifeProjectController:Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public LifeProjectController(ApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllLifeProjects(string id) 
        { 
            try
            {
                IEnumerable<LifeProject> lifeProjects = await _db.LifeProjects.Include(p => p.LifeMilestones).Where(u => u.UserID == id).ToListAsync();
                if(lifeProjects!= null || lifeProjects.Count()> 0)
                { 
                    IEnumerable<LifeProjectResponseDTO> projects = _mapper.Map<IEnumerable<LifeProjectResponseDTO>>(lifeProjects);
                    
                    foreach (var item in projects)
                    {
                        item.LifeMileStoneCount = lifeProjects.Where(x => x.ProjectId == item.ProjectId).FirstOrDefault().LifeMilestones.Count();
                    }
                    
                    return Ok(projects);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex);
            }
            
        }

        [HttpGet("{id}/{projectId:int}")]
        public async Task<IActionResult> GetLifeProject(string id, int projectId ) 
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
        public async Task<IActionResult> CreateLifeProject([FromBody] LifeProjectCreateDTO lifeProject) 
        { 
             try
            {
               
                if(lifeProject!= null)
                {

                     var user = await _db.ApplicationUsers.Where( u=> u.Id ==lifeProject.UserID).FirstOrDefaultAsync();
                    if(user== null)
                        return BadRequest();
                    LifeProject project = _mapper.Map<LifeProject>(lifeProject);
                    await _db.LifeProjects.AddAsync(project);
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

        [HttpPut("{projectId:int}")]
        public async Task<IActionResult> EditLifeProject(int projectId ,[FromBody] LifeProjectUpdateDTO lifeProject )
         { 
            try
            {
                LifeProject project = await _db.LifeProjects.Where(u => u.UserID == lifeProject.UserID && u.ProjectId == projectId).FirstOrDefaultAsync();
                if(project!= null && projectId==lifeProject.ProjectId)
                {
                    _db.LifeProjects.Update(_mapper.Map<LifeProjectUpdateDTO, LifeProject>(lifeProject, project));
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
        public async Task<IActionResult> DeleteLifeProject(string id, int projectId ) 
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