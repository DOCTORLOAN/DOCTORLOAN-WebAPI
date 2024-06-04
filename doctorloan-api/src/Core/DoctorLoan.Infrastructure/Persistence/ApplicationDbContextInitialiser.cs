using BCrypt.Net;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Domain.Entities.Departments;
using DoctorLoan.Domain.Entities.MedicalRecord;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Domain.Entities.Roles;
using DoctorLoan.Domain.Entities.Users;
using DoctorLoan.Domain.Enums.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsNpgsql())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            await TrySeedProductAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.SymptomGroups.Any())
        {
            var listSymptomGroups = new List<SymptomGroups> 
            {
                new SymptomGroups { Name = "Cột sống cổ - Đầu và mặt" },
                new SymptomGroups { Name = "Cột sống cổ - Cổ" },
                new SymptomGroups { Name = "Cột sống cổ - Tay" },
                new SymptomGroups { Name = "Cột sống ngực" },
                new SymptomGroups { Name = "Cột sống lưng" },
            };
            await _context.AddRangeAsync();
            await _context.SaveChangesAsync();
        }

        var _symptomGroupId = await _context.SymptomGroups.Include(s => s.Symptoms).Where(s => s.Name == "Cột sống cổ - Đầu và mặt" || s.Name == "Cột sống cổ - Cổ" || s.Name == "Cột sống cổ - Tay" || s.Name == "Cột sống ngực" || s.Name == "Cột sống lưng").ToListAsync();

        if (_symptomGroupId.Any() && !_context.Symptoms.Any())
        {
            var listSymptom = new List<Symptoms>
                {
                    new Symptoms {
                        Name = "U tai - nghe kém",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Đầu và mặt" )?.Id ?? 1
                    },
                    new Symptoms {
                        Name = "Khó nói",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Đầu và mặt" )?.Id ?? 1
                    },
                    new Symptoms {
                        Name = "Đau đầu - đau nửa đầu",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Đầu và mặt" )?.Id ?? 1
                    },
                    new Symptoms {
                        Name = "Hay quên",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Đầu và mặt" )?.Id ?? 1
                    },
                    new Symptoms {
                        Name = "Chóng mặt - rối loạn tiền đình - choáng váng",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Đầu và mặt" )?.Id ?? 1
                    },
                    new Symptoms {
                        Name = "Giam khả năng tập trung",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Đầu và mặt" )?.Id ?? 1
                    },
                    new Symptoms {
                        Name = "Nhìn mờ - mù thoáng qua",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Đầu và mặt" )?.Id ?? 1
                    },
                    new Symptoms {
                        Name = "Mất ngủ - khó ngủ",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Đầu và mặt" )?.Id ?? 1
                    },

                    new Symptoms {
                        Name = "Đau cổ",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Cổ" )?.Id ?? 2
                    },
                    new Symptoms {
                        Name = "Mỏi cổ",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Cổ" )?.Id ?? 2
                    },
                    new Symptoms {
                        Name = "Khó cử động",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Cổ" )?.Id ?? 2
                    },
                    new Symptoms {
                        Name = "Đau khi cử động cổ",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Cổ" )?.Id ?? 2
                    },

                    new Symptoms {
                        Name = "Cẳng tay",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Đau/tê/buốt",
                        parentId = 13,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Nhức mỏi",
                        parentId = 13,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Khó cử động",
                        parentId = 13,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Xuội/liệt",
                        parentId = 13,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Lạnh/nóng",
                        parentId = 13,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Bị teo",
                        parentId = 13,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Xưng/nóng/đỏ",
                        parentId = 13,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },

                    new Symptoms {
                        Name = "Vai",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Đau/tê/buốt",
                        parentId = 21,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Nhức mỏi",
                        parentId = 21,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Khó cử động",
                        parentId = 21,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Xuội/liệt",
                        parentId = 21,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Lạnh/nóng",
                        parentId = 21,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Bị teo",
                        parentId = 21,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Xưng/nóng/đỏ",
                        parentId = 21,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },

                    new Symptoms {
                        Name = "Cánh tay",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Đau/tê/buốt",
                        parentId = 29,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Nhức mỏi",
                        parentId = 29,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Khó cử động",
                        parentId = 29,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Xuội/liệt",
                        parentId = 29,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Lạnh/nóng",
                        parentId = 29,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Bị teo",
                        parentId = 29,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Xưng/nóng/đỏ",
                        parentId = 29,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },

                    new Symptoms {
                        Name = "Cánh tay",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Đau/tê/buốt",
                        parentId = 37,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Nhức mỏi",
                        parentId = 37,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Khó cử động",
                        parentId = 37,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Xuội/liệt",
                        parentId = 37,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Lạnh/nóng",
                        parentId = 37,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Bị teo",
                        parentId = 37,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },
                    new Symptoms {
                        Name = "Xưng/nóng/đỏ",
                        parentId = 37,
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống cổ - Tay" )?.Id ?? 3
                    },

                    new Symptoms {
                        Name = "Đau thắt lưng/liên sườn",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống ngực" )?.Id ?? 4
                    },
                    new Symptoms {
                        Name = "Khó thở",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống ngực" )?.Id ?? 4
                    },
                    new Symptoms {
                        Name = "Ho",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống ngực" )?.Id ?? 4
                    },
                    new Symptoms {
                        Name = "Thở ngắt quãng",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống ngực" )?.Id ?? 4
                    },
                    new Symptoms {
                        Name = "Rối loại nhịp tim",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống ngực" )?.Id ?? 4
                    },
                    new Symptoms {
                        Name = "Trào ngược dạ dày",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống ngực" )?.Id ?? 4
                    },

                    new Symptoms {
                        Name = "Đau/tê/mỏi Lưng",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống lưng" )?.Id ?? 5
                    },
                    new Symptoms {
                        Name = "Đau/tê/mỏi Mông",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống lưng" )?.Id ?? 5
                    },
                    new Symptoms {
                        Name = "Đau thần kinh tọa",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống lưng" )?.Id ?? 5
                    },
                    new Symptoms {
                        Name = "Đau/mỏi/tê Khớp háng",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống lưng" )?.Id ?? 5
                    },
                    new Symptoms {
                        Name = "Đau đùi",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống lưng" )?.Id ?? 5
                    },
                    new Symptoms {
                        Name = "Đau đầu gối",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống lưng" )?.Id ?? 5
                    },
                    new Symptoms {
                        Name = "Đau cẳng chân",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống lưng" )?.Id ?? 5
                    },
                    new Symptoms {
                        Name = "Đau cổ chân",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống lưng" )?.Id ?? 5
                    },
                    new Symptoms {
                        Name = "Đau gang bàn chân",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống lưng" )?.Id ?? 5
                    },
                    new Symptoms {
                        Name = "Đau gót chân",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống lưng" )?.Id ?? 5
                    },
                    new Symptoms {
                        Name = "Yếu/liệt chân",
                        SymptomGroupId = _symptomGroupId.Find(s=>s.Name =="Cột sống lưng" )?.Id ?? 5
                    },
                };
            await _context.AddRangeAsync(listSymptom);
            await _context.SaveChangesAsync();
        }

        if (!_context.Departments.Any())
        {
            var department = new Department() { Code = "DOCTORLOAN", Name = "DOCTORLOAN Office", OrderBy = 1 };

            if (!_context.Roles.Any())
            {
                var listRoles = new List<Role>
                {
                    new Role() { Code = "admin", Name = "Administrator", IsActive = true },
                    new Role() { Code = "leader", Name = "Leader", IsActive = true },
                    new Role() { Code = "user", Name = "User", IsActive = true }
                };

                department.DepartmentRoles.AddRange(listRoles);
                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();
            }
        }

        var adminRoleId = await _context.Roles.Include(s => s.Users).Where(s => s.Code == "admin" || s.Code == "leader" || s.Code == "user").ToListAsync();
        var hasAdmin = await _context.Users.AnyAsync(s => s.UserName == "admin");
        if (adminRoleId.Any() && !hasAdmin && !_context.Users.Any())
        {
            var testUsers = new List<User>
                {
                    new User {
                        FullName = "Admin",
                        UserName = "doctorloanadmin",
                        Phone = "",
                        Email = "coder.dl@doctorloan.vn",
                        Status= Domain.Enums.Users.UserStatus.Active,
                        SourcePlatform = SourcePlatform.WebAdmin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("giathaidoctorLoan@2023", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                        RoleId = adminRoleId.Find(s=>s.Code =="admin" )?.Id ?? 1
                    },
                    new User {
                        FullName = "Loan Pham",
                        UserName = "loan.pham@doctorloan.vn",
                        Phone = "",
                        Email = "loan.pham@doctorloan.vn",
                        Status= Domain.Enums.Users.UserStatus.Active,
                        SourcePlatform = SourcePlatform.WebAdmin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("giathaidoctorLoan@2023", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                        RoleId = adminRoleId.Find(s=>s.Code =="admin" )?.Id ?? 1
                    },
                    new User {
                        FullName = "Sale",
                        UserName = "cskh@doctorloan.vn",
                        Phone = "",
                        Email = "cskh@doctorloan.vn",
                        Status= Domain.Enums.Users.UserStatus.Active,
                        SourcePlatform = SourcePlatform.WebAdmin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("giathaidoctorLoan@2023", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                        RoleId = adminRoleId.Find(s=>s.Code =="user" )?.Id ?? 3
                    },
                    new User {
                        FullName = "Accountant",
                        UserName = "ketoantruong.dl@doctorloan.vn",
                        Phone = "",
                        Email = "ketoantruong.dl@doctorloan.vn",
                        Status= Domain.Enums.Users.UserStatus.Active,
                        SourcePlatform = SourcePlatform.WebAdmin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("giathaidoctorLoan@2023", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                        RoleId = adminRoleId.Find(s=>s.Code =="leader" )?.Id ?? 2
                    },
                    new User {
                        FullName = "HR",
                        UserName = "tpnhansu.dl@doctorloan.vn",
                        Phone = "",
                        Email = "tpnhansu.dl@doctorloan.vn",
                        Status= Domain.Enums.Users.UserStatus.Active,
                        SourcePlatform = SourcePlatform.WebAdmin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("giathaidoctorLoan@2023", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                        RoleId = adminRoleId.Find(s=>s.Code =="user" )?.Id ?? 3
                    },
                    new User {
                        FullName = "Helpdesk",
                        UserName = "it@doctorloan.vn",
                        Phone = "",
                        Email = "it@doctorloan.vn",
                        Status= Domain.Enums.Users.UserStatus.Active,
                        SourcePlatform = SourcePlatform.WebAdmin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("giathaidoctorLoan@2023", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                        RoleId = adminRoleId.Find(s=>s.Code =="leader" )?.Id ?? 2
                    },
                    new User {
                        FullName = "Thanh Mach",
                        UserName = "thanh.mach@doctorloan.vn",
                        Phone = "",
                        Email = "thanh.mach@doctorloan.vn",
                        Status= Domain.Enums.Users.UserStatus.Active,
                        SourcePlatform = SourcePlatform.WebAdmin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("giathaidoctorLoan@2023", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                        RoleId = adminRoleId.Find(s=>s.Code =="leader" )?.Id ?? 2
                    },
                    new User {
                        FullName = "Coder 2",
                        UserName = "coder2.dl@doctorloan.vn",
                        Phone = "",
                        Email = "coder2.dl@doctorloan.vn",
                        Status= Domain.Enums.Users.UserStatus.Active,
                        SourcePlatform = SourcePlatform.WebAdmin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("giathaidoctorLoan@2023", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                        RoleId = adminRoleId.Find(s=>s.Code =="admin" )?.Id ?? 1
                    }
                };

            await _context.Users.AddRangeAsync(testUsers);
            await _context.SaveChangesAsync();
        }

        if (false)
        {
            var listUserTest = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                listUserTest.Add(new User
                {
                    UUId = Guid.NewGuid(),
                    FullName = "Normal " + i,
                    UserName = "user" + i,
                    Status = Domain.Enums.Users.UserStatus.Active,
                    SourcePlatform = SourcePlatform.WebAdmin,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("giathaidoctorLoan@2023", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                    RoleId = adminRoleId.Find(s => s.Code == "user")?.Id ?? 3
                });
            }

            await _context.Users.AddRangeAsync(listUserTest);
            await _context.SaveChangesAsync();
        }

    }

    public async Task TrySeedProductAsync()
    {
        if (!_context.Categories.Any())
            await _context.Categories.AddAsync(new Domain.Entities.Products.Category{ Name = "Sản phẩm DOCTORLOAN",Slug = "sanpham-doctorloan",Status = StatusEnum.Publish,Sort = 0 });
        await _context.SaveChangesAsync();

        //if (!_context.AttributeGroups.Any())
        //{
        //    var attributeGroup = new List<Domain.Entities.Products.AttributeGroup>
        //    {
        //        new Domain.Entities.Products.AttributeGroup
        //        {
        //            Name = "Kích thước ghế",
        //            Attributes = new List<Domain.Entities.Products.Attribute>
        //            {
        //                new Domain.Entities.Products.Attribute { Name = "Kích thước" },
        //                new Domain.Entities.Products.Attribute { Name = "Khối lượng" }
        //            }
        //        },
        //        new Domain.Entities.Products.AttributeGroup
        //        {
        //            Name = "Thông tin tổng thể",
        //            Attributes = new List < Domain.Entities.Products.Attribute > 
        //            {
        //                new Domain.Entities.Products.Attribute { Name = "Kiểu dáng" },
        //                new Domain.Entities.Products.Attribute { Name = "Chất liệu lõi" },
        //                new Domain.Entities.Products.Attribute { Name = "Chất liệu bọc" },
        //                new Domain.Entities.Products.Attribute { Name = "Công nghệ sản xuất" },
        //                new Domain.Entities.Products.Attribute { Name = "Hiệu quả sử dụng" },
        //                new Domain.Entities.Products.Attribute { Name = "Hướng dẫn sử dụng" },
        //                new Domain.Entities.Products.Attribute { Name = "Bảo hành" },
        //                new Domain.Entities.Products.Attribute { Name = "Năm sản xuất" },
        //                new Domain.Entities.Products.Attribute { Name = "Sản xuất tại" }
        //            }
        //        }
        //    };

        //    await _context.AttributeGroups.AddRangeAsync(attributeGroup);
        //    await _context.SaveChangesAsync();
        //}

        //var _attributeGroupIds = await _context.AttributeGroups.Include(s => s.Attributes).Where(s => s.Name == "Kích thước ghế" || s.Name == "Thông tin tổng thể").ToListAsync();

        //    if (_attributeGroupIds.Any() && !_context.Attributes.Any())
        //    {
        //        var listAttribute = new List<Domain.Entities.Products.Attribute> 
        //        {
        //            new Domain.Entities.Products.Attribute { Name = "Kích thước", AttributeGroup = _attributeGroupIds.Find(s => s.Name == "Kích thước ghế")?.Id ?? 1 },
        //            new Domain.Entities.Products.Attribute { Name = "Khối lượng", AttributeGroupId = _attributeGroupIds.Find(s => s.Name == "Kích thước ghế")?.Id ?? 1 },
        //            new Domain.Entities.Products.Attribute { Name = "Kiểu dáng", AttributeGroupId = _attributeGroupIds.Find(s => s.Name == "Thông tin tổng thể")?.Id ?? 2 },
        //            new Attribute { Name = "Chất liệu lõi", AttributeGroupId = _attributeGroupIds.Find(s => s.Name == "Thông tin tổng thể")?.Id ?? 2 },
        //            new Attribute { Name = "Chất liệu bọc", AttributeGroupId = _attributeGroupIds.Find(s => s.Name == "Thông tin tổng thể")?.Id ?? 2 },
        //            new Attribute { Name = "Công nghệ sản xuất", AttributeGroupId = _attributeGroupIds.Find(s => s.Name == "Thông tin tổng thể")?.Id ?? 2 },
        //            new Attribute { Name = "Hiệu quả sử dụng", AttributeGroupId = _attributeGroupIds.Find(s => s.Name == "Thông tin tổng thể")?.Id ?? 2 },
        //            new Attribute { Name = "Hướng dẫn sử dụng", AttributeGroupId = _attributeGroupIds.Find(s => s.Name == "Thông tin tổng thể")?.Id ?? 2 },
        //            new Attribute { Name = "Bảo hành", AttributeGroupId = _attributeGroupIds.Find(s => s.Name == "Thông tin tổng thể")?.Id ?? 2 },
        //            new Attribute { Name = "Năm sản xuất", AttributeGroupId = _attributeGroupIds.Find(s => s.Name == "Thông tin tổng thể")?.Id ?? 2 },
        //            new Attribute { Name = "Sản xuất tại", AttributeGroupId = _attributeGroupIds.Find(s => s.Name == "Thông tin tổng thể")?.Id ?? 2 },
        //        };

        //    }

        if (!_context.ProductOptionGroups.Any())
        {
            _context.ProductOptionGroups.AddRange(new List<ProductOptionGroup>
            {
                new ProductOptionGroup{Name="Màu sắc"},
                new ProductOptionGroup{Name="Kích cỡ"},
            });
            await _context.SaveChangesAsync();
        }

        if (!_context.Brands.Any())
        {
            _context.Brands.AddRange(new List<Brand>
            {
                new Brand{Name="DOCTORLOAN"}
            });
            await _context.SaveChangesAsync();
        }

        //var _attribute = await _context.Attributes.Include(s => s.Name).Where(s => s.Name == "Kích thước" || 
        //                                                                            s.Name == "Khối lượng" ||
        //                                                                            s.Name == "Kiểu dáng" ||
        //                                                                            s.Name == "Chất liệu lõi" ||
        //                                                                            s.Name == "Công nghệ sản xuất" ||
        //                                                                            s.Name == "Hiệu quả sử dụng" ||
        //                                                                            s.Name == "Hướng dẫn sử dụng" ||
        //                                                                            s.Name == "Bảo hành" ||
        //                                                                            s.Name == "Năm sản xuất" ||
        //                                                                            s.Name == "Sản xuất tại").ToListAsync();
        // var _attributegroups = await _context.Attributes.Include(s => s.Name).Where(s => s.Name == "Kích thước ghế" || 
        //                                                                            s.Name == "Thông tin tổng thể").ToListAsync();

        //var _productCategories = await _context.Categories.Include(s => s.Slug).Where(s => s.Slug == "sanpham-doctorloan").ToListAsync();


        //if (_productCategories.Any() && _attributegroups.Any() && !_context.Products.Any())
        //{
        //    var products = new List<Product>
        //    {
        //        new Product
        //        {
        //           Name = "Ghế sáng chế DOCTORLOAN 135",
        //           Slug = "LC35LESY",
        //           Status = StatusEnum.Publish,
        //           ProductCategories = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1,
        //           BrandId = _context.Brands.FirstOrDefault().Id,
        //           Price = 37400000,
        //           ProductItems = new ProductItem 
        //           {
        //               Name = "Ghế sáng chế DOCTORLOAN 135",
        //               Sku = "LC35LESY",
        //               ProductOptions = new List<ProductOptionGroup>
        //               {
        //                   new ProductOption{ProductOptionGroupId = _context.ProductOptionGroups.FirstOrDefault().Id,Name="Màu nâu"},
        //               },
        //           },
        //           ProductAttributes = new List<ProductAttribute>
        //           {
        //               new Attribute 
        //               {
        //                   AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
        //                   Value = "Kích thước 135" 
        //               },
        //               new Attribute
        //               {
        //                    AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
        //                    Value = "10kg"
        //                  },
        //               new Attribute
        //               {
        //                    AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
        //                    Value = "Ghế nằm"
        //               },
        //               new Attribute
        //               {
        //                   AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
        //                   Value = "Gỗ tự nhiên"
        //               }
        //           },
        //           ProductDetails = new ProductDetail
        //           {
        //               Description = "ghế văn phòng",
        //               shortdescription = "ghế văn phòng",
        //               metatitle = "ghế văn phòng",
        //               metadescription = "ghế văn phòng",
        //               metakeywords = "ghế văn phòng",
        //               att = new List<Attribute>
        //               {
        //                   new Attribute{attributeid = _context.AttributeGroups.FirstOrDefault().Attributes.FirstOrDefault().Id,value=""},
        //                   new Attribute{attributeid = _context.AttributeGroups.FirstOrDefault().Attributes.FirstOrDefault().Id,value="10kg"},
        //                   new Attribute{attributeid = _context.AttributeGroups.FirstOrDefault().Attributes.FirstOrDefault().Id,value="kiểu dáng"},
        //                   new Attribute{attributeid = _context.AttributeGroups.FirstOrDefault().Attributes.FirstOrDefault().Id,value="chất liệu lõi"},
        //                   new Attribute{attributeid = _context.AttributeGroups.FirstOrDefault().Attributes.FirstOrDefault().Id,value="chất liệu bọc"},
        //                   new Attribute{ attributeid = _context.AttributeGroups.FirstOrDefault().Attributes.FirstOrDefault().Id, value = "công nghệ sản xuất" },
        //                   new Attribute{ attributeid = _context.AttributeGroups.FirstOrDefault().Attributes.FirstOrDefault().Id, value = "hiệu quả sử dụng" },
        //                   new attributevalue{attributeid = _context.attributegroups.last().attributes.skip(5).firstordefault().id,value="hướng dẫn sử dụng"},
        //                   new attributevalue{attributeid = _context.attributegroups.last().attributes.skip(6).firstordefault().id,value="bảo hành"},
        //                   new attributevalue{attributeid = _context.attributegroups.last().attributes.skip(7).firstordefault().id,value="2023"},
        //                   new attributevalue{attributeid = _context.attributegroups.last().attributes.skip(8).firstordefault().id,value="việt nam"},
        //               },
        //               productoptionvalues = new list<productoptionvalue>
        //               {
        //                   new productoptionvalue{productoptiongroupid = _context.productoptiongroups.firstordefault().id,value="đỏ"},
        //                   new productoptionvalue{productoptiongroupid = _context.productoptiongroups.last().id,value="135"},
        //               }
        //           },
        //           metatitle = "ghế văn phòng",
        //           metadescription = "ghế văn phòng",
        //           metakeywords = "ghế văn phòng",
        //        }
        //    };
        //    await _context.producttypes.addasync(new producttype
        //    {
        //        name = "ghế",
        //        slug = "ghe",
        //        status = statusenum.publish,
        //        sort = 0,
        //        categoryid = _context.categories.firstordefault().id
        //    });
        //    await _context.savechangesasync();
        //}
    }
}
