
1) Need to add the factory "ApplicationDbContextFactory"

2) To create it:
Uceme.Model % dotnet ef migrations add AddBlogColumns --context Uceme.Model.Data.ApplicationDbContext --verbose

3) To run it:
dotnet ef database update  --context Uceme.Model.Data.ApplicationDbContext --verbose    
