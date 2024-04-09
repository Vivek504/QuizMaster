import React, { useState } from "react";
import Navbar from "../../Navbar/Navbar";
import CourseForm from "../CourseForm/CourseForm";
import { addCourse as addCourseService } from "../../../services/courseService";
import LocalStorageKey from "../../../constants/LocalStorageKey";
import { useNavigate } from "react-router-dom";

const AddCourse = () => {
    const navigate = useNavigate();
    const [formError, setFormError] = useState();

    const addCourse = async (data) => {
        const response_data = await addCourseService(localStorage.getItem(LocalStorageKey.JWTTOKEN), data.courseName);
        
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
            <CourseForm title="Add Course" courseName="" handler={addCourse} formError={formError} />
        </>
    );
}

export default AddCourse;