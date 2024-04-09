import React, { useState } from "react";
import { useParams } from "react-router-dom";
import Navbar from "../Navbar/Navbar";
import { useEffect } from "react";
import { getCourseQuizzes as getCourseQuizzesService } from "../../services/quizService";
import LocalStorageKey from "../../constants/LocalStorageKey";
import ViewCourseQuizzes from "./ViewCourseQuizzes/ViewCourseQuizzes";
import { getStudentQuiz } from "../../services/studentQuizService";
import { HttpStatusCode } from "axios";
import ROLES from "../../constants/roles";

const CourseQuizzesIndex = () => {
    const { courseId } = useParams();
    const [quizzes, setQuizzes] = useState();
    const [canStartQuiz, setCanStartQuiz] = useState({});

    useEffect(() => {
        const getCourseQuizzes = async () => {
            const response_data = await getCourseQuizzesService(localStorage.getItem(LocalStorageKey.JWTTOKEN), courseId);

            setQuizzes(response_data.payload?.quizzes);
        }

        getCourseQuizzes();
    }, [courseId])

    const getStudentQuizResponse = async (quizId) => {
        let studentQuizResponse = await getStudentQuiz(localStorage.getItem(LocalStorageKey.JWTTOKEN), courseId, quizId);
        if (studentQuizResponse.responseCode === HttpStatusCode.BadRequest) {
            return null;
        }
        return studentQuizResponse;
    }

    useEffect(() => {
        if(quizzes){
            const verifyStartButton = async (quizId) => {
                let studentQuizResponse = await getStudentQuizResponse(quizId);

                setCanStartQuiz(prevCanStartQuiz => {
                    const updatedCanStartQuiz = { ...prevCanStartQuiz };
    
                    if (studentQuizResponse) {
                        if (studentQuizResponse.payload?.studentQuiz.endDateTime) {
                            updatedCanStartQuiz[quizId] = false;
                        } else {
                            updatedCanStartQuiz[quizId] = true;
                        }
                    } else {
                        updatedCanStartQuiz[quizId] = true;
                    }
                    
                    return updatedCanStartQuiz;
                });
    
            }
    
            if(localStorage.getItem(LocalStorageKey.ROLE) === ROLES.STUDENT){
                quizzes.forEach(quiz => {
                    verifyStartButton(quiz.id);
                });
            }
        }
    }, [quizzes]);

    return (
        <div>
            <Navbar />
            {quizzes && canStartQuiz && <ViewCourseQuizzes courseId={courseId} quizzes={quizzes} canStartQuiz={canStartQuiz} getStudentQuizResponse={localStorage.getItem(LocalStorageKey.ROLE) === ROLES.STUDENT && getStudentQuizResponse} />}
        </div>
    );
}

export default CourseQuizzesIndex;