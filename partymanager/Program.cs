using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using partymanager.Domain.Entidades;
using partymanager.Domain.Interfaces;
using partymanager.Infrastructure.Data.Context;
using partymanager.Infrastructure.Data.Repository;
using partymanager.Application.Models;
using partymanager.Service.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner

var configuration = builder.Configuration;

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Configuração da Autenticação JWT
var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddControllers();

// Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "partymanager API", Version = "v1" });
});

// Configuração do JSON
builder.Services.AddMvc().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Configuração do AutoMapper
builder.Services.AddSingleton(new MapperConfiguration(config =>
{
    // Mapeamento para Status, Genero, PerfilUsuario
    config.CreateMap<Status, StatusModel>();
    config.CreateMap<StatusModel, Status>();

    config.CreateMap<Genero, GeneroModel>();
    config.CreateMap<GeneroModel, Genero>();

    config.CreateMap<PerfilUsuario, PerfilUsuarioModel>();
    config.CreateMap<PerfilUsuarioModel, PerfilUsuario>();

    // Mapeamento para as novas entidades
    config.CreateMap<TipoItem, TipoItemModel>();
    config.CreateMap<TipoItemModel, TipoItem>();

    config.CreateMap<TipoEvento, TipoEventoModel>();
    config.CreateMap<TipoEventoModel, TipoEvento>();

    config.CreateMap<Item, ItemModel>();
    config.CreateMap<ItemModel, Item>();

    config.CreateMap<ItemCalculado, ItemCalculadoModel>();
    config.CreateMap<ItemCalculadoModel, ItemCalculado>();

    config.CreateMap<Evento, EventoModel>();
    config.CreateMap<EventoModel, Evento>();

    config.CreateMap<Usuario, UsuarioModel>();
    config.CreateMap<UsuarioModel, Usuario>();

    config.CreateMap<TipoEventoItem, TipoEventoItemModel>();
    config.CreateMap<TipoEventoItemModel, TipoEventoItem>();






}).CreateMapper());

// Configuração dos serviços e repositórios
builder.Services.AddDbContext<SqlServerContext>();

// Configuração dos serviços e repositórios
builder.Services.AddScoped<IBaseService<Status>, BaseService<Status>>();
builder.Services.AddScoped<IBaseRepository<Status>, BaseRepository<Status>>();

builder.Services.AddScoped<IBaseService<Genero>, BaseService<Genero>>();
builder.Services.AddScoped<IBaseRepository<Genero>, BaseRepository<Genero>>();

builder.Services.AddScoped<IBaseService<PerfilUsuario>, BaseService<PerfilUsuario>>();
builder.Services.AddScoped<IBaseRepository<PerfilUsuario>, BaseRepository<PerfilUsuario>>();

builder.Services.AddScoped<IBaseService<TipoItem>, BaseService<TipoItem>>();
builder.Services.AddScoped<IBaseRepository<TipoItem>, BaseRepository<TipoItem>>();

builder.Services.AddScoped<IBaseService<TipoEvento>, BaseService<TipoEvento>>();
builder.Services.AddScoped<IBaseRepository<TipoEvento>, BaseRepository<TipoEvento>>();

builder.Services.AddScoped<IBaseService<Item>, BaseService<Item>>();
builder.Services.AddScoped<IBaseRepository<Item>, BaseRepository<Item>>();

builder.Services.AddScoped<IBaseService<ItemCalculado>, BaseService<ItemCalculado>>();
builder.Services.AddScoped<IBaseRepository<ItemCalculado>, BaseRepository<ItemCalculado>>();

builder.Services.AddScoped<IBaseService<Evento>, BaseService<Evento>>();
builder.Services.AddScoped<IBaseRepository<Evento>, BaseRepository<Evento>>();

builder.Services.AddScoped<IBaseService<Usuario>, BaseService<Usuario>>();
builder.Services.AddScoped<IBaseRepository<Usuario>, BaseRepository<Usuario>>();

builder.Services.AddScoped<IBaseService<TipoEventoItem>, BaseService<TipoEventoItem>>();
builder.Services.AddScoped<IBaseRepository<TipoEventoItem>, BaseRepository<TipoEventoItem>>();

var app = builder.Build();

// Configuração do pipeline de requisição

// Ativar Swagger sempre, independentemente do ambiente
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "partymanager API v1"));

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve arquivos estáticos da pasta wwwroot por padrão

app.UseRouting();

// Ativando CORS
app.UseCors("CorsPolicy");

// Ativando Autenticação e Autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();




//var app = builder.Build();

//// Configuração do pipeline de requisição

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "partymanager API v1"));
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles(); // Serve arquivos estáticos da pasta wwwroot por padrão

//app.UseRouting();

//// Ativando CORS
//app.UseCors("CorsPolicy");

//// Ativando Autenticação e Autorização
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();

