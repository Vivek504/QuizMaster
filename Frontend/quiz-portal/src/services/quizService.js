import axios from 'axios';

const apiUrl = window.location.origin + `:${process.env.REACT_APP_BACKEND_PORT}/api`;

export const getCourseQuizzes = async (token, courseId) => {
    try {
        const response = await axios.get(
            `${apiUrl}/Quiz/getCourseQuizzes?courseId=${courseId}`,
            {
                headers: {
                    'accept': 'text/plain',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            }
        );

        return response.data;
    } catch (error) {
        console.error(error.message);
    }
}

export const getQuiz = async (token, quizId) => {
    try {
        const response = await axios.get(
            `${apiUrl}/Quiz/getQuiz?quizId=${quizId}`,
            {
                headers: {
                    'accept': 'text/plain',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            }
        );

        return response.data;
    } catch (error) {
        console.error(error.message);
    }
}

export const addQuiz = async (token, courseId, quizTitle, startDateTime, endDateTime, quizType) => {
    try {
        const response = await axios.post(
            `${apiUrl}/Quiz/addQuiz`,
            {
                courseId,
                quizTitle,
                startDateTime,
                endDateTime,
                quizType
            },
            {
                headers: {
                    'accept': 'text/plain',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            }
        );

        return response.data;
    } catch (error) {
        console.error(error.message);
    }
}

export const updateQuiz = async (token, quizId, quizTitle, startDateTime, endDateTime, quizType) => {
    try {
        const response = await axios.put(
            `${apiUrl}/Quiz/updateQuiz`,
            {
                id: quizId,
                quizTitle,
                startDateTime,
                endDateTime,
                quizType
            },
            {
                headers: {
                    'accept': 'text/plain',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            }
        );

        return response.data;
    } catch (error) {
        console.error(error.message);
    }
}