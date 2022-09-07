using DogeFriendsAPI.Extensions;

namespace DogeFriendsAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> Roles => Set<UserRole>();
        public DbSet<Dog> Dogs => Set<Dog>();
        public DbSet<Breed> Breeds => Set<Breed>();
        public DbSet<DogPhoto> DogPhotos => Set<DogPhoto>();
        public DbSet<DogPhotoLike> DogPhotoLikes => Set<DogPhotoLike>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(c => c.Roles)
                .WithMany(s => s.Users)
                .UsingEntity(t => t.ToTable("UserToRole"));            

            modelBuilder.Entity<Dog>()
                .HasOne(c => c.User)
                .WithMany(s => s.Dogs)
                .HasForeignKey(k => k.UserId);

            modelBuilder.Entity<Breed>()
                .HasMany(c => c.Dogs)
                .WithOne(s => s.Breed)
                .HasForeignKey(k => k.BreedId);

            modelBuilder.Entity<DogPhoto>()
                .HasOne(c => c.Dog)
                .WithMany(s => s.Photos)
                .HasForeignKey(k => k.DogId);

            modelBuilder.Entity<DogPhotoLike>()
                .HasKey(k => new { k.LikedPhotoId, k.SourceUserId });

            modelBuilder.Entity<DogPhotoLike>()
                .HasOne(c => c.SourceUser)
                .WithMany(s => s.DogPhotoLikes)
                .HasForeignKey(k => k.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.SeedUserRoles();
        }
    }
}
