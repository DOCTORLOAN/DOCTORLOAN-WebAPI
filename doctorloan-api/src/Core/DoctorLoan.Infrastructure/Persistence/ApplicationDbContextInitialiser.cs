using BCrypt.Net;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Domain.Entities.Departments;
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
        if (!_context.Departments.Any())
        {
            var department = new Department() { Code = "D1", Name = "Department Test", OrderBy = 1 };

            if (!_context.Roles.Any())
            {
                var listRoles = new List<Role>
                {
                    new Role() { Code = "admin", Name = "Administrator", IsActive = true },
                    new Role() { Code = "user", Name = "User", IsActive = true }
                };

                department.DepartmentRoles.AddRange(listRoles);
                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();
            }
        }

        var adminRoleId = await _context.Roles.Include(s => s.Users).Where(s => s.Code == "admin" || s.Code == "user").ToListAsync();
        var hasAdmin = await _context.Users.AnyAsync(s => s.UserName == "admin");
        if (adminRoleId.Any() && !hasAdmin)
        {
            var testUsers = new List<User>
                {
                    new User {
                        FullName = "Admin",
                        UserName = "admin",
                        Phone = "0346068139",
                        Email = "nguyenthanhtai168+doctor1@gmail.com",
                        Status= Domain.Enums.Users.UserStatus.Active,
                        SourcePlatform = SourcePlatform.WebAdmin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                        RoleId = adminRoleId.Find(s=>s.Code =="admin" )?.Id ?? 1
                    },
                    new User {
                        FullName = "Tai Nguyen",
                        UserName = "nguyenthanhtai168+doctor2@gmail.com",
                        Phone = "0376757833",
                        Email = "nguyenthanhtai168+doctor2@gmail.com",
                        Status= Domain.Enums.Users.UserStatus.Active,
                        SourcePlatform = SourcePlatform.WebAdmin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                        RoleId = adminRoleId.Find(s=>s.Code =="user" )?.Id ?? 2
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
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123", BCrypt.Net.BCrypt.GenerateSalt(10), false, HashType.SHA512),
                    RoleId = adminRoleId.Find(s => s.Code == "user")?.Id ?? 2
                });
            }

            await _context.Users.AddRangeAsync(listUserTest);
            await _context.SaveChangesAsync();
        }

    }

    public async Task TrySeedProductAsync()
    {
        if (!_context.Categories.Any())
            await _context.Categories.AddAsync(new Domain.Entities.Products.Category { Name = "Ghế", Slug = "ghe", Sort = 0 });
        await _context.SaveChangesAsync();
        if (!_context.AttributeGroups.Any())
        {
            await _context.AttributeGroups.AddAsync(new AttributeGroup
            {
                Name = "Kích thước ghế",
                Attributes = new List<Domain.Entities.Products.Attribute> {
                 new Domain.Entities.Products.Attribute{Name="Chiều cao"},
                 new Domain.Entities.Products.Attribute{Name="Cân nặng"},
                 new Domain.Entities.Products.Attribute{Name="Khổi lượng tối đa"}
            }
            });
            await _context.AttributeGroups.AddAsync(new AttributeGroup
            {
                Name = "Thông tin tổng thể",
                Attributes = new List<Domain.Entities.Products.Attribute> {
                new Domain.Entities.Products.Attribute{Name="Kích thước sản phẩm"},
                new Domain.Entities.Products.Attribute{Name="Khối lượng"},
                new Domain.Entities.Products.Attribute{Name="Chế độ bảo hành"}
            }
            });
        }
        if (!_context.ProductOptionGroups.Any())
        {
            _context.ProductOptionGroups.AddRange(new List<ProductOptionGroup>
            {
                new ProductOptionGroup{Name="Màu sắc"},
                 new ProductOptionGroup{Name="Kích cỡ"},
            });
        }
        if (!_context.Brands.Any())
        {
            _context.Brands.AddRange(new List<Brand>
            {
                new Brand{Name="Doctor Loan"}
            });
        }

        await _context.SaveChangesAsync();
    }
}
