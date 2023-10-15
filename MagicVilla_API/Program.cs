
using MagicVilla_API;
using MagicVilla_API.Datos;
using MagicVilla_API.Repositorio;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Configuración de Swagger para probar JWT desde ahí
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });
    // Otras configuraciones de Swagger, si es necesario


    //Datos que se mostrarán en una caja cuando se presiona el botón de "Authorize" mostrando los datos de
    //"In, Description, Name y Type" en la caja
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT en el encabezado de autorización como 'Bearer {tu_token_jwt}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    //Agrega un requisito de seguridad
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            //Indica que hay un nuevo esquema de seguridad y el id de este es Bearer, el cual nos pedirá en el encabezado de las solicitudes protegidas que 
            //se autorizen con la sintaxis "Bearer {TokenGenerado}"
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },

            //Si los endpoints sólo podrán ser accedidos mediante el un rol o dato en específico lo guardamos en los scopes del jwt y en esta línea se agregarán
            //Si queremos que solo el administrador pueda entrar a los endpoints protegidos entonces se escribiría de la siquiente manera
            //new string[] { "admin" }
            new string[] { }
        }
    });
});
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Conexion"));
});

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<IVillaRepositorio, VillaRepositorio>();

builder.Services.AddScoped<INumeroVillaRepositorio, NumeroVillaRepositorio>();

//Agrega el esquema de autenticación de JWT 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    //Se configura el JWT con sus parámetros de validación
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "issuerDePrueba",
        ValidAudience = "audienceDePrueba",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("claveDePrueba123claveDePrueba123")),
        ClockSkew = TimeSpan.Zero
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        //Mandamos el JWT a la dirección /swagger/v1/
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//Para trabajar con JWT hay que instalar lo siguiente
//dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
//dotnet add package System.IdentityModel.Tokens.Jwt
//dotnet add package Swashbuckle.AspNetCore