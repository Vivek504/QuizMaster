import React, { useState } from "react";
import Navbar from "../../Navbar/Navbar";
import CourseForm from "../CourseForm/CourseForm";
import { useParams } from "react-router-dom";
import { useEffect } from "react";
import { getCourse as getCourseService } from "../../../services/courseService";
import LocalStorageKey from "../../../constants/LocalStorageKey";
import { updateCourse as udpateCourseService } from "../../../services/courseService";
import { useNavigate } from "react-router-dom";

const UpdateCourse = () => {
    const { id } = useParams();
    const [course, setCourse] = useState();
    const [renderForm, setRenderForm] = useState(false);
    const [formError, setFormError] = useState();
    const navigate = useNavigate();

    useEffect(() => {
        const getCourse = async () => {
            const response_data = await getCourseService(localStorage.getItem(LocalStorageKey.JWTTOKEN), id);

            setCourse(response_data.payload?.course);
            setRenderForm(true);
        }

        getCourse();
    }, [])

    const updateCourse = async (data) => {
        const response_data = await udpateCourseService(localStorage.getItem(LocalStorageKey.JWTTOKEN), id, data.courseName);
        
        if(response_data.responseCode === 200){
            navigate('/courses');
        }
        else{
            setFormError(response_data.message);
        }
    }

    return (
        <>
            <Navbar />
            {renderForm && <CourseForm title="Update Course" courseName={course && course.courseName} handler={updateCourse} formError={formError} />}
        </>
    );
}

export default UpdateCourse;