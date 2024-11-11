using AutoMapper;

namespace Demo.WebApi.Model.Mapping;

/// <summary>
/// 测试映射
/// </summary>
public class TestProfile : Profile
{
    /// <summary>
    /// 初始化
    /// </summary>
    public TestProfile()
    {
        CreateMap<MappingTestEntity, MappingTestDto>()
            .ForMember(dto => dto.Name, opt => opt.MapFrom(entity => $"{entity.FirstName} {entity.LastName}"))
            .ForMember(dto => dto.Age, opt => opt.MapFrom(entity => entity.Age()))
            .ForMember(dto => dto.Gender, opt => opt.MapFrom(entity => entity.GenderStr()));
    }
}