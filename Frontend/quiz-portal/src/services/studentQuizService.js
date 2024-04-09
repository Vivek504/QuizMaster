import axios from 'axios';

const apiUrl = window.location.origin + `:${process.env.REACT_APP_BACKEND_PORT}/api`;

export const getStudentQuizById = async (token, studentQuizId) => {
    try {
        const response = await axios.get(
            `${apiUrl}/StudentQuiz/getStudentQuiz?studentQuizId=${studentQuizId}`,
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

export const getStudentQuiz = async (token, studentCourseId, quizId) => {
    try {
        const response = await axios.get(
            `${apiUrl}/StudentQuiz/getStudentQuizByStudentCourseAndQuiz?studentCourseId=${studentCourseId}&quizId=${quizId}`,
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

export const addStudentQuiz = async (token, studentCourseId, quizId, startDateTime) => {
    try {
        const response = await axios.post(
            `${apiUrl}/StudentQuiz/addStudentQuiz`,
            {
                studentCourseId,
                quizId,
                startDateTime
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

export const endStudentQuiz = async (token, studentQuizId, endDateTime) => {
    try {
        const response = await axios.put(
            `${apiUrl}/StudentQuiz/endStudentQuiz`,
            {
                studentQuizId,
                endDateTime
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