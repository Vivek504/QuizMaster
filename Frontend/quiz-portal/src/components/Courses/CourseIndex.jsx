import React, { useState, useEffect } from "react";
import Navbar from "../Navbar/Navbar";
import ViewCourses from "./ViewCourses/ViewCourses";
import { viewProfessorCourses } from "../../services/professorCourseService";
import LocalStorageKey from "../../constants/LocalStorageKey";

const CourseIndex = () => {
    const [courses, setCourses] = useState();

    useEffect(() => {
        const getCourses = async () => {
            const response_data = await viewProfessorCourses(localStorage.getItem(LocalStorageKey.JWTTOKEN));

            setCourses(response_data.payload?.courses);
        }

        getCourses();
    }, []);

    return (
        <div>
            <Navbar />
            <ViewCourses courses={courses} />
        </div>
    );
}

export default CourseIndex;