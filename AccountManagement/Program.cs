var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AccountDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AccountDbContext>();

TypeAdapterConfig<UserRegistrationDto, User>
    .NewConfig()
    .Map(dest => dest.UserName, src => src.Email);

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