using AutoMapper;
using LifeLogger.DataAccess.Data;
using LifeLogger.Models;
using LifeLogger.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LifeLogger.API.Controllers
{
     [Route("api/LifeIncident")]
    [ApiController]
    [Authorize]
    public class LifeIncidentController:Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public LifeIncidentController(ApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet("{milestoneId:int}")]
        public async Task<IActionResult> GetAllLifeIncident(int milestoneId) 
        { 
            try
            {
                IEnumerable<LifeIncident> lifeIncident = await _db.LifeIncidents.Where(u => u.MilestoneID == milestoneId).ToListAsync();
                if(lifeIncident!= null || lifeIncident.Count()> 0)
                { 
                    IEnumerable<LifeIncidentResponseDTO> incidents = _mapper.Map<IEnumerable<LifeIncidentResponseDTO>>(lifeIncident);

                    // foreach (var item in projects)
                    // {
                    //     item.LifeMileStoneCount = lifeProjects.Where(x => x.ProjectId == item.ProjectId).FirstOrDefault().LifeMilestones.Count();
                    // }
                    
                    return Ok(incidents);
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

        [HttpGet("{milestoneId:int}/{incidentId:int}")]
        public async Task<IActionResult> GetLifeIncident(int milestoneId, int incidentId ) 
        { 
             try
            {
                LifeIncident lifeIncident = await _db.LifeIncidents.Where(u => u.MilestoneID == milestoneId && u.IncidentID == incidentId).FirstOrDefaultAsync();
                if(lifeIncident!= null)
                {
                   
                    return Ok(lifeIncident);
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
        public async Task<IActionResult> CreateLifeIncident([FromBody] LifeIncidentCreateDTO lifeIncident) 
        { 
              try
            {
                if(lifeIncident!= null){
                     var lifeMilestone = await _db.LifeMilestones.Where( u=> u.MilestoneID ==lifeIncident.MilestoneID).FirstOrDefaultAsync();
                    if(lifeMilestone== null)
                        return BadRequest();
                    LifeIncident incident = _mapper.Map<LifeIncident>(lifeIncident);
                    await _db.LifeIncidents.AddAsync(incident);
                    await _db.SaveChangesAsync();     
                    return Ok();
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

        [HttpPut("{milestoneId:int}/{incidentId:int}")]
        public async Task<IActionResult> EditLifeIncident( int milestoneId ,int incidentId,[FromBody] LifeIncidentUpdateDTO lifeIncident )
         { 
            try
            {
                LifeIncident lifeIncidentToUpdate = await _db.LifeIncidents.Where(u => u.MilestoneID == milestoneId && u.IncidentID== lifeIncident.IncidentID)
                                                            .FirstOrDefaultAsync();
                if(lifeIncidentToUpdate!= null && milestoneId==lifeIncident.MilestoneID)
                {
                    
                    // lifeMileStoneToUpdate.MilestoneName = lifeMilestone.MilestoneName;
                    // lifeMileStoneToUpdate.Date = lifeMilestone.Date;
                    // lifeMileStoneToUpdate.Location = lifeMilestone.Location;
                    // lifeMileStoneToUpdate.Sentiment = lifeMilestone.Sentiment;
                    // lifeMileStoneToUpdate.Description = lifeMilestone.Description;
                    _db.LifeIncidents.Update(_mapper.Map<LifeIncidentUpdateDTO, LifeIncident>(lifeIncident, lifeIncidentToUpdate));
                    await _db.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex);
            }; 
         }

        [HttpDelete("{lifeMilestoneId:int}/{incidentId:int}")]
        public async Task<IActionResult> DeleteLifeIncident( int lifeMilestoneId , int incidentId) 
        { 
            try
            {
                LifeIncident lifeIncidentToDelete = await _db.LifeIncidents.Where(u => u.MilestoneID == lifeMilestoneId && u.IncidentID== incidentId)
                                                            .FirstOrDefaultAsync();
                if(lifeIncidentToDelete!= null)
                {
                    _db.LifeIncidents.Remove(lifeIncidentToDelete);
                    await _db.SaveChangesAsync();
                    return Ok();
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