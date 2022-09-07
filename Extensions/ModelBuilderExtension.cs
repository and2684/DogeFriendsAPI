namespace DogeFriendsAPI.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void SeedUserRoles(this ModelBuilder modelBuilder)
        {
            // Роли, которые есть в системе по умолчанию
            modelBuilder.Entity<UserRole>()
                        .HasData(
                         new UserRole { Id = 1, Role = "Administrator" },
                         new UserRole { Id = 2, Role = "User" },
                         new UserRole { Id = 3, Role = "Company" });                      
        }
    }
}