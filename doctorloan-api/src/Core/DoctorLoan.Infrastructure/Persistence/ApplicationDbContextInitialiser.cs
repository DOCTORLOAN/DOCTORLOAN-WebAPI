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
            await _context.AddRangeAsync(listSymptomGroups);
            await _context.SaveChangesAsync();
        }

        var _symptomGroupId = await _context.SymptomGroups
            .Where(s => s.Name == "Cột sống cổ - Đầu và mặt" || s.Name == "Cột sống cổ - Cổ" || s.Name == "Cột sống cổ - Tay" || s.Name == "Cột sống ngực" || s.Name == "Cột sống lưng")
            .ToListAsync();

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
        {
            var listCategories = new List<Category>
            {
                new Category { Name = "Sản phẩm DOCTORLOAN", Slug = "sanpham-doctorloan", Status = StatusEnum.Publish, Sort = 0 },
                new Category { Name = "Sản phẩm BestSaller", Slug = "bestsaller-doctorloan", Status = StatusEnum.Publish, Sort = 1 },
                new Category { Name = "Ghế nằm sáng chế DOCTORLOAN", Slug = "ghe-nam-doctorloan", Status = StatusEnum.Publish, ParentId = 2, Sort = 0 },
                new Category { Name = "Ghế ngồi sáng chế DOCTORLOAN 95 ", Slug = "ghe-ngoi-95-doctorloan", Status = StatusEnum.Publish, ParentId = 2, Sort = 1 },
                new Category { Name = "Ghế ngồi sáng chế DOCTORLOAN 90D ", Slug = "ghe-ngoi-90d-doctorloan", Status = StatusEnum.Publish, ParentId = 2, Sort = 2 },
                new Category { Name = "Ghế ngồi sáng chế DOCTORLOAN 90T ", Slug = "ghe-ngoi-90t-doctorloan", Status = StatusEnum.Publish, ParentId = 2, Sort = 3 },
                new Category { Name = "Ghế ngồi sáng chế DOCTORLOAN N85 ", Slug = "ghe-ngoi-n85-doctorloan", Status = StatusEnum.Publish, ParentId = 2, Sort = 4 },
                new Category { Name = "Gối cổ sáng chế DOCTORLOAN ", Slug = "goi-co-doctorloan", Status = StatusEnum.Publish, ParentId = 2, Sort = 5 },
                new Category { Name = "Gối lưng sáng chế DOCTORLOAN ", Slug = "goi-lung-doctorloan", Status = StatusEnum.Publish, ParentId = 2, Sort = 6 },
                new Category { Name = "Đệm thiền sáng chế DOCTORLOAN ", Slug = "dem-thien-doctorloan", Status = StatusEnum.Publish, ParentId = 2, Sort = 7 },
                new Category { Name = "Gối DOCTORLOAN khác", Slug = "goi-khac-doctorloan", Status = StatusEnum.Publish, ParentId = 2, Sort = 8 },
            };
            await _context.Categories.AddRangeAsync(listCategories);
            await _context.SaveChangesAsync();
        }

        if (!_context.AttributeGroups.Any())
        {
            var attributeGroup = new List<Domain.Entities.Products.AttributeGroup>
            {
                new Domain.Entities.Products.AttributeGroup
                {
                    Name = "Kích thước ghế",
                    Attributes = new List<Domain.Entities.Products.Attribute>
                    {
                        new Domain.Entities.Products.Attribute { Name = "Kích thước" },
                        new Domain.Entities.Products.Attribute { Name = "Khối lượng" }
                    }
                },
                new Domain.Entities.Products.AttributeGroup
                {
                    Name = "Thông tin tổng thể",
                    Attributes = new List < Domain.Entities.Products.Attribute >
                    {
                        new Domain.Entities.Products.Attribute { Name = "Kiểu dáng" },
                        new Domain.Entities.Products.Attribute { Name = "Chất liệu lõi" },
                        new Domain.Entities.Products.Attribute { Name = "Chất liệu bọc" },
                        new Domain.Entities.Products.Attribute { Name = "Công nghệ sản xuất" },
                        new Domain.Entities.Products.Attribute { Name = "Hiệu quả sử dụng" },
                        new Domain.Entities.Products.Attribute { Name = "Hướng dẫn sử dụng" },
                        new Domain.Entities.Products.Attribute { Name = "Bảo hành" },
                        new Domain.Entities.Products.Attribute { Name = "Năm sản xuất" },
                        new Domain.Entities.Products.Attribute { Name = "Sản xuất tại" }
                    }
                }
            };

            await _context.AttributeGroups.AddRangeAsync(attributeGroup);
            await _context.SaveChangesAsync();
        }

        if (!_context.ProductOptionGroups.Any())
        {
            _context.ProductOptionGroups.AddRange(new List<ProductOptionGroup>
            {
                new ProductOptionGroup{Name="Màu sắc"},
                new ProductOptionGroup{Name="Kích cỡ"},
                new ProductOptionGroup{Name="Cấu trúc"},
                new ProductOptionGroup{Name="Phiên bản"},
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

        var _attribute = await _context.Attributes
            .Where(s => s.Name == "Kích thước" || s.Name == "Khối lượng" ||
                s.Name == "Kiểu dáng" || s.Name == "Chất liệu lõi" ||
                s.Name == "Chất liệu bọc" || s.Name == "Công nghệ sản xuất" ||
                s.Name == "Hiệu quả sử dụng" || s.Name == "Hướng dẫn sử dụng" ||
                s.Name == "Bảo hành" || s.Name == "Năm sản xuất" ||
                s.Name == "Sản xuất tại")
            .ToListAsync();

        var _productCategories = await _context.Categories
            .Where(c => c.Slug == "sanpham-doctorloan" || c.Slug == "bestsaller-doctorloan" ||
                        c.Slug == "ghe-nam-doctorloan" || c.Slug == "ghe-ngoi-95-doctorloan" ||
                        c.Slug == "ghe-ngoi-90d-doctorloan" || c.Slug == "ghe-ngoi-90t-doctorloan" ||
                        c.Slug == "ghe-ngoi-n85-doctorloan" || c.Slug == "goi-co-doctorloan" ||
                        c.Slug == "goi-lung-doctorloan" || c.Slug == "dem-thien-doctorloan" ||
                        c.Slug == "goi-khac-doctorloan")
            .ToListAsync();

        if (_productCategories.Any() && _context.Brands.Any() && !_context.Products.Any())
        {
            var products = new List<DoctorLoan.Domain.Entities.Products.Product>
            {
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Ghế sáng chế DOCTORLOAN 135",
                   Sku = "LC35LESY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 37400000,
                   Quantity = 100,
                   Slug = "ghesangche-doctorloan-135",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "143 x 68 x 102 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "21 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Ghế nằm"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "Composite"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                           Value = "Simili"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "Composite"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "10 Năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                          new DoctorLoan.Domain.Entities.Products.ProductCategory
                          {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1,
                          },
                          new DoctorLoan.Domain.Entities.Products.ProductCategory
                          {
                              CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2,
                          },
                          new DoctorLoan.Domain.Entities.Products.ProductCategory
                          {
                            CategoryId = _productCategories.Find(s => s.Slug == "ghe-nam-doctorloan")?.Id ?? 3,
                          }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Ghế sáng chế DOCTORLOAN 95",
                   Sku = "SC95VESY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 27300000,
                   Quantity = 100,
                   Slug = "ghesangche-doctorloan-95",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "67 x 48 x [106-111] (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "15 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Ghế ngồi"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "Composite"
                       },
                      new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                           Value = "Simili"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "Composite"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "10 Năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "ghe-ngoi-95-doctorloan")?.Id ?? 4
                        }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Ghế sáng chế DOCTORLOAN 90D",
                   Sku = "SC90DESY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 24300000,
                   Quantity = 100,
                   Slug = "ghesangche-doctorloan-90d",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "55 x 46 x [100-105] (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "15 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Ghế ngồi"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "Composite"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                           Value = "Simili"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "Composite"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "10 Năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "ghe-ngoi-90d-doctorloan")?.Id ?? 5
                        }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Ghế sáng chế DOCTORLOAN 90T",
                   Sku = "SC90TESY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 17300000,
                   Quantity = 100,
                   Slug = "ghesangche-doctorloan-90t",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "50 x 46 x [103-108] (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "10,5 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Ghế ngồi"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "Composite"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                           Value = "Simili"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "Composite"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "10 Năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "ghe-ngoi-90t-doctorloan")?.Id ?? 6
                        }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Ghế sáng chế DOCTORLOAN N85",
                   Sku = "PL85SFOR",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 1320000,
                   Quantity = 100,
                   Slug = "ghesangche-doctorloan-n85",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = " 44 x 38 x [74, 83] (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "3.5 kg "
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Ghế ngồi"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "Nhựa PP 100%"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Không"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "Đúc khuôn"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "3 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "ghe-ngoi-n85-doctorloan")?.Id ?? 7
                        }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ sáng chế DOCTORLOAN F4",
                   Sku = "NP04SEFY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 5300000,
                   Quantity = 100,
                   Slug = "goicosangche-f4",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "60 x 40 x 9/17 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "1,3 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Gối cổ"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Vải kate"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "goi-co-doctorloan")?.Id ?? 8
                        }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ sáng chế DOCTORLOAN F5",
                   Sku = "NP05SEFY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 3000000,
                   Quantity = 100,
                   Slug = "goicosangche-f5",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "40 x 41 x 9/13 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "0,7 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Gối cổ"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Vải kate"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        },
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ sáng chế DOCTORLOAN F6",
                   Sku = "NP06SEFY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 4900000,
                   Quantity = 100,
                   Slug = "goicosangche-f6",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "50 x 50 x 9/16 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "0,5 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Gối cổ"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Vải kate"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        },
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Đệm thiền sáng chế DOCTORLOAN",
                   Sku = "SDNLSESY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 4480000,
                   Quantity = 100,
                   Slug = "demthien-doctorloan",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "47 x 50 x 14 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "1,3kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Đệm ngồi"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Simili"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                        CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "dem-thien-doctorloan")?.Id ?? 10
                        }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ sơ sinh DOCTORLOAN",
                   Sku = "NP00OEFY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 300000,
                   Quantity = 100,
                   Slug = "goicososinh-doctorloan",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "40 x 26.5 x 4.5/6 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "0,4 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Gối cổ"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Vải kate"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ trẻ em DOCTORLOAN 2/6",
                   Sku = "NP26OEFY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 800000,
                   Quantity = 100,
                   Slug = "goicotreem-doctorloan-26",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "60 x 26 x 5.5/8.0 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "0,4 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Gối cổ"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Vải kate"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ trẻ em DOCTORLOAN 6/10",
                   Sku = "NP61OEFY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 1000000,
                   Quantity = 100,
                   Slug = "goicotreem-doctorloan-61",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "60 x 26 x 6.5/ 8.0 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "0,6 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Gối cổ"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Vải kate"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối lưng sáng chế DOCTORLOAN F3/C",
                   Sku = "BP03CEFY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 2600000,
                   Quantity = 100,
                   Slug = "goilung-doctorloan-f3c",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "48 x 25 x 12 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "0,6 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Gối cổ"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Vải kate"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "goi-lung-doctorloan")?.Id ?? 9
                        }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối lưng sáng chế DOCTORLOAN F1",
                   Sku = "BP01LEFY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 1000000,
                   Quantity = 100,
                   Slug = "goilung-doctorloan-f1",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "40 x 15 x 7 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "0,4 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Gối cổ"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Vải kate"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ người lớn DOCTORLOAN",
                   Sku = "NPAUNEFY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 2300000,
                   Quantity = 100,
                   Slug = "goiconguoilon",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "60 x 42 x 9 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "1 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Gối cổ"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Vải kate"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ du lịch DOCTORLOAN",
                   Sku = "NPTLSEFY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 1500000,
                   Quantity = 100,
                   Slug = "goicodulich",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "40cm x 27cm x 9cm"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "0.4 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Gối cổ"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Vải kate"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        },
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ đi xe DOCTORLOAN",
                   Sku = "NPCROEFY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 600000,
                   Quantity = 100,
                   Slug = "goicodixe",
                   ProductAttributes = new List<DoctorLoan.Domain.Entities.Products.ProductAttribute>
                   {
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                           AttributeId = _attribute.Find(s => s.Name == "Kích thước")?.Id ?? 1,
                           Value = "30 x 25 x 6,5 (cm)"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Khối lượng")?.Id ?? 2,
                            Value = "0,3 kg"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Kiểu dáng")?.Id ?? 3,
                            Value = "Gối cổ"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu lõi")?.Id ?? 4,
                            Value = "PU foam"
                       },new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                            AttributeId = _attribute.Find(s => s.Name == "Chất liệu bọc")?.Id ?? 5,
                            Value = "Vải kate"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Công nghệ sản xuất")?.Id ?? 6,
                             Value = "PU"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hiệu quả sử dụng")?.Id ?? 7,
                             Value = "Bảo vệ cột sống"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                             AttributeId = _attribute.Find(s => s.Name == "Hướng dẫn sử dụng")?.Id ?? 8,
                             Value = "Đọc kỹ hướng dẫn sử dụng trước khi sử dụng"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Bảo hành")?.Id ?? 9,
                              Value = "1 năm"
                        },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Năm sản xuất")?.Id ?? 10,
                              Value = "2023"
                       },
                       new DoctorLoan.Domain.Entities.Products.ProductAttribute
                       {
                              AttributeId = _attribute.Find(s => s.Name == "Sản xuất tại")?.Id ?? 11,
                              Value = "Việt Nam"
                       }
                   },
                   ProductCategories = new List<DoctorLoan.Domain.Entities.Products.ProductCategory>
                   {
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "bestsaller-doctorloan")?.Id ?? 2
                        },
                        new DoctorLoan.Domain.Entities.Products.ProductCategory
                        {
                            CategoryId = _productCategories.Find(s => s.Slug == "goi-khac-doctorloan")?.Id ?? 11
                        }
                   },
                },
            };
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
        }

        var _product = _context.Products
            .Where(s => s.Sku == "LC35LESY" ||
                        s.Sku == "SC95VESY" || s.Sku == "SC90DESY" || s.Sku == "SC90TESY" || s.Sku == "PL85SFOR" ||
                        s.Sku == "NP04SEFY" || s.Sku == "NP05SEFY" || s.Sku == "NP06SEFY" || s.Sku == "SDNLSESY" ||
                        s.Sku == "NP00OEFY" || s.Sku == "NP26OEFY" || s.Sku == "NP61OEFY" || s.Sku == "BP03CEFY" ||
                        s.Sku == "BP01LEFY" || s.Sku == "NPAUNEFY" || s.Sku == "NPTLSEFY" || s.Sku == "NPCROEFY")
            .ToList();

        if (_product.Any() && !_context.ProductItems.Any())
        {
            var productItems = new List<DoctorLoan.Domain.Entities.Products.ProductItem>
            {
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "LC35LESY")?.Id ?? 1, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 135/B", Sku = "LC35LESB", Price = 37400000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "LC35LESY")?.Id ?? 1, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 135/P", Sku = "LC35LESP", Price = 37400000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "LC35LESY")?.Id ?? 1, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 135/G", Sku = "LC35LESG", Price = 37400000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "LC35LESY")?.Id ?? 1, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 135/Y", Sku = "LC35LESY", Price = 37400000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC95VESY")?.Id ?? 2, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 95/B", Sku = "SC95VESB", Price = 27300000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC95VESY")?.Id ?? 2, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 95/P", Sku = "SC95VESP", Price = 27300000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC95VESY")?.Id ?? 2, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 95/G", Sku = "SC95VESG", Price = 27300000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC95VESY")?.Id ?? 2, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 95/Y", Sku = "SC95VESY", Price = 27300000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC90DESY")?.Id ?? 3, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 90D/B", Sku = "SC90DESB", Price = 24300000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC90DESY")?.Id ?? 3, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 90D/P", Sku = "SC90DESP", Price = 24300000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC90DESY")?.Id ?? 3, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 90D/G", Sku = "SC90DESG", Price = 24300000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC90DESY")?.Id ?? 3, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 90D/Y", Sku = "SC90DESY", Price = 24300000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC90TESY")?.Id ?? 4, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 90T/B", Sku = "SC90TESB", Price = 13700000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC90TESY")?.Id ?? 4, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 90T/P", Sku = "SC90TESP", Price = 13700000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC90TESY")?.Id ?? 4, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 90T/G", Sku = "SC90TESG", Price = 13700000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SC90TESY")?.Id ?? 4, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN 90T/Y", Sku = "SC90TESY", Price = 13700000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-S/OR", Sku = "PL85SFOR", Price = 1320000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-S/RE", Sku = "PL85SFRE", Price = 1320000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-S/YL", Sku = "PL85SFYL", Price = 1320000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-S/GR", Sku = "PL85SFGR", Price = 1320000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-S/BL", Sku = "PL85SFBL", Price = 1320000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-S/GE", Sku = "PL85SFGE", Price = 1320000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-L/OR", Sku = "PL85LFOR", Price = 1430000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-L/RE", Sku = "PL85LFRE", Price = 1430000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-L/YL", Sku = "PL85LFYL", Price = 1430000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-L/GR", Sku = "PL85LFGR", Price = 1430000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-L/BL", Sku = "PL85LFBL", Price = 1430000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5, Quantity = 25, Name = "Ghế sáng chế DOCTORLOAN N85-L/GE", Sku = "PL85LFGE", Price = 1430000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "NP04SEFY")?.Id ?? 6, Quantity = 50, Name = "Gối cổ sáng chế DOCTORLOAN F4/09", Sku = "NP04SEFY", Price = 5300000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "NP04SEFY")?.Id ?? 6, Quantity = 50, Name = "Gối cổ sáng chế DOCTORLOAN F4/12", Sku = "NP04MEFY", Price = 5500000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "NP05SEFY")?.Id ?? 7, Quantity = 100, Name = "Gối cổ sáng chế DOCTORLOAN F5/S", Sku = "NP05SEFY", Price = 3000000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "NP06SEFY")?.Id ?? 8, Quantity = 100, Name = "Gối cổ sáng chế DOCTORLOAN F6/09", Sku = "NP06SEFY", Price = 4900000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SDNLSESY")?.Id ?? 9, Quantity = 50, Name = "Đệm thiền sáng chế DOCTORLOAN/S", Sku = "SDNLSESY", Price = 4480000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "SDNLSESY")?.Id ?? 9, Quantity = 50, Name = "Đệm thiền sáng chế DOCTORLOAN/M", Sku = "SDNLMESY", Price = 4760000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "NP00OEFY")?.Id ?? 10, Quantity = 100, Name = "Gối cổ sơ sinh DOCTORLOAN", Sku = "NP00OEFY", Price = 300000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "NP26OEFY")?.Id ?? 11, Quantity = 100, Name = "Gối cổ trẻ em DOCTORLOAN 2/6", Sku = "NP26OEFY", Price = 800000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "NP61OEFY")?.Id ?? 12, Quantity = 100, Name = "Gối cổ trẻ em DOCTORLOAN 6/10", Sku = "NP61OEFY", Price = 1000000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "BP03CEFY")?.Id ?? 13, Quantity = 100, Name = "Gối lưng sáng chế DOCTORLOAN F3/C", Sku = "BP03CEFY", Price = 2600000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "BP01LEFY")?.Id ?? 14, Quantity = 500, Name = "Gối lưng sáng chế DOCTORLOAN F1/S", Sku = "BP01LEFY", Price = 2000000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "BP01LEFY")?.Id ?? 14, Quantity = 50, Name = "Gối lưng sáng chế DOCTORLOAN F1/L", Sku = "BP01SEFY", Price = 1000000, },

                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "NPAUNEFY")?.Id ?? 15, Quantity = 50, Name = "Gối cổ người lớn", Sku = "NPAUNEFY", Price = 2300000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "NPTLSEFY")?.Id ?? 16, Quantity = 50, Name = "Gối cổ du lịch/09", Sku = "NPTLSEFY", Price = 1500000, },
                new ProductItem { Available = 0, ProductId = _product.Find(s => s.Sku == "NPCROEFY")?.Id ?? 17, Quantity = 50, Name = "Gối cổ đi xe", Sku = "NPCROEFY", Price = 600000, },
            };
            await _context.ProductItems.AddRangeAsync(productItems);
            await _context.SaveChangesAsync();
        }

        var _productItems = _context.ProductItems
            .Where(s => s.Sku == "LC35LESB" || s.Sku == "LC35LESP" || s.Sku == "LC35LESG" || s.Sku == "LC35LESY" ||
                        s.Sku == "SC95VESB" || s.Sku == "SC95VESP" || s.Sku == "SC95VESG" || s.Sku == "SC95VESY" ||
                        s.Sku == "SC90DESB" || s.Sku == "SC90DESP" || s.Sku == "SC90DESG" || s.Sku == "SC90DESY" ||
                        s.Sku == "SC90TESB" || s.Sku == "SC90TESP" || s.Sku == "SC90TESG" || s.Sku == "SC90TESY" ||
                        s.Sku == "PL85SFOR" || s.Sku == "PL85SFRE" || s.Sku == "PL85SFYL" || s.Sku == "PL85SFGR" || s.Sku == "PL85SFBL" || s.Sku == "PL85SFGE" ||
                        s.Sku == "PL85LFOR" || s.Sku == "PL85LFRE" || s.Sku == "PL85LFYL" || s.Sku == "PL85LFGR" || s.Sku == "PL85LFBL" || s.Sku == "PL85LFGE" ||
                        s.Sku == "NP04SEFY" || s.Sku == "NP04MEFY" ||
                        s.Sku == "NP05SEFY" ||
                        s.Sku == "NP06SEFY" ||
                        s.Sku == "SDNLSESY" || s.Sku == "SDNLMESY" ||
                        s.Sku == "NP00OEFY" || s.Sku == "NP26OEFY" || s.Sku == "NP61OEFY" ||
                        s.Sku == "BP03CEFY" ||
                        s.Sku == "BP01LEFY" || s.Sku == "BP01SEFY" ||
                        s.Sku == "NPAUNEFY" || s.Sku == "NPTLSEFY" || s.Sku == "NPCROEFY")
            .ToList();

        var _productOptionGroup = await _context.ProductOptionGroups
            .Where(s => s.Name == "Màu sắc" || s.Name == "Kích thước" || s.Name == "Cấu trúc" || s.Name == "Phiên bản")
            .ToListAsync();

        if (_productItems.Any() && _productOptionGroup.Any() && !_context.ProductOptions.Any())
        {
            var productOptions = new List<ProductOption>
            {
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "LC35LESB")?.Id ?? 1,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu đen",
                    DisplayValue = "Màu đen",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "LC35LESP")?.Id ?? 1,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu nho",
                    DisplayValue = "Màu nho",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "LC35LESG")?.Id ?? 1,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu xanh",
                    DisplayValue = "Màu xanh",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "LC35LESY")?.Id ?? 1,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu vàng",
                    DisplayValue = "Màu vàng",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC95VESB")?.Id ?? 2,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu đen",
                    DisplayValue = "Màu đen",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC95VESP")?.Id ?? 2,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu nho",
                    DisplayValue = "Màu nho",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC95VESG")?.Id ?? 2,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu xanh",
                    DisplayValue = "Màu xanh",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC95VESY")?.Id ?? 2,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu vàng",
                    DisplayValue = "Màu vàng",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC90DESB")?.Id ?? 3,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu đen",
                    DisplayValue = "Màu đen",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC90DESP")?.Id ?? 3,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu nho",
                    DisplayValue = "Màu nho",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC90DESG")?.Id ?? 3,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu xanh",
                    DisplayValue = "Màu xanh",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC90DESY")?.Id ?? 3,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu vàng",
                    DisplayValue = "Màu vàng",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC90TESB")?.Id ?? 4,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu đen",
                    DisplayValue = "Màu đen",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC90TESP")?.Id ?? 4,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu nho",
                    DisplayValue = "Màu nho",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC90TESG")?.Id ?? 4,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu xanh",
                    DisplayValue = "Màu xanh",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SC90TESY")?.Id ?? 4,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu vàng",
                    DisplayValue = "Màu vàng",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu cam",
                    DisplayValue = "Màu cam",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân ngắn",
                    DisplayValue = "Chân ngắn",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFRE")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu đỏ",
                    DisplayValue = "Màu đỏ",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFRE")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân ngắn",
                    DisplayValue = "Chân ngắn",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFYL")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu vàng",
                    DisplayValue = "Màu vàng",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFYL")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân ngắn",
                    DisplayValue = "Chân ngắn",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFGR")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu xanh lá",
                    DisplayValue = "Màu xanh lá",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFGR")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân ngắn",
                    DisplayValue = "Chân ngắn",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFBL")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu xanh biển",
                    DisplayValue = "Màu xanh biển",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFBL")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân ngắn",
                    DisplayValue = "Chân ngắn",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFGE")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu xám",
                    DisplayValue = "Màu xám",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85SFGE")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân ngắn",
                    DisplayValue = "Chân ngắn",
                },

                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFOR")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu cam",
                    DisplayValue = "Màu cam",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFOR")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân dài",
                    DisplayValue = "Chân dài",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFRE")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu đỏ",
                    DisplayValue = "Màu đỏ",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFRE")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân dài",
                    DisplayValue = "Chân dài",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFYL")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu vàng",
                    DisplayValue = "Màu vàng",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFYL")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân dài",
                    DisplayValue = "Chân dài",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFGR")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu xanh lá",
                    DisplayValue = "Màu xanh lá",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFGR")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân dài",
                    DisplayValue = "Chân dài",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFBL")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu xanh biển",
                    DisplayValue = "Màu xanh biển",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFBL")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân dài",
                    DisplayValue = "Chân dài",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFGE")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Màu sắc")?.Id ?? 1,
                    Name = "Màu xám",
                    DisplayValue = "Màu xám",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "PL85LFGE")?.Id ?? 5,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Chân dài",
                    DisplayValue = "Chân dài",
                },

                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "NP04SEFY")?.Id ?? 6,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "9 cm",
                    DisplayValue = "9 cm",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "NP04MEFY")?.Id ?? 6,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "12 cm",
                    DisplayValue = "12 cm",
                },

                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "NP05SEFY")?.Id ?? 7,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Cấu trúc")?.Id ?? 3,
                    Name = "Mềm",
                    DisplayValue = "Mềm",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "NP06SEFY")?.Id ?? 8,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "9 cm",
                    DisplayValue = "9 cm",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SDNLSESY")?.Id ?? 9,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Size nhỏ",
                    DisplayValue = "Size nhỏ",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "SDNLMESY")?.Id ?? 9,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Size lớn",
                    DisplayValue = "Size lớn",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "NP00OEFY")?.Id ?? 10,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Phiên bản")?.Id ?? 4,
                    Name = "Sơ sinh",
                    DisplayValue = "Sơ sinh",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "NP26OEFY")?.Id ?? 11,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Phiên bản")?.Id ?? 4,
                    Name = "Gối cổ 2/6 tuổi",
                    DisplayValue = "Gối cổ 2/6 tuổi",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "NP61OEFY")?.Id ?? 12,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Phiên bản")?.Id ?? 4,
                    Name = "Gối cổ 6/10 tuổi",
                    DisplayValue = "Gối cổ 6/10 tuổi",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "BP03CEFY")?.Id ?? 13,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "C",
                    DisplayValue = "C",
                },

                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "BP01LEFY")?.Id ?? 14,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Size ngắn",
                    DisplayValue = "Size ngắn",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "BP01SEFY")?.Id ?? 14,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "Size dài",
                    DisplayValue = "Size dài",
                },
                 new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "NPAUNEFY")?.Id ?? 15,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Phiên bản")?.Id ?? 4,
                    Name = "Gối cổ người lớn",
                    DisplayValue = "Gối cổ người lớn",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "NPTLSEFY")?.Id ?? 16,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "9 cm",
                    DisplayValue = "9 cm",
                },
                new ProductOption
                {
                    ProductItemId = _productItems.Find(s => s.Sku == "NPCROEFY")?.Id ?? 17,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Phiên bản")?.Id ?? 4,
                    Name = "Gối cổ đi xe",
                    DisplayValue = "Gối cổ đi xe",
                }
            };
            await _context.ProductOptions.AddRangeAsync(productOptions);
            await _context.SaveChangesAsync();
        }

        if (_product.Any() && !_context.ProductDetails.Any())
        {
            var listProductDetails = new List<ProductDetail>
            {
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "LC35LESY")?.Id ?? 1,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "Ghế SÁNG CHẾ có  đặc điểm LỒI TRƯỚC LÕM SAU bảo đảm cho khung xương được ÔM CHẶT - GIỮ CHUẨN - CHỐNG TRƯỢT\r\nĐiểm 1: Khối lõm ÔM CHẶT - GIỮ CHUẨN, tạo bởi liên kết lõm phía sau mặt ghế và lõm phía dưới lưng ghế\r\nĐiểm 2: Khối lồi trung tâm trước của mặt ghế giúp CHỐNG TRƯỢT",
                    Description = "* Ghế DOCTORLOAN 135 được thiết kế với độ nghiêng 135 độ, ở góc độ này áp lực trên cột sống và đĩa đệm giảm nhiều so với tư thế khác. \r\nCác đường cong lồi lõm được thiết kế phù hợp với tỷ lệ cấu trúc xương sống và xương chậu.\r\nChỉnh toàn bộ khung xương chậu và cột sống khi nằm.\r\nGiảm áp lực lên đĩa đệm, ngăn ngừa chèn ép rễ thần kinh.\r\nKhôi phục đường cong tự nhiên của cột sống ở lưng, ngực và cổ.\r\nTăng cường lưu thông máu, tái tạo năng lượng và phục hồi tổn thương cho cơ thể.",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "SC95VESY")?.Id ?? 2,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "Ghế SÁNG CHẾ có  đặc điểm LỒI TRƯỚC LÕM SAU bảo đảm cho khung xương được ÔM CHẶT - GIỮ CHUẨN - CHỐNG TRƯỢT\r\nĐiểm 1: Khối lõm ÔM CHẶT - GIỮ CHUẨN, tạo bởi liên kết lõm phía sau mặt ghế và lõm phía dưới lưng ghế\r\nĐiểm 2: Khối lồi trung tâm trước của mặt ghế giúp CHỐNG TRƯỢT",
                    Description = "* Ghế SÁNG CHẾ có đặc điểm LỒI TRƯỚC LÕM SAU tạo chức năng điều chỉnh xương chậu, xương cùng, xương sống về cấu trúc chuẩn\r\n* Ghế Sáng chế có tác dụng:\r\n- Giảm tối đa áp lực trên đĩa đệm do nâng đỡ và giữ cấu trúc xương chuẩn\r\n- Ngăn chặn các yếu tố gây biến dạng xương chậu, xương cùng, xương sống\r\n- Tốt cho mọi lứa tuổi trong điều chỉnh khung xương và ngăn ngừa bệnh cột sống",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "SC90DESY")?.Id ?? 3,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "Ghế SÁNG CHẾ có  đặc điểm LỒI TRƯỚC LÕM SAU bảo đảm cho khung xương được ÔM CHẶT - GIỮ CHUẨN - CHỐNG TRƯỢT\r\nĐiểm 1: Khối lõm ÔM CHẶT - GIỮ CHUẨN, tạo bởi liên kết lõm phía sau mặt ghế và lõm phía dưới lưng ghế\r\nĐiểm 2: Khối lồi trung tâm trước của mặt ghế giúp CHỐNG TRƯỢT",
                    Description = "* Ghế SÁNG CHẾ có đặc điểm LỒI TRƯỚC LÕM SAU tạo chức năng điều chỉnh xương chậu, xương cùng, xương sống về cấu trúc chuẩn\r\n* Ghế Sáng chế có tác dụng:\r\n- Giảm tối đa áp lực trên đĩa đệm do nâng đỡ và giữ cấu trúc xương chuẩn\r\n- Ngăn chặn các yếu tố gây biến dạng xương chậu, xương cùng, xương sống\r\n- Tốt cho mọi lứa tuổi trong điều chỉnh khung xương và ngăn ngừa bệnh cột sống",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "SC90TESY")?.Id ?? 4,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "Ghế SÁNG CHẾ có  đặc điểm LỒI TRƯỚC LÕM SAU bảo đảm cho khung xương được ÔM CHẶT - GIỮ CHUẨN - CHỐNG TRƯỢT\r\nĐiểm 1: Khối lõm ÔM CHẶT - GIỮ CHUẨN, tạo bởi liên kết lõm phía sau mặt ghế và lõm phía dưới lưng ghế\r\nĐiểm 2: Khối lồi trung tâm trước của mặt ghế giúp CHỐNG TRƯỢT",
                    Description = "* Ghế SÁNG CHẾ có đặc điểm LỒI TRƯỚC LÕM SAU tạo chức năng điều chỉnh xương chậu, xương cùng, xương sống về cấu trúc chuẩn\r\n* Ghế Sáng chế có tác dụng:\r\n- Giảm tối đa áp lực trên đĩa đệm do nâng đỡ và giữ cấu trúc xương chuẩn\r\n- Ngăn chặn các yếu tố gây biến dạng xương chậu, xương cùng, xương sống\r\n- Tốt cho mọi lứa tuổi trong điều chỉnh khung xương và ngăn ngừa bệnh cột sống",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "PL85SFOR")?.Id ?? 5,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "Gối SÁNG CHẾ có  đặc điểm LỒI TRƯỚC LÕM SAU bảo đảm cho khung xương được ÔM CHẶT - GIỮ CHUẨN - CHỐNG TRƯỢT\r\nĐiểm 1: Khối lõm ÔM CHẶT - GIỮ CHUẨN, tạo bởi liên kết lõm phía sau mặt ghế và lõm phía dưới lưng ghế\r\nĐiểm 2: Khối lồi trung tâm trước của mặt ghế giúp CHỐNG TRƯỢT",
                    Description = "* Gối SÁNG CHẾ có đặc điểm LỒI TRƯỚC LÕM SAU tạo chức năng điều chỉnh xương chậu, xương cùng, xương sống về cấu trúc chuẩn\r\n* Gối Sáng chế có tác dụng:\r\n- Giảm tối đa áp lực trên đĩa đệm do nâng đỡ và giữ cấu trúc xương chuẩn\r\n- Ngăn chặn các yếu tố gây biến dạng xương chậu, xương cùng, xương sống\r\n- Tốt cho mọi lứa tuổi trong điều chỉnh khung xương và ngăn ngừa bệnh cột sống",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "NP04SEFY")?.Id ?? 6,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "- Đầu thấp (9cm): Thiết kế theo cấu trúc Cổ - Vai - Đầu: kết hợp với chất liệu có độ cơ tính bên chắc cao giúp nâng đỡ tốt và bảo vệ Cột Sống Cổ. Dùng để ngủ, nghỉ ngơi hàng ngày. \r\n- Đầu cao (17cm): Có lõi để điều chỉnh Cột Sống Cổ (Sử dụng 15'/ lần). Có thể sử dụng nhiều lần trong ngày.",
                    Description = "Dùng để ngủ, nghỉ ngơi hàng ngày. Có 2 đầu tác dụng khác nhau. Đầu cao chỉ dùng điều chỉnh xương cột sống cổ mỗi lần 15 phút, ngày nhiều lần giúp giảm đau cổ vai cánh tay bàn tay và ngón tay. Đầu thấp dùng ngủ suốt đêm, có tác dụng giữ cho cột sống cổ có đường cong chuẩn, dốt sống cổ đúng vị trí , không bị trật vẹo khi nằm. Phòng bệnh cột sống cổ do sai tư thế nằm, làm việc.",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "NP05SEFY")?.Id ?? 7,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "- Đầu thấp (9cm): Thiết kế theo cấu trúc Cổ - Vai - Đầu: kết hợp với chất liệu có độ cơ tính bên chắc cao giúp nâng đỡ tốt và bảo vệ Cột Sống Cổ. Dùng để ngủ, nghỉ ngơi hàng ngày. \r\n- Đầu cao (13cm): Có lõi để điều chỉnh Cột Sống Cổ (Sử dụng 15'/ lần). Có thể sử dụng nhiều lần trong ngày.",
                    Description = "Dùng để ngủ, nghỉ ngơi hàng ngày. Có 2 đầu tác dụng khác nhau. Đầu cao chỉ dùng điều chỉnh xương cột sống cổ mỗi lần 15 phút, ngày nhiều lần giúp giảm đau cổ vai cánh tay bàn tay và ngón tay. Đầu thấp dùng ngủ suốt đêm, có tác dụng giữ cho cột sống cổ có đường cong chuẩn, đốt sống cổ đúng vị trí , không bị trật vẹo khi nằm. Phòng bệnh cột sống cổ do sai tư thế nằm, làm việc.",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "NP06SEFY")?.Id ?? 8,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "- Đầu thấp (9cm): Thiết kế theo cấu trúc Cổ - Vai - Đầu: kết hợp với chất liệu có độ cơ tính bên chắc cao giúp nâng đỡ tốt và bảo vệ Cột Sống Cổ. Dùng để ngủ, nghỉ ngơi hàng ngày. \r\n- Đầu cao (17cm): Có lõi để điều chỉnh Cột Sống Cổ (Sử dụng 15'/ lần). Có thể sử dụng nhiều lần trong ngày.",
                    Description = "Dùng để ngủ, nghỉ ngơi hàng ngày. Có 2 đầu tác dụng khác nhau. Đầu cao chỉ dùng điều chỉnh xương cột sống cổ mỗi lần 15 phút, ngày nhiều lần giúp giảm đau cổ vai cánh tay bàn tay và ngón tay. Đầu thấp dùng ngủ suốt đêm, có tác dụng giữ cho cột sống cổ có đường cong chuẩn, đốt sống cổ đúng vị trí , không bị trật vẹo khi nằm. Phòng bệnh cột sống cổ do sai tư thế nằm, làm việc.",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "SDNLSESY")?.Id ?? 9,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "- Đệm ngồi được thiết kế với các bậc uốn cong theo cấu trúc xương chậu - xương cùng - xương cụt. \r\n- Đệm ngồi DOCTORLOAN – giải pháp chăm sóc khung xương chậu và xương sống an toàn và hiệu quả. Sử dụng công nghệ sáng chế vượt trội tạo kết cấu đặc biệt (đã được bảo hộ độc quyền).\r\n- Giảm đau và tê ở mông, lưng, chân khi ngồi dưới đất",
                    Description = "Dùng để ngồi trên sản nhà khi thiền, nghi lễ, công việc, sinh hoạt hàng ngày. Có tác dụng giữ chặt xương chậu, xương cùng, xương cụt ở vị trí chuẩn, không lệch vẹo, khi dùng lâu có tác dụng chỉnh các xương trên về vị trí bình thường. \r\n• GIẢI PHÁP: \r\n- Giảm đau đầu gối, mông và tê chân khi ngồi dưới đất. \r\n- Tăng khả năng tập trung bằng cách cải thiện lưu thông máu đến não. \r\n- Công nghệ bảo vệ xương chậu - xương cùng \r\n- cột sống thẳng và chuẩn khi ngồi dưới đất. \r\n- Đột phá về điều chỉnh xương cùng, xương chậu trong tư thế ngồi thiền",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "NP00OEFY")?.Id ?? 10,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "- Thiết kế ôm sát Đầu - Vai - Cổ kết hợp vật liệu đặc biệt  \r\n- Đầu thấp: dành cho trẻ sơ sinh đến 3 tháng tuổi \r\n- Đầu cao: dành cho trẻ từ 3 tháng tuổi đến 1 tuổi ",
                    Description = "Gối sơ sinh được làm từ mút cao cấp, an toàn. Có tác dụng bảo vệ cột sống cổ tốt nhất giúp trẻ phát triển trí tuệ, thể chất và tinh thần toàn diện cho trẻ dưới 24 tháng tuổi.\r\n• GIẢI PHÁP:\r\n- Gối được thiết kế phù hợp cấu trúc Cổ - Vai - Đầu của trẻ sơ sinh, hỗ trợ nâng đỡ cột sống cổ trong tư thế nằm của trẻ sơ sinh\r\n- Hỗ trợ điều trị và ngăn ngừa các bệnh cột sống cổ, giúp điều chỉnh cột sống cổ trở lại trạng thái khỏe mạnh bình thường.\r\n- Có tác dụng bảo vệ cột sống cổ tốt nhất giúp trẻ phát triển trí tuệ, thể chất và tinh thần toàn diện"
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "NP26OEFY")?.Id ?? 11,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "- Thiết kế ôm sát Đầu - Vai - Cổ kết hợp vật liệu đặc biệt có tác dụng bảo vệ cột sống cổ tốt nhất giúp trẻ phát triển trí tuệ, thể chất và tinh thần toàn diện \r\n- Đầu thấp: dành cho trẻ 2 tuổi đến 4 tuổi \r\n- Đầu cao: dành cho trẻ 4 tuổi đến 6 tuổi ",
                    Description = "Gối trẻ em 2/6 được làm từ mút cao cấp, an toàn. Có tác dụng bảo vệ cột sống cổ tốt nhất giúp trẻ phát triển trí tuệ, thể chất và tinh thần toàn diện cho trẻ từ 2 đến 6 tuổi.\r\n• GIẢI PHÁP:\r\n- Gối được thiết kế phù hợp cấu trúc Cổ - Vai - Đầu của trẻ em, hỗ trợ nâng đỡ cột sống cổ trong tư thế nằm của trẻ em\r\n- Hỗ trợ điều trị và ngăn ngừa các bệnh cột sống cổ, giúp điều chỉnh cột sống cổ trở lại trạng thái khỏe mạnh bình thường.\r\n- Có tác dụng bảo vệ cột sống cổ tốt nhất giúp trẻ phát triển trí tuệ, thể chất và tinh thần toàn diện"
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "NP61OEFY")?.Id ?? 12,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "- Thiết kế ôm sát Đầu - Vai - Cổ kết hợp vật liệu đặc biệt có tác dụng bảo vệ cột sống cổ tốt nhất giúp trẻ phát triển trí tuệ, thể chất và tinh thần toàn diện \r\n- Đầu thấp: dành cho trẻ 6 tuổi đến 8 tuổi \r\n- Đầu cao: dành cho trẻ 8 tuổi đến 10 tuổi ",
                    Description = "Gối trẻ em 6/10 được làm từ mút cao cấp, an toàn. Có tác dụng bảo vệ cột sống cổ tốt nhất giúp trẻ phát triển trí tuệ, thể chất và tinh thần toàn diện cho trẻ từ 6 - 10 tuổi.\r\n• GIẢI PHÁP:\r\n- Gối được thiết kế phù hợp cấu trúc Cổ - Vai - Đầu của trẻ em, hỗ trợ nâng đỡ cột sống cổ trong tư thế nằm của trẻ em\r\n- Hỗ trợ điều trị và ngăn ngừa các bệnh cột sống cổ, giúp điều chỉnh cột sống cổ trở lại trạng thái khỏe mạnh bình thường.\r\n- Có tác dụng bảo vệ cột sống cổ tốt nhất giúp trẻ phát triển trí tuệ, thể chất và tinh thần toàn diện"
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "BP03CEFY")?.Id ?? 13,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "Có lõi cứng đặc biệt bên trong giúp điều chỉnh cột sống lưng và đĩa đệm",
                    Description = "Gối lưng sáng chế DOCTORLOAN F3/C có lõi cứng đặc biệt bên trong giúp điều chỉnh cột sống lưng và đĩa đệm trở lại trạng thái khỏe mạnh. \r\n• GIẢI PHÁP: \r\n- Công nghệ điều chỉnh bệnh vẹo, trật, trượt cột sống lưng, thoát vị đĩa đệm \r\n- Công nghệ chăm sóc cột sống khỏe \r\n- Giảm đau, tê, nhức, buốt, mỏi cứng ở vùng thắt lưng, vùng mông và hai chân \r\n- Chuyển giao chăm sóc cột sống tại nhà, bảo đảm năng suất lao động, học tập hiệu quả",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "BP01LEFY")?.Id ?? 14,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "Có lõi cứng đặc biệt bên trong giúp điều chỉnh cột sống lưng và đĩa đệm khi ngồi làm việc hoặc đi lại trên các phương tiện (Oto, Máy bay, ...)",
                    Description = "Gối lưng có lõi dùng tại nhà giúp điều chỉnh cột sống lưng và cột sống ngực về hình dạng cong bình thường chuẩn.\r\nNgoài ra có tác dụng đưa đĩa đệm về đúng vị trí bình thường. \r\nGiảm đau lưng, giảm các chứng đau như thần kinh toạ, đau đầu gối, đau chân, đau bàn và gót chân.",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "NPAUNEFY")?.Id ?? 15,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "- Đầu thấp (9cm): Thiết kế theo cấu trúc Cổ - Vai - Đầu: kết hợp với chất liệu có độ cơ tính bên chắc cao giúp nâng đỡ tốt và bảo vệ Cột Sống Cổ. Dùng hàng ngày. \r\n- Đầu cao (12cm): Không lõi để điều chỉnh Cột Sống Cổ. Có thể sử dụng nhiều lần trong ngày, và ngủ qua đêm. An toàn sử dụng cho người đã phẩu thuật cột sống cổ",
                    Description = "Dùng để ngủ, nghỉ ngơi hàng ngày. Có 2 đầu tác dụng khác nhau. \r\nKhông lõi để điều chỉnh Cột Sống Cổ. Có thể sử dụng nhiều lần trong ngày, và ngủ qua đêm. An toàn sử dụng cho người đã phẩu thuật cột sống cổ.\r\nThiết kế theo cấu trúc Cổ - Vai - Đầu: kết hợp với chất liệu có độ cơ tính bên chắc cao giúp nâng đỡ tốt và bảo vệ Cột Sống Cổ. Dùng hàng ngày. "
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "NPTLSEFY")?.Id ?? 16,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "- Đầu thấp: Thiết kế theo cấu trúc Cổ - Vai - Đầu: kết hợp với chất liệu có độ cơ tính bên chắc cao giúp nâng đỡ tốt và bảo vệ Cột Sống Cổ. Dùng để ngủ, nghỉ ngơi hàng ngày. \r\n- Đầu cao: Có lõi để điều chỉnh Cột Sống Cổ (Sử dụng 15'/ lần). Thiết kê nhỏ gọn tiện cho những chuyến công tác và du lịch.",
                    Description = "Dùng để ngủ, nghỉ ngơi hàng ngày. Có 2 đầu tác dụng khác nhau. \r\nKhông lõi để điều chỉnh Cột Sống Cổ. Có thể sử dụng nhiều lần trong ngày, và ngủ qua đêm.\r\nThiết kế theo cấu trúc Cổ - Vai - Đầu: kết hợp với chất liệu có độ cơ tính bên chắc cao giúp nâng đỡ tốt và bảo vệ Cột Sống Cổ.\r\nƯu điểm nhỏ ngọn thuận tiện cho những chuyến công tác",
                },
                new ProductDetail
                {
                    ProductId = _product.Find(s => s.Sku == "NPCROEFY")?.Id ?? 17,
                    LanguageId = (int)LanguageEnum.VN,
                    Summary = "- Gối được làm từ vật liệu mousse có tỷ trọng cao, tạo khả năng cố định không thay đổi hình dáng cột sống cổ.\r\n- Với các đường “ lồi, lõm” tuân thủ theo hình dạng cấu trúc của cột sống cổ, bảo toàn toàn bộ cấu trúc của cột sống cổ, phòng ngừa bệnh cột sống cổ mỗi ngày khi ngủ ngồi, nghỉ ngơi cho mọi tuổi, mọi người.",
                    Description = "* Dùng trong khi di chuyển bằng  (oto, máy bay,..). \r\nKhông lõi để điều chỉnh Cột Sống Cổ. Có thể sử dụng nhiều lần trong ngày, và ngủ qua đêm. An toàn sử dụng cho người đã phẩu thuật cột sống cổ.",
                },
            };
            await _context.ProductDetails.AddRangeAsync(listProductDetails);
            await _context.SaveChangesAsync();
        }
    }
}
