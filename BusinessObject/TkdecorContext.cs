﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObject;

public partial class TkdecorContext : DbContext
{
    public TkdecorContext()
    {
    }

    public TkdecorContext(DbContextOptions<TkdecorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<CouponType> CouponTypes { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Product3DModel> Product3Dmodels { get; set; }

    public virtual DbSet<ProductFavorite> ProductFavorites { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductReviewInteraction> ProductReviewInteractions { get; set; }

    public virtual DbSet<ProductReviewInteractionStatus> ProductReviewInteractionStatuses { get; set; }

    public virtual DbSet<ProductReport> ProductReports { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<ReportProductReview> ReportProductReviews { get; set; }

    public virtual DbSet<ReportStatus> ReportStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=tcp:server-database-duyen.database.windows.net,1433;Initial Catalog=TKDecor;Persist Security Info=False;User ID=database_azure;Password=01248163264Tkdecor;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PK__Article__CC36F660180F7E7F");

            entity.ToTable("Article");

            entity.HasIndex(e => e.UserId, "IX_Article_user_id");

            entity.HasIndex(e => e.Slug, "IX_Article_slug")
                .IsUnique();

            entity.Property(e => e.Slug).HasColumnName("slug");

            entity.Property(e => e.ArticleId).HasColumnType("uniqueidentifier").HasColumnName("article_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.IsPublish).HasColumnName("is_publish");
            entity.Property(e => e.Thumbnail).HasColumnName("thumbnail");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");

            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Articles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Article_User");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__2EF52A27823B5610");

            entity.ToTable("Cart");

            entity.HasIndex(e => e.ProductId, "IX_Cart_product_id");

            entity.HasIndex(e => e.UserId, "IX_Cart_user_id");

            entity.Property(e => e.CartId).HasColumnType("uniqueidentifier").HasColumnName("cart_id");
            entity.Property(e => e.ProductId).HasColumnType("uniqueidentifier").HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Product");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B430138B56");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "IX_Category_name").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnType("uniqueidentifier").HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");
            entity.Property(e => e.Name)
                .HasColumnName("name");
            entity.Property(e => e.Thumbnail)
                .HasColumnName("thumbnail");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PK__Coupon__58CF6389A836CD8D");

            entity.ToTable("Coupon");

            entity.HasIndex(e => e.Code, "IX_Coupon_code").IsUnique();

            entity.HasIndex(e => e.CouponTypeId, "IX_Coupon_coupon_type_id");

            entity.Property(e => e.CouponId).HasColumnType("uniqueidentifier").HasColumnName("coupon_id");
            entity.Property(e => e.Code)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.CouponTypeId).HasColumnType("uniqueidentifier").HasColumnName("coupon_type_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.RemainingUsageCount).HasColumnName("remaining_usage_count");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Value)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("value");
            entity.Property(e => e.MaxValue)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("max_value");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");

            entity.HasOne(d => d.CouponType).WithMany(p => p.Coupons)
                .HasForeignKey(d => d.CouponTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Coupon_CouponType");
        });

        modelBuilder.Entity<CouponType>(entity =>
        {
            entity.HasKey(e => e.CouponTypeId).HasName("PK__CouponTy__AD2AFC0A104B34A3");

            entity.ToTable("CouponType");

            entity.Property(e => e.CouponTypeId).HasColumnType("uniqueidentifier").HasColumnName("coupon_type_id");
            entity.Property(e => e.Name)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__Chat__0BBF6EE6112EA443");

            entity.ToTable("Chat");

            entity.HasIndex(e => e.ReceiverId, "IX_Chat_receiver_id");

            entity.HasIndex(e => e.SenderId, "IX_Chat_sender_id");

            entity.Property(e => e.ChatId).HasColumnType("uniqueidentifier").HasColumnName("chat_id");

            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.ReceiverId).HasColumnType("uniqueidentifier").HasColumnName("receiver_id");
            entity.Property(e => e.SenderId).HasColumnType("uniqueidentifier").HasColumnName("sender_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");

            entity.HasOne(d => d.Receiver).WithMany(p => p.ChatReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chat_User1");

            entity.HasOne(d => d.Sender).WithMany(p => p.ChatSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chat_User");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842F2D0624C3");

            entity.ToTable("Notification");

            entity.HasIndex(e => e.UserId, "IX_Notification_user_id");

            entity.Property(e => e.NotificationId).HasColumnType("uniqueidentifier").HasColumnName("notification_id");
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_User");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__465962292D23A46C");

            entity.ToTable("Order");

            entity.HasIndex(e => e.OrderStatusId, "IX_Order_order_status_id");

            entity.HasIndex(e => e.UserId, "IX_Order_user_id");

            entity.HasIndex(e => e.CouponId, "IX_Order_coupon_id");

            entity.Property(e => e.OrderId).HasColumnType("uniqueidentifier").HasColumnName("order_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.FullName)
                .HasColumnName("full_name");
            entity.Property(e => e.OrderStatusId).HasColumnType("uniqueidentifier").HasColumnName("order_status_id");
            entity.Property(e => e.Phone)                
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("total_price");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");

            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").HasColumnName("user_id");
            entity.Property(e => e.CouponId).HasColumnType("uniqueidentifier").HasColumnName("coupon_id");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_OrderStatus");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_User");

            entity.HasOne(d => d.Coupon).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CouponId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Coupon");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__3C5A4080BBA82BF6");

            entity.ToTable("OrderDetail");

            entity.HasIndex(e => e.OrderId, "IX_OrderDetail_order_id");

            entity.HasIndex(e => e.ProductId, "IX_OrderDetail_product_id");

            entity.Property(e => e.OrderDetailId).HasColumnType("uniqueidentifier").HasColumnName("order_detail_id");
            entity.Property(e => e.OrderId).HasColumnType("uniqueidentifier").HasColumnName("order_id");
            entity.Property(e => e.PaymentPrice)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("payment_price");
            entity.Property(e => e.ProductId).HasColumnType("uniqueidentifier").HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Product");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId).HasName("PK__OrderSta__A499CF231D746F37");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.OrderStatusId).HasColumnType("uniqueidentifier").HasColumnName("order_status_id");
            entity.Property(e => e.Name)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__47027DF5960BA9EF");

            entity.ToTable("Product");

            entity.HasIndex(e => e.CategoryId, "IX_Product_category_id");

            entity.HasIndex(e => e.Product3DModelId, "IX_Product_3d_Model_id");

            entity.HasIndex(e => e.Slug, "IX_Product_slug")
                .IsUnique()
                .HasFilter("([slug] IS NOT NULL)");

            entity.Property(e => e.ProductId).HasColumnType("uniqueidentifier").HasColumnName("product_id");
            entity.Property(e => e.CategoryId).HasColumnType("uniqueidentifier").HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");
            entity.Property(e => e.Product3DModelId).HasColumnType("uniqueidentifier").HasColumnName("product_3d_model_id");
            entity.Property(e => e.Name)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Slug)
                .HasColumnName("slug");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(p => p.Product3DModel)
                .WithOne(m => m.Product)
                .HasForeignKey<Product>(p => p.Product3DModelId);
        });

        modelBuilder.Entity<Product3DModel>(entity =>
        {
            entity.HasKey(e => e.Product3DModelId).HasName("PK__Product3__7A982983809D9053");

            entity.ToTable("Product3DModel");

            entity.Property(e => e.Product3DModelId).HasColumnType("uniqueidentifier").HasColumnName("product_3d_model_id");
            entity.Property(e => e.ModelUrl)
                .HasColumnName("model_url");
            entity.Property(e => e.ThumbnailUrl)
                .HasColumnName("thumbnail_url");
            entity.Property(e => e.VideoUrl)
                .HasColumnName("video_url");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");
        });

        modelBuilder.Entity<ProductFavorite>(entity =>
        {
            entity.ToTable("ProductFavorite");

            entity.HasIndex(e => e.ProductId, "IX_ProductFavorite_ProductId");

            entity.HasIndex(e => e.UserId, "IX_ProductFavorite_UserId");

            entity.Property(e => e.ProductFavoriteId).HasColumnType("uniqueidentifier").HasColumnName("product_favorite_id");
            entity.Property(e => e.ProductFavoriteId).HasColumnType("uniqueidentifier").HasColumnName("user_id");
            entity.Property(e => e.ProductFavoriteId).HasColumnType("uniqueidentifier").HasColumnName("product_d");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductFavorites).HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.User).WithMany(p => p.ProductFavorites).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ProductImageId).HasName("PK__ProductI__7A342910809D9053");

            entity.ToTable("ProductImage");

            entity.HasIndex(e => e.ProductId, "IX_ProductImage_product_id");

            entity.Property(e => e.ProductImageId).HasColumnType("uniqueidentifier").HasColumnName("productImage_id");
            entity.Property(e => e.ImageUrl)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.ProductId).HasColumnType("uniqueidentifier").HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductImage_Product");
        });

        modelBuilder.Entity<ProductReviewInteraction>(entity =>
        {
            entity.HasKey(e => e.ProductReviewInteractionId).HasName("PK__ProductI__A2F12EDBBAC0359E");

            entity.ToTable("ProductReviewInteraction");

            entity.HasIndex(e => e.ProductReviewId, "IX_ProductReviewInteraction_product_review_id");

            entity.HasIndex(e => e.ProductInteractionStatusId, "IX_ProductReviewInteraction_product_interaction_status_id");

            entity.HasIndex(e => e.UserId, "IX_ProductReviewInteraction_user_id");

            entity.Property(e => e.ProductReviewInteractionId).HasColumnType("uniqueidentifier").HasColumnName("product_review_interaction_id");
            entity.Property(e => e.ProductReviewId).HasColumnType("uniqueidentifier").HasColumnName("product_review_id");
            entity.Property(e => e.ProductInteractionStatusId).HasColumnType("uniqueidentifier").HasColumnName("product_interaction_status_id");
            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");

            entity.HasOne(d => d.ProductReview).WithMany(p => p.ProductReviewInteractions)
                .HasForeignKey(d => d.ProductReviewId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductReviewInteraction_Product");

            entity.HasOne(d => d.ProductReviewInteractionStatuses).WithMany(p => p.ProductReviewInteractions)
                .HasForeignKey(d => d.ProductInteractionStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductReviewInteraction_ProductReviewInteractionStatus");

            entity.HasOne(d => d.User).WithMany(p => p.ProductInteractions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductReviewInteraction_User");
        });

        modelBuilder.Entity<ProductReviewInteractionStatus>(entity =>
        {
            entity.HasKey(e => e.ProductReviewInteractionStatusId).HasName("PK__ProductI__0CC0092882FD7968");

            entity.ToTable("ProductReviewInteractionStatus");

            entity.Property(e => e.ProductReviewInteractionStatusId).HasColumnType("uniqueidentifier").HasColumnName("product_review_interaction_status_id");
            entity.Property(e => e.Name)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ProductReport>(entity =>
        {
            entity.HasKey(e => e.ProductReportId).HasName("PK__ProductR__DC0B4A22C1B82B5E");

            entity.ToTable("ProductReport");

            entity.HasIndex(e => e.ProductReportedId, "IX_ProductReport_product_reported_id");

            entity.HasIndex(e => e.ReportStatusId, "IX_ProductReport_report_status_id");

            entity.HasIndex(e => e.UserReportId, "IX_ProductReport_user_report_id");

            entity.Property(e => e.ProductReportId).HasColumnType("uniqueidentifier").HasColumnName("product_report_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductReportedId).HasColumnType("uniqueidentifier").HasColumnName("product_reported_id");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.ReportStatusId).HasColumnType("uniqueidentifier").HasColumnName("report_status_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");
            entity.Property(e => e.UserReportId).HasColumnType("uniqueidentifier").HasColumnName("user_report_id");

            entity.HasOne(d => d.ProductReported).WithMany(p => p.ProductReports)
                .HasForeignKey(d => d.ProductReportedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductReport_Product");

            entity.HasOne(d => d.ReportStatus).WithMany(p => p.ProductReports)
                .HasForeignKey(d => d.ReportStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductReport_ReportStatus");

            entity.HasOne(d => d.UserReport).WithMany(p => p.ProductReports)
                .HasForeignKey(d => d.UserReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductReport_User");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.ProductReviewId).HasName("PK__ProductR__8440EB03DB89B961");

            entity.ToTable("ProductReview");

            entity.HasIndex(e => e.ProductId, "IX_ProductReview_product_id");

            entity.HasIndex(e => e.UserId, "IX_ProductReview_user_id");

            entity.Property(e => e.ProductReviewId).HasColumnType("uniqueidentifier").HasColumnName("product_review_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");
            entity.Property(e => e.ProductId).HasColumnType("uniqueidentifier").HasColumnName("product_id");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductReview_Product");

            entity.HasOne(d => d.User).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductReview_User");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken");

            entity.HasIndex(e => e.UserId, "IX_RefreshToken_user_id");

            entity.Property(e => e.RefreshTokenId)
                .ValueGeneratedNever()
                .HasColumnName("refresh_token_id");
            entity.Property(e => e.ExpiredAt)
                .HasColumnType("datetime")
                .HasColumnName("expired_at");
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.IssuedAt)
                .HasColumnType("datetime")
                .HasColumnName("issued_at");
            entity.Property(e => e.JwtId).HasColumnName("jwt_id");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefreshToken_User");
        });

        modelBuilder.Entity<ReportProductReview>(entity =>
        {
            entity.HasKey(e => e.ReportProductReviewId).HasName("PK__ReportPr__09EE80B34DFBA358");

            entity.ToTable("ReportProductReview");

            entity.HasIndex(e => e.ProductReviewReportedId, "IX_ReportProductReview_product_review_reported_id");

            entity.HasIndex(e => e.ReportStatusId, "IX_ReportProductReview_report_status_id");

            entity.HasIndex(e => e.UserReportId, "IX_ReportProductReview_user_report_id");

            entity.Property(e => e.ReportProductReviewId).HasColumnType("uniqueidentifier").HasColumnName("report_product_review_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductReviewReportedId).HasColumnType("uniqueidentifier").HasColumnName("product_review_reported_id");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.ReportStatusId).HasColumnType("uniqueidentifier").HasColumnName("report_status_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");
            entity.Property(e => e.UserReportId).HasColumnType("uniqueidentifier").HasColumnName("user_report_id");

            entity.HasOne(d => d.ProductReviewReported).WithMany(p => p.ReportProductReviews)
                .HasForeignKey(d => d.ProductReviewReportedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportProductReview_ProductReview");

            entity.HasOne(d => d.ReportStatus).WithMany(p => p.ReportProductReviews)
                .HasForeignKey(d => d.ReportStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportProductReview_ReportStatus");

            entity.HasOne(d => d.UserReport).WithMany(p => p.ReportProductReviews)
                .HasForeignKey(d => d.UserReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportProductReview_User");
        });

        modelBuilder.Entity<ReportStatus>(entity =>
        {
            entity.HasKey(e => e.ReportStatusId).HasName("PK__ReportSt__09E0D88687C41A16");

            entity.ToTable("ReportStatus");

            entity.Property(e => e.ReportStatusId).HasColumnType("uniqueidentifier").HasColumnName("report_status_id");
            entity.Property(e => e.Name)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CC5F8795DF");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnType("uniqueidentifier").HasColumnName("role_id");
            entity.Property(e => e.Name)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F0AD19949");

            entity.ToTable("User");

            entity.HasIndex(e => e.RoleId, "IX_User_role_id");

            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").HasColumnName("user_id");
            entity.Property(e => e.AvatarUrl)
                .IsUnicode(false)
                .HasColumnName("avatar_url");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EmailConfirmationCode)
                .IsUnicode(false)
                .HasColumnName("email_confirmation_code");
            entity.Property(e => e.EmailConfirmationSentAt)
                .HasColumnType("datetime")
                .HasColumnName("email_confirmation_sent_at");
            entity.Property(e => e.EmailConfirmed).HasColumnName("email_confirmed");
            entity.Property(e => e.FullName)
                .HasColumnName("full_name");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");
            entity.Property(e => e.IsSubscriber).HasColumnName("is_subscriber");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.ResetPasswordCode)
                .IsUnicode(false)
                .HasColumnName("reset_password_code");
            entity.Property(e => e.ResetPasswordRequired).HasColumnName("reset_password_required");
            entity.Property(e => e.ResetPasswordSentAt)
                .HasColumnType("datetime")
                .HasColumnName("reset_password_sent_at");
            entity.Property(e => e.RoleId).HasColumnType("uniqueidentifier").HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.UserAddressId).HasName("PK__UserAddr__FEC0352E4E3DC7B4");

            entity.ToTable("UserAddress");

            entity.HasIndex(e => e.UserId, "IX_UserAddress_user_id");

            entity.Property(e => e.UserAddressId).HasColumnType("uniqueidentifier").HasColumnName("user_address_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FullName)
                .HasColumnName("full_name");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.Phone)                
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").HasColumnName("user_id");
            entity.Property(e => e.IsDelete).HasColumnName("is_delete");

            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAddress_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
