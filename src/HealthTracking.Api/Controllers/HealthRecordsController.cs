using AutoMapper;
using HealthTracking.Api.Data;
using HealthTracking.Application.Dtos;
using HealthTracking.Domain;
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
        /// <param name="mapper"> 
        /// The AutoMapper instance used to convert domain entities into 
        /// see cref="HealthRecordReadDto"/> objects for the API response. 
        /// </param>        
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing a collection of 
        /// <see cref="HealthRecordReadDto"/> objects that fall within the specified date range.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthRecordReadDto>>> GetHealthRecords(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromServices] IMapper mapper)
        {

            var records = await _context.HealthRecords
                .Where(r => r.RecordedAt >= startDate && r.RecordedAt <= endDate)
                .OrderBy(r => r.RecordedAt)
                .ToListAsync();

            var dto = mapper.Map<IEnumerable<HealthRecordReadDto>>(records);

            return Ok(dto);
        }

        /// <summary>
        /// Creates a new health record entry.
        /// </summary>
        /// <remarks>
        /// Accepts a <see cref="HealthRecordCreateDto"/> in the request body, maps it to a
        /// <see cref="HealthRecord"/> domain entity, and persists it to the database.
        ///
        /// Returns <c>201 Created</c> along with the created <see cref="HealthRecordReadDto"/>.
        /// </remarks>
        /// <param name="dto">
        /// The data transfer object containing the values required to create a new health record.
        /// Fields include <c>MedicationType</c>, <c>WeightKg</c>, <c>BodyFatPercentage</c>,
        /// and <c>BodyWaterPercentage</c>. The <c>RecordedAt</c> timestamp is assigned by the server.
        /// </param>
        /// <param name="mapper">
        /// The AutoMapper instance used to convert between DTOs and domain entities.
        /// </param>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing the created <see cref="HealthRecordReadDto"/>.
        /// Returns <c>201 Created</c> on success.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<HealthRecord>> PostHealthRecord(
            HealthRecordCreateDto dto,
            [FromServices] IMapper mapper)
        {
            var entity = mapper.Map<HealthRecord>(dto);

            _context.HealthRecords.Add(entity);
            await _context.SaveChangesAsync();

            var readDto = mapper.Map<HealthRecordReadDto>(entity);
            return CreatedAtAction(nameof(GetHealthRecords), new { id = entity.Id }, readDto);
        }
    }
}
