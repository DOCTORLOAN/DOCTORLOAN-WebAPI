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
            await _context.Categories.AddAsync(new Domain.Entities.Products.Category { Name = "Sản phẩm DOCTORLOAN", Slug = "sanpham-doctorloan", Status = StatusEnum.Publish, Sort = 0 });
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
            .Where(c => c.Slug == "sanpham-doctorloan")
            .ToListAsync();

        if (_context.Brands.Any() && !_context.Products.Any())
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
                            CategoryId = _productCategories.Find(s => s.Slug == "sanpham-doctorloan")?.Id ?? 1
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
                          }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ sáng chế F4",
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
                          }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ sáng chế F5",
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
                          }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ sáng chế F6",
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
                          }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Đệm thiền DOCTORLOAN",
                   Sku = "SDNLSESY",
                   Status = StatusEnum.Publish,
                   BrandId = _context.Brands.FirstOrDefault().Id,
                   Price = 44800000,
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
                          }
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
                   Name = "Gối lưng DOCTORLOAN F3/C",
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
                          }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối lưng DOCTORLOAN F1",
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
                          }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ người lớn",
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
                          }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ du lịch",
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
                          }
                   },
                },
                new DoctorLoan.Domain.Entities.Products.Product
                {
                   Name = "Gối cổ đi xe",
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
            .Where(s => s.Name == "Màu sắc" || s.Name == "Kích thước")
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
                    ProductItemId = _productItems.Find(s => s.Sku == "NPTLSEFY")?.Id ?? 16,
                    OptionGroupId = _productOptionGroup.Find(s => s.Name == "Kích thước")?.Id ?? 2,
                    Name = "9 cm",
                    DisplayValue = "9 cm",
                },
            };
            await _context.ProductOptions.AddRangeAsync(productOptions);
            await _context.SaveChangesAsync();
        }
    }
}
