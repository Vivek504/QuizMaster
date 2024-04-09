import React, { useState, useEffect } from "react";
import Navbar from "../Navbar/Navbar";
import LocalStorageKey from "../../constants/LocalStorageKey";
import { viewStudentCourses } from "../../services/studentCourseService";
import ViewStudentCourses from "./ViewStudentCourses/ViewStudentCourses";

const StudentCourseIndex = () => {
    const [courses, setCourses] = useState();

    useEffect(() => {
        const getCourses = async () => {
            const response_data = await viewStudentCourses(localStorage.getItem(LocalStorageKey.JWTTOKEN));
            
            setCourses(response_data.payload?.courses);
        }

        getCourses();
    }, []);

    return (
        <div>
            <Navbar />
            <ViewStudentCourses courses={courses} />
        </div>
    );
}

export default StudentCourseIndex;