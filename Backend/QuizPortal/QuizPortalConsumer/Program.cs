using QuizPortalConsumer;

using QuizPortal.IManagers;
using QuizPortal.Managers;
using Amazon.SQS;
using Amazon.SimpleNotificationService;
using QuizPortalConsumer.Workflows;
using Amazon.Rekognition;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using QuizPortal.DBModels;
using Microsoft.EntityFrameworkCore;
using QuizPortalConsumer.IManagers;
using QuizPortalConsumer.Managers;
using QuizPortalConsumer.Utility;

var builder = Host.CreateDefaultBuilder(args);

IHost host = builder
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<MailSettings>(hostContext.Configuration.GetSection("MailSettings"));

        services.AddAWSService<IAmazonSQS>();
        services.AddAWSService<IAmazonSimpleNotificationService>();
        services.AddAWSService<IAmazonRekognition>();

        string connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDBContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        services.AddSingleton<SNSWorkflow>();

        services.AddHostedService<Worker>();

        services.AddScoped<ISQSManager, SQSManager>();
        services.AddScoped<IVideoManager, VideoManager>();
        services.AddScoped<IStudentResultManager, StudentResultManager>();
        services.AddScoped<IQuestionManager, QuestionManager>();
        services.AddScoped<IAnswerManager, AnswerManager>();
        services.AddScoped<IOpenAIManager, OpenAIManager>();
        services.AddScoped<IAccuracyManager, AccuracyManager>();
        services.AddScoped<IStudentQuizManager, StudentQuizManager>();
        services.AddScoped<QuizPortalConsumer.IManagers.IMailManager, QuizPortalConsumer.Managers.MailManager>();

        services.AddScoped<RekognitionWorkflow>();
        services.AddScoped<TranscribeResponseWorkflow>();
        services.AddScoped<OpenAIWorkflow>();
        services.AddScoped<ResultWorkflow>();
        services.AddScoped<MailWorkflow>();
    })
    .Build();

host.Run();
