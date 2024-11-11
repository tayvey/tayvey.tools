namespace Demo.WebApi.Model.Mapping;

/// <summary>
/// 映射测试实体
/// </summary>
public class MappingTestEntity
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string FirstName { get; set; } = "";

    /// <summary>
    /// 姓氏
    /// </summary>
    public string LastName { get; set; } = "";

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// 性别 1男 2女 其他
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    /// 年龄
    /// </summary>
    /// <returns></returns>
    public int Age()
    {
        var today = DateTime.Today;
        var age = today.Year - BirthDate.Year;

        if (BirthDate.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    /// <summary>
    /// 性别
    /// </summary>
    /// <returns></returns>
    public string GenderStr() => Gender switch
    {
        1 => "男",
        2 => "女",
        _ => "其他"
    };
}