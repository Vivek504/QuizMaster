import axios from 'axios';

const apiUrl = window.location.origin + `:${process.env.REACT_APP_BACKEND_PORT}/api`;

export const addAnswer = async (token, studentQuizId, questionId, answerText, isLastAnswer) => {
    try {
        const response = await axios.post(
            `${apiUrl}/Answer/addAnswer`,
            {
                studentQuizId,
                questionId,
                answerText,
                isLastAnswer
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