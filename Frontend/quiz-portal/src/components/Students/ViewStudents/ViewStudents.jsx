import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import LocalStorageKey from "../../../constants/LocalStorageKey";
import { getStudentCourse } from "../../../services/studentCourseService";
import { getStudentQuiz } from "../../../services/studentQuizService";
import { HttpStatusCode } from "axios";
import { getStudentResult } from "../../../services/studentResultService";

const ViewStudents = ({ courseId, students, courseQuizzes }) => {
    const navigate = useNavigate();

    const [studentQuizzes, setStudentQuizzes] = useState();
    const [studentResults, setStudentResults] = useState();
    const [cheatingMessage, setCheatingMessage] = useState();

    const onAddStudentsRedirect = () => {
        navigate(`/course/${courseId}/add-student`);
    }

    const handleViewScore = async (studentId) => {
        const student_course_response_data = await getStudentCourse(localStorage.getItem(LocalStorageKey.JWTTOKEN), studentId, courseId);

        const fetchedStudentQuizzes = []
        for (const courseQuiz of courseQuizzes) {
            const student_quiz_response_data = await getStudentQuiz(localStorage.getItem(LocalStorageKey.JWTTOKEN), student_course_response_data.payload?.studentCourse.id, courseQuiz.id);

            if (student_quiz_response_data.responseCode === HttpStatusCode.Ok) {
                fetchedStudentQuizzes.push(student_quiz_response_data.payload?.studentQuiz);
            }
        }
        setStudentQuizzes(fetchedStudentQuizzes);

        const fetchedStudentResults = []
        const fetchedCheating = []
        for (const studentQuiz of fetchedStudentQuizzes) {
            const student_result_response_data = await getStudentResult(localStorage.getItem(LocalStorageKey.JWTTOKEN), studentQuiz.id);

            if (student_result_response_data.responseCode === HttpStatusCode.BadRequest) {
                fetchedStudentResults.push('Student has not completed the quiz.')
            }
            else {
                const student_result = student_result_response_data.payload?.studentResult;

                if (!student_result.score) {
                    fetchedStudentResults.push('Please wait for the result.');
                }
                else {
                    fetchedStudentResults.push(`Overall score: ${student_result.score}`);
                }

                if (student_result.isCheatingFound === true) {
                    fetchedCheating.push('Student has been caught cheating.');
                }
                else {
                    fetchedCheating.push();
                }
            }
        }
        setStudentResults(fetchedStudentResults)
        setCheatingMessage(fetchedCheating);

        document.getElementById('my_modal_1').showModal();
    }

    return (
        <div>
            <div className="flex items-center">
                <article class="prose">
                    <h1 className="ml-10">Students</h1>
                </article>
                <button className="btn btn-primary ml-auto mr-5" onClick={onAddStudentsRedirect}>Add</button>
            </div>
            <div className="m-5 flex flex-wrap">
                {students ? (
                    students.length > 0 ? (
                        students.map((student) => (
                            <div key={student.id} className="card w-80 bg-base-100 shadow-xl ml-5 mt-5">
                                <div className="card-body">
                                    <h2 className="card-title">{student.email}</h2>
                                    <div className="flex justify-end mt-auto">
                                        <button className="btn btn-primary ml-2" onClick={() => handleViewScore(student.id)}>View Score</button>
                                        <dialog id="my_modal_1" className="modal">
                                            <div className="modal-box">
                                                <h3 className="font-bold text-4xl">Result</h3>
                                                <div className="py-4">
                                                    {studentResults && cheatingMessage && courseQuizzes.map((quiz, index) => (
                                                        <div className="mt-5">
                                                            <p className="text-2xl font-semibold">{quiz.quizTitle}</p>
                                                            <p>{studentResults[index]}{studentResults[index].includes('score') && (<>%</>)}</p>
                                                            <p className="text-red-500">{cheatingMessage[index]}</p>
                                                        </div>
                                                    ))}
                                                </div>

                                                <div className="modal-action">
                                                    <form method="dialog">
                                                        <button className="btn" onClick={() => { }}>Close</button>
                                                    </form>
                                                </div>
                                            </div>
                                        </dialog>
                                    </div>
                                </div>
                            </div>
                        ))
                    ) : (
                        <p className="text-xl mt-5 ml-5">No students have been added.</p>
                    )
                ) : (
                    <div className="ml-5 mt-5">
                        <span className="loading loading-bars loading-xs"></span>
                        <span className="loading loading-bars loading-sm ml-2"></span>
                        <span className="loading loading-bars loading-md ml-2"></span>
                        <span className="loading loading-bars loading-lg ml-2"></span>
                    </div>
                )}
            </div>
        </div>
    );
};

export default ViewStudents;