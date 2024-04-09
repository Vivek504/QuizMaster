    using System;

    using Amazon.Rekognition;
    using Amazon.Rekognition.Model;
    using QuizPortal.IManagers;
    using QuizPortal.SQSModels;
    using QuizPortal.DBModels;
    using QuizPortalConsumer.Utility;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using Amazon.SQS;
    using Amazon.S3;
    using Amazon.S3.Model;
    using Amazon.S3.Transfer;

    namespace QuizPortalConsumer.Workflows
    {
	    public class RekognitionWorkflow
	    {
            private readonly IConfiguration _configuration;
		    private readonly IAmazonRekognition _amazonRekognition;

		    public RekognitionWorkflow(IConfiguration configuration, IAmazonRekognition amazonRekognition)
		    {
                _configuration = configuration;
			    _amazonRekognition = amazonRekognition;
            }

		    public async Task CheckFaceRotation(ISQSManager _sqsManager, IVideoManager _videoManager, IStudentResultManager _studentResultManager, IStudentQuizManager _studentQuizManager, FaceAnalysisMessage faceAnalysisMessage)
		    {

                try
                {
                    QuizPortal.DBModels.Video? video = await _videoManager.GetVideo(faceAnalysisMessage.VideoId);
                if (video == null)
                {
                    return;
                }

                if (video.StudentQuiz.Quiz.QuizType == QuizPortal.Utility.QuizTypeEnum.RECORDING)
                {
                    TranscribeRequestMessage transcribeRequestMessage = new TranscribeRequestMessage()
                    {
                        MessageType = QuizPortal.Utility.MessageTypeEnum.TRANSCRIBE_REQUEST,
                        VideoId = video.Id,
                        BucketName = _configuration[AppSettingsConstantsPath.S3BucketName],
                        ObjectName = video.S3ObjectName,
                        IsLastAnswer = video.IsLastVideo
                    };
                    string transcribeQueueUrl = await _sqsManager.GetQueueUrlAsync(_configuration[AppSettingsConstantsPath.TranscribeRequestQueueName]);

                    _sqsManager.PublishToQueueAsync(transcribeQueueUrl, JsonConvert.SerializeObject(transcribeRequestMessage));
                }

                StudentResult studentResult = await _studentResultManager.GetByStudentQuizId(video.StudentQuiz.Id);
                if (studentResult == null)
                {
                    studentResult = new StudentResult()
                    {
                        StudentQuiz = video.StudentQuiz,
                        IsCheatingFound = false
                    };
                    await _studentResultManager.Add(studentResult);
                }

                Console.WriteLine("trying to get from s3.");

                string S3ObjectName = video.S3ObjectName;
                string inputFileName = S3ObjectName;
                string outputFileName = inputFileName.Substring(0, inputFileName.LastIndexOf('.')) + ".mp4";
                string localFilePath = _configuration[AppSettingsConstantsPath.VIDEO_STORAGE_PATH];
                string localInputFilePath = localFilePath + "/" + inputFileName;
                string localOutputFilePath = localFilePath + "/" + outputFileName;
            
                Console.WriteLine("local file path: " + localFilePath);

                using (var s3Client = new AmazonS3Client())
                {
                    GetObjectRequest getObjectRequest = new GetObjectRequest
                    {
                        BucketName = _configuration[Utility.AppSettingsConstantsPath.S3BucketName],
                        Key = S3ObjectName
                    };

                    Console.WriteLine("created s3 get request");

                    using (GetObjectResponse response = await s3Client.GetObjectAsync(getObjectRequest))
                    {
                        Console.WriteLine("got the response from s3.");
                        using (Stream responseStream = response.ResponseStream)
                        {
                            Console.WriteLine("got the response stream from s3.");
                            using (var fileStream = File.Create(localInputFilePath))
                            {
                                responseStream.CopyTo(fileStream);
                                Console.WriteLine("copied into local.");
                            }
                        }
                    }
                }

                Console.WriteLine("trying to run ffmpeg command.");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-i \"{localInputFilePath}\" \"{localOutputFilePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Console.WriteLine(psi.FileName + " " + psi.Arguments);
                    using (Process process = new Process())
                    {
                        process.StartInfo = psi;

                        process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                        process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        process.WaitForExit();
                    }

                    using (var s3Client = new AmazonS3Client())
                    {
                        using (var fileTransferUtility = new TransferUtility(s3Client))
                        {
                            await fileTransferUtility.UploadAsync(localOutputFilePath, _configuration[Utility.AppSettingsConstantsPath.S3BucketName], outputFileName);
                        }

                        DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
                        {
                            BucketName = _configuration[Utility.AppSettingsConstantsPath.S3BucketName],
                            Key = S3ObjectName
                        };

                        await s3Client.DeleteObjectAsync(deleteObjectRequest);

                    }

                    File.Delete(localInputFilePath);
                    File.Delete(localOutputFilePath);

                    video.S3ObjectName = outputFileName;
                    await _videoManager.UpdateVideo(video);

                    var startFaceDetectionRequest = new StartFaceDetectionRequest
                    {
                        Video = new Amazon.Rekognition.Model.Video
                        {
                            S3Object = new Amazon.Rekognition.Model.S3Object
                            {
                                Bucket = _configuration[Utility.AppSettingsConstantsPath.S3BucketName],
                                Name = video.S3ObjectName
                            }
                        }
                    };

                    var startFaceDetectionResponse = await _amazonRekognition.StartFaceDetectionAsync(startFaceDetectionRequest);

                    GetFaceDetectionResponse faceDetectionResponse;
                    do
                    {
                        GetFaceDetectionRequest getFaceDetectionRequest = new GetFaceDetectionRequest()
                        {
                            JobId = startFaceDetectionResponse.JobId
                        };

                        faceDetectionResponse = await _amazonRekognition.GetFaceDetectionAsync(getFaceDetectionRequest);

                        await Task.Delay(1000);
                    } while (faceDetectionResponse.JobStatus == VideoJobStatus.IN_PROGRESS);

                    foreach (var faceDetection in faceDetectionResponse.Faces)
                    {
                        var faceDetail = faceDetection.Face;
                        if (Math.Abs(faceDetail.Pose.Roll) > 15 || Math.Abs(faceDetail.Pose.Yaw) > 15 || Math.Abs(faceDetail.Pose.Pitch) > 15)
                        {
                            studentResult.IsCheatingFound = true;
                            await _studentResultManager.Update(studentResult);
                        }
                    }


                    video.IsAnalysisCompleted = true;
                    await _videoManager.UpdateVideo(video);

                    List<QuizPortal.DBModels.Video> videos = await _videoManager.GetVideosByStudentQuizId(video.StudentQuiz.Id);
                    if (videos != null)
                    {
                        QuizPortal.DBModels.Video? lastVideo = videos.Where(v => v.IsLastVideo).FirstOrDefault();
                        if (lastVideo != null)
                        {
                            bool areAllAnalyzed = true;

                            foreach (var v in videos)
                            {
                                if (!v.IsAnalysisCompleted)
                                {
                                    areAllAnalyzed = false;
                                    break;
                                }
                            }

                            if (areAllAnalyzed)
                            {
                                video.StudentQuiz.AnalyzedVideos = true;
                                await _studentQuizManager.UpdateStudentQuiz(video.StudentQuiz);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
