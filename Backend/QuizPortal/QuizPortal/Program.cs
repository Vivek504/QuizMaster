using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using QuizPortal.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using QuizPortal.IManagers;
using QuizPortal.Managers;
using Microsoft.OpenApi.Models;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

////Configure JWT
//var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
//var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = jwtIssuer,
//         ValidAudience = jwtIssuer,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//     };
// });

// Configure MySQL connection using connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine(connectionString);

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddAWSService<IAmazonSQS>();

builder.Services.AddScoped<IProfessorManager, ProfessorManager>();
builder.Services.AddScoped<IStudentManager, StudentManager>();
builder.Services.AddScoped<ICourseManager, CourseManager>();
builder.Services.AddScoped<IProfessorCourseManager, ProfessorCourseManager>();
builder.Services.AddScoped<IStudentCourseManager, StudentCourseManager>();
builder.Services.AddScoped<IQuizManager, QuizManager>();
builder.Services.AddScoped<IQuestionManager, QuestionManager>();
builder.Services.AddScoped<IUserCodeManager, UserCodeManager>();
builder.Services.AddScoped<ISQSManager, SQSManager>();
builder.Services.AddScoped<ISNSManager, SNSManager>();
builder.Services.AddScoped<IStudentQuizManager, StudentQuizManager>();
builder.Services.AddScoped<IStudentResultManager, StudentResultManager>();
builder.Services.AddScoped<IVideoManager, VideoManager>();
builder.Services.AddScoped<IAnswerManager, AnswerManager>();
builder.Services.AddScoped<IAccuracyManager, AccuracyManager>();
builder.Services.AddScoped<IMailManager, MailManager>();

// For Identity  
builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();
// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWTKey:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]))
                };
            });

var allowedOrigin = builder.Configuration["AllowedOrigin"];

Console.WriteLine(allowedOrigin);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins(allowedOrigin)
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();

