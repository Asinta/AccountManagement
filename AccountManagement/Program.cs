var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AccountDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AccountDbContext>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

TypeAdapterConfig<UserRegistrationDto, User>
    .NewConfig()
    .Map(dest => dest.UserName, src => src.Email);

TypeAdapterConfig<IdentityRole, RoleDto>
    .NewConfig()
    .Map(dest => dest.RoleName, src => src.Name)
    .Map(dest => dest.Id, src => Guid.Parse(src.Id));

TypeAdapterConfig<RoleCreationDto, IdentityRole>
    .NewConfig()
    .Map(dest => dest.Name, src => src.RoleName);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
