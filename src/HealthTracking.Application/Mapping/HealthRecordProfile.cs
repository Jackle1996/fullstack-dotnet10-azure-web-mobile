using AutoMapper;
using HealthTracking.Application.Dtos;
using HealthTracking.Domain;

namespace HealthTracking.Application.Mapping;

public class HealthRecordProfile : Profile
{
    public HealthRecordProfile()
    {
        CreateMap<HealthRecordCreateDto, HealthRecord>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RecordedAt, opt => opt.Ignore());
        CreateMap<HealthRecord, HealthRecordReadDto>();
    }
}
