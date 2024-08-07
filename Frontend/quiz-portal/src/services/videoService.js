import axios from 'axios';

const apiUrl = `${process.env.REACT_APP_BACKEND_URL}` + `:${process.env.REACT_APP_BACKEND_PORT}/api`;

export const addVideo = async (token, studentQuizId, questionId, s3ObjectName, isLastVideo) => {
    try {
        const response = await axios.post(
            `${apiUrl}/Video/addVideo`,
            {
                studentQuizId,
                questionId,
                s3ObjectName,
                isLastVideo
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

export const getVideosByStudentQuizId = async (token, studentQuizId) => {
    try {
        const response = await axios.get(
            `${apiUrl}/Video/getVideosByStudentQuizId?studentQuizId=${studentQuizId}`,
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