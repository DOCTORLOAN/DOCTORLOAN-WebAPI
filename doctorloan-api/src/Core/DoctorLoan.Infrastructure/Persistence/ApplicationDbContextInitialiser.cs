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
                new SymptomGroups { Id = 1, Name = "Cột sống cổ - Đầu và mặt" },
                new SymptomGroups { Id = 2, Name = "Cột sống cổ - Cổ" },
                new SymptomGroups { Id = 3, Name = "Cột sống cổ - Tay" },
                new SymptomGroups { Id = 4, Name = "Cột sống ngực" },
                new SymptomGroups { Id = 5, Name = "Cột sống lưng" },
            };
            await _context.AddRangeAsync();
            await _context.SaveChangesAsync();

            if (!_context.Symptoms.Any())
            {
                var listSymptom = new List<Symptoms>
                {
                    new Symptoms { Id = 1, Name = "U tai - nghe kém", SymptomGroupId = 1 },
                    new Symptoms { Id = 2, Name = "Khó nói", SymptomGroupId = 1 },
                    new Symptoms { Id = 3, Name = "Đau đầu - đau nửa đầu", SymptomGroupId = 1 },
                    new Symptoms { Id = 4, Name = "Hay quên", SymptomGroupId = 1 },
                    new Symptoms { Id = 5, Name = "Chóng mặt - rối loạn tiền đình - choáng váng", SymptomGroupId = 1 },
                    new Symptoms { Id = 6, Name = "Giam khả năng tập trung", SymptomGroupId = 1 },
                    new Symptoms { Id = 7, Name = "Nhìn mờ - mù thoáng qua", SymptomGroupId = 1 },
                    new Symptoms { Id = 8, Name = "Mất ngủ - khó ngủ", SymptomGroupId = 1 },

                    new Symptoms { Id = 9, Name = "Đau cổ", SymptomGroupId = 2 },
                    new Symptoms { Id = 10, Name = "Mỏi cổ", SymptomGroupId = 2 },
                    new Symptoms { Id = 11, Name = "Khó cử động", SymptomGroupId = 2 },
                    new Symptoms { Id = 12, Name = "Đau khi cử động cổ", SymptomGroupId = 2 },

                    new Symptoms { Id = 13, Name = "Cẳng tay", SymptomGroupId = 3 },
                    new Symptoms { Id = 14, Name = "Đau/tê/buốt", parentId = 13,  SymptomGroupId = 3 },
                    new Symptoms { Id = 15, Name = "Nhức mỏi", parentId = 13, SymptomGroupId = 3 },
                    new Symptoms { Id = 16, Name = "Khó cử động", parentId = 13, SymptomGroupId = 3 },
                    new Symptoms { Id = 17, Name = "Xuội/liệt", parentId = 13, SymptomGroupId = 3 },
                    new Symptoms { Id = 18, Name = "Lạnh/nóng", parentId = 13, SymptomGroupId = 3 },
                    new Symptoms { Id = 19, Name = "Bị teo", parentId = 13, SymptomGroupId = 3 },
                    new Symptoms { Id = 20, Name = "Xưng/nóng/đỏ", parentId = 13, SymptomGroupId = 3 },

                    new Symptoms { Id = 21, Name = "Vai", SymptomGroupId = 3 },
                    new Symptoms { Id = 22, Name = "Đau/tê/buốt", parentId = 21,  SymptomGroupId = 3 },
                    new Symptoms { Id = 23, Name = "Nhức mỏi", parentId = 21, SymptomGroupId = 3 },
                    new Symptoms { Id = 24, Name = "Khó cử động", parentId = 21, SymptomGroupId = 3 },
                    new Symptoms { Id = 25, Name = "Xuội/liệt", parentId = 21, SymptomGroupId = 3 },
                    new Symptoms { Id = 26, Name = "Lạnh/nóng", parentId = 21, SymptomGroupId = 3 },
                    new Symptoms { Id = 27, Name = "Bị teo", parentId = 21, SymptomGroupId = 3 },
                    new Symptoms { Id = 28, Name = "Xưng/nóng/đỏ", parentId = 21, SymptomGroupId = 3 },

                    new Symptoms { Id = 29, Name = "Cánh tay", SymptomGroupId = 3 },
                    new Symptoms { Id = 30, Name = "Đau/tê/buốt", parentId = 29,  SymptomGroupId = 3 },
                    new Symptoms { Id = 31, Name = "Nhức mỏi", parentId = 29, SymptomGroupId = 3 },
                    new Symptoms { Id = 32, Name = "Khó cử động", parentId = 29, SymptomGroupId = 3 },
                    new Symptoms { Id = 33, Name = "Xuội/liệt", parentId = 29, SymptomGroupId = 3 },
                    new Symptoms { Id = 34, Name = "Lạnh/nóng", parentId = 29, SymptomGroupId = 3 },
                    new Symptoms { Id = 35, Name = "Bị teo", parentId = 29, SymptomGroupId = 3 },
                    new Symptoms { Id = 36, Name = "Xưng/nóng/đỏ", parentId = 29, SymptomGroupId = 3 },

                    new Symptoms { Id = 37, Name = "Cánh tay", SymptomGroupId = 3 },
                    new Symptoms { Id = 38, Name = "Đau/tê/buốt", parentId = 37,  SymptomGroupId = 3 },
                    new Symptoms { Id = 39, Name = "Nhức mỏi", parentId = 37, SymptomGroupId = 3 },
                    new Symptoms { Id = 40, Name = "Khó cử động", parentId = 37, SymptomGroupId = 3 },
                    new Symptoms { Id = 41, Name = "Xuội/liệt", parentId = 37, SymptomGroupId = 3 },
                    new Symptoms { Id = 42, Name = "Lạnh/nóng", parentId = 37, SymptomGroupId = 3 },
                    new Symptoms { Id = 43, Name = "Bị teo", parentId = 37, SymptomGroupId = 3 },
                    new Symptoms { Id = 44, Name = "Xưng/nóng/đỏ", parentId = 37, SymptomGroupId = 3 },

                    new Symptoms { Id = 45, Name = "Đau thắt lưng/liên sườn", SymptomGroupId = 4 },
                    new Symptoms { Id = 46, Name = "Khó thở", SymptomGroupId = 4 },
                    new Symptoms { Id = 47, Name = "Ho", SymptomGroupId = 4 },
                    new Symptoms { Id = 48, Name = "Thở ngắt quãng", SymptomGroupId = 4 },
                    new Symptoms { Id = 49, Name = "Rối loại nhịp tim", SymptomGroupId = 4 },
                    new Symptoms { Id = 50, Name = "Trào ngược dạ dày", SymptomGroupId = 4 },

                    new Symptoms { Id = 51, Name = "Đau/tê/mỏi Lưng", SymptomGroupId = 5 },
                    new Symptoms { Id = 52, Name = "Đau/tê/mỏi Mông", SymptomGroupId = 5 },
                    new Symptoms { Id = 53, Name = "Đau thần kinh tọa", SymptomGroupId = 5 },
                    new Symptoms { Id = 54, Name = "Đau/mỏi/tê Khớp háng", SymptomGroupId = 5 },
                    new Symptoms { Id = 55, Name = "Đau đùi", SymptomGroupId = 5 },
                    new Symptoms { Id = 56, Name = "Đau đầu gối", SymptomGroupId = 5 },
                    new Symptoms { Id = 57, Name = "Đau cẳng chân", SymptomGroupId = 5 },
                    new Symptoms { Id = 58, Name = "Đau cổ chân", SymptomGroupId = 5 },
                    new Symptoms { Id = 59, Name = "Đau gang bàn chân", SymptomGroupId = 5 },
                    new Symptoms { Id = 60, Name = "Đau gót chân", SymptomGroupId = 5 },
                    new Symptoms { Id = 61, Name = "Yếu/liệt chân", SymptomGroupId = 5 },
                };
                await _context.AddRangeAsync(listSymptom);
                await _context.SaveChangesAsync();
            }
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
