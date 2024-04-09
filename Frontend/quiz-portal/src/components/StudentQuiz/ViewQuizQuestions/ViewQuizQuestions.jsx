import React, { useState, useRef, useEffect } from "react";
import Webcam from "react-webcam";
import { useNavigate } from "react-router-dom";
import { PutObjectCommand, S3Client } from "@aws-sdk/client-s3";
import { addVideo } from "../../../services/videoService";
import LocalStorageKey from "../../../constants/LocalStorageKey";
import QUIZ_TYPES from "../../../constants/QuizTypes";
import { addAnswer } from "../../../services/answerService";
import { endStudentQuiz } from "../../../services/studentQuizService";
import moment from "moment";

const ViewQuizQuestions = ({ courseId, studentQuizId, quiz, questions, currentQuestionIndex, setCurrentQuestionIndex, isLastQuestion }) => {
    const [disableNext, setDisableNext] = useState(true);
    const [disableQuit, setDisableQuit] = useState(true);
    const webcamRef = useRef(null);
    const mediaRecorderRef = useRef(null);
    const [recordedChunks, setRecordedChunks] = useState([]);
    const [startRecording, setStartRecording] = useState(false);
    const navigate = useNavigate();
    const [nextQuestionRequest, setNextQuestionRequest] = useState(false);
    const recordAudio = quiz.quizType.toLowerCase() === QUIZ_TYPES.RECORDING.toLowerCase();
    const isTypingQuiz = quiz.quizType.toLowerCase() === QUIZ_TYPES.TYPING.toLowerCase()
    const [currentAnswer, setCurrentAnswer] = useState('');
    const [answerToSubmit, setAnswerToSubmit] = useState('');

    useEffect(() => {
        if (startRecording) {
            handleStartRecording();
        }
    }, [startRecording]);

    const handleStartRecording = () => {
        const webcamVideo = webcamRef.current.video;
        const mediaStream = webcamVideo.srcObject;
        if (mediaStream) {
            if (!nextQuestionRequest) {
                setDisableNext(false);
            }
            if (isLastQuestion) {
                setDisableNext(true);
            }
            setDisableQuit(false);
            let stream;

            if (recordAudio) {
                const audioTracks = mediaStream.getAudioTracks();
                if (audioTracks.length > 0) {
                    stream = new MediaStream([...webcamVideo.srcObject.getTracks(), ...audioTracks]);
                    handleMediaRecorder(stream, webcamVideo);
                }
                else {
                    console.error("No audio tracks available");
                    setTimeout(handleStartRecording, 1000);
                }
            }
            else {
                stream = new MediaStream(webcamVideo.srcObject.getTracks());
                handleMediaRecorder(stream, webcamVideo);
            }
        }
        else {
            setTimeout(handleStartRecording, 1000);
        }
    };

    const handleMediaRecorder = (stream, webcamVideo) => {
        mediaRecorderRef.current = new MediaRecorder(stream, {
            mimeType: "video/webm",
        });

        mediaRecorderRef.current.addEventListener("dataavailable", (event) => {
            if (event.data.size > 0) {
                setRecordedChunks(prevChunks => {
                    return [...prevChunks, event.data];
                });

            }
        });

        webcamVideo.play().then(() => {
            mediaRecorderRef.current.start();
        }).catch(error => console.error("Error starting webcam video playback:", error));
    }

    useEffect(() => {
        if (recordedChunks.length !== 0) {
            const uploadToS3 = async (recording) => {
                const s3_bucket_name = "quiz-master-video-storage";

                const creds = {
                    accessKeyId: process.env.REACT_APP_ACCESS_KEY_ID,
                    secretAccessKey: process.env.REACT_APP_SECRET_ACCESS_KEY,
                    sessionToken: process.env.REACT_APP_SESSION_TOKEN
                };

                const s3 = new S3Client({
                    region: 'us-east-1',
                    credentials: creds
                });

                let questionIndex = currentQuestionIndex;
                let questionId;
                if (!nextQuestionRequest) {
                    questionIndex++;
                    questionId = questions[currentQuestionIndex].id;
                }
                else {
                    questionId = questions[currentQuestionIndex - 1].id;
                }

                const params = {
                    Bucket: s3_bucket_name,
                    Key: `${studentQuizId}-${questionIndex}.webm`,
                    Body: recording
                }

                const command = new PutObjectCommand(params);

                try {
                    await s3.send(command);

                    await addVideo(localStorage.getItem(LocalStorageKey.JWTTOKEN), studentQuizId, questionId, params.Key, !nextQuestionRequest);
                    
                    if(isTypingQuiz){
                        await addAnswer(localStorage.getItem(LocalStorageKey.JWTTOKEN), studentQuizId, questionId, answerToSubmit, !nextQuestionRequest);
                    }
                    
                    setAnswerToSubmit('');
                }
                catch (error) {
                    console.error(error);
                }

                if (nextQuestionRequest) {
                    handleStartRecording();
                }
                else {
                    const formattedEndDateTime = moment().format('YYYY-MM-DDTHH:mm:ss.SSS[Z]');
                    await endStudentQuiz(localStorage.getItem(LocalStorageKey.JWTTOKEN), studentQuizId, formattedEndDateTime);
                    navigate(`/course/${courseId}/quizzes`);
                }
            }

            const handleRecording = async () => {
                const recording = new Blob(recordedChunks, { type: 'video/webm' });
                setRecordedChunks([]);
                await uploadToS3(recording);
            }

            handleRecording();
        }

    }, [recordedChunks]);

    const handleStopRecording = () => {
        mediaRecorderRef.current.stop();
    };

    const handleNextQuestion = () => {
        setAnswerToSubmit(currentAnswer);
        setCurrentAnswer('');
        setNextQuestionRequest(true);
        if (currentQuestionIndex < questions.length - 1) {
            setCurrentQuestionIndex(currentQuestionIndex + 1);
            if (currentQuestionIndex + 1 === questions.length - 1) {
                setDisableNext(true);
            }
            setDisableQuit(true);
        }
        handleStopRecording();
    };

    const handleQuitQuiz = () => {
        setAnswerToSubmit(currentAnswer);
        setCurrentAnswer('');
        setNextQuestionRequest(false);
        handleStopRecording();
    };

    const cancelQuiz = () => {
        navigate(`/course/${courseId}/quizzes`);
    }

    const handleAnswerChange = (event) => {
        setCurrentAnswer(event.target.value);
    }

    return (
        <div class="flex justify-center">
            {startRecording ?
                <div>
                    <div className="flex items-center justify-center">
                        <article class="prose">
                            <h1>Questions</h1>
                        </article>
                    </div>
                    <div className="m-5 flex flex-col items-start">
                        <div key={questions[currentQuestionIndex].id} className="card bg-base-100 shadow-xl ml-5 mt-5" style={{ width: recordAudio ? '42rem' : '50rem' }}>
                            <div className="card-body">
                                {recordAudio ? (
                                    <div>
                                        <h2 className="card-title">Question {currentQuestionIndex + 1}</h2>
                                        <p>{questions[currentQuestionIndex].questionText}</p>
                                        <div>
                                            <Webcam
                                                audio={recordAudio}
                                                muted={true}
                                                ref={webcamRef}
                                                screenshotFormat="image/jpeg"
                                            />
                                        </div>
                                    </div>
                                ) : (
                                    <div class="flex" style={{ height: '30rem' }}>
                                        <div class="flex-1">
                                            <Webcam
                                                audio={recordAudio}
                                                muted={true}
                                                ref={webcamRef}
                                                screenshotFormat="image/jpeg"
                                                class="mt-20"
                                            />
                                        </div>
                                        <div class="flex-1 p-4">
                                            <h2 class="text-xl font-semibold">Question {currentQuestionIndex + 1}</h2>
                                            <p class="text-gray-700">{questions[currentQuestionIndex].questionText}</p>
                                            <textarea class="w-full h-64 mt-4 resize-none border border-gray-300 rounded-md p-2" value={currentAnswer} onChange={handleAnswerChange} />
                                        </div>
                                    </div>
                                )}
                            </div>
                        </div>
                        <div class="flex justify-between ml-5 mt-5" style={{ width: '42rem' }}>
                            <button class="btn btn-error" onClick={handleQuitQuiz} disabled={disableQuit}>Quit</button>
                            <button class="btn btn-primary" onClick={handleNextQuestion} disabled={disableNext}>Next</button>
                        </div>
                    </div>
                </div>
                :
                <div class="flex flex-col items-center justify-center h-full">
                    <div role="alert" className="alert w-50">
                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" className="stroke-info shrink-0 w-6 h-6"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
                        <span>Quiz will begin recording your video. Please confirm your consent to proceed.</span>
                        <div>
                            <button className="btn btn-sm" onClick={() => cancelQuiz()}>Cancel</button>
                            <button className="btn btn-sm btn-primary ml-2" onClick={() => setStartRecording(true)}>Confirm</button>
                        </div>
                    </div>
                </div>
            }
        </div>
    );
}

export default ViewQuizQuestions;