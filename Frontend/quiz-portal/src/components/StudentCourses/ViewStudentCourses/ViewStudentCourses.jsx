import React from "react";
import { useNavigate } from "react-router-dom";

const ViewStudentCourses = ({ courses }) => {
    const navigate = useNavigate();

    const viewQuiz = (courseId) => {
        navigate(`/course/${courseId}/quizzes`);
    }

    return (
        <div><div className="flex items-center">
            <article class="prose">
                <h1 className="ml-10">Courses</h1>
            </article>
        </div>
            <div className="m-5 flex flex-wrap">
                {courses ? (
                    courses.length > 0 ? (
                        courses.map((course) => (
                            <div key={course.id} className="card w-80 bg-base-100 shadow-xl ml-5 mt-5">
                                <div className="card-body">
                                    <h2 className="card-title">{course.courseName}</h2>
                                    <div className="flex justify-end mt-auto">
                                        <button className="btn btn-primary ml-2" onClick={() => viewQuiz(course.id)}>Quiz</button>
                                    </div>
                                </div>
                            </div>
                        ))
                    ) : (
                        <p className="text-xl mt-5 ml-5">No courses available.</p>
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

export default ViewStudentCourses;