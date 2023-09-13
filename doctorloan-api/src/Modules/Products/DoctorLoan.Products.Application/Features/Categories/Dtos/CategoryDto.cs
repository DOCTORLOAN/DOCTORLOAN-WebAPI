using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorLoan.Domain.Common;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Products.Application.Features.Categories.Dtos;

public class CategoryDto 
{
    public int Id { get; set; }
    public string Name { get; set; }=string.Empty;
    public int ParentId { get; set; }
    public string MetaTitle { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Sort { get; set; }
    public string Slug { get; set; } = string.Empty;
    public StatusEnum Status { get; set; }
    public string Code { get; set; } = string.Empty;
    public string ParentName { get; internal set; }
    public DateTimeOffset LastModified { get; set; }
}

