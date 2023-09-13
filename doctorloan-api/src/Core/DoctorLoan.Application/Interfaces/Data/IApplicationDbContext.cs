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
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Application.Interfaces.Data;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    #region System
    DbSet<Role> Roles { get; }
    DbSet<Department> Departments { get; }
    #endregion

    #region Authrorizations
    DbSet<PermissionAction> PermissionActions { get; }
    DbSet<UserPermission> UserPermissions { get; }
    #endregion

    #region User
    DbSet<Device> Devices { get; }
    DbSet<User> Users { get; }
    DbSet<UserDetail> UserDetails { get; }
    DbSet<UserDevice> UserDevices { get; }
    DbSet<UserBankBranch> UserBankBranchs { get; }
    DbSet<UserIdentity> UserIdentities { get; }
    DbSet<UserIdentityLog> UserIdentityLogs { get; }
    DbSet<UserMedia> UserMedias { get; }
    DbSet<UserAddress> UserAddresses { get; }
    DbSet<UserActivity> UserActivities { get; }
    #endregion

    #region Banks
    DbSet<Bank> Banks { get; }
    DbSet<Branch> Branchs { get; }
    DbSet<BankBranch> BankBranchs { get; }
    #endregion

    #region Setting
    DbSet<Setting> Settings { get; }
    DbSet<SettingApp> SettingApps { get; }
    DbSet<SettingAppContent> SettingAppContents { get; }
    DbSet<SettingUser> SettingUsers { get; }
    DbSet<SettingUserLog> SettingUserLogs { get; }
    #endregion

    #region Notification
    DbSet<EmailRequest> EmailRequests { get; }
    #endregion

    #region Blob stroage
    DbSet<Media> Medias { get; }
    #endregion

    #region Commons
    DbSet<Job> Jobs { get; }
    DbSet<Language> Languages { get; }
    DbSet<LocalizedProperty> LocalizedProperties { get; }
    #endregion

    #region Addresses
    DbSet<Address> Addresses { get; }
    DbSet<Country> Countries { get; }
    DbSet<District> Districts { get; }
    DbSet<Province> Provinces { get; }
    DbSet<Ward> Wards { get; }
    #endregion

    #region Contents
    DbSet<Content> Contents { get; }
    #endregion

    #region Products
    DbSet<Domain.Entities.Products.Attribute> Attributes { get; }
    DbSet<AttributeGroup> AttributeGroups { get; }
    DbSet<Brand> Brands { get; }
    DbSet<EntityLanguage> EntityLanguages { get; }
    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }
    DbSet<ProductAttribute> ProductAttributes { get; }
    DbSet<ProductDetail> ProductDetails { get; }
    DbSet<ProductItem> ProductItems { get; }
    DbSet<ProductMedia> ProductMedias { get; }
    DbSet<ProductOption> ProductOptions { get; }
    DbSet<ProductOptionGroup> ProductOptionGroups { get; }
    DbSet<ProductCategory> ProductCategories { get; }
    #endregion

    #region News
    DbSet<NewsCategory> NewsCategories { get; }
    DbSet<NewsItem> NewsItems { get; }
    DbSet<NewsMedia> NewsMedias { get; }
    DbSet<NewsItemDetail> NewsItemDetails { get; }
    DbSet<NewsTag> NewsTags { get; }
    DbSet<NewsTagsMapping> NewsTagsMappings { get; }
    DbSet<NewsCategoryMapping> NewsCategoryMappings { get; }

    #endregion

    #region Customers
    DbSet<Customer> Customers { get; }
    DbSet<CustomerAddress> CustomerAddresses { get; }
    #endregion

    #region Bookings
    DbSet<Booking> Bookings { get; }
    #endregion

    #region Orders
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    #endregion
}
