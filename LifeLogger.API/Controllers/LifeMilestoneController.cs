

using AutoMapper;
using LifeLogger.DataAccess.Data;
using LifeLogger.Models;
using LifeLogger.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifeLogger.API.Controllers
{
    [Route("api/LifeMilestone")]
    [ApiController]
    [Authorize]
    public class LifeMilestoneController:Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public LifeMilestoneController(ApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet("{projectId:int}")]
        public async Task<IActionResult> GetAllLifeMilestones(int projectId) 
        { 
            try
            {
                IEnumerable<LifeMilestone> lifeMilestones = await _db.LifeMilestones.Include(x=> x.LifeIncidents).Where(u => u.ProjectID== projectId ).ToListAsync();
                if(lifeMilestones!= null || lifeMilestones.Count()> 0)
                {
                    IEnumerable<LifeMilestoneResponseDTO> milestones = _mapper.Map<IEnumerable<LifeMilestoneResponseDTO>>(lifeMilestones);
                    foreach (var item in milestones)
                    {
                        item.LifeIncidentCount = lifeMilestones.Where(x => x.MilestoneID == item.MilestoneID).FirstOrDefault().LifeIncidents.Count();
                    }
                    return Ok(milestones);
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

        [HttpGet("{projectId:int}/{lifeMilestoneId:int}")]
        public async Task<IActionResult> GetLifeMilestone(int projectId,int lifeMilestoneId) 
        { 
             try
            {
                LifeMilestone lifeMilestone = await _db.LifeMilestones.Where(u =>  u.ProjectID == projectId && u.MilestoneID== lifeMilestoneId)
                                                            .FirstOrDefaultAsync();
                if(lifeMilestone!= null)
                {
                   
                    return Ok(lifeMilestone);
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
        public async Task<IActionResult> CreateLifeMilestone([FromBody] LifeMilestoneCreateDTO lifeMilestone) 
        { 
             try
            {
                if(lifeMilestone!= null){
                     var project = await _db.LifeProjects.Where( u=> u.ProjectId ==lifeMilestone.ProjectID).FirstOrDefaultAsync();
                    if(project== null)
                        return BadRequest();
                    LifeMilestone milestone = _mapper.Map<LifeMilestone>(lifeMilestone);
                    await _db.LifeMilestones.AddAsync(milestone);
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

        [HttpPut("{projectId:int}/{lifeMilestoneId:int}")]
        public async Task<IActionResult> EditLifeMilestone( int projectId ,int lifeMilestoneId,[FromBody] LifeMilestoneUpdateDTO lifeMilestone )
         { 
            try
            {
                LifeMilestone lifeMileStoneToUpdate = await _db.LifeMilestones.Where(u => u.ProjectID == projectId && u.MilestoneID== lifeMilestone.MilestoneID)
                                                            .FirstOrDefaultAsync();
                if(lifeMileStoneToUpdate!= null && projectId==lifeMilestone.ProjectID)
                {
                    
                    // lifeMileStoneToUpdate.MilestoneName = lifeMilestone.MilestoneName;
                    // lifeMileStoneToUpdate.Date = lifeMilestone.Date;
                    // lifeMileStoneToUpdate.Location = lifeMilestone.Location;
                    // lifeMileStoneToUpdate.Sentiment = lifeMilestone.Sentiment;
                    // lifeMileStoneToUpdate.Description = lifeMilestone.Description;
                    _db.LifeMilestones.Update(_mapper.Map<LifeMilestoneUpdateDTO, LifeMilestone>(lifeMilestone, lifeMileStoneToUpdate));
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

        [HttpDelete("{projectId:int}/{lifeMilestoneId:int}")]
        public async Task<IActionResult> DeleteLifeMilestone( int projectId , int lifeMilestoneId) 
        { 
            try
            {
                LifeMilestone lifeMileStoneToDelete = await _db.LifeMilestones.Where(u => u.ProjectID == projectId && u.MilestoneID== lifeMilestoneId)
                                                            .FirstOrDefaultAsync();
                if(lifeMileStoneToDelete!= null)
                {
                    _db.LifeMilestones.Remove(lifeMileStoneToDelete);
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