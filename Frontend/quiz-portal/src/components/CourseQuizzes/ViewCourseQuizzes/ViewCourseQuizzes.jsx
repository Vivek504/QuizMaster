import React, { useEffect, useState } from "react";
import { compareDateAndTime, originalToFormatted } from "../../../dateUtility/DateTimeConverter";
import { useNavigate } from "react-router-dom";
import LocalStorageKey from "../../../constants/LocalStorageKey";
import ROLES from "../../../constants/roles";
import { addStudentQuiz, getStudentQuiz } from "../../../services/studentQuizService";
import { getStudentResult } from "../../../services/studentResultService";
import { HttpStatusCode } from "axios";
import moment from 'moment';

const ViewCourseQuizzes = ({ courseId, quizzes, canStartQuiz, getStudentQuizResponse }) => {
    const role = localStorage.getItem(LocalStorageKey.ROLE);
    const navigate = useNavigate();
    const [modalMessage, setModalMessage] = useState();
    const [cheatingMessge, setCheatingMessage] = useState();

    const viewQuizQuestions = (quizId) => {
        navigate(`/course/${courseId}/quiz/${quizId}/questions`);
    }

    const addCourseQuizRedirect = () => {
        navigate(`/course/${courseId}/add-quiz`);
    }

    const updateCourseQuizRedirect = (quizId) => {
        navigate(`/course/${courseId}/update-quiz/${quizId}`);
    }

    const startQuizRedirect = async (quizId) => {
        let studentQuizResponse = await getStudentQuizResponse(quizId);
        if (!studentQuizResponse) {
            const formattedStartDateTime = moment().format('YYYY-MM-DDTHH:mm:ss.SSS[Z]');
            studentQuizResponse = await addStudentQuiz(localStorage.getItem(LocalStorageKey.JWTTOKEN), courseId, quizId, formattedStartDateTime);
            navigate(`/course/${courseId}/student-quiz/${studentQuizResponse.payload.studentQuiz.id}`);
        }
        else {
            navigate(`/course/${courseId}/student-quiz/${studentQuizResponse.payload.studentQuiz.id}`);
        }
    }

    const handleViewScore = async (quizId) => {
        const studentQuizResponse = await getStudentQuizResponse(quizId);
        if (!studentQuizResponse) {
            setModalMessage('Please complete the quiz.');
        }
        else {
            const studentResultResponse = await getStudentResult(localStorage.getItem(LocalStorageKey.JWTTOKEN), studentQuizResponse.payload?.studentQuiz.id);
            if (studentResultResponse.responseCode === HttpStatusCode.BadRequest) {
                setModalMessage('Please wait for the result.');
            }
            else {
                const score = studentResultResponse.payload?.studentResult.score;
                const isCheatingFound = studentResultResponse.payload?.studentResult.isCheatingFound;

                if (score === null) {
                    setModalMessage('Please wait for the result.');
                }
                else {
                    if (isCheatingFound) {
                        setCheatingMessage('You have been caught cheating.');
                    }
                    setModalMessage(`Overall Score: ${score}%`)
                }
            }
        }
        document.getElementById('my_modal_1').showModal();
    }

    return (
        <div>
            <div className="flex items-center">
                <article class="prose">
                    <h1 className="ml-10">Quizzes</h1>
                </article>
                {role === ROLES.PROFESSOR && <button className="btn btn-primary ml-auto mr-5" onClick={addCourseQuizRedirect}>Add</button>}
            </div>
            <div className="m-5 flex flex-wrap">
                {quizzes ? (
                    quizzes.length > 0 ? (
                        quizzes.map((quiz) => (
                            <div key={quiz.id} className="card w-200 bg-base-100 shadow-xl ml-5 mt-5">
                                <div className="card-body">
                                    <h2 className="card-title">{quiz.quizTitle}</h2>
                                    <div>
                                        <span className="font-bold mr-2">Start Date & Time:</span>
                                        <span>{originalToFormatted(quiz.startDateTime)}</span>
                                    </div>
                                    <div>
                                        <span className="font-bold mr-2">End Date & Time:</span>
                                        <span>{originalToFormatted(quiz.endDateTime)}</span>
                                    </div>
                                    <div>
                                        <span className="font-bold">Quiz Type:</span>
                                        <span>{quiz.quizType}</span>
                                    </div>
                                    {role === ROLES.PROFESSOR && (
                                        <div className="flex justify-end mt-auto">
                                            <button className="btn btn-primary" onClick={() => updateCourseQuizRedirect(quiz.id)}>Update</button>
                                            <button className="btn btn-primary ml-2" onClick={() => viewQuizQuestions(quiz.id)}>Questions</button>
                                        </div>
                                    )}
                                    {role === ROLES.STUDENT && (
                                        <div className="flex justify-end mt-auto">
                                            {compareDateAndTime(quiz.startDateTime, quiz.endDateTime) && canStartQuiz[quiz.id] && (<button key={quiz.id} className="btn btn-primary" onClick={() => startQuizRedirect(quiz.id)}>Start Quiz</button>)}
                                            <button className="btn btn-primary ml-2" onClick={() => handleViewScore(quiz.id)}>View Score</button>
                                            <dialog id="my_modal_1" className="modal">
                                                <div className="modal-box">
                                                    <h3 className="font-bold text-lg">Result</h3>
                                                    <div className="py-4">
                                                        {modalMessage}
                                                    </div>
                                                    {cheatingMessge &&
                                                        <div role="alert" className="alert alert-error">
                                                            <svg xmlns="http://www.w3.org/2000/svg" className="stroke-current shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                                                            <span>{cheatingMessge}</span>
                                                        </div>
                                                    }
                                                    <div className="modal-action">
                                                        <form method="dialog">
                                                            <button className="btn" onClick={() => { setCheatingMessage(); setModalMessage(); }}>Close</button>
                                                        </form>
                                                    </div>
                                                </div>
                                            </dialog>
                                        </div>
                                    )}
                                </div>
                            </div>
                        ))
                    ) : (
                        <p className="text-xl mt-5 ml-5">No quizzes have been added.</p>
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
}

export default ViewCourseQuizzes;