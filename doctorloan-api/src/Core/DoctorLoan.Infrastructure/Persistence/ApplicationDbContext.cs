using System.Reflection;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Domain.Entities.Addresses;
using DoctorLoan.Domain.Entities.Authorizations;
using DoctorLoan.Domain.Entities.Banks;
using DoctorLoan.Domain.Entities.Bookings;
using DoctorLoan.Domain.Entities.Commons;
using DoctorLoan.Domain.Entities.Contents;
using DoctorLoan.Domain.Entities.Customers;
using DoctorLoan.Domain.Entities.Departments;
using DoctorLoan.Domain.Entities.Emails;
using DoctorLoan.Domain.Entities.Medias;
using DoctorLoan.Domain.Entities.News;
using DoctorLoan.Domain.Entities.Orders;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Domain.Entities.Roles;
using DoctorLoan.Domain.Entities.Settings;
using DoctorLoan.Domain.Entities.Users;
using DoctorLoan.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    #region Fields
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    #endregion

    #region Ctor
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }
    #endregion

    #region Methods
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.HasPostgresExtension("ltree");
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region Properties
    #region Settings
    public DbSet<Setting> Settings => Set<Setting>();
    #endregion

    #region System
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Department> Departments => Set<Department>();
    #endregion

    #region Authorizations
    public DbSet<PermissionAction> PermissionActions => Set<PermissionAction>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
    #endregion

    #region User
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserDetail> UserDetails => Set<UserDetail>();
    public DbSet<UserDevice> UserDevices => Set<UserDevice>();
    public DbSet<UserBankBranch> UserBankBranchs => Set<UserBankBranch>();
    public DbSet<UserIdentity> UserIdentities => Set<UserIdentity>();
    public DbSet<UserIdentityLog> UserIdentityLogs => Set<UserIdentityLog>();
    public DbSet<UserMedia> UserMedias => Set<UserMedia>();
    public DbSet<UserAddress> UserAddresses => Set<UserAddress>();
    public DbSet<UserActivity> UserActivities => Set<UserActivity>();
    #endregion

    #region Banks
    public DbSet<Bank> Banks => Set<Bank>();
    public DbSet<Branch> Branchs => Set<Branch>();
    public DbSet<BankBranch> BankBranchs => Set<BankBranch>();
    #endregion

    #region Setting
    public DbSet<SettingApp> SettingApps => Set<SettingApp>();
    public DbSet<SettingAppContent> SettingAppContents => Set<SettingAppContent>();
    public DbSet<SettingUser> SettingUsers => Set<SettingUser>();
    public DbSet<SettingUserLog> SettingUserLogs => Set<SettingUserLog>();
    #endregion

    #region Notification
    public DbSet<EmailRequest> EmailRequests => Set<EmailRequest>();
    #endregion

    #region Medias
    public DbSet<Media> Medias => Set<Media>();
    #endregion

    #region Commons
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<LocalizedProperty> LocalizedProperties => Set<LocalizedProperty>();
    #endregion

    #region Addresses
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<District> Districts => Set<District>();
    public DbSet<Province> Provinces => Set<Province>();
    public DbSet<Ward> Wards => Set<Ward>();
    #endregion

    #region Contents
    public DbSet<Content> Contents => Set<Content>();


    #endregion

    #region Products
    public DbSet<Domain.Entities.Products.Attribute> Attributes => Set<Domain.Entities.Products.Attribute>();

    public DbSet<AttributeGroup> AttributeGroups => Set<AttributeGroup>();

    public DbSet<Brand> Brands => Set<Brand>();

    public DbSet<EntityLanguage> EntityLanguages => Set<EntityLanguage>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<ProductAttribute> ProductAttributes => Set<ProductAttribute>();

    public DbSet<ProductDetail> ProductDetails => Set<ProductDetail>();

    public DbSet<ProductItem> ProductItems => Set<ProductItem>();

    public DbSet<ProductMedia> ProductMedias => Set<ProductMedia>();

    public DbSet<ProductOption> ProductOptions => Set<ProductOption>();

    public DbSet<ProductOptionGroup> ProductOptionGroups => Set<ProductOptionGroup>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();


    #endregion

    #region News
    public DbSet<NewsCategory> NewsCategories => Set<NewsCategory>();

    public DbSet<NewsItem> NewsItems => Set<NewsItem>();

    public DbSet<NewsMedia> NewsMedias => Set<NewsMedia>();

    public DbSet<NewsItemDetail> NewsItemDetails => Set<NewsItemDetail>();

    public DbSet<NewsTag> NewsTags => Set<NewsTag>();

    public DbSet<NewsTagsMapping> NewsTagsMappings => Set<NewsTagsMapping>();

    public DbSet<NewsCategoryMapping> NewsCategoryMappings => Set<NewsCategoryMapping>();
    #endregion

    #region Customers
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
    #endregion

    #region Bookings
    public DbSet<Booking> Bookings => Set<Booking>();
    #endregion

    #region Orders
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    #endregion

    #endregion
}
