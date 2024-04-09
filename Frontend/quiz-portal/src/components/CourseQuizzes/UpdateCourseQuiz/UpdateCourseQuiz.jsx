import React, { useState } from "react";
import Navbar from "../../Navbar/Navbar";
import CourseQuizForm from "../CourseQuizForm/CourseQuizForm";
import { useParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { combineDateAndTime, extractDateAndTime } from "../../../dateUtility/DateTimeConverter";
import LocalStorageKey from "../../../constants/LocalStorageKey";
import { getQuiz as getQuizService } from "../../../services/quizService";
import { updateQuiz as updateQuizService } from "../../../services/quizService";
import { useEffect } from "react";

const UpdateCourseQuiz = () => {
    const { courseId } = useParams();
    const { quizId } = useParams();
    const [formError, setFormError] = useState();
    const [quiz, setQuiz] = useState();
    const navigate = useNavigate();

    useEffect(() => {
        const getQuiz = async () => {
            const response_data = await getQuizService(localStorage.getItem(LocalStorageKey.JWTTOKEN), Number(quizId));

            setQuiz(response_data.payload?.quiz);
        }

        getQuiz();
    }, [])


    const updateQuiz = async (data) => {
        const response_data = await updateQuizService(localStorage.getItem(LocalStorageKey.JWTTOKEN), Number(quizId), data.quizTitle, combineDateAndTime(data.startDate, data.startTime), combineDateAndTime(data.endDate, data.endTime), data.quizType);

        if (response_data.responseCode === 200) {
            redirectToBackPage();
        }
        else {
            setFormError(response_data.message);
        }
    }

    const redirectToBackPage = () => {
        navigate(`/course/${courseId}/quizzes`);
    }
    return (
        <div>
            <Navbar />
            {quiz && <CourseQuizForm title="Add Quiz" handler={updateQuiz} formError={formError} redirectToBackPage={redirectToBackPage}
                quizTitle={quiz.quizTitle}
                startDate={extractDateAndTime(quiz.startDateTime).date}
                startTime={extractDateAndTime(quiz.startDateTime).time}
                endDate={extractDateAndTime(quiz.endDateTime).date}
                endTime={extractDateAndTime(quiz.endDateTime).time}
                quizType={quiz.quizType.toUpperCase()}
            />
            }
        </div>
    );
}

export default UpdateCourseQuiz;