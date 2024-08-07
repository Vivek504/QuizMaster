import axios from 'axios';

const apiUrl = `${process.env.REACT_APP_BACKEND_URL}` + `:${process.env.REACT_APP_BACKEND_PORT}/api`;

export const getQuizQuestions = async (token, quizId) => {
    try {
        const response = await axios.get(
            `${apiUrl}/Question/getQuizQuestions?quizId=${quizId}`,
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

export const getQuestion = async (token, questionId) => {
    try {
        const response = await axios.get(
            `${apiUrl}/Question/getQuestion?questionId=${questionId}`,
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

export const addQuestion = async (token, quizId, questionText) => {
    try {
        const response = await axios.post(
            `${apiUrl}/Question/addQuestion`,
            {
                quizId,
                questionText
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

export const updateQuestion = async (token, id, questionText) => {
    try {
        const response = await axios.put(
            `${apiUrl}/Question/updateQuestion`,
            {
                id,
                questionText
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