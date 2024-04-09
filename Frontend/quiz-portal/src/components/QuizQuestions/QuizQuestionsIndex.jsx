import React, { useState, useEffect } from "react";
import Navbar from "../Navbar/Navbar";
import ViewQuizQuestions from "./ViewQuizQuestions/ViewQuizQuestions";
import { getQuizQuestions } from "../../services/questionService";
import LocalStorageKey from "../../constants/LocalStorageKey";
import { useParams } from "react-router-dom";

const QuizQuestionsIndex = () => {
    const { courseId } = useParams();
    const { quizId } = useParams();
    const [questions, setQuestions] = useState();

    useEffect(() => {
        const getQuestion = async () => {
            const response_data = await getQuizQuestions(localStorage.getItem(LocalStorageKey.JWTTOKEN), quizId);

            setQuestions(response_data.payload?.questions);
        }

        getQuestion();
    }, []);

    return (
        <div>
            <Navbar />
            <ViewQuizQuestions courseId={courseId} quizId={quizId} questions={questions} />
        </div>
    );
}

export default QuizQuestionsIndex;