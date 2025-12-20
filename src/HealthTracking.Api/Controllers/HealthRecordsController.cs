using HealthTracking.Api.Data;
using HealthTracking.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthTracking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthRecordsController : ControllerBase
    {
        private readonly HealthTrackingDbContext _context;

        public HealthRecordsController(HealthTrackingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves health records within a specified date range.
        /// </summary>
        /// <remarks>
        /// Use query parameters to filter results:
        /// - <c>startDate</c>: The beginning of the time window (inclusive).
        /// - <c>endDate</c>: The end of the time window (inclusive).
        /// 
        /// Records are returned in ascending order by <c>RecordedAt</c>.
        /// </remarks>
        /// <param name="startDate">
        /// The start date of the time window. Records with <c>RecordedAt</c> greater than or equal to this value are included.
        /// </param>
        /// <param name="endDate">
        /// The end date of the time window. Records with <c>RecordedAt</c> less than or equal to this value are included.
        /// </param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing a collection of <see cref="HealthRecord"/> objects
        /// that fall within the specified date range.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthRecord>>> GetHealthRecords([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {

            return Ok(await _context.HealthRecords
                .Where(r => r.RecordedAt >= startDate && r.RecordedAt <= endDate)
                .OrderBy(r => r.RecordedAt)
                .ToListAsync()
            );
        }

        /// <summary>
        /// Creates a new health record entry.
        /// </summary>
        /// <remarks>
        /// Accepts a <see cref="HealthRecordDto"/> object in the request body, converts it to a
        /// <see cref="HealthRecord"/> entity, and persists it to the database.
        /// 
        /// The response returns <c>201 Created</c> if successful.
        /// </remarks>
        /// <param name="healthRecordDto">
        /// The data transfer object containing the health record details to be created.
        /// Required fields include values such as <c>MedicationType</c>, <c>WeightKg</c>,
        /// <c>BodyFatPercentage</c>, and <c>BodyWaterPercentage</c>. The <c>RecordedAt</c>
        /// property defaults to the current UTC time.
        /// </param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing the created <see cref="HealthRecord"/> entity.
        /// Returns <c>201 Created</c> on success.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<HealthRecord>> PostHealthRecord(HealthRecordDto healthRecordDto)
        {
            _context.HealthRecords.Add(healthRecordDto.ToEntity());
            await _context.SaveChangesAsync();
            return Created();
        }
    }
}
