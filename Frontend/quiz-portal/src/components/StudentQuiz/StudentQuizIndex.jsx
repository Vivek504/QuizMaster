import React, { useEffect, useState } from "react";
import Navbar from "../Navbar/Navbar";
import { useParams } from "react-router-dom";
import { getQuizQuestions } from "../../services/questionService";
import LocalStorageKey from "../../constants/LocalStorageKey";
import ViewQuizQuestions from "./ViewQuizQuestions/ViewQuizQuestions";
import { getStudentQuizById } from "../../services/studentQuizService";
import { getVideosByStudentQuizId } from "../../services/videoService";
import { getQuiz } from "../../services/quizService";

const StudentQuizIndex = () => {
    const { courseId } = useParams();
    const { studentQuizId } = useParams();
    const [studentQuiz, setStudentQuiz] = useState();
    const [questions, setQuestions] = useState();
    const [videos, setVideos] = useState();
    const [currentQuestionIndex, setCurrentQuestionIndex] = useState(null);
    const [isLastQuestion, setIsLastQuestion] = useState(null);
    const [quiz, setQuiz] = useState();

    useEffect(() => {
        const handleGetStudentQuiz = async () => {
            const response_data = await getStudentQuizById(localStorage.getItem(LocalStorageKey.JWTTOKEN), studentQuizId);

            setStudentQuiz(response_data.payload?.studentQuiz);
        }

        const handleGetVideos = async () => {
            const response_data = await getVideosByStudentQuizId(localStorage.getItem(LocalStorageKey.JWTTOKEN), studentQuizId);

            setVideos(response_data.payload?.videos);
        }

        handleGetStudentQuiz();
        handleGetVideos();
    }, []);

    useEffect(() => {
        if (questions && videos) {
            if (videos.length === 0) {
                setCurrentQuestionIndex(0);
                if (questions.length === 1) {
                    setIsLastQuestion(true);
                }
                else {
                    setIsLastQuestion(false);
                }
            }
            else {
                setCurrentQuestionIndex(videos.length);
                if (questions.length - videos.length === 1) {
                    setIsLastQuestion(true);
                }
                else {
                    setIsLastQuestion(false);
                }
            }
        }
    }, [questions, videos])

    useEffect(() => {
        const getQuestion = async () => {
            const response_data = await getQuizQuestions(localStorage.getItem(LocalStorageKey.JWTTOKEN), studentQuiz.quiz.id);

            setQuestions(response_data.payload?.questions);
        }

        const handleGetQuiz = async () => {
            const response_data = await getQuiz(localStorage.getItem(LocalStorageKey.JWTTOKEN), studentQuiz.quiz.id);

            setQuiz(response_data.payload?.quiz);
        }

        if (studentQuiz) {
            getQuestion();
            handleGetQuiz();
        }
    }, [studentQuiz]);
    
    return (
        <div>
            <Navbar />
            {questions && currentQuestionIndex !== null && isLastQuestion !== null && quiz && <ViewQuizQuestions courseId={courseId} studentQuizId={studentQuizId} quizId={studentQuiz.quiz.id} questions={questions} currentQuestionIndex={currentQuestionIndex} setCurrentQuestionIndex={setCurrentQuestionIndex} isLastQuestion={isLastQuestion} quiz={quiz} />}
        </div>
    )
}

export default StudentQuizIndex;