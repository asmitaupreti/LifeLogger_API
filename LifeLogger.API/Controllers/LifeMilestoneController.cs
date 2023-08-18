

using LifeLogger.DataAccess.Data;
using LifeLogger.Models;
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

        public LifeMilestoneController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("{projectId:int}")]
        public async Task<IActionResult> GetAllLifeMilestones(int projectId) 
        { 
            try
            {
                IEnumerable<LifeMilestone> LifeMilestones = await _db.LifeMilestones.Where(u => u.ProjectID== projectId ).ToListAsync();
                if(LifeMilestones!= null && LifeMilestones.Count()> 0)
                {
                    return Ok(LifeMilestones);
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
        public async Task<IActionResult> CreateLifeMilestone([FromBody] LifeMilestone lifeMilestone) 
        { 
             try
            {
                    await _db.LifeMilestones.AddAsync(lifeMilestone);
                    await _db.SaveChangesAsync();
                    return Ok(lifeMilestone);
               
            
            }
            catch (System.Exception)
            {

                return BadRequest();
            }; 
        }

        [HttpPut("{projectId:int}/{lifeMilestoneId:int}")]
        public async Task<IActionResult> EditLifeMilestone( int projectId ,int lifeMilestoneId,[FromBody] LifeMilestone lifeMilestone )
         { 
            try
            {
                LifeMilestone lifeMileStoneToUpdate = await _db.LifeMilestones.Where(u => u.ProjectID == projectId && u.MilestoneID== lifeMilestone.MilestoneID)
                                                            .FirstOrDefaultAsync();
                if(lifeMileStoneToUpdate!= null && projectId==lifeMilestone.ProjectID)
                {
                    lifeMileStoneToUpdate.MilestoneName = lifeMilestone.MilestoneName;
                    lifeMileStoneToUpdate.Date = lifeMilestone.Date;
                    lifeMileStoneToUpdate.Location = lifeMilestone.Location;
                    lifeMileStoneToUpdate.Sentiment = lifeMilestone.Sentiment;
                    lifeMileStoneToUpdate.Description = lifeMilestone.Description;
                    _db.LifeMilestones.Update(lifeMileStoneToUpdate);
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