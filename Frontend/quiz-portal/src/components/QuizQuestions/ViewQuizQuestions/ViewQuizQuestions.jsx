import React from "react";
import { useNavigate } from "react-router-dom";

const Viewquestions = ({ courseId, quizId, questions }) => {
    const navigate = useNavigate();

    const addQuizQuestionRedirect = () => {
        navigate(`/course/${courseId}/quiz/${quizId}/add-question`);
    }

    const updateQuizQuestionRedirect = (questionId) => {
        navigate(`/course/${courseId}/quiz/${quizId}/update-question/${questionId}`);
    }

    return (
        <div>
            <div className="flex items-center">
                <article class="prose">
                    <h1 className="ml-10">Questions</h1>
                </article>
                <button className="btn btn-primary ml-auto mr-5" onClick={addQuizQuestionRedirect}>Add</button>
            </div>
            <div className="m-5 flex flex-wrap">
                {questions ? (
                    questions.length > 0 ? (
                        questions.map((question) => (
                            <div key={question.id} className="card w-80 bg-base-100 shadow-xl ml-5 mt-5">
                                <div className="card-body">
                                    <p>{question.questionText}</p>
                                    <div className="flex justify-end mt-auto">
                                        <button className="btn btn-primary" onClick={() => updateQuizQuestionRedirect(question.id)}>Update</button>
                                    </div>
                                </div>
                            </div>
                        ))
                    ) : (
                        <p className="text-xl mt-5 ml-5">No questions available.</p>
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

export default Viewquestions;