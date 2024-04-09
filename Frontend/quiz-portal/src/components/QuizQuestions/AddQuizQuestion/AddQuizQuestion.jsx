import React, { useState } from "react";
import Navbar from "../../Navbar/Navbar";
import { addQuestion as addQuestionService } from "../../../services/questionService";
import LocalStorageKey from "../../../constants/LocalStorageKey";
import { useNavigate, useParams } from "react-router-dom";
import QuizQuestionForm from "../QuizQuestionForm/QuizQuestionForm";

const AddQuizQuestion = () => {
    const { courseId } = useParams();
    const { quizId } = useParams();

    const navigate = useNavigate();
    const [formError, setFormError] = useState();

    const addQuestion = async (data) => {
        const response_data = await addQuestionService(localStorage.getItem(LocalStorageKey.JWTTOKEN), quizId, data.questionText);

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
            <QuizQuestionForm title="Add Question" questionText="" handler={addQuestion} formError={formError} redirectToBackPage={redirectToBackPage} />
        </>
    );
}

export default AddQuizQuestion;