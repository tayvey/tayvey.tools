using System.ComponentModel.DataAnnotations;

namespace Demo.WebApi.Model;

/// <summary>
/// 模型校验入参
/// </summary>
public class ModelStateDemoParam
{
    /// <summary>
    /// 名称
    /// </summary>
    [Display(Name = "名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [StringLength(5, ErrorMessage = "{0}不能超过{1}个字符")]
    public string Name { get; set; } = "";
}