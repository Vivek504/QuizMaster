import React, { useState } from "react";
import Navbar from "../Navbar/Navbar";
import { viewCourseStudents } from "../../services/studentCourseService";
import { useEffect } from "react";
import LocalStorageKey from "../../constants/LocalStorageKey";
import { useParams } from "react-router-dom";
import ViewStudents from "./ViewStudents/ViewStudents";
import { getStudentCourse } from "../../services/studentCourseService";
import { getCourseQuizzes } from "../../services/quizService";

const StudentIndex = () => {
    const { id } = useParams();
    const [students, setStudents] = useState();
    const [courseQuizzes, setCourseQuizzes] = useState();

    useEffect(() => {
        const getStudents = async () => {
            const response_data = await viewCourseStudents(localStorage.getItem(LocalStorageKey.JWTTOKEN), id);

            setStudents(response_data.payload?.students);
        }

        const handleGetCourseQuizzes = async () => {
            const response_data = await getCourseQuizzes(localStorage.getItem(LocalStorageKey.JWTTOKEN), id);

            setCourseQuizzes(response_data.payload?.quizzes);
        }

        getStudents();
        handleGetCourseQuizzes();
    }, [id])

    return (
        <div>
            <Navbar />
            {students && courseQuizzes && <ViewStudents courseId={id} students={students} courseQuizzes={courseQuizzes} />}
        </div>
    );
};

export default StudentIndex;