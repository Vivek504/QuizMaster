namespace QuizPortalConsumer;

using QuizPortal.IManagers;
using QuizPortal.Utility;
using QuizPortal.SQSModels;
using Newtonsoft.Json;
using QuizPortalConsumer.Workflows;
using Microsoft.Extensions.DependencyInjection;
using QuizPortalConsumer.IManagers;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Worker> _logger;

    private readonly IConfiguration _configuration;

    private readonly SNSWorkflow _snsWorkflow;

    public Worker(ILogger<Worker> logger, IConfiguration configuration,
        SNSWorkflow snsWorkflow, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
        _snsWorkflow = snsWorkflow;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            ISQSManager _sqsManager = scope.ServiceProvider.GetRequiredService<ISQSManager>();

            List<string> queueUrls = new List<string>()
            {
                await _sqsManager.GetQueueUrlAsync(_configuration[Utility.AppSettingsConstantsPath.mailQueueName]),
                await _sqsManager.GetQueueUrlAsync(_configuration[Utility.AppSettingsConstantsPath.faceAnalysisQueueName]),
                await _sqsManager.GetQueueUrlAsync(_configuration[Utility.AppSettingsConstantsPath.TranscribeResponseQueueName]),
                await _sqsManager.GetQueueUrlAsync(_configuration[Utility.AppSettingsConstantsPath.FindAnswerAccuracyQueueName]),
                await _sqsManager.GetQueueUrlAsync(_configuration[Utility.AppSettingsConstantsPath.ResultQueueName])
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (string queueUrl in queueUrls)
                {
                    try
                    {
                        var messages = await _sqsManager.ReceiveMessageAsync(queueUrl);

                        if (messages.Any())
                        {
                            _logger.LogInformation("message recieved.");

                            foreach (var message in messages)
                            {
                                dynamic body = JsonConvert.DeserializeObject(message.Body);

                                if (body != null)
                                {
                                    if (body.MessageType == MessageTypeEnum.SUBSCRIBE_TO_TOPIC)
                                    {
                                        _logger.LogInformation("sending an email to subscribe.");
                                        _snsWorkflow.SubscribeEmailToTopic(JsonConvert.DeserializeObject<SubscribeToSNSMessage>(message.Body));
                                    }
                                    if (body.MessageType == MessageTypeEnum.SEND_MAIL)
                                    {
                                        _logger.LogInformation("sending an email.");
                                        QuizPortalConsumer.IManagers.IMailManager _mailManager = scope.ServiceProvider.GetRequiredService<QuizPortalConsumer.IManagers.IMailManager>();
                                        MailWorkflow _mailWorkflow = scope.ServiceProvider.GetRequiredService<MailWorkflow>();

                                        _mailWorkflow.SendEmail(JsonConvert.DeserializeObject<SendEmailMessage>(message.Body), _mailManager);
                                    }
                                    if (body.MessageType == MessageTypeEnum.FACE_ANALYSIS)
                                    {
                                        IVideoManager _videoManager = scope.ServiceProvider.GetRequiredService<IVideoManager>();
                                        IStudentResultManager _studentResultManager = scope.ServiceProvider.GetRequiredService<IStudentResultManager>();
                                        IStudentQuizManager _studentQuizManager = scope.ServiceProvider.GetRequiredService<IStudentQuizManager>();
                                        RekognitionWorkflow _rekognitionWorkflow = scope.ServiceProvider.GetRequiredService<RekognitionWorkflow>();
                                        _logger.LogInformation("analysing the face.");
                                        _rekognitionWorkflow.CheckFaceRotation(_sqsManager, _videoManager, _studentResultManager, _studentQuizManager, JsonConvert.DeserializeObject<FaceAnalysisMessage>(message.Body));
                                    }
                                    if (MessageTypeEnum.TRANSCRIBE_RESPONSE.ToString().Equals(((MessageTypeEnum)(body.MessageType)).ToString()))
                                    {
                                        IVideoManager _videoManager = scope.ServiceProvider.GetRequiredService<IVideoManager>();
                                        IAnswerManager _answerManager = scope.ServiceProvider.GetRequiredService<IAnswerManager>();
                                        TranscribeResponseWorkflow _transcribeResponseWorkflow = scope.ServiceProvider.GetRequiredService<TranscribeResponseWorkflow>();
                                        _logger.LogInformation("handling the transcribe response.");
                                        _transcribeResponseWorkflow.HandleTranscribeResponse(_videoManager, _answerManager, JsonConvert.DeserializeObject<TranscribeResponseMessage>(message.Body));
                                    }
                                    if (body.MessageType == MessageTypeEnum.FIND_ANSWER_ACCURACY)
                                    {
                                        _logger.LogInformation("finding answer accuracy.");
                                        IAnswerManager _answerManager = scope.ServiceProvider.GetRequiredService<IAnswerManager>();
                                        IAccuracyManager _accuracyManager = scope.ServiceProvider.GetRequiredService<IAccuracyManager>();
                                        IOpenAIManager _openAIManager = scope.ServiceProvider.GetRequiredService<IOpenAIManager>();
                                        IVideoManager _videoManager = scope.ServiceProvider.GetRequiredService<IVideoManager>();
                                        IStudentQuizManager _studentQuizManager = scope.ServiceProvider.GetRequiredService<IStudentQuizManager>();
                                        OpenAIWorkflow _openAIWorkflow = scope.ServiceProvider.GetRequiredService<OpenAIWorkflow>();
                                        _openAIWorkflow.FindAnswerAccuracy(_answerManager, _accuracyManager, _videoManager, _studentQuizManager, _openAIManager, JsonConvert.DeserializeObject<FindAnswerAccuracyMessage>(message.Body));
                                    }
                                    if (body.MessageType == MessageTypeEnum.FIND_RESULT)
                                    {
                                        _logger.LogInformation("finding the result.");
                                        IStudentQuizManager _studentQuizManager = scope.ServiceProvider.GetRequiredService<IStudentQuizManager>();
                                        IStudentResultManager _studentResultManager = scope.ServiceProvider.GetRequiredService<IStudentResultManager>();
                                        IAccuracyManager _accuracyManager = scope.ServiceProvider.GetRequiredService<IAccuracyManager>();
                                        IQuestionManager _questionManager = scope.ServiceProvider.GetRequiredService<IQuestionManager>();
                                        ResultWorkflow _resultWorkflow = scope.ServiceProvider.GetRequiredService<ResultWorkflow>();
                                        _resultWorkflow.FindResult(_studentQuizManager, _studentResultManager, _accuracyManager, _questionManager, JsonConvert.DeserializeObject<FindResultMessage>(message.Body));
                                    }
                                }

                                _logger.LogInformation("message is processed.");

                                await _sqsManager.DeleteMessageAsync(queueUrl, message.Handle);

                                _logger.LogInformation("message is deleted.");
                            }
                        }

                        await Task.Delay(500, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }
        }   
    }
}
