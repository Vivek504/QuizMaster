import React, { useState } from "react";
import Navbar from "../../Navbar/Navbar";
import { getQuestion as getQuestionService, updateQuestion as updateQuestionService } from "../../../services/questionService";
import LocalStorageKey from "../../../constants/LocalStorageKey";
import { useNavigate, useParams } from "react-router-dom";
import QuizQuestionForm from "../QuizQuestionForm/QuizQuestionForm";
import { useEffect } from "react";

const UpdateQuizQuestion = () => {
    const { courseId } = useParams();
    const { quizId } = useParams();
    const { questionId } = useParams();

    const navigate = useNavigate();
    const [formError, setFormError] = useState();
    const [question, setQuestion] = useState();

    useEffect(() => {
        const getQuestion = async () => {
            const response_data = await getQuestionService(localStorage.getItem(LocalStorageKey.JWTTOKEN), questionId);
            
            setQuestion(response_data.payload?.question);
        };

        getQuestion();
    }, [questionId]);

    const updateQuestion = async (data) => {
        const response_data = await updateQuestionService(localStorage.getItem(LocalStorageKey.JWTTOKEN), questionId, data.questionText);

        if (response_data.responseCode === 200) {
            redirectToBackPage();
        }
        else {
            setFormError(response_data.message);
        }
    }

    const redirectToBackPage = () => {
        navigate(`/course/${courseId}/quiz/${quizId}/questions`);
    }

    return (
        <>
            <Navbar />
            {question && <QuizQuestionForm title="Update Question" questionText={question && question.questionText} handler={updateQuestion} formError={formError} redirectToBackPage={redirectToBackPage} /> }
        </>
    );
}

export default UpdateQuizQuestion;