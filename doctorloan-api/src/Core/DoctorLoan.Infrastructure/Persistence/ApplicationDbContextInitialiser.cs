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
        if (!_context.AttributeGroups.Any())
        {
            await _context.AttributeGroups.AddAsync(new AttributeGroup
            {
                Name = "Kích thước ghế",
                Attributes = new List<Domain.Entities.Products.Attribute> {
                new Domain.Entities.Products.Attribute{Name="Kích thước"},
                new Domain.Entities.Products.Attribute{Name="Khối lượng"},
            }
            });
            await _context.AttributeGroups.AddAsync(new AttributeGroup
            {
                Name = "Thông tin tổng thể",
                Attributes = new List<Domain.Entities.Products.Attribute> {
                new Domain.Entities.Products.Attribute{Name="Kiểu dáng"},
                new Domain.Entities.Products.Attribute{Name="Chất liệu lõi"},
                new Domain.Entities.Products.Attribute{Name="Chất liệu bọc"},
                new Domain.Entities.Products.Attribute{Name="Công nghệ sản xuất"},
                new Domain.Entities.Products.Attribute{Name="Hiệu quả sử dụng"},
                new Domain.Entities.Products.Attribute{Name="Hướng dẫn sử dụng"},
                new Domain.Entities.Products.Attribute{Name="Bảo hành"},
                new Domain.Entities.Products.Attribute{Name="Năm sản xuất"},
                new Domain.Entities.Products.Attribute{Name="Sản xuất tại"},
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
                new Brand{Name="DOCTORLOAN"}
            });
        }

        //if (!_context.products.any())
        //{
        //    var products = new list<product>
        //    {
        //        new product
        //        {
        //           name = "ghế sáng chế doctorloan 135",
        //           slug = "lc35les",
        //           status = statusenum.publish,
        //           categoryid = _context.categories.firstordefault().id,
        //           brandid = _context.brands.firstordefault().id,
        //           price = 37400000,
        //           productdetails = new productdetail
        //           {
        //               description = "ghế văn phòng",
        //               shortdescription = "ghế văn phòng",
        //               metatitle = "ghế văn phòng",
        //               metadescription = "ghế văn phòng",
        //               metakeywords = "ghế văn phòng",
        //               attributevalues = new list<attributevalue>
        //               {
        //                   new attributevalue{attributeid = _context.attributegroups.firstordefault().attributes.firstordefault().id,value="kích thước 135"},
        //                   new attributevalue{attributeid = _context.attributegroups.firstordefault().attributes.last().id,value="10kg"},
        //                   new attributevalue{attributeid = _context.attributegroups.last().attributes.firstordefault().id,value="kiểu dáng"},
        //                   new attributevalue{attributeid = _context.attributegroups.last().attributes.skip(1).firstordefault().id,value="chất liệu lõi"},
        //                   new attributevalue{attributeid = _context.attributegroups.last().attributes.skip(2).firstordefault().id,value="chất liệu bọc"},
        //                   new attributevalue{attributeid = _context.attributegroups.last().attributes.skip(3).firstordefault().id,value="công nghệ sản xuất"},
        //                   new attributevalue{attributeid = _context.attributegroups.last().attributes.skip(4).firstordefault().id,value="hiệu quả sử dụng"},
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

        await _context.SaveChangesAsync();
    }
}
