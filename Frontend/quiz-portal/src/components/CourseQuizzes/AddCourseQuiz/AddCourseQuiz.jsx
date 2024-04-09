import React, { useState } from "react";
import Navbar from "../../Navbar/Navbar";
import CourseQuizForm from "../CourseQuizForm/CourseQuizForm";
import { addQuiz as addQuizService } from "../../../services/quizService";
import { useParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { combineDateAndTime } from "../../../dateUtility/DateTimeConverter";
import LocalStorageKey from "../../../constants/LocalStorageKey";

const AddCourseQuiz = () => {
    const { courseId } = useParams();
    const [formError, setFormError] = useState();

    const navigate = useNavigate();

    const addQuiz = async (data) => {
        const response_data = await addQuizService(localStorage.getItem(LocalStorageKey.JWTTOKEN), Number(courseId), data.quizTitle, combineDateAndTime(data.startDate, data.startTime), combineDateAndTime(data.endDate, data.endTime), data.quizType);

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
            <CourseQuizForm title="Add Quiz" handler={addQuiz} formError={formError} redirectToBackPage={redirectToBackPage}
                quizTitle=''
                startDate=''
                startTime=''
                endDate=''
                endTime=''
                quizType=''
            />
        </div>
    );
}

export default AddCourseQuiz;